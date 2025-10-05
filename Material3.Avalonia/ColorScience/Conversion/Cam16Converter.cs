using Material3.Avalonia.Colors.Conversion;
using Material3.Avalonia.ColorScience.Models;
using Material3.Avalonia.ColorScience.Viewing;

namespace Material3.Avalonia.ColorScience.Conversion;

/// <summary>
/// Reference CAM16 converter (forward/inverse) per Li et al. 2017 (optimized steps * included).
/// </summary>
public sealed class Cam16Converter : ICam16Converter
{
    public static Cam16Converter Default { get; } = new();

    private Cam16Converter() { }

    /// <inheritdoc />
    public Cam16Color ToCam16(XyzColor xyz, IViewingConditions vc)
    {
        // Step 1: cone responses
        var (R, G, B) = Cam16Math.M16_XyzToRgb(xyz.X, xyz.Y, xyz.Z);

        // Step 2: D-adaptation in cone space
        var DR = Cam16Math.DFactor(vc.D, vc.White.Y, Cam16Math.Rw(vc.White));
        var DG = Cam16Math.DFactor(vc.D, vc.White.Y, Cam16Math.Gw(vc.White));
        var DB = Cam16Math.DFactor(vc.D, vc.White.Y, Cam16Math.Bw(vc.White));
        var Rc = DR * R; var Gc = DG * G; var Bc = DB * B;

        // Step 3*: response compression
        var (Ra, Ga, Ba) = Cam16Math.ResponseCompression(Rc, Gc, Bc, vc.FL);

        // Step 4*: opponent channels & auxiliaries
        Cam16Math.Opponents(Ra, Ga, Ba, out var p2, out var a, out var b, out var u);
        var h = Cam16Math.HueDegrees(a, b);

        // Step 5: eccentricity
        var et = Cam16Hue.ComputeEccentricity(h);

        // Step 6*: achromatic response
        var A = p2 * vc.Nbb;

        // Step 7: lightness
        var J = 100.0 * Math.Pow(A / vc.Aw, vc.C * vc.Z);

        // Step 8: brightness
        var Q = (4.0 / vc.C) * Math.Sqrt(Math.Max(J, 0.0) / 100.0) * (vc.Aw + 4.0) * Math.Pow(vc.FL, 0.25);

        // Step 9*: t, alpha, C, M, s
        var t = (50000.0 / 13.0) * vc.Nc * vc.Ncb * et * Math.Sqrt(a * a + b * b) / (u + 0.305);
        var alpha = Math.Pow(t, 0.9) * Math.Pow(1.64 - Math.Pow(0.29, vc.N), 0.73);
        var C = alpha * Math.Sqrt(Math.Max(J, 0.0) / 100.0);
        var M = C * Math.Pow(vc.FL, 0.25);
        var s = 50.0 * Math.Sqrt((alpha * vc.C) / (vc.Aw + 4.0));

        return new Cam16Color(
            Cam16Math.SafeFinite(J),
            Cam16Math.SafeNonNegative(C),
            h,
            Cam16Math.SafeNonNegative(M),
            Cam16Math.SafeNonNegative(s),
            Cam16Math.SafeNonNegative(Q));
    }

