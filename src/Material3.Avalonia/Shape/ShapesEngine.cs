using System.Numerics;
using Avalonia;
using Avalonia.Media;
using Material3.Avalonia.Controls;
using Material3.Avalonia.Shape.Internal;

namespace Material3.Avalonia.Shape;

public static class ShapesEngine
{
    public static RoundedPolygon GetShape(MaterialShapeKind kind) => kind switch
    {
        MaterialShapeKind.Circle => MaterialShapesData.Circle,
        MaterialShapeKind.Square => MaterialShapesData.Square,
        MaterialShapeKind.Slanted => MaterialShapesData.Slanted,
        MaterialShapeKind.Arch => MaterialShapesData.Arch,
        MaterialShapeKind.Fan => MaterialShapesData.Fan,
        MaterialShapeKind.Arrow => MaterialShapesData.Arrow,
        MaterialShapeKind.Semicircle => MaterialShapesData.SemiCircle,
        MaterialShapeKind.Oval => MaterialShapesData.Oval,
        MaterialShapeKind.Pill => MaterialShapesData.Pill,
        MaterialShapeKind.Triangle => MaterialShapesData.Triangle,
        MaterialShapeKind.Diamond => MaterialShapesData.Diamond,
        MaterialShapeKind.Clamshell => MaterialShapesData.ClamShell,
        MaterialShapeKind.Pentagon => MaterialShapesData.Pentagon,
        MaterialShapeKind.Gem => MaterialShapesData.Gem,
        MaterialShapeKind.VerySunny => MaterialShapesData.VerySunny,
        MaterialShapeKind.Sunny => MaterialShapesData.Sunny,
        MaterialShapeKind.Cookie4 => MaterialShapesData.Cookie4,
        MaterialShapeKind.Cookie6 => MaterialShapesData.Cookie6,
        MaterialShapeKind.Cookie7 => MaterialShapesData.Cookie7,
        MaterialShapeKind.Cookie9 => MaterialShapesData.Cookie9,
        MaterialShapeKind.Cookie12 => MaterialShapesData.Cookie12,
        MaterialShapeKind.Ghostish => MaterialShapesData.Ghostish,
        MaterialShapeKind.Clover4 => MaterialShapesData.Clover4,
        MaterialShapeKind.Clover8 => MaterialShapesData.Clover8,
        MaterialShapeKind.Burst => MaterialShapesData.Burst,
        MaterialShapeKind.SoftBurst => MaterialShapesData.SoftBurst,
        MaterialShapeKind.Boom => MaterialShapesData.Boom,
        MaterialShapeKind.SoftBoom => MaterialShapesData.SoftBoom,
        MaterialShapeKind.Flower => MaterialShapesData.Flower,
        MaterialShapeKind.Puffy => MaterialShapesData.Puffy,
        MaterialShapeKind.PuffyDiamond => MaterialShapesData.PuffyDiamond,
        MaterialShapeKind.PixelCircle => MaterialShapesData.PixelCircle,
        MaterialShapeKind.PixelTriangle => MaterialShapesData.PixelTriangle,
        MaterialShapeKind.Bun => MaterialShapesData.Bun,
        MaterialShapeKind.Heart => MaterialShapesData.Heart,
        _ => MaterialShapesData.Circle
    };

    public static Geometry BuildGeometry(RoundedPolygon unit, Rect dest, Stretch stretch)
    {
        var sx = dest.Width;
        var sy = dest.Height;
        double scaleX, scaleY;

        switch (stretch)
        {
            case Stretch.Fill:
                scaleX = sx;
                scaleY = sy;
                break;
            case Stretch.Uniform:
                var u = Math.Min(sx, sy);
                scaleX = u;
                scaleY = u;
                break;
            case Stretch.UniformToFill:
                var f = Math.Max(sx, sy);
                scaleX = f;
                scaleY = f;
                break;
            case Stretch.None:
            default:
                scaleX = 1.0;
                scaleY = 1.0;
                break;
        }

        var ox = dest.X + (sx - scaleX) / 2.0;
        var oy = dest.Y + (sy - scaleY) / 2.0;

        var sg = new StreamGeometry();
        using var ctx = sg.Open();

        var cubics = unit.Cubics;
        if (cubics.Count == 0) return sg;

        var first = cubics[0];
        var start = ScalePoint(first.A0);

        ctx.BeginFigure(start, isFilled: true);
        foreach (var c in cubics)
        {
            var c0 = ScalePoint(c.C0);
            var c1 = ScalePoint(c.C1);
            var a1 = ScalePoint(c.A1);
            ctx.CubicBezierTo(c0, c1, a1);
        }

        ctx.EndFigure(isClosed: true);
        return sg;

        Point ScalePoint(Vector2 p) => new(ox + p.X * scaleX, oy + p.Y * scaleY);
    }
}