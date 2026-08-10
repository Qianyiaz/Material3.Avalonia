/*
 * Portions of this file are derived from AndroidX (androidx.graphics.shapes),
 * Copyright (C) 2024 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *     https://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Modifications:
 *  - C# port, refactors, numerical tweaks and API changes for Avalonia
 *    Copyright (c) 2025 Nikita Manchuk (klorman)
 */

using System.Numerics;
using Avalonia;

namespace Material3.Avalonia.Shape.Internal;

using static MathF;

internal static class MaterialShapesData
{
    private static readonly CornerRounding R15 = new(0.15f);
    private static readonly CornerRounding R20 = new(0.2f);
    private static readonly CornerRounding R30 = new(0.3f);
    private static readonly CornerRounding R50 = new(0.5f);
    private static readonly CornerRounding R100 = new(1.0f);

    private static readonly Matrix3x2 RotNeg45 = Matrix3x2.CreateRotation(-45f.ToRadians());
    private static readonly Matrix3x2 RotNeg90 = Matrix3x2.CreateRotation(-90f.ToRadians());
    private static readonly Matrix3x2 RotNeg135 = Matrix3x2.CreateRotation(-135f.ToRadians());

    private static readonly Point DefaultCenter = new(0.5f, 0.5f);

    public static RoundedPolygon Circle => field ??= CreateCircle().Normalized();
    public static RoundedPolygon Square => field ??= CreateSquare().Normalized();
    public static RoundedPolygon Slanted => field ??= CreateSlanted().Normalized();
    public static RoundedPolygon Arch => field ??= CreateArch().Normalized();
    public static RoundedPolygon Fan => field ??= CreateFan().Normalized();
    public static RoundedPolygon Arrow => field ??= CreateArrow().Normalized();
    public static RoundedPolygon SemiCircle => field ??= CreateSemiCircle().Normalized();
    public static RoundedPolygon Oval => field ??= CreateOval().Normalized();
    public static RoundedPolygon Pill => field ??= CreatePill().Normalized();
    public static RoundedPolygon Triangle => field ??= CreateTriangle().Normalized();
    public static RoundedPolygon Diamond => field ??= CreateDiamond().Normalized();
    public static RoundedPolygon ClamShell => field ??= CreateClamShell().Normalized();
    public static RoundedPolygon Pentagon => field ??= CreatePentagon().Normalized();
    public static RoundedPolygon Gem => field ??= CreateGem().Normalized();
    public static RoundedPolygon VerySunny => field ??= CreateVerySunny().Normalized();
    public static RoundedPolygon Sunny => field ??= CreateSunny().Normalized();
    public static RoundedPolygon Cookie4 => field ??= CreateCookie4().Normalized();
    public static RoundedPolygon Cookie6 => field ??= CreateCookie6().Normalized();
    public static RoundedPolygon Cookie7 => field ??= CreateCookie7().Normalized();
    public static RoundedPolygon Cookie9 => field ??= CreateCookie9().Normalized();
    public static RoundedPolygon Cookie12 => field ??= CreateCookie12().Normalized();
    public static RoundedPolygon Ghostish => field ??= CreateGhostish().Normalized();
    public static RoundedPolygon Clover4 => field ??= CreateClover4().Normalized();
    public static RoundedPolygon Clover8 => field ??= CreateClover8().Normalized();
    public static RoundedPolygon Burst => field ??= CreateBurst().Normalized();
    public static RoundedPolygon SoftBurst => field ??= CreateSoftBurst().Normalized();
    public static RoundedPolygon Boom => field ??= CreateBoom().Normalized();
    public static RoundedPolygon SoftBoom => field ??= CreateSoftBoom().Normalized();
    public static RoundedPolygon Flower => field ??= CreateFlower().Normalized();
    public static RoundedPolygon Puffy => field ??= CreatePuffy().Normalized();
    public static RoundedPolygon PuffyDiamond => field ??= CreatePuffyDiamond().Normalized();
    public static RoundedPolygon PixelCircle => field ??= CreatePixelCircle().Normalized();
    public static RoundedPolygon PixelTriangle => field ??= CreatePixelTriangle().Normalized();
    public static RoundedPolygon Bun => field ??= CreateBun().Normalized();
    public static RoundedPolygon Heart => field ??= CreateHeart().Normalized();

