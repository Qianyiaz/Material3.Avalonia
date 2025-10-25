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

using Material3.Avalonia.Shape.Internal;

namespace Material3.Avalonia.Shape;

using static MathUtil;

public abstract class Feature(IReadOnlyList<Cubic> cubics)
{
    public IReadOnlyList<Cubic> Cubics { get; } = cubics;

    public abstract bool IsEdge { get; }
    public abstract bool IsConvexCorner { get; }
    public abstract bool IsConcaveCorner { get; }
    public abstract bool IsIgnorableFeature { get; }
    public abstract Feature Reverse();

    protected static void ValidateContinuous(IReadOnlyList<Cubic> cubics)
    {
        if (cubics.Count == 0) throw new ArgumentException("Feature must have at least one cubic");

        var prev = cubics[0];
        for (var i = 1; i < cubics.Count; ++i)
        {
            var c = cubics[i];
            if  (MathF.Abs(c.A0.X - prev.A1.X) > DistanceEpsilon ||
                 Math.Abs(c.A0.Y - prev.A1.Y) > DistanceEpsilon)
                throw new ArgumentException("Feature cubics must be continuous");

            prev = c;
        }
    }

    public sealed class Edge : Feature
    {
        public Edge(IReadOnlyList<Cubic> cubics) : base(cubics)
        {
            ValidateContinuous(cubics);
        }

        public override bool IsEdge => true;
        public override bool IsConvexCorner => false;
        public override bool IsConcaveCorner => false;
        public override bool IsIgnorableFeature => true;

        public override Feature Reverse()
        {
            var rev = new List<Cubic>(Cubics.Count);
            for (var i = Cubics.Count - 1; i >= 0; --i)
                rev.Add(Cubics[i].Reverse());
            return new Edge(rev);
        }

        public static Edge Build(Cubic c) => new(new[] { c });
    }

    public sealed class Corner : Feature
    {
        public Corner(IReadOnlyList<Cubic> cubics, bool convex) : base(cubics)
        {
            ValidateContinuous(cubics);
            Convex = convex;
        }
        
        public bool Convex { get; }
        public override bool IsEdge => false;
        public override bool IsConvexCorner => Convex;
        public override bool IsConcaveCorner => !Convex;
        public override bool IsIgnorableFeature => false;

        public override Feature Reverse()
        {
            var rev = new List<Cubic>(Cubics.Count);
            for (var i = Cubics.Count - 1; i >= 0; --i)
                rev.Add(Cubics[i].Reverse());
            return new Corner(rev, !Convex);
        }
    }
}