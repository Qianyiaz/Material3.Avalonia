namespace Material3.Avalonia.Tokens.Color.Reference;

/// <summary>
/// Utility for building reference token keys (MdRef…).
/// </summary>
public static class RefKeys
{
    /// <summary>Returns "MdRef{palette}{tone:D2}Color". Example: <c>MdRefPrimary40Color</c>.</summary>
    public static string ColorKey(PaletteKind palette, int tone) => $"MdRef{palette}{tone:D2}Color";

    /// <summary>Returns "MdRef{palette}{tone:D2}Brush". Example: <c>MdRefPrimary40Brush</c>.</summary>
    public static string BrushKey(PaletteKind palette, int tone) => $"MdRef{palette}{tone:D2}Brush";
}