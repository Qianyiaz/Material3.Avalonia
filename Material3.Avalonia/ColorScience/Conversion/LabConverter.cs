using Material3.Avalonia.ColorScience.Models;
using Material3.Avalonia.ColorScience.Viewing;

namespace Material3.Avalonia.ColorScience.Conversion;

/// <summary>
/// Canonical CIELAB implementation with robust piecewise functions.
/// XYZ scale is relative with Y_n = 100 (matches <see cref="WhitePoint.D65"/> and <see cref="Srgb"/> matrices).
/// </summary>
public sealed class LabConverter(XyzColor convenienceWhite) : ILabConverter
{
    /// <summary>Default instance using D65 as ARGB convenience white.</summary>
    public static LabConverter Default { get; } = new(WhitePoint.D65);
    
    /// <summary>Reference white used by ARGB convenience helpers.</summary>
    public XyzColor ConvenienceWhite { get; } = convenienceWhite;
    
    // CIE constants in rational form for numeric stability
    private const double Delta = 6.0 / 29.0;
    private const double Delta3 = Delta * Delta * Delta;
    
    /// <inheritdoc/>
    public LabColor XyzToLab(XyzColor xyz, XyzColor white)
    {
        var fx = F(xyz.X / white.X);
        var fy = F(xyz.Y / white.Y);
        var fz = F(xyz.Z / white.Z);

        var L = 116.0 * fy - 16.0;
        var a = 500.0 * (fx - fy);
        var b = 200.0 * (fy - fz);
        return new LabColor(L, a, b);
    }

    private double F(double t) => 
        t > Delta3 ? Math.Cbrt(t) : 841.0 / 108.0 * t + 4.0 / 29.0;
    
    /// <inheritdoc/>
    public XyzColor LabToXyz(LabColor lab, XyzColor white)
    {
        var fy = (lab.L + 16.0) / 116.0;
        var fx = fy + lab.A / 500.0;
        var fz = fy - lab.B / 200.0;
        
        var xr = Finv(fx);
        var yr = Finv(fy);
        var zr = Finv(fz);
        
        return new XyzColor(
            xr * white.X,
            yr * white.Y,
            zr * white.Z);
    }

    private double Finv(double ft) =>
        ft > Delta ? ft * ft * ft : (108.0 / 841.0) * (ft - 4.0 / 29.0);
    
    /// <inheritdoc/>
    public LabColor ArgbToLab(uint argb)
    {
        var (R, G, B) = Argb32.UnpackRgb(argb);
        var (r, g, b) = Srgb.ToLinear(R, G, B);
        var (X, Y, Z) = Srgb.LinearRgbToXyz(r, g, b); // D65, relative (Y≈100)
        return XyzToLab(new XyzColor(X, Y, Z), ConvenienceWhite);
    }
    
    /// <inheritdoc/>
    public uint LabToArgb(LabColor lab)
    {
        var xyz = LabToXyz(lab, ConvenienceWhite);
        var (r, g, b) = Srgb.XyzToLinearRgb(xyz.X, xyz.Y, xyz.Z);
        if (!RgbGamut.InLinearGamut(r, g, b))
        {
            // simple clipping; gamut mapping strategies can be added later
            r = Math.Clamp(r, 0.0, 1.0); g = Math.Clamp(g, 0.0, 1.0); b = Math.Clamp(b, 0.0, 1.0);
        }
        var (R, G, B) = Srgb.FromLinear(r, g, b);
        return Argb32.PackRgb(R, G, B);
    }
}
