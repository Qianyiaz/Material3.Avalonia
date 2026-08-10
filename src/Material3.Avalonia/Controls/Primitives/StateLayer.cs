using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;

namespace Material3.Avalonia.Controls.Primitives;

public class StateLayer : Control
{
    public static readonly StyledProperty<IBrush?> BrushProperty =
        AvaloniaProperty.Register<StateLayer, IBrush?>(nameof(Brush));

    public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.Register<StateLayer, CornerRadius>(nameof(CornerRadius));

    static StateLayer()
    {
        AffectsRender<StateLayer>(BrushProperty, CornerRadiusProperty);
    }

    public StateLayer()
    {
        IsHitTestVisible = false;
        Transitions =
        [
            new DoubleTransition()
            {
                Property = OpacityProperty,
                Duration = TimeSpan.FromMilliseconds(100)
            }
        ];
    }

    public IBrush? Brush
    {
        get => GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        if (Brush is null || Bounds.Width <= 0 || Bounds.Height <= 0) return;

        var rect = new Rect(0, 0, Bounds.Width, Bounds.Height);

        if (CornerRadius == default)
        {
            context.FillRectangle(Brush, rect);
        }
        else
        {
            var roundedRect = new RoundedRect(rect, CornerRadius);
            context.DrawRectangle(Brush, null, roundedRect);
        }
    }
}