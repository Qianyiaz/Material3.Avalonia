using Material3.Avalonia.ColorScience.Models;
using Material3.Avalonia.ColorScience.Viewing;

namespace Material3.Avalonia.ColorScience.Conversion;

/// <summary>
/// Converts between ARGB sRGB and HCT (Hue–Chroma–Tone).
/// HCT is defined as: hue/chroma from CAM16, tone equals CIE L*.
/// This implementation uses an explicit numeric solver to find an in‑gamut sRGB
/// color for a requested (h, c, L*), following the behavioral contract of Google’s HCT.
/// </summary>
public sealed class HctConverter : IHctConverter
{
    private readonly ICam16Converter _cam;
    private readonly ILabConverter _lab;
    private readonly IViewingConditions _vc;

    /// <summary>
    /// Create an HCT converter bound to specific viewing conditions. Lab white is D65 (sRGB).
    /// </summary>
    public HctConverter(IViewingConditions viewingConditions, ICam16Converter? cam16 = null, ILabConverter? lab = null)
    {
        _cam = cam16 ?? Cam16Converter.Default;
        _lab = lab ?? LabConverter.Default;
        _vc = viewingConditions;
    }

    // Constants for numeric solving
    private const int MaxIterJ = 24;          // iterations for solving CAM16 J to match L*
    private const int MaxIterChroma = 10;     // binary search over chroma
    private const double LstarEps = 0.01;     // acceptable |ΔL*|
    private const double GamutTolerance = 1e-7;     // tolerance for RGB in-gamut check (linear)

    public HctColor ArgbToHct(uint argb)
    {
        // 1) sRGB → linear → XYZ (relative to D65)
        var (R8, G8, B8) = Argb32.UnpackRgb(argb);
        var (r, g, b) = Srgb.ToLinear(R8, G8, B8);
        var (X, Y, Z) = Srgb.LinearRgbToXyz(r, g, b);
        var xyz = new XyzColor(X, Y, Z);

        // 2) XYZ → CAM16 (hue, chroma)
        Cam16Color cam = _cam.ToCam16(xyz, _vc);
        double h = cam.Hue;
        double c = cam.C;

        // 3) XYZ → Lab(L*) for tone
        var lab = _lab.XyzToLab(xyz, WhitePoint.D65);
        double t = lab.L;

        return new HctColor(h, c, t);
    }

    public uint HctToArgb(double hueDegrees, double chroma, double toneLstar)
    {
        // Fast exits for extremes of tone
        if (toneLstar <= 0) return 0xFF000000u;        // pure black
        if (toneLstar >= 100) return 0xFFFFFFFFu;      // pure white

        // Clamp inputs into sensible ranges
        double h = Hue.Normalize(hueDegrees);
        double cTarget = Math.Max(0, chroma);

        // Binary-search chroma downward until we can realize the tone in-gamut.
        double lo = 0.0, hi = Math.Min(cTarget, 200.0); // practical upper bound
        uint? best = null;
        double bestChroma = -1;

        for (int i = 0; i < MaxIterChroma; i++)
        {
            double c = (lo + hi) * 0.5;
            var (ok, argb) = TrySolveAtHueChroma(h, c, toneLstar);
            if (ok)
            {
                best = argb;
                bestChroma = c;
                lo = c; // try higher chroma
            }
            else
            {
                hi = c; // reduce chroma
            }
        }

        if (best.HasValue)
            return best.Value;

        // Fallback: return neutral of requested tone (a*=b*=0)
        return NeutralAtTone(toneLstar);
    }

    // --- Core numeric solver -------------------------------------------------

    private (bool ok, uint argb) TrySolveAtHueChroma(double hue, double chroma, double toneLstar)
    {
        // We search J (CAM16 lightness) that reproduces the desired L* after full round-trip
        double jLo = 0.0, jHi = 100.0;
        uint lastArgb = 0;
        bool found = false;

        for (int iter = 0; iter < MaxIterJ; iter++)
        {
            double jMid = 0.5 * (jLo + jHi);
            // Construct a CAM16 color with J=jMid, C=chroma, h=hue
            var cam = new Cam16Color(jMid, chroma, hue, m: 0.0, s: 0.0, q: 0.0);
            // Back to XYZ using the provided viewing conditions
            XyzColor xyz = _cam.ToXyz(cam, _vc);

            // XYZ → sRGB (linear)
            var (rl, gl, bl) = Srgb.XyzToLinearRgb(xyz.X, xyz.Y, xyz.Z);
            if (!RgbGamut.InLinearGamut( rl, gl, bl, GamutTolerance))
            {
                // Not representable at this J — move J toward being darker or lighter
                // Heuristic: if any channel < 0, lighten; if >1, darken
                if (rl < -GamutTolerance || gl < -GamutTolerance || bl < -GamutTolerance)
                    jLo = jMid; // too dark; try lighter
                else
                    jHi = jMid; // too light; try darker
                continue;
            }

            // Compute tone (L*) to see if we matched target
            var lab = _lab.XyzToLab(xyz, WhitePoint.D65);
            double L = lab.L;
            double dL = L - toneLstar;

            // Quantize to 8-bit for final ARGB
            var (R8, G8, B8) = Srgb.FromLinear(rl, gl, bl);
            lastArgb = Argb32.PackRgb(R8, G8, B8);

            if (Math.Abs(dL) <= LstarEps)
            {
                found = true;
                break; // matched tone sufficiently and in gamut
            }

            if (dL < 0)
                jLo = jMid;   // need higher J to raise L*
            else
                jHi = jMid;   // need lower J to reduce L*
        }

        return (found, lastArgb);
    }

    private uint NeutralAtTone(double toneLstar)
    {
        // a*=b*=0 at requested L*
        var lab = new LabColor(toneLstar, 0.0, 0.0);
        var xyz = _lab.LabToXyz(lab, WhitePoint.D65);
        var (rl, gl, bl) = Srgb.XyzToLinearRgb(xyz.X, xyz.Y, xyz.Z);
        var (R8, G8, B8) = Srgb.FromLinear(Math.Clamp(rl, 0.0, 1.0), Math.Clamp(gl, 0.0, 1.0), Math.Clamp(bl, 0.0, 1.0));
        return Argb32.PackRgb(R8, G8, B8);
    }
}
