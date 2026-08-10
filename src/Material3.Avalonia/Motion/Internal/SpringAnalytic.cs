namespace Material3.Avalonia.Motion.Internal;

internal static class SpringAnalytic
{
    public static double Evaluate(double tSeconds, double stiffness, double damping, double initialVelocity)
    {
        var w0 = Math.Sqrt(Math.Max(1e-9, stiffness));
        var z = Math.Max(0, damping);
        var v0 = initialVelocity;

        if (z < 1.0)
        {
            var wd = w0 * Math.Sqrt(1.0 - z * z);
            var e = Math.Exp(-z * w0 * tSeconds);
            var A = 1.0;
            var B = (z * w0 * A + v0) / wd;
            var x = e * (A * Math.Cos(wd * tSeconds) + B * Math.Sin(wd * tSeconds));
            return 1.0 - x;
        }
        else if (Math.Abs(z - 1.0) < 1e-6)
        {
            var e = Math.Exp(-w0 * tSeconds);
            var A = 1.0;
            var B = v0 + w0 * A;
            var x = (A + B * tSeconds) * e;
            return 1.0 - x;
        }
        else
        {
            var s = Math.Sqrt(z * z - 1.0);
            var r1 = -w0 * (z - s);
            var r2 = -w0 * (z + s);
            var C2 = (v0 - r1) / (r2 - r1);
            var C1 = 1 - C2;
            var x = C1 * Math.Exp(r1 * tSeconds) + C2 * Math.Exp(r2 * tSeconds);
            return 1.0 - x;
        }
    }
}