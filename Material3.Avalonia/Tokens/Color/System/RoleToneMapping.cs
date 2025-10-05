using Avalonia.Styling;
using Material3.Avalonia.Tokens.Color.Reference;

namespace Material3.Avalonia.Tokens.Color.System;

/// <summary>
/// Provides baseline mapping tables (role → palette/tone) for Light/Dark.
/// No contrast adjustments are applied here.
/// </summary>
public static class RoleToneMapping
{
    /// <summary>
    /// Returns the baseline mapping set for the given context (Light or Dark).
    /// </summary>
    public static IReadOnlyList<SystemRole> GetBase(ThemeVariant variant)
        => variant == ThemeVariant.Dark ? DarkStandard() : LightStandard();

    /// <summary>
    /// Light theme: baseline (standard) role → palette/tone mapping per Material.
    /// </summary>
    private static List<SystemRole> LightStandard()
    {
        var list = new List<SystemRole>
        {
            // Primary / Secondary / Tertiary groups
            new("MdSysColorPrimaryBrush", PaletteKind.Primary, 40),
            new("MdSysColorOnPrimaryBrush", PaletteKind.Primary, 100),
            new("MdSysColorPrimaryContainerBrush", PaletteKind.Primary, 90),
            new("MdSysColorOnPrimaryContainerBrush", PaletteKind.Primary, 30),

            new("MdSysColorSecondaryBrush", PaletteKind.Secondary, 40),
            new("MdSysColorOnSecondaryBrush", PaletteKind.Secondary, 100),
            new("MdSysColorSecondaryContainerBrush", PaletteKind.Secondary, 90),
            new("MdSysColorOnSecondaryContainerBrush", PaletteKind.Secondary, 30),

            new("MdSysColorTertiaryBrush", PaletteKind.Tertiary, 40),
            new("MdSysColorOnTertiaryBrush", PaletteKind.Tertiary, 100),
            new("MdSysColorTertiaryContainerBrush", PaletteKind.Tertiary, 90),
            new("MdSysColorOnTertiaryContainerBrush", PaletteKind.Tertiary, 30),

            // Error (static in M3, but tones still vary by theme)
            new("MdSysColorErrorBrush", PaletteKind.Error, 40),
            new("MdSysColorOnErrorBrush", PaletteKind.Error, 100),
            new("MdSysColorErrorContainerBrush", PaletteKind.Error, 90),
            new("MdSysColorOnErrorContainerBrush", PaletteKind.Error, 30),

            // Surface family (Neutral)
            new("MdSysColorSurfaceBrush", PaletteKind.Neutral, 98),
            new("MdSysColorOnSurfaceBrush", PaletteKind.Neutral, 10),
            new("MdSysColorSurfaceVariantBrush", PaletteKind.NeutralVariant, 90),
            new("MdSysColorOnSurfaceVariantBrush", PaletteKind.NeutralVariant, 30),

            // Surface containers
            new("MdSysColorSurfaceContainerLowestBrush", PaletteKind.Neutral, 100),
            new("MdSysColorSurfaceContainerLowBrush", PaletteKind.Neutral, 96),
            new("MdSysColorSurfaceContainerBrush", PaletteKind.Neutral, 94),
            new("MdSysColorSurfaceContainerHighBrush", PaletteKind.Neutral, 92),
            new("MdSysColorSurfaceContainerHighestBrush", PaletteKind.Neutral, 90),

            // Outline / OutlineVariant (NeutralVariant)
            new("MdSysColorOutlineBrush", PaletteKind.NeutralVariant, 50),
            new("MdSysColorOutlineVariantBrush", PaletteKind.NeutralVariant, 80),

            // Inverse
            new("MdSysColorInverseSurfaceBrush", PaletteKind.Neutral, 20),
            new("MdSysColorInverseOnSurfaceBrush", PaletteKind.Neutral, 95),
            new("MdSysColorInversePrimaryBrush", PaletteKind.Primary, 80),

            // Add-ons
            new("MdSysColorSurfaceDimBrush", PaletteKind.Neutral, 87),
            new("MdSysColorSurfaceBrightBrush", PaletteKind.Neutral, 98),

            // Scrim/Shadow often use black with opacities; map to dark neutrals for consistency
            new("MdSysColorScrimBrush", PaletteKind.Neutral, 0),
            new("MdSysColorShadowBrush", PaletteKind.Neutral, 0),

            // Primary fixed / Secondary fixed / Tertiary fixed groups
            new("MdSysColorPrimaryFixedBrush", PaletteKind.Primary, 90),
            new("MdSysColorPrimaryFixedDimBrush", PaletteKind.Primary, 80),
            new("MdSysColorOnPrimaryFixedBrush", PaletteKind.Primary, 10),
            new("MdSysColorOnPrimaryFixedVariantBrush", PaletteKind.Primary, 30),

            new("MdSysColorSecondaryFixedBrush", PaletteKind.Secondary, 90),
            new("MdSysColorSecondaryFixedDimBrush", PaletteKind.Secondary, 80),
            new("MdSysColorOnSecondaryFixedBrush", PaletteKind.Secondary, 10),
            new("MdSysColorOnSecondaryFixedVariantBrush", PaletteKind.Secondary, 30),

            new("MdSysColorTertiaryFixedBrush", PaletteKind.Tertiary, 90),
            new("MdSysColorTertiaryFixedDimBrush", PaletteKind.Tertiary, 80),
            new("MdSysColorOnTertiaryFixedBrush", PaletteKind.Tertiary, 10),
            new("MdSysColorOnTertiaryFixedVariantBrush", PaletteKind.Tertiary, 30),
        };

        return list;
    }