    /// <inheritdoc />
    public XyzColor ToXyz(Cam16Color input, IViewingConditions vc)
    {
        // Step 1: get J and t from available correlates
        var J = input.J;
        if (!double.IsFinite(J) || J < 0.0)
            J = 6.25 * vc.C * input.Q / ((vc.Aw + 4.0) * Math.Pow(vc.FL, 0.25));

        var alpha = 0.0;
        if (input.M > 0.0) // from M
        {
            var C = input.M / Math.Pow(vc.FL, 0.25);
            alpha = J > 0.0 ? C / Math.Sqrt(J / 100.0) : 0.0;
        }
        else if (input.C > 0.0) // from C
        {
            alpha = J > 0.0 ? input.C / Math.Sqrt(J / 100.0) : 0.0;
        }
        else if (input.S > 0.0) // from s
        {
            alpha = Math.Pow(input.S / 50.0, 2.0) * (vc.Aw + 4.0) / vc.C;
        }

        var t = alpha > 0.0
            ? Math.Pow(alpha / Math.Pow(1.64 - Math.Pow(0.29, vc.N), 0.73), 1.0 / 0.9)
            : 0.0;

        var h = input.Hue;

        // Step 2*: e_t, A, p1', p2'
        var et = 0.25 * (Math.Cos((h * Math.PI / 180.0) + 2.0) + 3.8);
        var A  = vc.Aw * Math.Pow(Math.Max(J, 0.0) / 100.0, 1.0 / (vc.C * vc.Z));
        var p1 = (50000.0 / 13.0) * vc.Nc * vc.Ncb * et;
        var p2 = A / vc.Nbb;

        // Step 3*: γ, a, b
        var cosh = Math.Cos(h * Math.PI / 180.0);
        var sinh = Math.Sin(h * Math.PI / 180.0);
        var denom = 23.0 * p1 + 11.0 * t * cosh + 108.0 * t * sinh;
        var gamma = denom == 0.0 ? 0.0 : (23.0 * (p2 + 0.305) * t) / denom;
        var aVal = gamma * cosh;
        var bVal = gamma * sinh;

        // Step 4: R'a, G'a, B'a
        var R_a = (460.0 * p2 + 451.0 * aVal + 288.0 * bVal) / 1403.0;
        var G_a = (460.0 * p2 - 891.0 * aVal - 261.0 * bVal) / 1403.0;
        var B_a = (460.0 * p2 - 220.0 * aVal - 6300.0 * bVal) / 1403.0;

        // Step 5*: inverse response compression
        var (Rc, Gc, Bc) = Cam16Math.ResponseExpansion(R_a, G_a, B_a, vc.FL);

        // Step 6: undo D-adaptation
        var (Rw, Gw, Bw) = Cam16Math.M16_XyzToRgb(vc.White.X, vc.White.Y, vc.White.Z);
        var DR = Cam16Math.DFactor(vc.D, vc.White.Y, Rw);
        var DG = Cam16Math.DFactor(vc.D, vc.White.Y, Gw);
        var DB = Cam16Math.DFactor(vc.D, vc.White.Y, Bw);
        var R = Rc / DR; var G = Gc / DG; var B = Bc / DB;

        // Step 7: RGB → XYZ
        var (X, Y, Z) = Cam16Math.M16_RgbToXyz(R, G, B);
        return new XyzColor(X, Y, Z);
    }

    /// <inheritdoc />
    public Cam16UcsColor ToUcs(Cam16Color cam)
    {
        var JStar = (1.0 + 100.0 * 0.007) * cam.J / (1.0 + 0.007 * cam.J);
        var Mp = Math.Log(1.0 + 0.0228 * cam.M) / 0.0228;
        var aStar = Mp * Math.Cos(cam.Hue * Math.PI / 180.0);
        var bStar = Mp * Math.Sin(cam.Hue * Math.PI / 180.0);
        return new Cam16UcsColor(JStar, aStar, bStar);
    }

    /// <inheritdoc />
    public Cam16Color FromUcs(Cam16UcsColor ucs, double hueDegrees, IViewingConditions vc)
    {
        var J = (ucs.JStar - 1.0) / (100.0 * 0.007 - 0.007 * (ucs.JStar - 1.0));
        var Mp = Math.Sqrt(ucs.AStar * ucs.AStar + ucs.BStar * ucs.BStar);
        var M  = (Math.Exp(0.0228 * Mp) - 1.0) / 0.0228;
        var C  = M / Math.Pow(vc.FL, 0.25);
        var Q  = (4.0 / vc.C) * Math.Sqrt(Math.Max(J,0.0) / 100.0) * (vc.Aw + 4.0) * Math.Pow(vc.FL, 0.25);
        var alpha = J > 0.0 ? C / Math.Sqrt(J / 100.0) : 0.0;
        var s  = 50.0 * Math.Sqrt((alpha * vc.C) / (vc.Aw + 4.0));
        return new Cam16Color(J, C, hueDegrees, M, s, Q);
    }
}