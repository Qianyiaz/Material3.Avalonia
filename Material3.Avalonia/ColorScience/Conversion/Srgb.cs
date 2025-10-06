namespace Material3.Avalonia.ColorScience.Conversion;

internal static class Srgb
{
    public static (double r, double g, double b) ToLinear(byte R, byte G, byte B)
    {
        static double U(byte u)
        {
            double s = u / 255.0;
            return s <= 0.04045 ? s / 12.92 : Math.Pow((s + 0.055) / 1.055, 2.4);
        }
        return (U(R), U(G), U(B));
    }

    public static (byte R, byte G, byte B) FromLinear(double r, double g, double b)
    {
        static byte Q(double v)
        {
            v = v < 0 ? 0 : (v > 1 ? 1 : v);
            double s = v <= 0.0031308 ? 12.92 * v : 1.055 * Math.Pow(v, 1.0 / 2.4) - 0.055;
            int ui = (int)Math.Round(s * 255.0, MidpointRounding.AwayFromZero);
            return (byte)(ui < 0 ? 0 : (ui > 255 ? 255 : ui));
        }
        return (Q(r), Q(g), Q(b));
    }

    // sRGB (D65) linear RGB ↔ XYZ (relative, Y_n=100)
    private const double Xr = 95.047, Yr = 100.0, Zr = 108.883;

    public static (double X, double Y, double Z) LinearRgbToXyz(double r, double g, double b)
    {
        // IEC 61966-2-1 with D65 white, scaled to Y=100
        double X = (0.4124564 * r + 0.3575761 * g + 0.1804375 * b) * 100.0;
        double Y = (0.2126729 * r + 0.7151522 * g + 0.0721750 * b) * 100.0;
        double Z = (0.0193339 * r + 0.1191920 * g + 0.9503041 * b) * 100.0;
        return (X, Y, Z);
    }

    public static (double r, double g, double b) XyzToLinearRgb(double X, double Y, double Z)
    {
        X /= 100.0; Y /= 100.0; Z /= 100.0;
        double r =  3.2404542 * X - 1.5371385 * Y - 0.4985314 * Z;
        double g = -0.9692660 * X + 1.8760108 * Y + 0.0415560 * Z;
        double b =  0.0556434 * X - 0.2040259 * Y + 1.0572252 * Z;
        return (r, g, b);
    }
}