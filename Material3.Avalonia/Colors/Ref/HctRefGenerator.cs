using Avalonia.Controls;
using Avalonia.Media;
using Material3.Avalonia.Colors.Conversion;
using Material3.Avalonia.Colors.Core;

namespace Material3.Avalonia.Colors.Ref;

/// <summary>
/// Generates Material Design 3 reference tonal palettes (Primary, Secondary, Tertiary,
/// Neutral, NeutralVariant, Error) from a seed sRGB color using HCT.
/// </summary>
/// <remarks>
/// MD3 intent implemented as follows:
/// - Primary: seed hue, high chroma (>= 48 or seed chroma if larger).
/// - Secondary: seed hue, reduced chroma (~16).
/// - Tertiary: hue rotated by +60°, moderate chroma (~24).
/// - Neutral / NeutralVariant: seed hue, very low chroma (~6 / ~8).
/// - Error: fixed red sector (~25°), noticeable chroma (~32).
/// Tones are taken from the standard tone grid available in <see cref="TonalPalettes"/>.
/// </remarks>
public static class HctRefGenerator
{
    // Canonical MD3-like chroma targets
    private const double PrimaryMaxChroma = 48.0; //48.0;
    private const double SecondaryChroma  = 16.0;
    private const double TertiaryChroma   = 24.0;
    private const double NeutralChroma    = 4.0; //6.0;
    private const double NeutralVarChroma = 8.0;
    private const double ErrorChroma      = 84.0;

    // Canonical hue choices
    private const double TertiaryHueDelta = 60.0;
    private const double ErrorHue         = 25.0;

    /// <summary>
    /// Builds all reference palettes from a seed sRGB color.
    /// </summary>
    /// <param name="seed">Seed color in sRGB (ARGB 8:8:8:8).</param>
    /// <param name="vcFactory">Viewing-conditions factory; defaults to sRGB/Average.</param>
    /// <returns>Aggregated reference palettes: Primary, Secondary, Tertiary, Neutral, NeutralVariant, Error.</returns>
    public static TonalPalettes GenerateFromSeed(
        Color seed,
        IViewingConditionsFactory? vcFactory = null)
    {
        uint argb = seed.ToUInt32();
        return GenerateFromSeed(argb, vcFactory);
    }

    /// <summary>
    /// Builds all reference palettes from a seed sRGB color.
    /// </summary>
    /// <param name="seedArgb">Seed color in ARGB (0xAARRGGBB).</param>
    /// <param name="vcFactory">Viewing-conditions factory; defaults to sRGB/Average.</param>
    /// <returns>Aggregated reference palettes: Primary, Secondary, Tertiary, Neutral, NeutralVariant, Error.</returns>
    public static TonalPalettes GenerateFromSeed(
        uint seedArgb,
        IViewingConditionsFactory? vcFactory = null)
    {
        vcFactory ??= new ViewingConditionsFactory();
        var vc = vcFactory.SrgbAverage;

        var hctConverter = new HctConverter(vc);
        var seedHct = hctConverter.ArgbToHct(seedArgb);

        double hSeed = seedHct.Hue;
        double cSeed = seedHct.Chroma;

        // Target chromas
        double cPri = Math.Max(PrimaryMaxChroma, cSeed);
        double cSec = SecondaryChroma;
        double cTer = TertiaryChroma;
        double cNeu = NeutralChroma;
        double cNuv = NeutralVarChroma;

        // Target hues
        double hPri = hSeed;
        double hSec = hSeed;
        double hTer = NormalizeHue(hSeed + TertiaryHueDelta);
        double hNeu = hSeed;
        double hNuv = hSeed;
        double hErr = ErrorHue;

        // Build palettes on the canonical tone grid
        int[] tones = Enumerable.Range(0, 101).ToArray(); // expected to exist in your codebase
        var primary   = BuildPalette(PaletteKind.Primary, hctConverter, hPri, cPri, tones);
        var secondary = BuildPalette(PaletteKind.Secondary, hctConverter, hSec, cSec, tones);
        var tertiary  = BuildPalette(PaletteKind.Tertiary, hctConverter, hTer, cTer, tones);
        var neutral   = BuildPalette(PaletteKind.Neutral, hctConverter, hNeu, cNeu, tones);
        var neutralV  = BuildPalette(PaletteKind.NeutralVariant, hctConverter, hNuv, cNuv, tones);
        var error     = BuildPalette(PaletteKind.Error, hctConverter, hErr, ErrorChroma, tones);

        return new TonalPalettes(primary, secondary, tertiary, neutral, neutralV, error);
    }

    /// <summary>
    /// Convenience API that returns both color and brush resources keyed as MdRef… for direct use in Avalonia.
    /// </summary>
    /// <param name="seed">Seed color in sRGB.</param>
    /// <param name="vcFactory">Viewing-conditions factory; defaults to sRGB/Average.</param>
    /// <returns>(colors, brushes) resource dictionaries.</returns>
    public static (ResourceDictionary Colors, ResourceDictionary Brushes)
        GenerateRefResources(Color seed, IViewingConditionsFactory? vcFactory = null)
    {
        var refs = GenerateFromSeed(seed, vcFactory);
        var colorRd = refs.ToColorResourceDictionary();
        var brushRd = refs.ToBrushResourceDictionary();
        return (colorRd, brushRd);
    }

    private static TonalPalette BuildPalette(PaletteKind kind, IHctConverter hct, double hue, double chroma, int[] tones)
    {
        var entries = new Dictionary<int, Color>(tones.Length);
        foreach (var t in tones)
        {
            uint argb = hct.HctToArgb(hue, chroma, t);
            entries[t] = Color.FromUInt32(argb);
        }
        return new TonalPalette(kind, entries);
    }

    private static double NormalizeHue(double h) => (h % 360.0 + 360.0) % 360.0;
}