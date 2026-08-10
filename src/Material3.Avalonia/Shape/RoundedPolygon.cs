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
using Material3.Avalonia.Shape.Internal;

namespace Material3.Avalonia.Shape;

using static MathUtil;
using static MathF;

public sealed class RoundedPolygon
{
    public RoundedPolygon(IReadOnlyList<Feature> features, Vector2 center)
    {
        if (features.Count < 2)
            throw new ArgumentException("Polygon Must have at least 2 features");

        Features = features;
        Center = center;

        Cubics = FlattenCubicsEnsuringClosure();
        ValidateContiguous();
    }

    public IReadOnlyList<Feature> Features { get; }
    public Vector2 Center { get; }
    public IReadOnlyList<Cubic> Cubics { get; }

    private void ValidateContiguous()
    {
        var prev = Cubics[^1];
        foreach (var cubic in Cubics)
        {
            if ((cubic.A0 - prev.A1).LengthSquared() > DistanceEpsilonSquared)
                throw new ArgumentException("RoundedPolygon must be contiguous");
            prev = cubic;
        }
    }

    private List<Cubic> FlattenCubicsEnsuringClosure()
    {
        var list = new List<Cubic>(Features.Sum(f => f.Cubics.Count) + 1);

        Cubic? first = null;
        Cubic? last = null;

        foreach (var feature in Features)
        {
            foreach (var cubic in feature.Cubics)
            {
                if (!cubic.IsZeroLength)
                {
                    if (last is not null) list.Add(last.Value);
                    last = cubic;
                    first ??= cubic;
                }
                else if (last is not null)
                {
                    last = new Cubic(last.Value.A0, last.Value.C0, last.Value.C1, cubic.A1);
                }
            }
        }

        if (last is not null && first is not null)
            list.Add(new Cubic(last.Value.A0, last.Value.C0, last.Value.C1, first.Value.A0));
        else
            list.Add(new Cubic(Center, Center, Center, Center));

        return list;
    }

    public static RoundedPolygon FromNumVertices(
        int numVertices,
        float radius = 1f,
        float centerX = 0f,
        float centerY = 0f,
        CornerRounding? rounding = null,
        IReadOnlyList<CornerRounding>? perVertexRounding = null,
        float startAngle = 0f)
    {
        if (numVertices < 3) throw new ArgumentException("Polygons must have at least 3 vertices");
        var verts = VerticesFromNumVerts(numVertices, radius, centerX, centerY, startAngle);
        return FromVertices(verts, rounding ?? CornerRounding.Unrounded, perVertexRounding, centerX, centerY);
    }

