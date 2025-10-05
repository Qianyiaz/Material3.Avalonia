namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// Pair of system roles to validate for contrast (foreground vs background).
/// </summary>
public readonly record struct RolePair(
    string ForegroundRole,
    string BackgroundRole,
    bool IsTextOrIcon);

/// <summary>
/// Provides default foreground/background role pairs for contrast validation.
/// </summary>
public static class RolePairs
{
    /// <summary>
    /// Default pairs used by the contrast engine.
    /// Text/icon pairs aim for TextAA/AAA, non-text pairs aim for NonText.
    /// </summary>
    public static readonly RolePair[] Default =
    {
        // Text/icon on brand
        new("MdSysColorOnPrimaryBrush", "MdSysColorPrimaryBrush", true),
        new("MdSysColorOnSecondaryBrush", "MdSysColorSecondaryBrush", true),
        new("MdSysColorOnTertiaryBrush", "MdSysColorTertiaryBrush", true),
        new("MdSysColorOnErrorBrush", "MdSysColorErrorBrush", true),

        new("MdSysColorOnPrimaryContainerBrush", "MdSysColorPrimaryContainerBrush", true),
        new("MdSysColorOnSecondaryContainerBrush", "MdSysColorSecondaryContainerBrush", true),
        new("MdSysColorOnTertiaryContainerBrush", "MdSysColorTertiaryContainerBrush", true),
        new("MdSysColorOnErrorContainerBrush", "MdSysColorErrorContainerBrush", true),

        // Text/icon on surfaces
        new("MdSysColorOnSurfaceBrush", "MdSysColorSurfaceBrush", true),
        new("MdSysColorOnSurfaceVariantBrush", "MdSysColorSurfaceVariantBrush", true),
        new("MdSysColorInverseOnSurfaceBrush", "MdSysColorInverseSurfaceBrush", true),

        // Non-text UI against surface (containers, outlines)
        new("MdSysColorPrimaryContainerBrush", "MdSysColorSurfaceBrush", false),
        new("MdSysColorSecondaryContainerBrush", "MdSysColorSurfaceBrush", false),
        new("MdSysColorTertiaryContainerBrush", "MdSysColorSurfaceBrush", false),
        new("MdSysColorErrorContainerBrush", "MdSysColorSurfaceBrush", false),

        new("MdSysColorOutlineBrush", "MdSysColorSurfaceBrush", false),
        new("MdSysColorOutlineVariantBrush", "MdSysColorSurfaceBrush", false),
    };
}