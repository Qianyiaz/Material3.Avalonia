using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Material3.Avalonia.Converters;

public sealed class FullCornerRadiusConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Rect rect)
        {
            var radius = 0.5 * Math.Min(rect.Width, rect.Height);
            return new CornerRadius(radius);
        }

        return new CornerRadius(0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => AvaloniaProperty.UnsetValue;
}