using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;
using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Scheme;
using Material3.Avalonia.Tokens.Color;
using Material3.Avalonia.Tokens.Elevation;

namespace Material3.Avalonia.Theme;

public class MaterialTheme : Styles
{
    public static readonly StyledProperty<Color> SourceColorProperty = 
        AvaloniaProperty.Register<MaterialTheme, Color>(nameof(SourceColor), MaterialThemeOptions.Defaults.SourceColor);
    
    public static readonly StyledProperty<DynamicSchemeVariant> VariantProperty = 
        AvaloniaProperty.Register<MaterialTheme, DynamicSchemeVariant>(nameof(Variant), MaterialThemeOptions.Defaults.Variant);
    
    public static readonly StyledProperty<ThemeMode> ModeProperty =
        AvaloniaProperty.Register<MaterialTheme, ThemeMode>(nameof(Mode), MaterialThemeOptions.Defaults.Mode);
    
    public static readonly StyledProperty<Contrast> ContrastProperty =
        AvaloniaProperty.Register<MaterialTheme, Contrast>(nameof(Contrast), MaterialThemeOptions.Defaults.Contrast);

    public Color SourceColor
    {
        get => GetValue(SourceColorProperty);
        set => SetValue(SourceColorProperty, value);
    }

    public DynamicSchemeVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
    
    public ThemeMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }
    
    public Contrast Contrast
    {
        get => GetValue(ContrastProperty);
        set => SetValue(ContrastProperty, value);
    }

    public MaterialThemeOptions Options
    {
        get => new(SourceColor, Variant, Mode, Contrast);
        set
        {
            SourceColor = value.SourceColor;
            Variant = value.Variant;
            Mode = value.Mode;
            Contrast = value.Contrast;
        }
    }

    private readonly IPlatformSettings? _platformSettings;
    private bool _isSubscribedToSystem;
    
    public MaterialTheme()
    {
        AvaloniaXamlLoader.Load(this);
        
        TryAdoptSystemAccent();
        
        OwnerChanged += (_, _) => UpdateSystemSubscription();
        
        _platformSettings = Application.Current?.PlatformSettings;
        
        UpdateSystemSubscription();
        Rebuild();
    }

    private void OnSystemColorValuesChanged(object? s, PlatformColorValues e)
    {
        if (Mode == ThemeMode.System)
            Rebuild();
    }

    private void TryAdoptSystemAccent()
    {
        try
        {
            var accentColor = Application.Current?.PlatformSettings?.GetColorValues().AccentColor1;
            if (accentColor is { } color)
                SetCurrentValue(SourceColorProperty, color);
        }
        catch
        {
            // ignored
        }
    }

    private void Rebuild()
    {
        var isDark = ResolveIsDark();
        var hct = Hct.FromInt(Options.SourceColor.ToUInt32());
        var scheme = DynamicSchemeMap.GetDynamicScheme(hct, isDark, Options.Contrast.Level, Options.Variant);
        
        SystemColorResourceWriter.Rebuild(Resources, scheme);
        ShadowResourceWriter.Rebuild(Resources, scheme);
        
        Resources["Material.DynamicScheme"] = scheme;
        Resources["Material.IsDark"] = isDark;
        Resources["Material.ContrastLevel"] = Options.Contrast.Level;
        Resources["Material.SourceColor"] = Options.SourceColor;
        Resources["Material.SchemeVariant"] = Options.Variant;
    }
    
    private bool ResolveIsDark()
    {
        return Mode switch
        {
            ThemeMode.Dark => true,
            ThemeMode.Light => false,
            ThemeMode.System => GetSystemIsDark() ?? false,
            _ => false
        };
    }
    
    private bool? GetSystemIsDark()
    {
        try
        {
            var values = _platformSettings?.GetColorValues();
            return values?.ThemeVariant switch
            {
                PlatformThemeVariant.Dark => true,
                PlatformThemeVariant.Light => false,
                _ => null
            };
        }
        catch
        {
            // ignored
        }
        return null;
    }
    
    private void UpdateSystemSubscription()
    {
        if (_platformSettings is null)
            return;

        var want = this.Owner is not null && Mode == ThemeMode.System;

        if (want && !_isSubscribedToSystem)
        {
            _platformSettings.ColorValuesChanged += OnSystemColorValuesChanged;
            _isSubscribedToSystem = true;
        }
        else if (!want && _isSubscribedToSystem)
        {
            _platformSettings.ColorValuesChanged -= OnSystemColorValuesChanged;
            _isSubscribedToSystem = false;
        }
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property == ModeProperty)
            UpdateSystemSubscription();
        
        if (change.Property == SourceColorProperty
            || change.Property == VariantProperty
            || change.Property == ModeProperty
            || change.Property == ContrastProperty)
            Rebuild();
    }
}