using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class PrivacyViewModel : LocalizedViewModelBase
{
    private readonly PrivacyService _privacyService = new();
    private readonly SmartCleanerService _cleanerService = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _refreshActionText = string.Empty;

    [ObservableProperty]
    private string _openSettingsText = string.Empty;

    [ObservableProperty]
    private string _cleanTempText = string.Empty;

    [ObservableProperty]
    private string _manageText = string.Empty;

    [ObservableProperty]
    private string _overviewTitle = string.Empty;

    public ObservableCollection<PrivacyActionItem> Actions { get; } = [];

    public PrivacyViewModel() => RefreshLocalizedStrings();

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Privacy");
        PageSubtitle = T("Privacy.Subtitle");
        RefreshActionText = T("Privacy.Refresh");
        OpenSettingsText = T("Privacy.OpenSettings");
        CleanTempText = T("Privacy.CleanTemp");
        ManageText = T("Privacy.Manage");
        OverviewTitle = T("Privacy.Overview");
        Refresh();
    }

    [RelayCommand]
    public void Refresh()
    {
        Actions.Clear();
        foreach (var action in _privacyService.GetPrivacyOverview())
        {
            Actions.Add(new PrivacyActionItem
            {
                Id = action.Id,
                Title = action.Title,
                Description = action.Description,
                Status = action.Status,
                IconGlyph = action.IconGlyph,
                IsActionAvailable = action.IsActionAvailable,
                ManageLabel = ManageText
            });
        }
    }

    [RelayCommand]
    public void OpenPrivacySettings() => _privacyService.OpenPrivacySettings();

    [RelayCommand]
    public async Task CleanTempDataAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var categories = _cleanerService.CreateDefaultCategories()
                .Where(c => c.Id is "temp-user" or "temp-local")
                .ToList();

            await _cleanerService.ScanAsync(categories);
            var cleaned = await _cleanerService.CleanAsync(categories);
            StatusMessage = Helpers.FormatHelper.FormatBytes(cleaned);
            ActivityLogService.Instance.Success("Privacy", StatusMessage);
            Refresh();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void ExecuteAction(PrivacyActionItem? action)
    {
        if (action is null)
        {
            return;
        }

        switch (action.Id)
        {
            case "telemetry":
            case "advertising":
                OpenPrivacySettings();
                break;
            case "location":
                _privacyService.OpenLocationSettings();
                break;
            case "temp-clean":
                _ = CleanTempDataAsync();
                break;
        }
    }
}
