using Avalonia.Controls;
using Avalonia.Media;

namespace Material3.Avalonia.Colors.Ref;

/// <summary>
/// Container for five tonal palettes (Primary, Secondary, Tertiary, Neutral, NeutralVariant, Error)
/// and helpers to export them into Avalonia resource dictionaries using Material 3 reference keys:
/// "MdRef{PaletteName}{Tone}Color/Brush".
/// </summary>
public sealed class TonalPalettes(
    TonalPalette primary,
    TonalPalette secondary,
    TonalPalette tertiary,
    TonalPalette neutral,
    TonalPalette neutralVariant,
    TonalPalette error)
{
    /// <summary>Primary tonal palette.</summary>
    public TonalPalette Primary { get; } = primary;

    /// <summary>Secondary tonal palette.</summary>
    public TonalPalette Secondary { get; } = secondary;

    /// <summary>Tertiary tonal palette.</summary>
    public TonalPalette Tertiary { get; } = tertiary;

    /// <summary>Neutral tonal palette (surfaces).</summary>
    public TonalPalette Neutral { get; } = neutral;

    /// <summary>Neutral-variant tonal palette (outlines/dividers).</summary>
    public TonalPalette NeutralVariant { get; } = neutralVariant;
    
    /// <summary>Error tonal palette.</summary>
    public TonalPalette Error { get; } = error;

    /// <summary>
    /// Resolves an sRGB <see cref="Color"/> from the given <paramref name="palette"/>
    /// at the specified <paramref name="tone"/> (0..100).
    /// </summary>
    /// <param name="palette">Palette kind (Primary, Secondary, etc.).</param>
    /// <param name="tone">Tone value (0..100). Value is clamped to the valid range.</param>
    /// <returns>Resolved sRGB color.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="palette"/> is unknown.</exception>
    public Color Resolve(PaletteKind palette, int tone)
        => GetPalette(palette)[ClampTone(tone)];

    /// <summary>
    /// Returns the underlying <see cref="TonalPalette"/> for the given <paramref name="palette"/> kind.
    /// </summary>
    /// <param name="palette">Palette kind.</param>
    /// <returns>The corresponding tonal palette.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="palette"/> is unknown.</exception>
    public TonalPalette GetPalette(PaletteKind palette) => palette switch
    {
        PaletteKind.Primary => Primary,
        PaletteKind.Secondary => Secondary,
        PaletteKind.Tertiary => Tertiary,
        PaletteKind.Neutral => Neutral,
        PaletteKind.NeutralVariant => NeutralVariant,
        PaletteKind.Error => Error,
        _ => throw new ArgumentOutOfRangeException(nameof(palette), palette, "Unknown palette kind.")
    };

    /// <summary>
    /// Indexer sugar for resolving color by kind and tone.
    /// </summary>
    /// <param name="palette">Palette kind.</param>
    /// <param name="tone">Tone value (0..100). Value is clamped.</param>
    public Color this[PaletteKind palette, int tone] => Resolve(palette, tone);

    private static int ClampTone(int tone) => tone < 0 ? 0 : (tone > 100 ? 100 : tone);

    /// <summary>
    /// Writes all reference tokens as <see cref="Color"/> resources into <paramref name="target"/>.
    /// Keys follow "MdRef{Palette}{Tone}Color".
    /// </summary>
    public void WriteRefColorsTo(ResourceDictionary target)
    {
        Write(Primary);
        Write(Secondary);
        Write(Tertiary);
        Write(Neutral);
        Write(NeutralVariant);
        Write(Error);
        return;

        void Write(TonalPalette p)
        {
            foreach (var (tone, color) in p.Enumerate())
                target[RefKeys.ColorKey(p.Kind, tone)] = color;
        }
    }

    /// <summary>
    /// Writes all reference tokens as <see cref="SolidColorBrush"/> resources into <paramref name="target"/>.
    /// Keys follow "MdRef{Palette}{Tone}Brush". Brush is created over the current color value.
    /// </summary>
    public void WriteRefBrushesTo(ResourceDictionary target)
    {
        Write(Primary);
        Write(Secondary);
        Write(Tertiary);
        Write(Neutral);
        Write(NeutralVariant);
        Write(Error);
        return;

        void Write(TonalPalette p)
        {
            foreach (var (tone, color) in p.Enumerate())
                target[RefKeys.BrushKey(p.Kind, tone)] = new SolidColorBrush(color);
        }
    }

    /// <summary>
    /// Creates a <see cref="ResourceDictionary"/> with all "MdRef…Color" entries.
    /// </summary>
    public ResourceDictionary ToColorResourceDictionary()
    {
        var dict = new ResourceDictionary();
        WriteRefColorsTo(dict);
        return dict;
    }

    /// <summary>
    /// Creates a <see cref="ResourceDictionary"/> with all "MdRef…Brush" entries.
    /// </summary>
    public ResourceDictionary ToBrushResourceDictionary()
    {
        var dict = new ResourceDictionary();
        WriteRefBrushesTo(dict);
        return dict;
    }
    
    /// <summary>
    /// Enumerates all (resourceKey, color) pairs that would be exported as "MdRef…Color".
    /// </summary>
    public IEnumerable<(string key, Color color)> EnumerateRefColorEntries()
    {
        foreach (var x in Dump(Primary)) yield return x;
        foreach (var x in Dump(Secondary)) yield return x;
        foreach (var x in Dump(Tertiary)) yield return x;
        foreach (var x in Dump(Neutral)) yield return x;
        foreach (var x in Dump(NeutralVariant)) yield return x;
        foreach (var x in Dump(Error)) yield return x;
        yield break;

        static IEnumerable<(string, Color)> Dump(TonalPalette p)
        {
            foreach (var kv in p.Enumerate())
                yield return (RefKeys.ColorKey(p.Kind, kv.Key), kv.Value);
        }
    }
    
    // ---------- Factories ----------

    /// <summary>
    /// Creates a <see cref="TonalPalettes"/> from raw dictionaries.
    /// Intended for dynamic (HCT) generation.
    /// </summary>
    public static TonalPalettes FromDictionaries(
        IDictionary<int, Color> primary,
        IDictionary<int, Color> secondary,
        IDictionary<int, Color> tertiary,
        IDictionary<int, Color> neutral,
        IDictionary<int, Color> neutralVariant,
        IDictionary<int, Color> error)
        => new(
            new TonalPalette(PaletteKind.Primary, primary),
            new TonalPalette(PaletteKind.Secondary, secondary),
            new TonalPalette(PaletteKind.Tertiary, tertiary),
            new TonalPalette(PaletteKind.Neutral, neutral),
            new TonalPalette(PaletteKind.NeutralVariant, neutralVariant),
            new TonalPalette(PaletteKind.Error, error)
        );
}
