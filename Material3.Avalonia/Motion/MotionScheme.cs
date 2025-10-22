namespace Material3.Avalonia.Motion;

public sealed class MotionScheme
{
    // Spatial
    public SpringToken SpatialFast { get; init; }
    public SpringToken SpatialDefault { get; init; }
    public SpringToken SpatialSlow { get; init; }
    
    // Effects
    public SpringToken EffectsFast { get; init; }
    public SpringToken EffectsDefault { get; init; }
    public SpringToken EffectsSlow { get; init; }

    public SpringToken Resolve(MotionStyle style, MotionSpeed speed) => (style, speed) switch
    {
        (MotionStyle.Spatial, MotionSpeed.Fast) => SpatialFast,
        (MotionStyle.Spatial, MotionSpeed.Default) => SpatialDefault,
        (MotionStyle.Spatial, MotionSpeed.Slow) => SpatialSlow,
        (MotionStyle.Effects, MotionSpeed.Fast) => EffectsFast,
        (MotionStyle.Effects, MotionSpeed.Default) => EffectsDefault,
        (MotionStyle.Effects, MotionSpeed.Slow) => EffectsSlow,
        _ => EffectsDefault
    };

    public static MotionScheme Standard => new()
    {
        SpatialFast = new SpringToken(1400, 0.9),
        SpatialDefault = new SpringToken(700, 0.9),
        SpatialSlow = new SpringToken(300, 0.9),

        EffectsFast = new SpringToken(3800, 1.0),
        EffectsDefault = new SpringToken(1600, 1.0),
        EffectsSlow = new SpringToken(800, 1.0)
    };

    public static MotionScheme Expressive => new()
    {
        SpatialFast = new SpringToken(800, 0.6),
        SpatialDefault = new SpringToken(380, 0.8),
        SpatialSlow = new SpringToken(200, 0.8),

        EffectsFast = new SpringToken(3800, 1.0),
        EffectsDefault = new SpringToken(1600, 1.0),
        EffectsSlow = new SpringToken(800, 1.0)
    };
}