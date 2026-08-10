namespace Material3.Avalonia.Motion.Internal;

internal static class SpringDuration
{
    private const double Epsilon = 0.01;

    public static double ComputeSeconds(double stiffness, double damping, bool effects)
    {
        var w0 = Math.Sqrt(Math.Max(1e-9, stiffness));
        var z = Math.Max(0, damping);

        var eps = effects ? Epsilon * 0.7 : Epsilon;

        if (z < 1.0)
        {
            return Math.Log(1.0 / eps) / (Math.Max(1e-9, z) * w0);
        }
        else if (Math.Abs(z - 1.0) < 1e-6)
        {
            var baseT = Math.Log(1.0 / eps) / w0;
            return baseT * 1.15;
        }
        else
        {
            var s = Math.Sqrt(z * z - 1.0);
            var r1 = -w0 * (z - s);
            return Math.Log(1.0 / eps) / Math.Abs(r1);
        }
    }
}