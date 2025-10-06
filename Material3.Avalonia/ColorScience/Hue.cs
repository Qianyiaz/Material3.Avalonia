namespace Material3.Avalonia.ColorScience;

internal static class Hue
{
    public static double Normalize(double hue)
    {
        if (!double.IsFinite(hue)) return 0.0;
        hue %= 360.0;
        return hue < 0 ? hue + 360.0 : hue;
    }

    public static double Add(double hue, double delta) => Normalize(hue + delta);

    public static double Distance(double a, double b)
    {
        var d = Math.Abs(Normalize(a) - Normalize(b));
        return d > 180 ? 360 - d : d;
    }
}