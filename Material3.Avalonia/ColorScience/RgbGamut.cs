namespace Material3.Avalonia.ColorScience;

internal static class RgbGamut
{
    public static bool InLinearGamut(double r, double g, double b, double tolerance = 0.0) =>
        r >= -tolerance && r <= 1.0 + tolerance &&
        g >= -tolerance && g <= 1.0 + tolerance &&
        b >= -tolerance && b <= 1.0 + tolerance;
}