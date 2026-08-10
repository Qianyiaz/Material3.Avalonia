namespace Material3.Avalonia.Motion;

public static class MotionSettings
{
    private static readonly List<WeakReference<EventHandler>> _globalSchemeChanged = new();

    public static MotionScheme GlobalScheme
    {
        get;
        set
        {
            if (!ReferenceEquals(field, value))
            {
                field = value;
                RaiseGlobalSchemeChanged();
            }
        }
    } = MotionScheme.Standard;

    public static bool ReduceMotion { get; set; } = false;

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
}