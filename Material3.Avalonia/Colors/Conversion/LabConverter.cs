using Material3.Avalonia.Colors.Core;

namespace Material3.Avalonia.Colors.Conversion;

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
        var (R, G, B) = UnpackRgb(argb);
        var (r, g, b) = Srgb.ToLinear(R, G, B);
        var (X, Y, Z) = Srgb.LinearRgbToXyz(r, g, b); // D65, relative (Y≈100)
        return XyzToLab(new XyzColor(X, Y, Z), ConvenienceWhite);
    }
    
    /// <inheritdoc/>
    public uint LabToArgb(LabColor lab)
    {
        var xyz = LabToXyz(lab, ConvenienceWhite);
        var (r, g, b) = Srgb.XyzToLinearRgb(xyz.X, xyz.Y, xyz.Z);
        if (!InGamut(r, g, b))
        {
            // simple clipping; gamut mapping strategies can be added later
            r = Clamp01(r); g = Clamp01(g); b = Clamp01(b);
        }
        var (R, G, B) = Srgb.FromLinear(r, g, b);
        return PackRgb(R, G, B);
    }
    
    private static bool InGamut(double r, double g, double b)
        => r >= 0.0 && r <= 1.0 && g >= 0.0 && g <= 1.0 && b >= 0.0 && b <= 1.0;
    
    private static double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
    
    private static (byte R, byte G, byte B) UnpackRgb(uint argb)
        => ((byte)((argb >> 16) & 0xFF), (byte)((argb >> 8) & 0xFF), (byte)(argb & 0xFF));
    
    private static uint PackRgb(byte R, byte G, byte B)
        => 0xFF000000u | ((uint)R << 16) | ((uint)G << 8) | B;
}
