using Avalonia.Media;

namespace Material3.Avalonia.Colors.Ref;

/// <summary>
/// Canonical Material 3 tonal palette kinds.
/// </summary>
public enum PaletteKind
{
    Primary,
    Secondary,
    Tertiary,
    Neutral,
    NeutralVariant,
    Error
}

/// <summary>
/// Represents a tonal palette for a single key color (e.g., Primary).
/// Contains a mapping of tone values (0..100 including 95/98/99) to colors.
/// This layer is reference-only (no roles, no brushes).
/// </summary>
public sealed class TonalPalette
{
    private readonly Dictionary<int, Color> _tones;
    
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
    public TonalPalette(PaletteKind kind, IDictionary<int, Color> tones)
    {
        Kind = kind;
        _tones = new Dictionary<int, Color>(tones ?? throw new ArgumentNullException(nameof(tones)));
    }

    /// <summary>Checks whether a color exists for the given tone.</summary>
    public bool ContainsTone(int tone) => _tones.ContainsKey(tone);

    /// <summary>Returns a color for the given tone or throws if tone is absent.</summary>
    public Color this[int tone] => _tones[tone];

    /// <summary>Attempts to get a color for the given tone.</summary>
    public bool TryGet(int tone, out Color color) => _tones.TryGetValue(tone, out color);

    /// <summary>Enumerates all tone → color pairs contained in this palette.</summary>
    public IEnumerable<KeyValuePair<int, Color>> Enumerate() => _tones;

    /// <summary>Returns a copy of this palette with provided overrides applied.</summary>
    public TonalPalette WithOverrides(IDictionary<int, Color> overrides)
    {
        var dict = new Dictionary<int, Color>(_tones);
        foreach (var kv in overrides) dict[kv.Key] = kv.Value;
        return new TonalPalette(Kind, dict);
    }
}
