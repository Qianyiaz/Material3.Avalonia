using Material3.Avalonia.Tokens.Color.Reference;

namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// Tone search: finds the nearest tone for a foreground role that satisfies a target ratio
/// to the given background tone, minimizing delta to preserve brand identity.
/// </summary>
public static class ToneSearch
{
    /// <summary>
    /// Finds a foreground tone that achieves at least <paramref name="targetRatio"/> over background,
    /// preferring the direction specified by <paramref name="preferLighter"/>.
    /// </summary>
    /// <param name="foregroundPalette">Foreground palette kind.</param>
    /// <param name="backgroundPalette">Background palette kind.</param>
    /// <param name="foregroundStartTone">Current foreground tone.</param>
    /// <param name="backgroundTone">Background tone.</param>
    /// <param name="targetRatio">Target WCAG ratio (e.g., 3.0, 4.5, 7.0).</param>
    /// <param name="preferLighter">If true, prefer larger tones; otherwise prefer smaller.</param>
    /// <param name="palettes">Reference tonal palettes used to resolve colors.</param>
    /// <returns>Nearest foreground tone that meets or exceeds the target ratio.</returns>
    public static int FindToneWithContrast(
        PaletteKind foregroundPalette,
        PaletteKind backgroundPalette,
        int foregroundStartTone,
        int backgroundTone,
        double targetRatio,
        bool preferLighter,
        TonalPalettes palettes)
    {
        var backgroundColor = palettes.Resolve(backgroundPalette, backgroundTone);
        var startColor = palettes.Resolve(foregroundPalette, foregroundStartTone);
        var startRatio = WcagContrast.Ratio(startColor, backgroundColor);

        if (startRatio >= targetRatio)
            return foregroundStartTone;

        var bestTone = foregroundStartTone;
        var bestDelta = int.MaxValue;

        for (var tone = 0; tone <= 100; ++tone)
        {
            var currentForegroundColor = palettes.Resolve(foregroundPalette, tone);
            var currentRatio = WcagContrast.Ratio(currentForegroundColor, backgroundColor);
            if (currentRatio < targetRatio) continue;
            
            var delta = Math.Abs(tone - foregroundStartTone);
            var isBetter = delta < bestDelta ||
                           delta == bestDelta && (preferLighter ? tone > bestTone : tone < bestTone);
            if (!isBetter) continue;
            
            bestDelta = delta;
            bestTone = tone;
        }

        return bestTone;
    }
}