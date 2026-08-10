using Avalonia;
using Avalonia.Controls;

namespace Material3.Avalonia.Controls.Helpers;

public enum MdTextStyle
{
    DisplayLarge,
    DisplayMedium,
    DisplaySmall,
    HeadlineLarge,
    HeadlineMedium,
    HeadlineSmall,
    TitleLarge,
    TitleMedium,
    TitleSmall,
    BodyLarge,
    BodyMedium,
    BodySmall,
    LabelLarge,
    LabelMedium,
    LabelSmall,

    EmphasizedDisplayLarge,
    EmphasizedDisplayMedium,
    EmphasizedDisplaySmall,
    EmphasizedHeadlineLarge,
    EmphasizedHeadlineMedium,
    EmphasizedHeadlineSmall,
    EmphasizedTitleLarge,
    EmphasizedTitleMedium,
    EmphasizedTitleSmall,
    EmphasizedBodyLarge,
    EmphasizedBodyMedium,
    EmphasizedBodySmall,
    EmphasizedLabelLarge,
    EmphasizedLabelMedium,
    EmphasizedLabelSmall
}

public static class Typography
{
    public static readonly AttachedProperty<MdTextStyle?> TextStyleProperty =
        AvaloniaProperty.RegisterAttached<Control, MdTextStyle?>("TextStyle", typeof(Typography), null, true);

    public static void SetTextStyle(AvaloniaObject obj, MdTextStyle? value) => obj.SetValue(TextStyleProperty, value);
    public static MdTextStyle? GetTextStyle(AvaloniaObject obj) => obj.GetValue(TextStyleProperty);
}