    /// <summary>
    /// Dark theme: baseline (standard) role → palette/tone mapping per Material.
    /// </summary>
    private static List<SystemRole> DarkStandard()
    {
        var list = new List<SystemRole>
        {
            // Primary / Secondary / Tertiary groups
            new("MdSysColorPrimaryBrush", PaletteKind.Primary, 80),
            new("MdSysColorOnPrimaryBrush", PaletteKind.Primary, 20),
            new("MdSysColorPrimaryContainerBrush", PaletteKind.Primary, 30),
            new("MdSysColorOnPrimaryContainerBrush", PaletteKind.Primary, 90),

            new("MdSysColorSecondaryBrush", PaletteKind.Secondary, 80),
            new("MdSysColorOnSecondaryBrush", PaletteKind.Secondary, 20),
            new("MdSysColorSecondaryContainerBrush", PaletteKind.Secondary, 30),
            new("MdSysColorOnSecondaryContainerBrush", PaletteKind.Secondary, 90),

            new("MdSysColorTertiaryBrush", PaletteKind.Tertiary, 80),
            new("MdSysColorOnTertiaryBrush", PaletteKind.Tertiary, 20),
            new("MdSysColorTertiaryContainerBrush", PaletteKind.Tertiary, 30),
            new("MdSysColorOnTertiaryContainerBrush", PaletteKind.Tertiary, 90),

            // Error
            new("MdSysColorErrorBrush", PaletteKind.Error, 80),
            new("MdSysColorOnErrorBrush", PaletteKind.Error, 20),
            new("MdSysColorErrorContainerBrush", PaletteKind.Error, 30),
            new("MdSysColorOnErrorContainerBrush", PaletteKind.Error, 90),

            // Surface family (Neutral)
            new("MdSysColorSurfaceBrush", PaletteKind.Neutral, 6),
            new("MdSysColorOnSurfaceBrush", PaletteKind.Neutral, 90),
            new("MdSysColorSurfaceVariantBrush", PaletteKind.NeutralVariant, 30),
            new("MdSysColorOnSurfaceVariantBrush", PaletteKind.NeutralVariant, 80),

            // Surface containers
            new("MdSysColorSurfaceContainerLowestBrush", PaletteKind.Neutral, 4),
            new("MdSysColorSurfaceContainerLowBrush", PaletteKind.Neutral, 10),
            new("MdSysColorSurfaceContainerBrush", PaletteKind.Neutral, 12),
            new("MdSysColorSurfaceContainerHighBrush", PaletteKind.Neutral, 17),
            new("MdSysColorSurfaceContainerHighestBrush", PaletteKind.Neutral, 22),

            // Outline / OutlineVariant (NeutralVariant)
            new("MdSysColorOutlineBrush", PaletteKind.NeutralVariant, 60),
            new("MdSysColorOutlineVariantBrush", PaletteKind.NeutralVariant, 30),

            // Inverse
            new("MdSysColorInverseSurfaceBrush", PaletteKind.Neutral, 90),
            new("MdSysColorInverseOnSurfaceBrush", PaletteKind.Neutral, 20),
            new("MdSysColorInversePrimaryBrush", PaletteKind.Primary, 40),

            // Add-ons
            new("MdSysColorSurfaceDimBrush", PaletteKind.Neutral, 6),
            new("MdSysColorSurfaceBrightBrush", PaletteKind.Neutral, 24),

            new("MdSysColorScrimBrush", PaletteKind.Neutral, 0),
            new("MdSysColorShadowBrush", PaletteKind.Neutral, 0),
            
            // Primary fixed / Secondary fixed / Tertiary fixed groups
            new("MdSysColorPrimaryFixedBrush", PaletteKind.Primary, 90),
            new("MdSysColorPrimaryFixedDimBrush", PaletteKind.Primary, 80),
            new("MdSysColorOnPrimaryFixedBrush", PaletteKind.Primary, 10),
            new("MdSysColorOnPrimaryFixedVariantBrush", PaletteKind.Primary, 30),

            new("MdSysColorSecondaryFixedBrush", PaletteKind.Secondary, 90),
            new("MdSysColorSecondaryFixedDimBrush", PaletteKind.Secondary, 80),
            new("MdSysColorOnSecondaryFixedBrush", PaletteKind.Secondary, 10),
            new("MdSysColorOnSecondaryFixedVariantBrush", PaletteKind.Secondary, 30),

            new("MdSysColorTertiaryFixedBrush", PaletteKind.Tertiary, 90),
            new("MdSysColorTertiaryFixedDimBrush", PaletteKind.Tertiary, 80),
            new("MdSysColorOnTertiaryFixedBrush", PaletteKind.Tertiary, 10),
            new("MdSysColorOnTertiaryFixedVariantBrush", PaletteKind.Tertiary, 30),
        };

        return list;
    }
}
