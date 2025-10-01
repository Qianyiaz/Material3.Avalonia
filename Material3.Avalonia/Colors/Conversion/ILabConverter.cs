using Material3.Avalonia.Colors.Core;

namespace Material3.Avalonia.Colors.Conversion;

/// <summary>
/// CIE 1976 L*a*b* (CIELAB) conversions per ISO/CIE 11664-4.
/// Assumes XYZ values are <em>relative</em> to the reference white with Y_n = 100.
/// White point is supplied explicitly to avoid hidden global state.
/// </summary>
public interface ILabConverter
{
    /// <summary>XYZ (relative) → Lab under the given reference white.</summary>
    LabColor XyzToLab(XyzColor xyz, XyzColor white);


    /// <summary>Lab → XYZ (relative) under the given reference white.</summary>
    XyzColor LabToXyz(LabColor lab, XyzColor white);


    /// <summary>
    /// Convenience: ARGB sRGB → Lab using sRGB/D65 matrices (XYZ relative to D65, Y_n=100).
    /// </summary>
    LabColor ArgbToLab(uint argb);


    /// <summary>
    /// Convenience: Lab → ARGB sRGB using sRGB/D65 matrices (XYZ relative to D65, Y_n=100).
    /// </summary>
    uint LabToArgb(LabColor lab);
}