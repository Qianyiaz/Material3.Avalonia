namespace Material3.Avalonia.ColorScience;

internal static class Argb32
{
    public static (byte R, byte G, byte B) UnpackRgb(uint argb) =>
        ((byte)((argb >> 16) & 0xFF), (byte)((argb >> 8) & 0xFF), (byte)(argb & 0xFF));

    public static uint PackRgb(byte R, byte G, byte B) =>
        0xFF000000u | ((uint)R << 16) | ((uint)G << 8) | B;
}