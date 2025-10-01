namespace Material3.Avalonia.Colors.Core;

/// <summary>
/// Default implementation of viewing conditions with all derived constants.
/// </summary>
public sealed class ViewingConditions(
    XyzColor white,
    double la,
    double yb,
    double f,
    double c,
    double nc,
    double d,
    double fl,
    double n,
    double nbb,
    double ncb,
    double z,
    (double Ra, double Ga, double Ba) whitePost,
    double aw)
    : IViewingConditions
{
    public XyzColor White { get; } = white;
    public double La { get; } = la;
    public double Yb { get; } = yb;
    public double F { get; } = f;
    public double C { get; } = c;
    public double Nc { get; } = nc;
    public double D { get; } = d;
    public double FL { get; } = fl;
    public double N { get; } = n;
    public double Nbb { get; } = nbb;
    public double Ncb { get; } = ncb;
    public double Z { get; } = z;
    public (double Ra, double Ga, double Ba) WhitePostAdapt { get; } = whitePost;
    public double Aw { get; } = aw;
}