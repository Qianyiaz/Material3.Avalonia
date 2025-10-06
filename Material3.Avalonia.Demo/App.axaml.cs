using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Material3.Avalonia.Demo.ViewModels;
using Material3.Avalonia.Demo.Views;
using Material3.Avalonia.Tokens.Color.Reference;
using Material3.Avalonia.Tokens.Color.System;

namespace Material3.Avalonia.Demo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        var seed = Color.Parse("#6750A4");
        
        var tonalPalettes = HctRefGenerator.GenerateFromSeed(seed);
        var refColorsRd = tonalPalettes.ToColorResourceDictionary();
        Current!.Resources.MergedDictionaries.Add(refColorsRd);
        
        var options = new ColorSchemeOptions(ThemeVariant.Light, ContrastLevel.Standard, MdStyle.Standard);
        var sysColorsRd = ColorSchemeGenerator.Build(tonalPalettes, options);
        Current!.Resources.MergedDictionaries.Add(sysColorsRd);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}