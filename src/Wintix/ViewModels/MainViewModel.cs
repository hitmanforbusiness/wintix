using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wintix.Localization;
using Wintix.Models;

namespace Wintix.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedNavigationKey = "Dashboard";

    [ObservableProperty]
    private string _topBarTitle = string.Empty;

    [ObservableProperty]
    private string _topBarSubtitle = string.Empty;

    [ObservableProperty]
    private string _appTagline = string.Empty;

    [ObservableProperty]
    private string _appFooter = string.Empty;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private string _systemReadyText = string.Empty;

    [ObservableProperty]
    private string _settingsShortcutText = string.Empty;

    public ObservableCollection<NavigationItem> NavigationItems { get; } = [];

    public MainViewModel()
    {
        LocalizationService.Instance.LanguageChanged += (_, _) => RefreshLocalizedStrings();
        RefreshLocalizedStrings();
    }

    public void SelectNavigation(string key)
    {
        SelectedNavigationKey = key;
        TopBarSubtitle = NavigationItems.FirstOrDefault(n => n.Key == key)?.Label ?? key;
        TopBarTitle = key switch
        {
            "Dashboard" => L.T("Title.Dashboard"),
            "SmartCleaner" => L.T("Title.SmartCleaner"),
            "Performance" => L.T("Title.Performance"),
            "Privacy" => L.T("Title.Privacy"),
            "Startup" => L.T("Title.Startup"),
            "Network" => L.T("Title.Network"),
            "Scheduler" => L.T("Title.Scheduler"),
            "Logs" => L.T("Title.Logs"),
            "Settings" => L.T("Title.Settings"),
            _ => key
        };
    }

    private void RefreshLocalizedStrings()
    {
        AppTagline = L.T("App.Tagline");
        AppFooter = L.T("App.Footer");
        AppVersion = L.T("App.Version");
        SystemReadyText = L.T("App.SystemReady");
        SettingsShortcutText = L.T("Nav.Settings");

        NavigationItems.Clear();
        NavigationItems.Add(new NavigationItem { Key = "Dashboard", Label = L.T("Nav.Dashboard"), IconGlyph = "\uE80F" });
        NavigationItems.Add(new NavigationItem { Key = "SmartCleaner", Label = L.T("Nav.SmartCleaner"), IconGlyph = "\uE74C" });
        NavigationItems.Add(new NavigationItem { Key = "Performance", Label = L.T("Nav.Performance"), IconGlyph = "\uE945" });
        NavigationItems.Add(new NavigationItem { Key = "Privacy", Label = L.T("Nav.Privacy"), IconGlyph = "\uE72E" });
        NavigationItems.Add(new NavigationItem { Key = "Startup", Label = L.T("Nav.Startup"), IconGlyph = "\uE7B5" });
        NavigationItems.Add(new NavigationItem { Key = "Network", Label = L.T("Nav.Network"), IconGlyph = "\uE701" });
        NavigationItems.Add(new NavigationItem { Key = "Scheduler", Label = L.T("Nav.Scheduler"), IconGlyph = "\uE823" });
        NavigationItems.Add(new NavigationItem { Key = "Logs", Label = L.T("Nav.Logs"), IconGlyph = "\uE7C3" });
        NavigationItems.Add(new NavigationItem { Key = "Settings", Label = L.T("Nav.Settings"), IconGlyph = "\uE713" });

        SelectNavigation(SelectedNavigationKey);
    }
}
