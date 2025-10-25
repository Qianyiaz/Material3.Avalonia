using System.Numerics;
using System.Runtime.CompilerServices;

namespace Material3.Avalonia.Shape;

public static class Vector2Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Rotate90Ccw(this Vector2 v) => new(-v.Y,  v.X);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Rotate90Cw (this Vector2 v) => new( v.Y, -v.X);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CrossZ(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;
}