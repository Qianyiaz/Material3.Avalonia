namespace Material3.Avalonia.Colors.Core;

/// <summary>
/// HCT color defined by Hue (0..360), Chroma (>=0), and Tone (L* 0..100).
/// Built upon CAM16 (for hue/chroma) and CIE L* (for tone).
/// </summary>
public readonly struct HctColor(double hue, double chroma, double tone)
{
    /// <summary>Hue angle in degrees [0, 360).</summary>
    public double Hue { get; } = ((hue % 360.0) + 360.0) % 360.0;

    /// <summary>Chroma (>= 0).</summary>
    public double Chroma { get; } = chroma < 0 ? 0 : chroma;

    /// <summary>Tone (CIE L*, 0..100).</summary>
    public double Tone { get; } = tone < 0 ? 0 : (tone > 100 ? 100 : tone);

    public override string ToString() => $"HCT(h={Hue:F2}, c={Chroma:F2}, t(L*)={Tone:F2})";
}