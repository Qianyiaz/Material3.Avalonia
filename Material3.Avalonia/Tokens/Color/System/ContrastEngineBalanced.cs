using Avalonia.Styling;
using Material3.Avalonia.Tokens.Color.Reference;

namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// Applies WCAG-aware tone adjustments to role mappings for Medium/High contrast.
/// Keeps brand identity by making minimal tone shifts needed to reach targets.
/// </summary>
public sealed class ContrastEngineBalanced
{
    /// <summary>Singleton instance for convenience.</summary>
    public static ContrastEngineBalanced Instance { get; } = new();

    /// <summary>Default thresholds: AA for text, 3:1 for non-text, AAA for High.</summary>
    public static ContrastThresholds DefaultThresholds { get; } = new();

    private ContrastEngineBalanced()
    {
    }

    /// <summary>
    /// Adjusts <paramref name="roles"/> to satisfy the target contrast for the given
    /// <paramref name="variant"/> and <paramref name="level"/>. Only the minimum necessary
    /// tone changes are applied.
    /// </summary>
    /// /// <param name="roles">Base role→tone mappings to adjust.</param>
    /// <param name="variant">Light or Dark theme variant.</param>
    /// <param name="level">Requested contrast level (Standard, Medium, High).</param>
    /// <param name="pairs">Foreground/background pairs to validate.</param>
    /// <param name="thresholds">WCAG targets for text and non-text.</param>
    /// <param name="palettes">Reference tonal palettes for color resolution.</param>
    /// <returns>Adjusted role list.</returns>
    public IReadOnlyList<SystemRole> Apply(
        IReadOnlyList<SystemRole> roles,
        ThemeVariant variant,
        ContrastLevel level,
        IReadOnlyList<RolePair> pairs,
        ContrastThresholds thresholds,
        TonalPalettes palettes)
    {
        var roleByNameDictionary = roles.ToDictionary(r => r.Name, r => r);

        foreach (var pair in pairs)
        {
            if (!roleByNameDictionary.TryGetValue(pair.ForegroundRole, out var fgRole))
                continue;
            if (!roleByNameDictionary.TryGetValue(pair.BackgroundRole, out var bgRole))
                continue;

            // Resolve preferred direction: in Light, text should get darker (lower tone);
            // in Dark, text should get lighter (higher tone). For non-text containers,
            // we push containers away from the Surface tone to increase separation.
            var preferLighter = GetPreferredLighter(pair, variant);

            var target = pair.IsTextOrIcon
                ? level == ContrastLevel.High ? thresholds.TextAAA : thresholds.TextAA
                : thresholds.NonText;

            var bestTone = ToneSearch.FindToneWithContrast(
                fgRole.Palette,
                bgRole.Palette,
                fgRole.Tone,
                bgRole.Tone,
                target,
                preferLighter,
                palettes);

            if (bestTone != fgRole.Tone)
                roleByNameDictionary[pair.ForegroundRole] = new SystemRole(fgRole.Name, fgRole.Palette, bestTone);
        }

        return roleByNameDictionary.Values.ToList();
    }

    // Preferred direction heuristics
    private static bool GetPreferredLighter(RolePair pair, ThemeVariant v)
    {
        // Text/icons: Light→prefer darker (lower tone), Dark→prefer lighter (higher tone)
        if (pair.IsTextOrIcon)
            return v == ThemeVariant.Dark;

        // Non-text containers: push away from surface.
        return v == ThemeVariant.Light; // Light→prefer lighter, Dark→prefer darker
    }
}