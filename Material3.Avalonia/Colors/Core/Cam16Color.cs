namespace Material3.Avalonia.Colors.Core;

/// <summary>CAM16 correlates container.</summary>
public readonly struct Cam16Color(double j, double c, double hue, double m, double s, double q)
{
    public double J { get; } = j;
    public double C { get; } = c;
    public double Hue { get; } = NormalizeHue(hue); // degrees [0,360)
    public double M { get; } = m;
    public double S { get; } = s;
    public double Q { get; } = q;

    private static double NormalizeHue(double h) => (h % 360.0 + 360.0) % 360.0;
    public override string ToString() => $"CAM16(J={J:F4}, C={C:F4}, h={Hue:F4}, M={M:F4}, s={S:F4}, Q={Q:F4})";
}
