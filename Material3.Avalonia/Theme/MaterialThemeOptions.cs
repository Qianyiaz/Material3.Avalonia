using Avalonia.Media;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;

namespace Material3.Avalonia.Theme;

public sealed record MaterialThemeOptions(
    Color SourceColor,
    DynamicSchemeVariant Variant,
    ThemeMode Mode,
    Contrast Contrast)
{
    public static MaterialThemeOptions Defaults => new(
        SourceColor: Color.FromUInt32(0xFF6750A4),
        Variant: DynamicSchemeVariant.TonalSpot,
        Mode: ThemeMode.System,
        Contrast: Contrast.Standard
    );
}