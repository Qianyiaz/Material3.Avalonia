namespace Material3.Avalonia.ColorScience.Models;

public readonly struct XyzColor(double x, double y, double z)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double Z { get; } = z;
    
    public override string ToString() => $"XYZ({X:F6}, {Y:F6}, {Z:F6})";
}
