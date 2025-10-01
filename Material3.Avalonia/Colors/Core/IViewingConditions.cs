namespace Material3.Avalonia.Colors.Core;

/// <summary>
/// Viewing conditions for CAM16 conversions.
/// </summary>
public interface IViewingConditions
{
    XyzColor White { get; }
    double La { get; }
    double Yb { get; }
    double F { get; }
    double C { get; }
    double Nc { get; }
    double D { get; }
    double FL { get; }
    double N { get; }
    double Nbb { get; }
    double Ncb { get; }
    double Z { get; }

    /// <summary>Post-adaptation compressed cone responses for the adopted white.</summary>
    (double Ra, double Ga, double Ba) WhitePostAdapt { get; }
    /// <summary>Achromatic response for the adopted white.</summary>
    double Aw { get; }
}