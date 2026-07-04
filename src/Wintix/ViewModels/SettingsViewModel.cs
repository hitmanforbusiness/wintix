using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wintix.Localization;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class SettingsViewModel : LocalizedViewModelBase
{
    private readonly SettingsService _settingsService = SettingsService.Instance;

    [ObservableProperty]
    private bool _useDarkTheme;

    [ObservableProperty]
    private bool _accentGlowEffects;

    [ObservableProperty]
    private bool _systemHealthAlerts;

    [ObservableProperty]
    private bool _scheduledScanReminders;

    [ObservableProperty]
    private bool _launchAtStartup;

    [ObservableProperty]
    private LanguageOption? _selectedLanguageOption;

    [ObservableProperty]
    private string _selectedLanguage = "tr";

    partial void OnSelectedLanguageOptionChanged(LanguageOption? value)
    {
        if (value is not null)
        {
            SelectedLanguage = value.Code;
        }
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _saveButtonText = string.Empty;

    [ObservableProperty]
    private string _resetButtonText = string.Empty;

    public IReadOnlyList<LanguageOption> LanguageOptions { get; private set; } = [];

    [ObservableProperty]
    private string _appearanceTitle = string.Empty;

    [ObservableProperty]
    private string _darkThemeHeader = string.Empty;

    [ObservableProperty]
    private string _accentGlowHeader = string.Empty;

    [ObservableProperty]
    private string _notificationsTitle = string.Empty;

    [ObservableProperty]
    private string _healthAlertsHeader = string.Empty;

    [ObservableProperty]
    private string _scanRemindersHeader = string.Empty;

    [ObservableProperty]
    private string _startupSectionTitle = string.Empty;

    [ObservableProperty]
    private string _launchAtStartupHeader = string.Empty;

    [ObservableProperty]
    private string _languageSectionTitle = string.Empty;

    [ObservableProperty]
    private string _languageHeader = string.Empty;

    [ObservableProperty]
    private string _aboutTitle = string.Empty;

    [ObservableProperty]
    private string _aboutText = string.Empty;

    [ObservableProperty]
    private string _aboutVersion = string.Empty;

    public SettingsViewModel()
    {
        LoadFromService();
        RefreshLocalizedStrings();
    }

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Settings");
        PageSubtitle = T("Settings.Subtitle");
        SaveButtonText = T("Settings.Save");
        ResetButtonText = T("Settings.Reset");
        AppearanceTitle = T("Settings.Appearance");
        DarkThemeHeader = T("Settings.DarkTheme");
        AccentGlowHeader = T("Settings.AccentGlow");
        NotificationsTitle = T("Settings.Notifications");
        HealthAlertsHeader = T("Settings.HealthAlerts");
        ScanRemindersHeader = T("Settings.ScanReminders");
        StartupSectionTitle = T("Settings.StartupSection");
        LaunchAtStartupHeader = T("Settings.LaunchAtStartup");
        LanguageSectionTitle = T("Settings.LanguageSection");
        LanguageHeader = T("Settings.Language");
        AboutTitle = T("Settings.About");
        AboutText = T("Settings.AboutText");
        AboutVersion = T("App.Version");
        LanguageOptions =
        [
            new LanguageOption { Code = "tr", Label = T("Settings.Language.Tr") },
            new LanguageOption { Code = "en", Label = T("Settings.Language.En") }
        ];
        SelectedLanguageOption = LanguageOptions.FirstOrDefault(o => o.Code == SelectedLanguage) ?? LanguageOptions[0];
        OnPropertyChanged(nameof(LanguageOptions));
    }

    [RelayCommand]
    public void LoadFromService()
    {
        var settings = _settingsService.Current;
        UseDarkTheme = settings.UseDarkTheme;
        AccentGlowEffects = settings.AccentGlowEffects;
        SystemHealthAlerts = settings.SystemHealthAlerts;
        ScheduledScanReminders = settings.ScheduledScanReminders;
        LaunchAtStartup = settings.LaunchAtStartup;
        SelectedLanguage = settings.Language is "tr" or "en" ? settings.Language : LocalizationService.Instance.CurrentLanguage;
    }

    [RelayCommand]
    public void Save()
    {
        var settings = new AppSettings
        {
            Language = SelectedLanguage,
            UseDarkTheme = UseDarkTheme,
            AccentGlowEffects = AccentGlowEffects,
            SystemHealthAlerts = SystemHealthAlerts,
            ScheduledScanReminders = ScheduledScanReminders,
            LaunchAtStartup = LaunchAtStartup
        };

        _settingsService.Save(settings);
        LocalizationService.Instance.SetLanguage(SelectedLanguage);
        ApplyStartupRegistration(LaunchAtStartup);
        StatusMessage = T("Settings.Saved");
    }

    private static void ApplyStartupRegistration(bool enabled)
    {
        try
        {
            const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
            const string valueName = "Wintix";
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyPath, writable: true);
            if (key is null)
            {
                return;
            }

            if (enabled)
            {
                var exePath = Environment.ProcessPath;
                if (!string.IsNullOrWhiteSpace(exePath))
                {
                    key.SetValue(valueName, $"\"{exePath}\"");
                }
            }
            else
            {
                key.DeleteValue(valueName, throwOnMissingValue: false);
            }
        }
        catch (Exception ex)
        {
            ActivityLogService.Instance.Warning("Settings", ex.Message);
        }
    }
}
