using Avalonia.Media;

namespace Material3.Avalonia.Tokens.Color.Reference;

/// <summary>
/// Represents a tonal palette for a single key color (e.g., Primary).
/// Contains a mapping of tone values (0..100 including 95/98/99) to colors.
/// This layer is reference-only (no roles, no brushes).
/// </summary>
public sealed class TonalPalette
{
    private readonly Dictionary<int, global::Avalonia.Media.Color> _tones;
    
    /// <summary>
    /// Palette kind (Primary, Secondary, Tertiary, Neutral, NeutralVariant).
    /// </summary>
    public PaletteKind Kind { get; }

    /// <summary>
    /// Convenience human-readable name (Kind.ToString()).
    /// Useful for diagnostics and forming resource keys.
    /// </summary>
    public string Name => Kind.ToString();

    /// <summary>
    /// Creates a tonal palette with a set of tone → color pairs.
    /// </summary>
    /// <param name="kind">Palette kind.</param>
    /// <param name="tones">Tone values mapped to <see cref="Color"/>.</param>
    /// <exception cref="ArgumentNullException">If tones is null.</exception>
    public TonalPalette(PaletteKind kind, IDictionary<int, global::Avalonia.Media.Color> tones)
    {
        Kind = kind;
        _tones = new Dictionary<int, global::Avalonia.Media.Color>(tones ?? throw new ArgumentNullException(nameof(tones)));
    }

    /// <summary>Checks whether a color exists for the given tone.</summary>
    public bool ContainsTone(int tone) => _tones.ContainsKey(tone);

    /// <summary>Returns a color for the given tone or throws if tone is absent.</summary>
    public global::Avalonia.Media.Color this[int tone] => _tones[tone];

    /// <summary>Attempts to get a color for the given tone.</summary>
    public bool TryGet(int tone, out global::Avalonia.Media.Color color) => _tones.TryGetValue(tone, out color);

    /// <summary>Enumerates all tone → color pairs contained in this palette.</summary>
    public IEnumerable<KeyValuePair<int, global::Avalonia.Media.Color>> Enumerate() => _tones;

    /// <summary>Returns a copy of this palette with provided overrides applied.</summary>
    public TonalPalette WithOverrides(IDictionary<int, global::Avalonia.Media.Color> overrides)
    {
        var dict = new Dictionary<int, global::Avalonia.Media.Color>(_tones);
        foreach (var kv in overrides) dict[kv.Key] = kv.Value;
        return new TonalPalette(Kind, dict);
    }
}
