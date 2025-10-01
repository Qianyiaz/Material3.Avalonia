using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Material3.Avalonia.Colors.Ref;
using Material3.Avalonia.Colors.Sys;
using Material3.Avalonia.Demo.ViewModels;
using Material3.Avalonia.Demo.Views;

namespace Material3.Avalonia.Demo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        var seed = Color.Parse("#6750A4");
        var (refColors, refBrushes) = HctRefGenerator.GenerateRefResources(seed);
        
        var refs = HctRefGenerator.GenerateFromSeed(seed);
        
        Current!.Resources.MergedDictionaries.Add(refColors);
        Current!.Resources.MergedDictionaries.Add(refBrushes);
        
        var options = new ColorSchemeOptions(ThemeVariant.Light, ContrastLevel.Standard, MdStyle.Standard);
        var sys = ColorSchemeGenerator.Build(refs, options);
        Current!.Resources.MergedDictionaries.Add(sys);
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