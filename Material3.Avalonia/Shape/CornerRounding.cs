namespace Material3.Avalonia.Shape;

public sealed class CornerRounding(float radius = 0f, float smoothing = 0f)
{
    public float Radius { get; } = MathF.Max(0f, radius);
    public float Smoothing { get; } = Math.Clamp(smoothing, 0f, 1f);

    public static readonly CornerRounding Unrounded = new();
}