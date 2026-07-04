using Microsoft.UI.Xaml;
using Wintix.Services;
using Wintix.Localization;

namespace Wintix;

public partial class App : Application
{
    private Window? _window;

    public static MainWindow? MainWindow { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        LocalizationService.Instance.Initialize(SettingsService.Instance.Current);
        MainWindow = new MainWindow();
        _window = MainWindow;
        _window.Activate();
    }
}
