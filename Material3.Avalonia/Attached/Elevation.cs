using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace Material3.Avalonia.Attached;

public enum ShadowMode
{
    Auto,
    Off,
    On
}

public static class Elevation
{
    public static readonly AttachedProperty<ShadowMode> ModeProperty =
        AvaloniaProperty.RegisterAttached<Control, ShadowMode>("Mode", typeof(Elevation),
            defaultValue: ShadowMode.Auto);
    public static void SetMode(Control control, ShadowMode value) => control.SetValue(ModeProperty, value);
    public static ShadowMode GetMode(Control control) => control.GetValue(ModeProperty);
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Avoid overriding component elevation. Prefer defaults from Material 3.")]
    public static readonly AttachedProperty<int?> RestingOverrideProperty =
        AvaloniaProperty.RegisterAttached<Control, int?>("RestingOverride", typeof(Elevation));
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Avoid overriding component elevation. Prefer defaults from Material 3.")]
    public static void SetRestingOverride(Control c, int? v)
        => c.SetValue(RestingOverrideProperty, v is { } x ? Math.Clamp(x, 0, 3) : v);
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Avoid overriding component elevation. Prefer defaults from Material 3.")]
    public static int? GetRestingOverride(Control c) => c.GetValue(RestingOverrideProperty);
}