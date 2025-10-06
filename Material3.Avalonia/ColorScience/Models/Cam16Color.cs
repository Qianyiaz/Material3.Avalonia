namespace Material3.Avalonia.ColorScience.Models;

/// <summary>CAM16 correlates container.</summary>
public readonly struct Cam16Color(double j, double c, double hue, double m, double s, double q)
{
    public double J { get; } = j;
    public double C { get; } = c;
    public double Hue { get; } = ColorScience.Hue.Normalize(hue);
    public double M { get; } = m;
    public double S { get; } = s;
    public double Q { get; } = q;

    public override string ToString() => $"CAM16(J={J:F4}, C={C:F4}, h={Hue:F4}, M={M:F4}, s={S:F4}, Q={Q:F4})";
}