    public static RoundedPolygon FromVertices(
        float[] vertices,
        CornerRounding? rounding = null,
        IReadOnlyList<CornerRounding>? perVertexRounding = null,
        float centerX = float.NaN,
        float centerY = float.NaN)
    {
        if (vertices.Length < 6) throw new ArgumentException("Polygons must have at least 3 vertices");
        if (vertices.Length % 2 != 0) throw new ArgumentException("Vertices must be even");
        if (perVertexRounding is not null && perVertexRounding.Count * 2 != vertices.Length)
            throw new ArgumentException("Per-vertex rounding must be specified for each vertex");

        var features = new List<Feature>();
        var n = vertices.Length / 2;

        var helpers = new RoundedCornerHelper[n];
        for (var i = 0; i < n; ++i)
        {
            var round = perVertexRounding is not null ? perVertexRounding[i] : (rounding ?? CornerRounding.Unrounded);
            var prev = ((i + n - 1) % n) * 2;
            var next = ((i + 1) % n) * 2;

            var p0 = new Vector2(vertices[prev], vertices[prev + 1]);
            var p1 = new Vector2(vertices[i * 2], vertices[i * 2 + 1]);
            var p2 = new Vector2(vertices[next], vertices[next + 1]);
            helpers[i] = new RoundedCornerHelper(p0, p1, p2, round);
        }

        var cuts = new (float roundCutRatio, float cutRatio)[n];
        for (var i = 0; i < n; ++i)
        {
            var rc = helpers[i].ExpectedRoundCut + helpers[(i + 1) % n].ExpectedRoundCut;
            var tc = helpers[i].ExpectedCut + helpers[(i + 1) % n].ExpectedCut;

            var vx = vertices[i * 2];
            var vy = vertices[i * 2 + 1];
            var nx = vertices[((i + 1) % n) * 2];
            var ny = vertices[((i + 1) % n) * 2 + 1];
            var side = Distance(vx - nx, vy - ny);

            if (rc > side)
                cuts[i] = (side / rc, 0f);
            else if (tc > side)
                cuts[i] = (1f, (side - rc) / (tc - rc));
            else
                cuts[i] = (1f, 1f);
        }

        var cornerCubics = new List<Cubic>[n];
        for (var i = 0; i < n; ++i)
        {
            var (roundRatioPrev, cutRatioPrev) = cuts[(i + n - 1) % n];
            var (roundRatioNext, cutRatioNext) = cuts[i];

            var allowed0 = helpers[i].ExpectedRoundCut * roundRatioPrev +
                           (helpers[i].ExpectedCut - helpers[i].ExpectedRoundCut) * cutRatioPrev;
            var allowed1 = helpers[i].ExpectedRoundCut * roundRatioNext +
                           (helpers[i].ExpectedCut - helpers[i].ExpectedRoundCut) * cutRatioNext;

            cornerCubics[i] = helpers[i].Build(allowed0, allowed1).ToList();
        }

        for (var i = 0; i < n; ++i)
        {
            var prevIdx = (i + n - 1) % n;
            var nextIdx = (i + 1) % n;

            var curr = new Vector2(vertices[i * 2], vertices[i * 2 + 1]);
            var prev = new Vector2(vertices[prevIdx * 2], vertices[prevIdx * 2 + 1]);
            var next = new Vector2(vertices[nextIdx * 2], vertices[nextIdx * 2 + 1]);

            var convex = Convex(prev, curr, next);
            features.Add(new Feature.Corner(cornerCubics[i], convex));

            var from = cornerCubics[i].Last().A1;
            var to = cornerCubics[(i + 1) % n].First().A0;
            features.Add(new Feature.Edge(new[] { Cubic.StraightLine(from, to) }));
        }

        Vector2 center;
        if (float.IsNaN(centerX) || float.IsNaN(centerY))
        {
            var sumX = 0f;
            var sumY = 0f;
            var count = 0;
            foreach (var cubic in features.SelectMany(feature => feature.Cubics))
            {
                sumX += cubic.A0.X;
                sumY += cubic.A0.Y;
                count++;
            }

            center = new Vector2(sumX / count, sumY / count);
        }
        else center = new Vector2(centerX, centerY);

        return new RoundedPolygon(features, center);
    }

    private static float[] VerticesFromNumVerts(
        int n,
        float r,
        float cx,
        float cy,
        float startAngle = 0f)
    {
        var arr = new float[n * 2];
        var idx = 0;
        for (var i = 0; i < n; ++i)
        {
            var ang = startAngle + 2 * PI * i / n;
            var v = RadialToCartesian(r, ang, new Vector2(cx, cy));
            arr[idx++] = v.X;
            arr[idx++] = v.Y;
        }

        return arr;
    }

    public RoundedPolygon Transformed(Func<Vector2, Vector2> transform)
    {
        var center = transform(Center);
        var features = new List<Feature>(Features.Count);
        foreach (var f in Features)
        {
            var cc = new List<Cubic>(f.Cubics.Count);
            cc.AddRange(f.Cubics.Select(cubic =>
                new Cubic(transform(cubic.A0), transform(cubic.C0), transform(cubic.C1), transform(cubic.A1))));
            features.Add(f is Feature.Corner corner ? new Feature.Corner(cc, corner.Convex) : new Feature.Edge(cc));
        }

        return new RoundedPolygon(features, center);
    }

    public RoundedPolygon Transformed(Matrix3x2 m)
        => Transformed(v => Vector2.Transform(v, m));

    public RoundedPolygon Normalized()
    {
        var (minX, minY, maxX, maxY) = CalculateBoundsApprox();
        var width = maxX - minX;
        var height = maxY - minY;
        var side = Max(width, height);
        var dx = (side - width) / 2f - minX;
        var dy = (side - height) / 2f - minY;

        return Transformed(p => new Vector2((p.X + dx) / side, (p.Y + dy) / side));
    }

