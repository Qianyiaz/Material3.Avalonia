using Material3.Avalonia.Colors.Core;

namespace Material3.Avalonia.Colors.Conversion;

/// <summary>
/// Converter between sRGB (ARGB) and HCT (Hue–Chroma–Tone).
/// HCT is defined as: hue/chroma from CAM16, tone equals CIE L*.
/// </summary>
public interface IHctConverter
{
    /// <summary>
    /// Converts an ARGB sRGB color to HCT using the converter's viewing conditions.
    /// </summary>
    /// <param name="argb">ARGB-packed sRGB color (0xAARRGGBB).</param>
    /// <returns>HCT representation: Hue (deg), Chroma, Tone (L*).</returns>
    HctColor ArgbToHct(uint argb);


    /// <summary>
    /// Converts HCT to an in-gamut ARGB sRGB color.
    /// Preserves hue and tone; chroma may be reduced if necessary to fit sRGB gamut.
    /// </summary>
    /// <param name="hueDegrees">Hue angle in degrees [0, 360).</param>
    /// <param name="chroma">Perceptual chroma (non-negative).</param>
    /// <param name="toneLstar">Tone as CIE L* in [0, 100].</param>
    /// <returns>ARGB-packed sRGB color (0xAARRGGBB).</returns>
    uint HctToArgb(double hueDegrees, double chroma, double toneLstar);


    /// <summary>
    /// Convenience overload that simply calls <see cref="HctToArgb(double,double,double)"/>.
    /// </summary>
    /// <param name="hct">HCT color.</param>
    /// <returns>ARGB-packed sRGB color (0xAARRGGBB).</returns>
    uint HctToArgb(HctColor hct) => HctToArgb(hct.Hue, hct.Chroma, hct.Tone);
}