    private static RoundedPolygon CreateCircle() => RoundedPolygon.Circle(10);

    private static RoundedPolygon CreateSquare() => RoundedPolygon.Rectangle(width: 1f, height: 1f, rounding: R30);

    private static RoundedPolygon CreateSlanted()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.926f, 0.970f), new CornerRounding(0.189f, 0.811f)),
                new PointNRound(new(-0.021f, 0.967f), new CornerRounding(0.187f, 0.057f)),
            }, reps: 2);
    }

    private static RoundedPolygon CreateArch()
    {
        var rp = RoundedPolygon.FromNumVertices(4, rounding: CornerRounding.Unrounded,
            perVertexRounding: new[] { R100, R100, R20, R20 });
        return rp.Transformed(RotNeg135);
    }

    private static RoundedPolygon CreateFan()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(1.004f, 1.000f), new CornerRounding(0.148f, 0.417f)),
                new PointNRound(new(0.000f, 1.000f), new CornerRounding(0.151f)),
                new PointNRound(new(0.000f, -0.003f), new CornerRounding(0.148f)),
                new PointNRound(new(0.978f, 0.020f), new CornerRounding(0.803f)),
            }, reps: 1);
    }

    private static RoundedPolygon CreateArrow()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.892f), new CornerRounding(0.313f)),
                new PointNRound(new(-0.216f, 1.050f), new CornerRounding(0.207f)),
                new PointNRound(new(0.499f, -0.160f), new CornerRounding(0.215f, 1.000f)),
                new PointNRound(new(1.225f, 1.060f), new CornerRounding(0.211f)),
            }, reps: 1);
    }

    private static RoundedPolygon CreateSemiCircle()
    {
        return RoundedPolygon.Rectangle(
            width: 1.6f, height: 1f,
            rounding: CornerRounding.Unrounded,
            perVertexRounding: new[] { R20, R20, R100, R100 }
        );
    }

    private static RoundedPolygon CreateOval()
    {
        var m = Matrix3x2.CreateScale(1f, 0.64f);
        return RoundedPolygon.Circle().Transformed(m).Transformed(RotNeg45);
    }

    private static RoundedPolygon CreatePill()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.961f, 0.039f), new CornerRounding(0.426f)),
                new PointNRound(new(1.001f, 0.428f), CornerRounding.Unrounded),
                new PointNRound(new(1.000f, 0.609f), new CornerRounding(1.000f)),
            }, reps: 2, mirroring: true);
    }

    private static RoundedPolygon CreateTriangle()
        => RoundedPolygon.FromNumVertices(3, rounding: R20).Transformed(RotNeg90);

    private static RoundedPolygon CreateDiamond()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 1.096f), new CornerRounding(0.151f, 0.524f)),
                new PointNRound(new(0.040f, 0.500f), new CornerRounding(0.159f)),
            }, reps: 2);
    }

    private static RoundedPolygon CreateClamShell()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.171f, 0.841f), new CornerRounding(0.159f)),
                new PointNRound(new(-0.020f, 0.500f), new CornerRounding(0.140f)),
                new PointNRound(new(0.170f, 0.159f), new CornerRounding(0.159f)),
            }, reps: 2);
    }

    private static RoundedPolygon CreatePentagon()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, -0.009f), new CornerRounding(0.172f)),
                new PointNRound(new(1.030f, 0.365f), new CornerRounding(0.164f)),
                new PointNRound(new(0.828f, 0.970f), new CornerRounding(0.169f)),
            }, reps: 1, mirroring: true);
    }

    private static RoundedPolygon CreateGem()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.499f, 1.023f), new CornerRounding(0.241f, 0.778f)),
                new PointNRound(new(-0.005f, 0.792f), new CornerRounding(0.208f)),
                new PointNRound(new(0.073f, 0.258f), new CornerRounding(0.228f)),
                new PointNRound(new(0.433f, -0.000f), new CornerRounding(0.491f)),
            }, reps: 1, mirroring: true);
    }

    private static RoundedPolygon CreateSunny()
        => RoundedPolygon.Star(verticesPerRadius: 8, innerRadius: .8f, outerRounding: R15);

    private static RoundedPolygon CreateVerySunny()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 1.080f), new CornerRounding(0.085f)),
                new PointNRound(new(0.358f, 0.843f), new CornerRounding(0.085f)),
            }, reps: 8);
    }

    private static RoundedPolygon CreateCookie4()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(1.237f, 1.236f), new CornerRounding(0.258f)),
                new PointNRound(new(0.500f, 0.918f), new CornerRounding(0.233f)),
            }, reps: 4);
    }

    private static RoundedPolygon CreateCookie6()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.723f, 0.884f), new CornerRounding(0.394f)),
                new PointNRound(new(0.500f, 1.099f), new CornerRounding(0.398f)),
            }, reps: 6);
    }

    private static RoundedPolygon CreateCookie7()
        => RoundedPolygon.Star(7, innerRadius: .75f, outerRounding: R50).Transformed(RotNeg90);

    private static RoundedPolygon CreateCookie9()
        => RoundedPolygon.Star(9, innerRadius: .8f, outerRounding: R50).Transformed(RotNeg90);

    private static RoundedPolygon CreateCookie12()
        => RoundedPolygon.Star(12, innerRadius: .8f, outerRounding: R50).Transformed(RotNeg90);

    private static RoundedPolygon CreateGhostish()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.000f), new CornerRounding(1.000f)),
                new PointNRound(new(1.000f, 0.000f), new CornerRounding(1.000f)),
                new PointNRound(new(1.000f, 1.140f), new CornerRounding(0.254f, 0.106f)),
                new PointNRound(new(0.575f, 0.906f), new CornerRounding(0.253f)),
            }, reps: 1, mirroring: true);
    }

    private static RoundedPolygon CreateClover4()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.074f), CornerRounding.Unrounded),
                new PointNRound(new(0.725f, -0.099f), new CornerRounding(0.476f)),
            }, reps: 4, mirroring: true);
    }

    private static RoundedPolygon CreateClover8()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.036f), CornerRounding.Unrounded),
                new PointNRound(new(0.758f, -0.101f), new CornerRounding(0.209f)),
            }, reps: 8);
    }

    private static RoundedPolygon CreateBurst()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, -0.006f), new CornerRounding(0.006f)),
                new PointNRound(new(0.592f, 0.158f), new CornerRounding(0.006f)),
            }, reps: 12);
    }

    private static RoundedPolygon CreateSoftBurst()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.193f, 0.277f), new CornerRounding(0.053f)),
                new PointNRound(new(0.176f, 0.055f), new CornerRounding(0.053f)),
            }, reps: 10);
    }

    private static RoundedPolygon CreateBoom()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.457f, 0.296f), new CornerRounding(0.007f)),
                new PointNRound(new(0.500f, -0.051f), new CornerRounding(0.007f)),
            }, reps: 15);
    }

    private static RoundedPolygon CreateSoftBoom()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.733f, 0.454f), CornerRounding.Unrounded),
                new PointNRound(new(0.839f, 0.437f), new CornerRounding(0.532f)),
                new PointNRound(new(0.949f, 0.449f), new CornerRounding(0.439f, 1.000f)),
                new PointNRound(new(0.998f, 0.478f), new CornerRounding(0.174f)),
            }, reps: 16, mirroring: true);
    }

    private static RoundedPolygon CreateFlower()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.370f, 0.187f), CornerRounding.Unrounded),
                new PointNRound(new(0.416f, 0.049f), new CornerRounding(0.381f)),
                new PointNRound(new(0.479f, 0.001f), new CornerRounding(0.095f)),
            }, reps: 8, mirroring: true);
    }

    private static RoundedPolygon CreatePuffy()
    {
        var m = Matrix3x2.CreateScale(1f, 0.742f);
        var rp = CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.053f), CornerRounding.Unrounded),
                new PointNRound(new(0.545f, -0.040f), new CornerRounding(0.405f)),
                new PointNRound(new(0.670f, -0.035f), new CornerRounding(0.426f)),
                new PointNRound(new(0.717f, 0.066f), new CornerRounding(0.574f)),
                new PointNRound(new(0.722f, 0.128f), CornerRounding.Unrounded),
                new PointNRound(new(0.777f, 0.002f), new CornerRounding(0.360f)),
                new PointNRound(new(0.914f, 0.149f), new CornerRounding(0.660f)),
                new PointNRound(new(0.926f, 0.289f), new CornerRounding(0.660f)),
                new PointNRound(new(0.881f, 0.346f), CornerRounding.Unrounded),
                new PointNRound(new(0.940f, 0.344f), new CornerRounding(0.126f)),
                new PointNRound(new(1.003f, 0.437f), new CornerRounding(0.255f)),
            }, reps: 2, mirroring: true);
        return rp.Transformed(m);
    }

    private static RoundedPolygon CreatePuffyDiamond()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.870f, 0.130f), new CornerRounding(0.146f)),
                new PointNRound(new(0.818f, 0.357f), CornerRounding.Unrounded),
                new PointNRound(new(1.000f, 0.332f), new CornerRounding(0.853f)),
            }, reps: 4, mirroring: true);
    }

    private static RoundedPolygon CreatePixelCircle()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.000f), CornerRounding.Unrounded),
                new PointNRound(new(0.704f, 0.000f), CornerRounding.Unrounded),
                new PointNRound(new(0.704f, 0.065f), CornerRounding.Unrounded),
                new PointNRound(new(0.843f, 0.065f), CornerRounding.Unrounded),
                new PointNRound(new(0.843f, 0.148f), CornerRounding.Unrounded),
                new PointNRound(new(0.926f, 0.148f), CornerRounding.Unrounded),
                new PointNRound(new(0.926f, 0.296f), CornerRounding.Unrounded),
                new PointNRound(new(1.000f, 0.296f), CornerRounding.Unrounded),
            }, reps: 2, mirroring: true);
    }

    private static RoundedPolygon CreatePixelTriangle()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.110f, 0.500f), CornerRounding.Unrounded),
                new PointNRound(new(0.113f, 0.000f), CornerRounding.Unrounded),
                new PointNRound(new(0.287f, 0.000f), CornerRounding.Unrounded),
                new PointNRound(new(0.287f, 0.087f), CornerRounding.Unrounded),
                new PointNRound(new(0.421f, 0.087f), CornerRounding.Unrounded),
                new PointNRound(new(0.421f, 0.170f), CornerRounding.Unrounded),
                new PointNRound(new(0.560f, 0.170f), CornerRounding.Unrounded),
                new PointNRound(new(0.560f, 0.265f), CornerRounding.Unrounded),
                new PointNRound(new(0.674f, 0.265f), CornerRounding.Unrounded),
                new PointNRound(new(0.675f, 0.344f), CornerRounding.Unrounded),
                new PointNRound(new(0.789f, 0.344f), CornerRounding.Unrounded),
                new PointNRound(new(0.789f, 0.439f), CornerRounding.Unrounded),
                new PointNRound(new(0.888f, 0.439f), CornerRounding.Unrounded),
            }, reps: 1, mirroring: true);
    }

    private static RoundedPolygon CreateBun()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.796f, 0.500f), CornerRounding.Unrounded),
                new PointNRound(new(0.853f, 0.518f), new CornerRounding(1f)),
                new PointNRound(new(0.992f, 0.631f), new CornerRounding(1f)),
                new PointNRound(new(0.968f, 1.000f), new CornerRounding(1f)),
            }, reps: 2, mirroring: true);
    }

    private static RoundedPolygon CreateHeart()
    {
        return CustomPolygon(
            new()
            {
                new PointNRound(new(0.500f, 0.268f), new CornerRounding(0.016f)),
                new PointNRound(new(0.792f, -0.066f), new CornerRounding(0.958f)),
                new PointNRound(new(1.064f, 0.276f), new CornerRounding(1.000f)),
                new PointNRound(new(0.501f, 0.946f), new CornerRounding(0.129f)),
            }, reps: 1, mirroring: true);
    }

    private static RoundedPolygon CustomPolygon(
        List<PointNRound> pnr, int reps, bool mirroring = false, Point? center = null)
    {
        var c = center ?? DefaultCenter;
        var points = DoRepeat(pnr, reps, c, mirroring);

        var verts = new float[points.Count * 2];
        var j = 0;
        foreach (var t in points)
        {
            verts[j++] = (float)t.P.X;
            verts[j++] = (float)t.P.Y;
        }

        var perVertexRounding = points.Select(p => p.R).ToArray();
        return RoundedPolygon.FromVertices(verts, CornerRounding.Unrounded, perVertexRounding, (float)c.X, (float)c.Y);
    }

    private static List<PointNRound> DoRepeat(List<PointNRound> src, int reps, Point center, bool mirroring)
    {
        if (!mirroring)
        {
            var result = new List<PointNRound>(src.Count * reps);
            var np = src.Count;
            var step = 360f / reps;
            for (var it = 0; it < np * reps; ++it)
            {
                var repIdx = Math.DivRem(it, np, out var baseIx);
                var rot = repIdx * step;
                var pt = RotateDegrees(src[baseIx].P, rot, center);
                result.Add(new PointNRound(pt, src[baseIx].R));
            }

            return result;
        }
        else
        {
            var angles = src.Select(s => AngleDegrees(s.P - center)).ToArray();
            var distances = src.Select(s => new Vector2((float)(s.P.X - center.X), (float)(s.P.Y - center.Y)).Length())
                .ToArray();
            var actualReps = reps * 2;
            var sectionAngle = 360f / actualReps;

            var res = new List<PointNRound>(src.Count * actualReps);

            for (var r = 0; r < actualReps; ++r)
            {
                for (var index = 0; index < src.Count; ++index)
                {
                    var i = (r % 2 == 0) ? index : (src.Count - 1 - index);
                    if (i > 0 || r % 2 == 0)
                    {
                        var a = sectionAngle * r + (r % 2 == 0 ? angles[i] : sectionAngle - angles[i] + 2 * angles[0]);
                        var aRad = a.ToRadians();
                        var finalPoint = new Point(Cos(aRad), Sin(aRad)) * distances[i] + center;
                        res.Add(new PointNRound(finalPoint, src[i].R));
                    }
                }
            }

            return res;
        }
    }

    private static Point RotateDegrees(Point p, float angleDeg, Point center)
    {
        var a = angleDeg.ToRadians();
        var off = p - center;
        return new Point(off.X * Cos(a) - off.Y * Sin(a), off.X * Sin(a) + off.Y * Cos(a)) + center;
    }

    private static float AngleDegrees(Point v) => (float)Math.Atan2(v.Y, v.X) * 180f / PI;

    private static float ToRadians(this float deg) => deg / 360f * 2 * PI;

    private readonly record struct PointNRound(Point P, CornerRounding R);
}