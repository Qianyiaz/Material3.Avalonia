using System.ComponentModel;
using System.Globalization;

namespace Material3.Avalonia.Theme;

public sealed class ContrastTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string str)
        {
            var text = str.Trim();

            if (double.TryParse(text, NumberStyles.Float, culture, out var level))
                return new Contrast(level);

            if (Enum.TryParse<Contrast.Preset>(text, true, out var preset))
                return (Contrast)preset;

            throw new FormatException(
                $"Invalid Contrast value: '{str}'. Use a number (e.g. 0.5) or one of: Reduced, Standard, Medium, High.");
        }

        return base.ConvertFrom(context, culture, value);
    }
}