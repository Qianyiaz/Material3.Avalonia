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
}