using Avalonia;
using Avalonia.Controls;

namespace Material3.Avalonia.Attached.Controls;

public enum ButtonVariant
{
    Filled,
    Elevated,
    Tonal,
    Outlined,
    Text
}

public enum ButtonSize
{
    ExtraSmall,
    Small,
    Medium,
    Large,
    ExtraLarge
}

public enum ButtonShape
{
    Round,
    Square
}

public static class ButtonAssist
{
    public static readonly AttachedProperty<object?> IconProperty =
        AvaloniaProperty.RegisterAttached<Button, object?>(
            "Icon", typeof(ButtonAssist));

    public static void SetIcon(Button b, object? value) => b.SetValue(IconProperty, value);
    public static object? GetIcon(Button b) => b.GetValue(IconProperty);
    
    public static readonly AttachedProperty<ButtonVariant> VariantProperty =
        AvaloniaProperty.RegisterAttached<Button, ButtonVariant>(
            "Variant", typeof(ButtonAssist), defaultValue: ButtonVariant.Filled);
    
    public static void SetVariant(Button b, ButtonVariant value) => b.SetValue(VariantProperty, value);
    public static ButtonVariant GetVariant(Button b) => b.GetValue(VariantProperty);

    public static readonly AttachedProperty<ButtonSize> SizeProperty =
        AvaloniaProperty.RegisterAttached<Button, ButtonSize>(
            "Size", typeof(ButtonAssist), defaultValue: ButtonSize.Small);
    
    public static void SetSize(Button b, ButtonSize value) => b.SetValue(SizeProperty, value);
    public static ButtonSize GetSize(Button b) => b.GetValue(SizeProperty);
    
    public static readonly AttachedProperty<ButtonShape> ShapeProperty =
        AvaloniaProperty.RegisterAttached<Button, ButtonShape>(
            "Shape", typeof(ButtonAssist), defaultValue: ButtonShape.Round);
    
    public static void SetShape(Button b, ButtonShape value) => b.SetValue(ShapeProperty, value);
    public static ButtonShape GetShape(Button b) => b.GetValue(ShapeProperty);
}