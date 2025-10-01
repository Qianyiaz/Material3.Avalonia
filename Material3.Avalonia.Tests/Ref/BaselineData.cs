using System.Collections.Generic;
using Avalonia.Media;

namespace Material3.Avalonia.Tests.Ref;

/// <summary>
/// Baseline reference colors from Material 3 docs (subset of tones: 0,10,..,90,95,99,100).
/// Keys are tone integers; values are sRGB hex strings (#AARRGGBB or #RRGGBB).
/// </summary>
internal static class BaselineData
{
    public static readonly Dictionary<int, string> Primary = new()
    {
        { 0, "#FF000000" }, {10, "#FF21005D"}, {20, "#FF381E72"}, {30, "#FF4F378B"},
        {40, "#FF6750A4"}, {50, "#FF7F67BE"}, {60, "#FF9A82DB"}, {70, "#FFB69DF8"},
        {80, "#FFD0BCFF"}, {90, "#FFEADDFF"}, {95, "#FFF6EDFF"}, {99, "#FFFFFBFE"},
        {100,"#FFFFFFFF"}
    };

    public static readonly Dictionary<int, string> Secondary = new()
    {
        { 0, "#FF000000" }, {10, "#FF1D192B"}, {20, "#FF332D41"}, {30, "#FF4A4458"},
        {40, "#FF625B71"}, {50, "#FF7A7289"}, {60, "#FF958DA5"}, {70, "#FFB0A7C0"},
        {80, "#FFCCC2DC"}, {90, "#FFE8DEF8"}, {95, "#FFF6EDFF"}, {99, "#FFFFFBFE"},
        {100,"#FFFFFFFF"}
    };

    public static readonly Dictionary<int, string> Tertiary = new()
    {
        { 0, "#FF000000" }, {10, "#FF31111D"}, {20, "#FF492532"}, {30, "#FF633B48"},
        {40, "#FF7D5260"}, {50, "#FF986977"}, {60, "#FFB58392"}, {70, "#FFD29DAC"},
        {80, "#FFEFB8C8"}, {90, "#FFFFD8E4"}, {95, "#FFFFECF1"}, {99, "#FFFFFBFA"},
        {100,"#FFFFFFFF"}
    };

    public static readonly Dictionary<int, string> Neutral = new()
    {
        { 0, "#FF000000" }, {10, "#FF1C1B1F"}, {20, "#FF313033"}, {30, "#FF484649"},
        {40, "#FF605D62"}, {50, "#FF787579"}, {60, "#FF939094"}, {70, "#FFAEAAAE"},
        {80, "#FFC9C5CA"}, {90, "#FFE6E1E5"}, {95, "#FFF4EFF4"}, {99, "#FFFFFBFE"},
        {100,"#FFFFFFFF"}
    };

    public static readonly Dictionary<int, string> NeutralVariant = new()
    {
        { 0, "#FF000000" }, {10, "#FF1D1A22"}, {20, "#FF322F37"}, {30, "#FF49454F"},
        {40, "#FF605D66"}, {50, "#FF79747E"}, {60, "#FF938F99"}, {70, "#FFAEA9B4"},
        {80, "#FFCAC4D0"}, {90, "#FFE7E0EC"}, {95, "#FFF5EEFA"}, {99, "#FFFFFBFE"},
        {100,"#FFFFFFFF"}
    };

    public static readonly Dictionary<int, string> Error = new()
    {
        { 0, "#FF000000" }, {10, "#FF410004"}, {20, "#FF690005"}, {30, "#FF93000A"},
        {40, "#FFBA1A1A"}, {50, "#FFDE3730"}, {60, "#FFFF5449"}, {70, "#FFFF897D"},
        {80, "#FFFFB4AB"}, {90, "#FFFFDAD6"}, {95, "#FFFFEDEA"}, {99, "#FFFFFBFB"},
        {100,"#FFFFFFFF"}
    };

    public static Color Parse(string hex) => Color.Parse(hex.Length == 7 ? "#FF" + hex[1..] : hex);
}
