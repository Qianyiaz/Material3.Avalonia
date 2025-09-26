using Material3.Avalonia.Colors.Ref;

namespace Material3.Avalonia.Colors.Sys;

/// <summary>
/// Describes a single system color role mapping to a tonal palette and tone index.
/// Example: "MdSysColorPrimaryBrush" → Primary @ tone 40.
/// </summary>
public readonly record struct SystemRole(string Name, PaletteKind Palette, int Tone);