    public (float minX, float minY, float maxX, float maxY) CalculateBoundsApprox()
    {
        var minX = float.PositiveInfinity;
        var minY = float.PositiveInfinity;
        var maxX = float.NegativeInfinity;
        var maxY = float.NegativeInfinity;

        foreach (var cubic in Cubics)
        {
            var (minx, miny, maxx, maxy) = cubic.BoundsApprox();
            minX = Min(minX, minx);
            minY = Min(minY, miny);
            maxX = Max(maxX, maxx);
            maxY = Max(maxY, maxy);
        }

        return (minX, minY, maxX, maxY);
    }

    public static RoundedPolygon Circle(int vertices = 8, float radius = 1f, float cx = 0f, float cy = 0f)
    {
        if (vertices < 3) throw new ArgumentException("Circle needs at least 3 vertices");
        var theta = PI / vertices;
        var polyR = radius / Cos(theta);
        return FromNumVertices(vertices, polyR, cx, cy, new CornerRounding(radius));
    }

    public static RoundedPolygon Rectangle(
        float width = 2f,
        float height = 2f,
        CornerRounding? rounding = null,
        IReadOnlyList<CornerRounding>? perVertexRounding = null,
        float cx = 0f,
        float cy = 0f)
    {
        var left = cx - width / 2f;
        var top = cy - height / 2f;
        var right = cx + width / 2f;
        var bottom = cy + height / 2f;

        var verts = new[]
        {
            right, bottom,
            left, bottom,
            left, top,
            right, top
        };
        return FromVertices(verts, rounding, perVertexRounding, cx, cy);
    }

    public static RoundedPolygon Pill(float width = 2f, float height = 1f, float smoothing = 0f, float cx = 0f,
        float cy = 0f)
    {
        if (width <= 0f || height <= 0f) throw new ArgumentException("Pill width and height must be > 0");
        var w2 = width / 2f;
        var h2 = height / 2f;
        var verts = new[]
        {
            cx + w2, cy + h2,
            cx - w2, cy + h2,
            cx - w2, cy - h2,
            cx + w2, cy - h2
        };
        var rounding = new CornerRounding(Min(w2, h2), smoothing);
        return FromVertices(verts, rounding, null, cx, cy);
    }

    public static RoundedPolygon RegularPolygon(int n, float radius = 1f, float startAngle = 0f, float cx = 0f,
        float cy = 0f,
        CornerRounding? rounding = null, IReadOnlyList<CornerRounding>? perVertex = null)
        => FromNumVertices(n, radius, cx, cy, rounding, perVertex, startAngle);

    public static RoundedPolygon Star(
        int verticesPerRadius, float outerRadius = 1f, float innerRadius = .5f,
        CornerRounding? outerRounding = null, CornerRounding? innerRounding = null,
        float cx = 0f, float cy = 0f)
    {
        if (outerRadius <= 0f || innerRadius <= 0f) throw new ArgumentException("Star radii must be > 0");
        if (innerRadius >= outerRadius) throw new ArgumentException("Inner radius must be < outer radius");

        IReadOnlyList<CornerRounding>? perVertex = null;
        if (innerRounding is not null)
        {
            var list = new List<CornerRounding>(verticesPerRadius * 2);
            for (var i = 0; i < verticesPerRadius; ++i)
            {
                list.Add(outerRounding ?? CornerRounding.Unrounded);
                list.Add(innerRounding);
            }

            perVertex = list;
        }

        var arr = new float[verticesPerRadius * 4];
        var idx = 0;
        for (var i = 0; i < verticesPerRadius; ++i)
        {
            var v1 = RadialToCartesian(outerRadius, 2 * PI * i / verticesPerRadius, new Vector2(cx, cy));
            arr[idx++] = v1.X;
            arr[idx++] = v1.Y;
            var v2 = RadialToCartesian(innerRadius, 2 * PI * (2 * i + 1) / (2 * verticesPerRadius),
                new Vector2(cx, cy));
            arr[idx++] = v2.X;
            arr[idx++] = v2.Y;
        }

        return FromVertices(arr, outerRounding ?? CornerRounding.Unrounded, perVertex, cx, cy);
    }
}