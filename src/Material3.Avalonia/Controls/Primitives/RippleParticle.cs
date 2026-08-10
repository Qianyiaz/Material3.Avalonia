using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;

namespace Material3.Avalonia.Controls.Primitives;

internal sealed class RippleParticle : Animatable
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RippleParticle, double>(nameof(Radius));

    public static readonly StyledProperty<double> OpacityProperty =
        AvaloniaProperty.Register<RippleParticle, double>(nameof(Opacity));

    private readonly DoubleTransition _opacityTransition = new() { Property = OpacityProperty };

    private readonly DoubleTransition _radiusTransition = new() { Property = RadiusProperty };

    public RippleParticle()
    {
        Transitions = new Transitions { _radiusTransition, _opacityTransition };
    }

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public double Opacity
    {
        get => GetValue(OpacityProperty);
        set => SetValue(OpacityProperty, value);
    }

    public Point Center { get; init; }
    public double MaxRadius { get; init; }

    public void ConfigureGrow(TimeSpan growDuration, TimeSpan fadeInDuration, Easing? growEasing)
    {
        _radiusTransition.Duration = growDuration;
        _radiusTransition.Easing = growEasing ?? new CubicEaseOut();
        _opacityTransition.Duration = fadeInDuration;
    }

    public void ConfigureFadeOut(TimeSpan fadeOutDuration)
    {
        _opacityTransition.Duration = fadeOutDuration;
    }
}