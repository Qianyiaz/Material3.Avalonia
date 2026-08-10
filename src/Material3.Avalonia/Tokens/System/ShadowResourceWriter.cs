using Avalonia.Controls;
using Avalonia.Media;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;

namespace Material3.Avalonia.Tokens.System;

internal static class ShadowResourceWriter
{
    private const double KeyOpacity = 0.3;
    private const double AmbientOpacity = 0.15;

    private static BoxShadow GetKeyShadow(int y, int blur) =>
        new() { OffsetX = 0, OffsetY = y, Blur = blur, Spread = 0 };

    private static BoxShadow GetAmbientShadow(int y, int blur, int spread) =>
        new() { OffsetX = 0, OffsetY = y, Blur = blur, Spread = spread };

    private static string BuildShadowKey(int level) => $"MdSysShadowLevel{level}";

    private static void Upsert(IResourceDictionary dict, string key, BoxShadows value)
        => dict[key] = value;

    private static Color WithAlpha(Color color, double opacity)
        => Color.FromArgb((byte)Math.Clamp(Math.Round(255 * opacity), 0, 255), color.R, color.G, color.B);

    public static void Rebuild(IResourceDictionary dict, DynamicScheme scheme)
    {
        var color = Color.FromUInt32(scheme.Shadow);
        var keyColor = WithAlpha(color, KeyOpacity);
        var ambientColor = WithAlpha(color, AmbientOpacity);

        Upsert(dict, BuildShadowKey(0), new BoxShadows());
        Upsert(dict, BuildShadowKey(1), new BoxShadows
        (
            GetKeyShadow(1, 2) with { Color = keyColor },
            [GetAmbientShadow(1, 3, 1) with { Color = ambientColor }]
        ));
        Upsert(dict, BuildShadowKey(2), new BoxShadows
        (
            GetKeyShadow(1, 2) with { Color = keyColor },
            [GetAmbientShadow(2, 6, 2) with { Color = ambientColor }]
        ));
        Upsert(dict, BuildShadowKey(3), new BoxShadows
        (
            GetKeyShadow(1, 3) with { Color = keyColor },
            [GetAmbientShadow(4, 8, 3) with { Color = ambientColor }]
        ));
        Upsert(dict, BuildShadowKey(4), new BoxShadows
        (
            GetKeyShadow(2, 3) with { Color = keyColor },
            [GetAmbientShadow(6, 10, 4) with { Color = ambientColor }]
        ));
        Upsert(dict, BuildShadowKey(5), new BoxShadows
        (
            GetKeyShadow(4, 4) with { Color = keyColor },
            [GetAmbientShadow(8, 12, 6) with { Color = ambientColor }]
        ));
    }
}