using Avalonia.Controls;
using Avalonia.Media;
using Material3.Avalonia.Tokens.Color.Reference;

namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// Builds the System layer: SolidColorBrush resources for all system roles,
/// using tones pulled from the reference (Ref) tonal palettes and applying contrast policy.
/// </summary>
public static class ColorSchemeGenerator
{
    /// <summary>
    /// Creates a <see cref="ResourceDictionary"/> containing all system brushes
    /// (keys like "MdSysColorPrimaryBrush") for the given options.
    /// </summary>
    /// <param name="refs">Reference tonal palettes (already built from baseline or dynamic source).</param>
    /// <param name="options">Scheme options (theme variant, contrast, style).</param>
    public static ResourceDictionary Build(TonalPalettes refs, ColorSchemeOptions options)
    {
        var dict = new ResourceDictionary();
        var baseMap = RoleToneMapping.GetBase(options.Variant);

        var adjusted = ContrastEngineBalanced.Instance.Apply(
            roles: baseMap,
            variant: options.Variant,
            level: options.Contrast,
            pairs: RolePairs.Default,
            thresholds: ContrastEngineBalanced.DefaultThresholds,
            palettes: refs);
        
        foreach (var role in adjusted)
        {
            var color = refs.Resolve(role.Palette, role.Tone);
            dict[role.Name] = new SolidColorBrush(color);
        }

        return dict;
    }
}
