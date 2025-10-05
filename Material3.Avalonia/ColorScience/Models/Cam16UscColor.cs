namespace Material3.Avalonia.ColorScience.Models;

/// <summary>CAM16-UCS coordinates (J*, a*, b*).</summary>
public readonly struct Cam16UcsColor(double jStar, double aStar, double bStar)
{
    public double JStar { get; } = jStar;
    public double AStar { get; } = aStar;
    public double BStar { get; } = bStar;

    public override string ToString() => $"UCS(J*={JStar:F4}, a*={AStar:F4}, b*={BStar:F4})";
}