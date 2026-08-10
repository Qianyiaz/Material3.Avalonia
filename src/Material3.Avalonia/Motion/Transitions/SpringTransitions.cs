using Avalonia;
using Avalonia.Media;

namespace Material3.Avalonia.Motion.Transitions;

public sealed class SpringDoubleTransition : SpringTransitionBase<double, DoubleAdapter>;

public sealed class SpringPointTransition : SpringTransitionBase<Point, PointAdapter>;

public sealed class SpringVectorTransition : SpringTransitionBase<Vector, VectorAdapter>;

public sealed class SpringThicknessTransition : SpringTransitionBase<Thickness, ThicknessAdapter>;

public sealed class SpringCornerRadiusTransition : SpringTransitionBase<CornerRadius, CornerRadiusAdapter>;

public sealed class SpringColorTransition : SpringTransitionBase<Color, ColorAdapter>
{
    public SpringColorTransition()
    {
        Style = MotionStyle.Effects;
    }
}