using Avalonia;
using Avalonia.Media;

namespace Material3.Avalonia.Motion;

public interface ISpringValueAdapter<T>
{
    int Components { get; }
    void Read(T value, Span<double> into);
    T Make(ReadOnlySpan<double> components);
}

public sealed class DoubleAdapter : ISpringValueAdapter<double>
{
    public static readonly DoubleAdapter Instance = new();
    public int Components => 1;
    public void Read(double value, Span<double> into) => into[0] = value;
    public double Make(ReadOnlySpan<double> components) => components[0];
}

public sealed class PointAdapter : ISpringValueAdapter<Point>
{
    public static readonly PointAdapter Instance = new();
    public int Components => 2;
    public void Read(Point value, Span<double> into)
    {
        into[0] = value.X;
        into[1] = value.Y;
    }
    public Point Make(ReadOnlySpan<double> components) => new(components[0], components[1]);
}

public sealed class VectorAdapter : ISpringValueAdapter<Vector>
{
    public static readonly VectorAdapter Instance = new();
    public int Components => 2;
    public void Read(Vector value, Span<double> into)
    {
        into[0] = value.X;
        into[1] = value.Y;
    }
    public Vector Make(ReadOnlySpan<double> components) => new(components[0], components[1]);
}

public sealed class ThicknessAdapter : ISpringValueAdapter<Thickness>
{
    public static readonly ThicknessAdapter Instance = new();
    public int Components => 4;
    public void Read(Thickness value, Span<double> into)
    {
        into[0] = value.Left;
        into[1] = value.Top;
        into[2] = value.Right;
        into[3] = value.Bottom;
    }
    public Thickness Make(ReadOnlySpan<double> components) => new(components[0], components[1], components[2], components[3]);
}

public sealed class CornerRadiusAdapter : ISpringValueAdapter<CornerRadius>
{
    public static readonly CornerRadiusAdapter Instance = new();
    public int Components => 4;
    public void Read(CornerRadius value, Span<double> into)
    {
        into[0] = value.TopLeft;
        into[1] = value.TopRight;
        into[2] = value.BottomRight;
        into[3] = value.BottomLeft;
    }
    public CornerRadius Make(ReadOnlySpan<double> components) => new(components[0], components[1], components[2], components[3]);
}

public sealed class ColorAdapter : ISpringValueAdapter<Color>
{
    public static readonly ColorAdapter Instance = new();
    public int Components => 4;
    public void Read(Color value, Span<double> into)
    {
        into[0] = value.A;
        into[1] = value.R;
        into[2] = value.G;
        into[3] = value.B;
    }
    public Color Make(ReadOnlySpan<double> components) => Color.FromArgb((byte)components[0], (byte)components[1], (byte)components[2], (byte)components[3]);   
}