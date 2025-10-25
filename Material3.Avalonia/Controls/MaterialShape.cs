using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Material3.Avalonia.Shape;

namespace Material3.Avalonia.Controls;

public enum MaterialShapeKind
{
    Circle,
    Square,
    Slanted,
    Arch, // I use Arch, btw
    Semicircle,
    Oval,
    Pill,
    Triangle,
    Arrow,
    Fan,
    Diamond,
    Clamshell,
    Pentagon,
    Gem,
    VerySunny,
    Sunny,
    Cookie4,
    Cookie6,
    Cookie7,
    Cookie9,
    Cookie12,
    Clover4,
    Clover8,
    Burst,
    SoftBurst,
    Boom,
    SoftBoom,
    Flower,
    Puffy,
    PuffyDiamond,
    Ghostish,
    PixelCircle,
    PixelTriangle,
    Bun,
    Heart
}

public sealed class MaterialShape : Control
{
    public static readonly StyledProperty<MaterialShapeKind> KindProperty =
        AvaloniaProperty.Register<MaterialShape, MaterialShapeKind>(nameof(Kind));

    public static readonly StyledProperty<IBrush?> FillProperty =
        AvaloniaProperty.Register<MaterialShape, IBrush?>(nameof(Fill));

    public static readonly StyledProperty<IBrush?> StrokeProperty =
        AvaloniaProperty.Register<MaterialShape, IBrush?>(nameof(Stroke));

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<MaterialShape, double>(nameof(StrokeThickness), 0d);

    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<MaterialShape, Stretch>(nameof(Stretch), Stretch.Uniform);

    public static readonly StyledProperty<Thickness> InsetProperty =
        AvaloniaProperty.Register<MaterialShape, Thickness>(nameof(Inset), new Thickness(0));

    public static readonly StyledProperty<PenLineJoin> StrokeJoinProperty =
        AvaloniaProperty.Register<MaterialShape, PenLineJoin>(nameof(StrokeJoin), PenLineJoin.Round);

    public MaterialShapeKind Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public IBrush? Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public Thickness Inset
    {
        get => GetValue(InsetProperty);
        set => SetValue(InsetProperty, value);
    }

    public PenLineJoin StrokeJoin
    {
        get => GetValue(StrokeJoinProperty);
        set => SetValue(StrokeJoinProperty, value);
    }

    private Geometry? _cachedGeometry;
    private Size _cachedSize;
    private MaterialShapeKind _cachedKind;

    public override void Render(DrawingContext ctx)
    {
        base.Render(ctx);
        
        var rect = new Rect(Bounds.Size).Deflate(Inset);
        if (rect.Width <= 0 || rect.Height <= 0) return;

        if (_cachedGeometry is null || _cachedSize != rect.Size || _cachedKind != Kind)
        {
            var unit = ShapesEngine.GetShape(Kind);
            _cachedGeometry = ShapesEngine.BuildGeometry(unit, rect, Stretch);
            _cachedSize = rect.Size;
            _cachedKind = Kind;
        }
        
        var pen  = StrokeThickness > 0 && Stroke is not null
                   ? new Pen(Stroke, StrokeThickness) { LineJoin = StrokeJoin }
                   : null;

        ctx.DrawGeometry(Fill, pen, _cachedGeometry);
    }
}
