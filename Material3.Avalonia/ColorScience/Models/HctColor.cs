namespace Material3.Avalonia.ColorScience.Models;

/// <summary>
/// HCT color defined by Hue (0..360), Chroma (>=0), and Tone (L* 0..100).
/// Built upon CAM16 (for hue/chroma) and CIE L* (for tone).
/// </summary>
public readonly struct HctColor(double hue, double chroma, double tone)
{
    /// <summary>Hue angle in degrees [0, 360).</summary>
    public double Hue { get; } = ColorScience.Hue.Normalize(hue);

    /// <summary>Chroma (>= 0).</summary>
    public double Chroma { get; } = Math.Max(0, chroma);

    /// <summary>Tone (CIE L*, 0..100).</summary>
    public double Tone { get; } = Math.Clamp(tone, 0.0, 100.0);

    public override string ToString() => $"HCT(h={Hue:F2}, c={Chroma:F2}, t(L*)={Tone:F2})";
}