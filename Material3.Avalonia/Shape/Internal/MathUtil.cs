using System.Numerics;
using System.Runtime.CompilerServices;

namespace Material3.Avalonia.Shape.Internal;

using static MathF;

internal static class MathUtil
{
    public const float DistanceEpsilon = 1e-4f;
    public const float DistanceEpsilonSquared = DistanceEpsilon * DistanceEpsilon;
    public const float AngleEpsilon = 1e-6f;
    public const float RelaxedDistanceEpsilon = 5e-3f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(float dx, float dy) => Sqrt(dx * dx + dy * dy);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    
    public static float DistanceSquare(float dx, float dy) => dx * dx + dy * dy;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Direction(float angleRad) => new(Cos(angleRad), Sin(angleRad));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RadialToCartesian(float radius, float angleRad, Vector2? center = null)
        => (center ?? Vector2.Zero) + Direction(angleRad) * radius;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Convex(Vector2 previous, Vector2 current, Vector2 next)
    {
        var v1 = current - previous;
        var v2 = next - current;
        return v1.CrossZ(v2) >= 0f;
    }
}