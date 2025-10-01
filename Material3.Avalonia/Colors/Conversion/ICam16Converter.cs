using Material3.Avalonia.Colors.Core;

namespace Material3.Avalonia.Colors.Conversion;

/// <summary>
/// CAM16 forward and inverse transforms under specified viewing conditions.
/// </summary>
public interface ICam16Converter
{
    /// <summary>Forward model: XYZ → CAM16 (J, C, h, M, s, Q).</summary>
    Cam16Color ToCam16(XyzColor xyz, IViewingConditions vc);

    /// <summary>Inverse model: CAM16 (J, C, h, optional Q/M/s) → XYZ.</summary>
    XyzColor ToXyz(Cam16Color cam, IViewingConditions vc);

    /// <summary>CAM16 → CAM16-UCS.</summary>
    Cam16UcsColor ToUcs(Cam16Color cam);

    /// <summary>CAM16-UCS → CAM16 (requires hue to reconstruct direction).</summary>
    Cam16Color FromUcs(Cam16UcsColor ucs, double hueDegrees, IViewingConditions vc);
}
