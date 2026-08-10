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

namespace Material3.Avalonia.Shape.Internal;

using static MathUtil;
using static MathF;

internal sealed class RoundedCornerHelper
{
    private readonly float _cornerRadius;
    private readonly float _cosAngle;

    private readonly Vector2 _d1;
    private readonly Vector2 _d2;
    private readonly Vector2 _p0;
    private readonly Vector2 _p1;
    private readonly Vector2 _p2;
    private readonly float _smoothing;

    public RoundedCornerHelper(Vector2 p0, Vector2 p1, Vector2 p2, CornerRounding? rounding)
    {
        _p0 = p0;
        _p1 = p1;
        _p2 = p2;

        var v01 = p0 - p1;
        var v21 = p2 - p1;
        var d01 = v01.Length();
        var d21 = v21.Length();

        if (d01 > 0f && d21 > 0f)
        {
            _d1 = v01 / d01;
            _d2 = v21 / d21;
            _cornerRadius = rounding?.Radius ?? 0f;
            _smoothing = rounding?.Smoothing ?? 0f;
            _cosAngle = Vector2.Dot(_d1, _d2);
            ExpectedRoundCut = Sqrt(Math.Clamp(1f - _cosAngle * _cosAngle, 0f, 1f));
        }
        else
        {
            _d1 = Vector2.Zero;
            _d2 = Vector2.Zero;
            _cornerRadius = 0f;
            _smoothing = 0f;
            _cosAngle = 0f;
            ExpectedRoundCut = 0f;
        }
    }

    public float ExpectedRoundCut =>
        field > 1e-3f ? _cornerRadius * (_cosAngle + 1f) / field : 0f;

    public float ExpectedCut => (1f + _smoothing) * ExpectedRoundCut;

    private static float ActualSmoothingValue(float allowedCut, float expectedRoundCut, float expectedCut,
        float smoothing)
    {
        if (allowedCut > expectedCut) return smoothing;
        if (allowedCut > expectedRoundCut)
        {
            var denom = expectedCut - expectedRoundCut;
            if (denom <= DistanceEpsilon) return 0f;
            return smoothing * (allowedCut - expectedRoundCut) / denom;
        }

        return 0f;
    }

    public IReadOnlyList<Cubic> Build(float allowedCutPrevSide, float allowedCutNextSide)
    {
        var allowed = Min(allowedCutPrevSide, allowedCutNextSide);
        if (ExpectedRoundCut < DistanceEpsilon || allowed < DistanceEpsilon || _cornerRadius < DistanceEpsilon)
        {
            return new[] { Cubic.StraightLine(_p1, _p1) };
        }

        var actualRoundCut = Min(allowed, ExpectedRoundCut);
        var sm0 = ActualSmoothingValue(allowedCutPrevSide, ExpectedRoundCut, ExpectedCut, _smoothing);
        var sm1 = ActualSmoothingValue(allowedCutNextSide, ExpectedRoundCut, ExpectedCut, _smoothing);

        var actualR = _cornerRadius * (actualRoundCut / ExpectedRoundCut);

        var centerDir = (_d1 + _d2) / 2f;
        var center = _p1 + Vector2.Normalize(centerDir) * Sqrt(actualR * actualR + actualRoundCut * actualRoundCut);

        var circleI0 = _p1 + _d1 * actualRoundCut;
        var circleI2 = _p1 + _d2 * actualRoundCut;

        var flank0 = ComputeFlank(actualRoundCut, sm0, _p1, _p0, circleI0, circleI2, center, actualR);
        var flank2 = ComputeFlank(actualRoundCut, sm1, _p1, _p2, circleI2, circleI0, center, actualR).Reverse();

        var arc = Cubic.CircularArc(center, flank0.A1, flank2.A0);
        return new[] { flank0, arc, flank2 };
    }

    private static Cubic ComputeFlank(float roundCut, float smooth, Vector2 corner, Vector2 sideStart,
        Vector2 circleSegIntersection, Vector2 otherCircleSegIntersection,
        Vector2 circleCenter, float r)
    {
        var sideDir = Vector2.Normalize(sideStart - corner);
        var start = corner + sideDir * roundCut * (1f + smooth);

        var mid = Vector2.Lerp(circleSegIntersection, (circleSegIntersection + otherCircleSegIntersection) * 0.5f,
            smooth);
        var endDir = Vector2.Normalize(mid - circleCenter);
        var end = circleCenter + endDir * r;

        var tangent = (end - circleCenter).Rotate90Ccw();
        var anchorEnd = LineIntersection(sideStart, sideDir, end, tangent) ?? circleSegIntersection;

        var anchorStart = (start + anchorEnd * 2f) / 3f;
        return new Cubic(start, anchorStart, anchorEnd, end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2? LineIntersection(Vector2 p0, Vector2 d0, Vector2 p1, Vector2 d1)
    {
        var den = d0.CrossZ(d1);
        if (Abs(den) < DistanceEpsilon) return null;

        var num = (p1 - p0).CrossZ(d1);
        if (Abs(den) < DistanceEpsilon * Abs(num)) return null;

        var k = num / den;
        return p0 + d0 * k;
    }
}