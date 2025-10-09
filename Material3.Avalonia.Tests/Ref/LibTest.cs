using Avalonia.Media;
using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using FluentAssertions;
using Material3.Avalonia.Tests.Ref;

public enum PaletteKind { Primary, Secondary, Tertiary, Neutral, NeutralVariant, Error }

public sealed class RefPalettes
{
    public TonalPalette Primary { get; }
    public TonalPalette Secondary { get; }
    public TonalPalette Tertiary { get; }
    public TonalPalette Neutral { get; }
    public TonalPalette NeutralVariant { get; }
    public TonalPalette Error { get; }

    public RefPalettes(TonalPalette primary, TonalPalette secondary, TonalPalette tertiary,
                       TonalPalette neutral, TonalPalette neutralVariant, TonalPalette error)
        => (Primary, Secondary, Tertiary, Neutral, NeutralVariant, Error)
           = (primary, secondary, tertiary, neutral, neutralVariant, error);

    public Color Resolve(PaletteKind kind, int tone)
    {
        uint argb = kind switch
        {
            PaletteKind.Primary        => Primary.Tone((uint)tone),
            PaletteKind.Secondary      => Secondary.Tone((uint)tone),
            PaletteKind.Tertiary       => Tertiary.Tone((uint)tone),
            PaletteKind.Neutral        => Neutral.Tone((uint)tone),
            PaletteKind.NeutralVariant => NeutralVariant.Tone((uint)tone),
            PaletteKind.Error          => Error.Tone((uint)tone),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
        return FromArgb(argb);
    }

    private static Color FromArgb(uint argb)
        => Color.FromArgb(
            (byte)((argb & 0xFF000000) >> 24),
            (byte)((argb & 0x00FF0000) >> 16),
            (byte)((argb & 0x0000FF00) >> 8),
            (byte)( argb & 0x000000FF));
}

public static class HctRefGenerator
{
    /// <summary>
    /// Генерирует "референсные" палитры из seed. Значения chroma подобраны под
    /// Material baseline. ВАЖНО: Neutral принудительно chroma≈4 для совпадения с твоим BaselineData.
    /// </summary>
    public static RefPalettes GenerateFromSeed(Color seed)
    {
        // HCT исходного цвета
        var seedArgb = ((uint)seed.A << 24) | ((uint)seed.R << 16) | ((uint)seed.G << 8) | seed.B;
        var hct = Hct.FromInt(seedArgb); // API: Hct.FromInt(uint) :contentReference[oaicite:7]{index=7}
        var hue = hct.Hue;

        // Primary: тональное семейство по самому seed (тонал-палитра по hue+chroma)
        var primary = TonalPalette.FromInt(seedArgb); // :contentReference[oaicite:8]{index=8}

        // Secondary: та же hue, пониже chroma (≈16)
        var secondary = TonalPalette.FromHueAndChroma(hue, 16.0); // :contentReference[oaicite:9]{index=9}

        // Tertiary: hue + 60°, chroma ≈24 (как в TonalSpot)
        var tertiary  = TonalPalette.FromHueAndChroma((hue + 60.0) % 360.0, 24.0); // :contentReference[oaicite:10]{index=10}

        // Neutral: исторический baseline ≈4 (а не 6)
        var neutral   = TonalPalette.FromHueAndChroma(hue, 4.0);

        // Neutral Variant: ≈8
        var neutralV  = TonalPalette.FromHueAndChroma(hue, 8.0); // :contentReference[oaicite:11]{index=11}

        // Error: фиксированный hue≈25°, chroma≈84 в Material (палитра ошибок не зависит от brand hue)
        var error     = TonalPalette.FromHueAndChroma(25.0, 84.0);

        return new RefPalettes(primary, secondary, tertiary, neutral, neutralV, error);
    }
}
public class LibTest
{
    /// <summary>
    /// Baseline demos center around the violet brand color (Primary@40 ≈ #6750A4).
    /// Using it as seed should reproduce published baseline refs.
    /// </summary>
    private static readonly Color Seed = Color.Parse("#6750A4");

    /// <summary>
    /// Allow small 8-bit sRGB rounding differences (per channel).
    /// </summary>
    private const int MaxDelta = 2;

    [Fact]
    public void Primary_ShouldMatchBaselineStops()
        => AssertPalette(BaselineData.Primary, PaletteKind.Primary);

    [Fact]
    public void Secondary_ShouldMatchBaselineStops()
        => AssertPalette(BaselineData.Secondary, PaletteKind.Secondary);

    [Fact]
    public void Tertiary_ShouldMatchBaselineStops()
        => AssertPalette(BaselineData.Tertiary, PaletteKind.Tertiary);

    [Fact]
    public void Neutral_ShouldMatchBaselineStops()
        // Historical baseline uses Neutral chroma ≈ 4; set explicitly for exactness.
        => AssertPalette(BaselineData.Neutral, PaletteKind.Neutral);

    [Fact]
    public void NeutralVariant_ShouldMatchBaselineStops()
        => AssertPalette(BaselineData.NeutralVariant, PaletteKind.NeutralVariant);

    [Fact]
    public void Error_ShouldMatchBaselineStops()
        => AssertPalette(BaselineData.Error, PaletteKind.Error);

    private static void AssertPalette(
        IReadOnlyDictionary<int, string> baseline,
        PaletteKind kind)
    {
        // Arrange
        var refs = HctRefGenerator.GenerateFromSeed(Seed);

        // Act & Assert
        foreach (var (tone, hex) in baseline.OrderBy(kv => kv.Key))
        {
            var expected = BaselineData.Parse(hex);
            var actual = refs.Resolve(kind, tone);

            MaxChannelDelta(actual, expected)
                .Should().BeLessThanOrEqualTo(MaxDelta,
                    $"{kind}@{tone} should match baseline within {MaxDelta} per-channel (got {actual} vs {expected})");
        }
    }

    /// <summary>Returns the maximum absolute per-channel sRGB difference.</summary>
    private static int MaxChannelDelta(Color a, Color b)
        => new[] { System.Math.Abs(a.R - b.R), System.Math.Abs(a.G - b.G), System.Math.Abs(a.B - b.B) }.Max();
}