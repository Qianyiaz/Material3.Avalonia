using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Material3.Avalonia.Attached;

public static class FocusRing
{
    public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.RegisterAttached<Control, CornerRadius>("CornerRadius", typeof(FocusRing),
            new CornerRadius(0));

    public static readonly AttachedProperty<Thickness> ThicknessProperty =
        AvaloniaProperty.RegisterAttached<Control, Thickness>("Thickness", typeof(FocusRing), new Thickness(3));

    public static readonly AttachedProperty<Thickness> OffsetProperty =
        AvaloniaProperty.RegisterAttached<Control, Thickness>("Offset", typeof(FocusRing), new Thickness(2));

    public static readonly AttachedProperty<IBrush?> BrushProperty =
        AvaloniaProperty.RegisterAttached<Control, IBrush?>("Brush", typeof(FocusRing));

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly AttachedProperty<Thickness> EffectiveMarginProperty =
        AvaloniaProperty.RegisterAttached<Control, Thickness>("EffectiveMargin", typeof(FocusRing), new Thickness(0));

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly AttachedProperty<CornerRadius> EffectiveCornerRadiusProperty =
        AvaloniaProperty.RegisterAttached<Control, CornerRadius>("EffectiveCornerRadius", typeof(FocusRing),
            new CornerRadius(0));


    static FocusRing()
    {
        ThicknessProperty.Changed.AddClassHandler<Control>((o, _) => Recalculate(o));
        OffsetProperty.Changed.AddClassHandler<Control>((o, _) => Recalculate(o));
        CornerRadiusProperty.Changed.AddClassHandler<Control>((o, _) => Recalculate(o));
    }

    public static void SetCornerRadius(AvaloniaObject o, CornerRadius v) => o.SetValue(CornerRadiusProperty, v);
    public static CornerRadius GetCornerRadius(AvaloniaObject o) => o.GetValue(CornerRadiusProperty);
    public static void SetThickness(AvaloniaObject o, Thickness v) => o.SetValue(ThicknessProperty, v);
    public static Thickness GetThickness(AvaloniaObject o) => o.GetValue(ThicknessProperty);
    public static void SetOffset(AvaloniaObject o, Thickness v) => o.SetValue(OffsetProperty, v);
    public static Thickness GetOffset(AvaloniaObject o) => o.GetValue(OffsetProperty);
    public static void SetBrush(AvaloniaObject o, IBrush? v) => o.SetValue(BrushProperty, v);
    public static IBrush? GetBrush(AvaloniaObject o) => o.GetValue(BrushProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void SetEffectiveMargin(AvaloniaObject o, Thickness v) => o.SetValue(EffectiveMarginProperty, v);

    public static Thickness GetEffectiveMargin(AvaloniaObject o) => o.GetValue(EffectiveMarginProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void SetEffectiveCornerRadius(AvaloniaObject o, CornerRadius v) =>
        o.SetValue(EffectiveCornerRadiusProperty, v);

    public static CornerRadius GetEffectiveCornerRadius(AvaloniaObject o) => o.GetValue(EffectiveCornerRadiusProperty);

    private static void Recalculate(Control c)
    {
        var t = GetThickness(c);
        var d = GetOffset(c);
        var baseCr = GetCornerRadius(c);

        SetEffectiveMargin(c,
            new Thickness(-t.Left - d.Left, -t.Top - d.Top, -t.Right - d.Right, -t.Bottom - d.Bottom));

        var addL = t.Left / 2f + d.Left;
        var addT = t.Top / 2f + d.Top;
        var addR = t.Right / 2f + d.Right;
        var addB = t.Bottom / 2f + d.Bottom;

        SetEffectiveCornerRadius(c, new CornerRadius(
            baseCr.TopLeft + Math.Max(addL, addT),
            baseCr.TopRight + Math.Max(addR, addT),
            baseCr.BottomRight + Math.Max(addR, addB),
            baseCr.BottomLeft + Math.Max(addL, addB)
        ));
    }
}