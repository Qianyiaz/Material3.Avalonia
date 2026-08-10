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
using System.Runtime.CompilerServices;
using Material3.Avalonia.Shape.Internal;

namespace Material3.Avalonia.Shape;

using static MathUtil;
using static MathF;

public readonly struct Cubic(Vector2 a0, Vector2 c0, Vector2 c1, Vector2 a1)
{
    public Vector2 A0 { get; } = a0;
    public Vector2 C0 { get; } = c0;
    public Vector2 C1 { get; } = c1;
    public Vector2 A1 { get; } = a1;

    public bool IsZeroLength => (A1 - A0).LengthSquared() < DistanceEpsilonSquared;

    public Cubic Reverse() => new(A1, C1, C0, A0);

    public Vector2 PointOnCurve(float t)
    {
        var u = 1f - t;
        var tt = t * t;
        var uu = u * u;
        var uuu = uu * u;
        var ttu = tt * u;
        var ttt = tt * t;
        var tuu = t * uu;

        var x = A0.X * uuu + C0.X * (3 * tuu) + C1.X * (3 * ttu) + A1.X * ttt;
        var y = A0.Y * uuu + C0.Y * (3 * tuu) + C1.Y * (3 * ttu) + A1.Y * ttt;
        return new Vector2(x, y);
    }

    public (Cubic left, Cubic right) Split(float t)
    {
        var u = 1f - t;

        var p01 = A0 * u + C0 * t;
        var p12 = C0 * u + C1 * t;
        var p23 = C1 * u + A1 * t;

        var p012 = p01 * u + p12 * t;
        var p123 = p12 * u + p23 * t;
        var point = p012 * u + p123 * t;

        var left = new Cubic(A0, p01, p012, point);
        var right = new Cubic(point, p123, p23, A1);
        return (left, right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Cubic StraightLine(Vector2 p0, Vector2 p1)
    {
        var d = (p1 - p0) / 3f;
        return new Cubic(p0, p0 + d, p1 - d, p1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Cubic CircularArc(Vector2 center, Vector2 p0, Vector2 p1)
    {
        var v0 = p0 - center;
        var v1 = p1 - center;

        var r0 = v0.Length();
        var r1 = v1.Length();
        if (r0 <= 0f || r1 <= 0f)
            return StraightLine(p0, p1);

        var u0 = v0 / r0;
        var u1 = v1 / r1;

        var cosA = Vector2.Dot(u0, u1);
        if (cosA > 0.999f)
            return StraightLine(p0, p1);

        var sinA = u0.CrossZ(u1);
        var theta = Atan2(sinA, cosA);
        var k = r0 * 4f / 3f * Tan(theta / 4f);

        var t0 = u0.Rotate90Ccw();
        var t1 = u1.Rotate90Ccw();

        var c0 = p0 + t0 * k;
        var c1 = p1 - t1 * k;
        return new Cubic(p0, c0, c1, p1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (float minX, float minY, float maxX, float maxY) BoundsApprox()
    {
        if (IsZeroLength)
            return (A0.X, A0.Y, A0.X, A0.Y);

        var minX = Min(Min(A0.X, A1.X), Min(C0.X, C1.X));
        var minY = Min(Min(A0.Y, A1.Y), Min(C0.Y, C1.Y));
        var maxX = Max(Max(A0.X, A1.X), Max(C0.X, C1.X));
        var maxY = Max(Max(A0.Y, A1.Y), Max(C0.Y, C1.Y));
        return (minX, minY, maxX, maxY);
    }
}