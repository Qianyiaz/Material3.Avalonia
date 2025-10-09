using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;

namespace Material3.Avalonia.Demo.Converters;

public sealed class TonalSwatchBrushConverter : IValueConverter
{
    public static readonly TonalSwatchBrushConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DynamicScheme scheme || parameter is not string p || string.IsNullOrWhiteSpace(p))
            return null;

        if (!TryParse(p, out var palette, out var tone)) return null;

        var tp = palette switch
        {
            PaletteKind.Primary => scheme.PrimaryPalette,
            PaletteKind.Secondary => scheme.SecondaryPalette,
            PaletteKind.Tertiary => scheme.TertiaryPalette,
            PaletteKind.Neutral => scheme.NeutralPalette,
            PaletteKind.NeutralVariant => scheme.NeutralVariantPalette,
            PaletteKind.Error => scheme.ErrorPalette,
            PaletteKind.Success => scheme.SuccessPalette,
            PaletteKind.Info => scheme.InfoPalette,
            PaletteKind.Warning => scheme.WarningPalette,
            _ => scheme.PrimaryPalette
        };

        var argb = tp[(uint)tone];
        var color = Color.FromUInt32(argb);
        return new SolidColorBrush(color);
    }

    public object? ConvertBack(object? v, Type t, object? p, CultureInfo c) => throw new NotSupportedException();

    private static bool TryParse(string s, out PaletteKind kind, out int tone)
    {
        var tags = new (string tag, PaletteKind kind)[]
        {
            ("NeutralVariant", PaletteKind.NeutralVariant),
            ("Primary", PaletteKind.Primary),
            ("Secondary", PaletteKind.Secondary),
            ("Tertiary", PaletteKind.Tertiary),
            ("Neutral", PaletteKind.Neutral),
            ("Error", PaletteKind.Error),
            ("Success", PaletteKind.Success),
            ("Info", PaletteKind.Info),
            ("Warning", PaletteKind.Warning),
        };

        foreach (var (tag, k) in tags)
        {
            if (s.StartsWith(tag, StringComparison.OrdinalIgnoreCase))
            {
                var num = s.Substring(tag.Length);
                if (int.TryParse(num, NumberStyles.Integer, CultureInfo.InvariantCulture, out var t))
                {
                    kind = k;
                    tone = t;
                    return true;
                }
            }
        }

        kind = default;
        tone = 0;
        return false;
    }
}

public enum PaletteKind
{
    Primary,
    Secondary,
    Tertiary,
    Neutral,
    NeutralVariant,
    Error,
    Success,
    Info,
    Warning
}