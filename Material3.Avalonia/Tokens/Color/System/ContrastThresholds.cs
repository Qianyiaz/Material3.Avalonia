namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// WCAG thresholds used by the contrast engine.
/// You can override the defaults if your product requires different goals.
/// </summary>
public sealed class ContrastThresholds
{
    /// <summary>Target contrast for text/icons (WCAG AA).</summary>
    public double TextAA { get; init; } = 4.5;

    /// <summary>Target contrast for text/icons on High (WCAG AAA guidance).</summary>
    public double TextAAA { get; init; } = 7.0;

    /// <summary>Target contrast for non-text UI elements (WCAG non-text content).</summary>
    public double NonText { get; init; } = 3.0;
}