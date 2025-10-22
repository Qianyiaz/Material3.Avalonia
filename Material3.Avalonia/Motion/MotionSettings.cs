using Avalonia;

namespace Material3.Avalonia.Motion;

public static class MotionSettings
{
    private static MotionScheme _globalScheme = MotionScheme.Standard;
    private static readonly List<WeakReference<EventHandler>> _globalSchemeChanged = new();
    
    public static event EventHandler? GlobalSchemeChanged
    {
        add
        {
            if (value is null) return;
            lock (_globalSchemeChanged)
                _globalSchemeChanged.Add(new WeakReference<EventHandler>(value));
        }
        remove
        {
            if (value is null) return;
            lock (_globalSchemeChanged)
                _globalSchemeChanged.RemoveAll(w => !w.TryGetTarget(out var handler) || handler == value);
        }
    }
    
    public static MotionScheme GlobalScheme
    {
        get => _globalScheme;
        set
        {
            if (!ReferenceEquals(_globalScheme, value))
            {
                _globalScheme = value;
                RaiseGlobalSchemeChanged();
            }
        }
    }

    private static void RaiseGlobalSchemeChanged()
    {
        WeakReference<EventHandler>[] snapshot;
        lock (_globalSchemeChanged)
            snapshot = _globalSchemeChanged.ToArray();
        
        foreach (var weakRef in snapshot)
            if (weakRef.TryGetTarget(out var handler))
                handler.Invoke(null, EventArgs.Empty);
        
        lock (_globalSchemeChanged)
            _globalSchemeChanged.RemoveAll(w => !w.TryGetTarget(out _));
    }

    public static bool ReduceMotion { get; set; } = false;

    public static readonly AttachedProperty<MotionScheme?> SchemeProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, MotionScheme?>("Scheme", typeof(MotionSettings));
    
    public static void SetScheme(AvaloniaObject obj, MotionScheme? value) => obj.SetValue(SchemeProperty, value);
    public static MotionScheme? GetScheme(AvaloniaObject obj) => obj.GetValue(SchemeProperty);
    
    public static MotionScheme Resolve(AvaloniaObject obj) => GetScheme(obj) ?? GlobalScheme;
}