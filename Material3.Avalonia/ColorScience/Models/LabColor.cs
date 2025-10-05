namespace Material3.Avalonia.ColorScience.Models;

/// <summary>
/// Immutable CIELAB container. No implicit clamping; consumers handle bounds.
/// </summary>
public readonly struct LabColor(double l, double a, double b)
{
    public double L { get; } = l;
    public double A { get; } = a;
    public double B { get; } = b;
    
    public override string ToString() => $"Lab(L={L:F4}, a={A:F4}, b={B:F4})";
}