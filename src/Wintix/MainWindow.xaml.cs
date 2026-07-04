using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Wintix.Controls;
using Wintix.Localization;
using Wintix.Services;
using Wintix.ViewModels;
using Wintix.Views;
using WinRT.Interop;

namespace Wintix;

public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; } = new();
    private string? _currentNavigationKey;

    public MainWindow()
    {
        InitializeComponent();
        Title = "Wintix";
        ConfigureWindowChrome();
        NavRepeater.ElementPrepared += NavRepeater_ElementPrepared;
        ContentFrame.Navigate(typeof(DashboardPage));
        ViewModel.SelectNavigation("Dashboard");
        UpdateNavSelection();
        ActivityLogService.Instance.Info("System", "Wintix started.");
        LocalizationService.Instance.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        UpdateNavSelection();
        var key = _currentNavigationKey ?? "Dashboard";
        _currentNavigationKey = null;
        NavigateToKey(key);
    }

    private void NavRepeater_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
    {
        if (args.Element is SidebarNavItem navItem)
        {
            navItem.IsActive = navItem.NavigationKey == ViewModel.SelectedNavigationKey;
            navItem.ItemClick -= NavItem_ItemClick;
            navItem.ItemClick += NavItem_ItemClick;
        }
    }

    private void UpdateNavSelection()
    {
        for (var i = 0; i < ViewModel.NavigationItems.Count; i++)
        {
            if (NavRepeater.TryGetElement(i) is SidebarNavItem navItem)
            {
                navItem.IsActive = navItem.NavigationKey == ViewModel.SelectedNavigationKey;
            }
        }
    }

    private void ConfigureWindowChrome()
    {
        var hwnd = WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow is not null)
        {
            appWindow.Resize(new Windows.Graphics.SizeInt32(1360, 860));
            if (appWindow.TitleBar is not null)
            {
                appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
                appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appWindow.TitleBar.BackgroundColor = ColorHelper.FromArgb(255, 10, 16, 25);
                appWindow.TitleBar.ForegroundColor = ColorHelper.FromArgb(255, 241, 245, 249);
                appWindow.TitleBar.InactiveForegroundColor = ColorHelper.FromArgb(255, 148, 163, 184);
            }
        }

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TopBar);
    }

    private void NavItem_ItemClick(object? sender, TappedRoutedEventArgs e)
    {
        if (sender is SidebarNavItem navItem)
        {
            NavigateToKey(navItem.NavigationKey);
        }
    }

    private void SettingsShortcut_Click(object sender, RoutedEventArgs e) => NavigateToModule("Settings");

    public void NavigateToModule(string key) => NavigateToKey(key);

    private void NavigateToKey(string key)
    {
        ViewModel.SelectNavigation(key);

        if (_currentNavigationKey == key)
        {
            return;
        }

        _currentNavigationKey = key;
        UpdateNavSelection();
        ContentFrame.Navigate(ResolvePageType(key));
    }

    private static Type ResolvePageType(string key) => key switch
    {
        "Dashboard" => typeof(DashboardPage),
        "SmartCleaner" => typeof(SmartCleanerPage),
        "Performance" => typeof(PerformancePage),
        "Privacy" => typeof(PrivacyPage),
        "Startup" => typeof(StartupPage),
        "Network" => typeof(NetworkPage),
        "Scheduler" => typeof(SchedulerPage),
        "Logs" => typeof(ActivityLogsPage),
        "Settings" => typeof(SettingsPage),
        _ => typeof(DashboardPage)
    };
}
