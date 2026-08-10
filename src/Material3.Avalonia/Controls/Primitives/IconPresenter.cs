using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Material3.Avalonia.Controls.Primitives;

public class IconPresenter : ContentControl
{
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<IconPresenter, object?>(nameof(Value));

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<IconPresenter, double>(nameof(Size), 18d);

    static IconPresenter()
    {
        ValueProperty.Changed.AddClassHandler<IconPresenter>((x, _) => x.UpdateVisual());
        SizeProperty.Changed.AddClassHandler<IconPresenter>((x, _) => x.UpdateVisual());
        ForegroundProperty.Changed.AddClassHandler<IconPresenter>((x, _) => x.UpdateVisual());
    }

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Content = Value switch
        {
            null => null,
            Geometry geometry => MakeVector(geometry),
            IImage image => MakeBitmap(image),
            Uri u => TryLoadBitmap(u, out var fromUri) ? MakeBitmap(fromUri!) : Fallback(u.ToString()),
            string s => BuildFromString(s),
            Control c => c,
            _ => Fallback(Value.ToString() ?? "icon")
        };
    }

    private static bool TryParseGeometry(string s, out Geometry? g)
    {
        try
        {
            g = Geometry.Parse(s);
            return true;
        }
        catch
        {
            g = null;
            return false;
        }
    }

    private static bool TryLoadBitmap(Uri uri, out Bitmap? bmp)
    {
        try
        {
            if (uri.IsAbsoluteUri && uri.Scheme.Equals("avares", StringComparison.OrdinalIgnoreCase))
            {
                if (AssetLoader.Exists(uri))
                {
                    using var stream = AssetLoader.Open(uri);
                    bmp = new Bitmap(stream);
                    return true;
                }
            }

            if (uri.IsAbsoluteUri && uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                var path = uri.LocalPath;
                if (File.Exists(path))
                {
                    bmp = new Bitmap(path);
                    return true;
                }
            }

            bmp = null;
            return false;
        }
        catch
        {
            bmp = null;
            return false;
        }
    }

    private static bool TryLoadBitmapPath(string path, out Bitmap? bmp)
    {
        try
        {
            if (File.Exists(path))
            {
                bmp = new Bitmap(path);
                return true;
            }

            bmp = null;
            return false;
        }
        catch
        {
            bmp = null;
            return false;
        }
    }

    private Control BuildFromString(string s)
    {
        if (TryParseGeometry(s, out var geom))
            return MakeVector(geom!);

        if (Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out var uri) && TryLoadBitmap(uri, out var fromUri))
            return MakeBitmap(fromUri!);

        // 3) Абсолютный/относительный путь к файлу
        if (TryLoadBitmapPath(s, out var fromPath))
            return MakeBitmap(fromPath!);

        return Fallback(s);
    }

    private PathIcon MakeVector(Geometry g) => new PathIcon
    {
        Data = g,
        Width = Size,
        Height = Size,
        Foreground = Foreground
    };

    private Image MakeBitmap(IImage img) => new Image
    {
        Source = img,
        Stretch = Stretch.Uniform,
        Width = Size,
        Height = Size
    };

    private TextBlock Fallback(string txt) => new TextBlock
    {
        Text = txt,
        FontSize = Math.Max(10, Size * 0.7),
        VerticalAlignment = VerticalAlignment.Center,
        HorizontalAlignment = HorizontalAlignment.Center
    };
}