namespace Material3.Avalonia.Colors.Core;

public readonly struct XyzColor(double x, double y, double z)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double Z { get; } = z;
    
    public override string ToString() => $"XYZ({X:F6}, {Y:F6}, {Z:F6})";
}

/// <summary>Common reference whites (relative; Y=100).</summary>
public static class WhitePoint
{
    public static readonly XyzColor D65 = new(95.047, 100.000, 108.883);
}
