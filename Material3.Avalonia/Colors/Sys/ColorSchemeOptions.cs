using Avalonia.Styling;

namespace Material3.Avalonia.Colors.Sys;

/// <summary>
/// Options that drive system token generation (roles → tones).
/// </summary>
public enum ContrastLevel
{
    Standard,
    Medium,
    High
}

/// <summary>
/// Visual style profile. Standard follows baseline Material.
/// Expressive can remap some roles to accentuate Secondary/Tertiary in components.
/// </summary>
public enum MdStyle
{
    Standard,
    Expressive
}

/// <summary>
/// Options for building a color scheme. Source color/image are handled at the Ref layer,
/// this struct is only about how system roles are resolved.
/// </summary>
public sealed record ColorSchemeOptions(
    ThemeVariant Variant,
    ContrastLevel Contrast,
    MdStyle Style
);