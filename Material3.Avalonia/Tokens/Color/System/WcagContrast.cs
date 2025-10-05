namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// WCAG contrast helpers: relative luminance and contrast ratio.
/// </summary>
public static class WcagContrast
{
    /// <summary>Computes relative luminance (0..1) of an sRGB color.</summary>
    public static double RelativeLuminance(global::Avalonia.Media.Color c)
    {
        var r = Chan(c.R);
        var g = Chan(c.G);
        var b = Chan(c.B);
        return 0.2126 * r + 0.7152 * g + 0.0722 * b;

        static double Chan(byte ch)
        {
            var v = ch / 255.0;
            return v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        }
    }

    /// <summary>Computes contrast ratio between two sRGB colors, per WCAG 2.x.</summary>
    public static double Ratio(global::Avalonia.Media.Color a, global::Avalonia.Media.Color b)
    {
        var la = RelativeLuminance(a);
        var lb = RelativeLuminance(b);
        var (max, min) = la > lb ? (la, lb) : (lb, la);
        return (max + 0.05) / (min + 0.05);
    }
}