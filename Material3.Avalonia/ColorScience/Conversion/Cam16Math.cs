using Material3.Avalonia.ColorScience.Models;

namespace Material3.Avalonia.ColorScience.Conversion;

/// <summary>
/// Internal numeric helpers for CAM16 (matrices, response curves, opponents).
/// </summary>
internal static class Cam16Math
{
    // M16: XYZ -> RGB
    private const double M11 =  0.401288, M12 =  0.650173, M13 = -0.051461;
    private const double M21 = -0.250268, M22 =  1.204414, M23 =  0.045854;
    private const double M31 = -0.002079, M32 =  0.048952, M33 =  0.953127;

    // M16^-1: RGB -> XYZ
    private const double IM11 =  1.86206786, IM12 = -1.01125463, IM13 =  0.14918677;
    private const double IM21 =  0.38752654, IM22 =  0.62144744, IM23 = -0.00897398;
    private const double IM31 = -0.01584150, IM32 = -0.03412294, IM33 =  1.04996444;

    public static (double R, double G, double B) M16XyzToRgb(double X, double Y, double Z) =>
        (M11 * X + M12 * Y + M13 * Z,
            M21 * X + M22 * Y + M23 * Z,
            M31 * X + M32 * Y + M33 * Z);

    public static (double X, double Y, double Z) M16RgbToXyz(double R, double G, double B) =>
        (IM11 * R + IM12 * G + IM13 * B,
            IM21 * R + IM22 * G + IM23 * B,
            IM31 * R + IM32 * G + IM33 * B);

    // convenience: cone white components from XYZ white
    public static double Rw(XyzColor white) => M11 * white.X + M12 * white.Y + M13 * white.Z;
    public static double Gw(XyzColor white) => M21 * white.X + M22 * white.Y + M23 * white.Z;
    public static double Bw(XyzColor white) => M31 * white.X + M32 * white.Y + M33 * white.Z;

    /// <summary>Chromatic adaptation coefficient per channel: D_R = D*(Y_w/R_w) - 1 + D.</summary>
    public static double DFactor(double D, double Yw, double Rw) => D * (Yw / Rw) + 1.0 - D;

    /// <summary>
    /// Positive-only response compression (Step 0 for adopted white).
    /// Assumes x >= 0; no sign/abs branches per CAM16 step 0.
    /// </summary>
    public static double ResponseCompressionPositive(double x, double FL)
    {
        var t = Math.Pow(FL * x / 100.0, 0.42);
        return (400.0 * t) / (27.13 + t);
    }
        
    /// <summary>Forward response compression (Step 3*).</summary>
    public static (double Ra, double Ga, double Ba) ResponseCompression(double Rc, double Gc, double Bc, double FL)
    {
        return (F(Rc, FL), F(Gc, FL), F(Bc, FL));

        static double F(double x, double FL)
        {
            var t = Math.Pow(FL * Math.Abs(x) / 100.0, 0.42);
            return Math.Sign(x) * (400.0 * t) / (27.13 + t);
        }
    }

    /// <summary>Inverse response compression (Step 5* inverse part).</summary>
    public static (double Rc, double Gc, double Bc) ResponseExpansion(double Ra, double Ga, double Ba, double FL)
    {
        static double FInv(double x, double FL)
        {
            var s = Math.Sign(x);
            var ax = Math.Abs(x);
            if (ax >= 400.0 - 1e-9) ax = 400.0 - 1e-9; // guard
            var t = (27.13 * ax) / (400.0 - ax);
            var v = Math.Pow(t, 1.0 / 0.42);
            return s * (100.0 / FL) * v;
        }
        return (FInv(Ra, FL), FInv(Ga, FL), FInv(Ba, FL));
    }

    /// <summary>Opponent channels and auxiliaries (Step 4*): p2', a, b, u.</summary>
    public static void Opponents(double Ra, double Ga, double Ba, out double p2, out double a, out double b, out double u)
    {
        p2 = (2.0 * Ra) + Ga + 0.05 * Ba;
        a  = Ra - (12.0 / 11.0) * Ga + (1.0 / 11.0) * Ba;
        b  = (Ra + Ga - 2.0 * Ba) / 9.0;
        u  = Ra + Ga + 1.05 * Ba;
    }

    public static double HueDegrees(double a, double b)
    {
        var h = Math.Atan2(b, a) * 180.0 / Math.PI;
        return h < 0.0 ? h + 360.0 : h;
    }

    public static double SafeFinite(double v) => double.IsFinite(v) ? v : 0.0;
    public static double SafeNonNegative(double v) => (!double.IsFinite(v) || v < 0.0) ? 0.0 : v;
}