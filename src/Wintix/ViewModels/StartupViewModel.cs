using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class StartupViewModel : LocalizedViewModelBase
{
    private readonly StartupService _startupService = new();

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
    private string _entriesTitle = string.Empty;

    [ObservableProperty]
    private string _disableText = string.Empty;

    [ObservableProperty]
    private string _enabledText = string.Empty;

    public ObservableCollection<StartupItem> Items { get; } = [];

    public StartupViewModel() => RefreshLocalizedStrings();

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Startup");
        PageSubtitle = T("Startup.Subtitle");
        RefreshActionText = T("Startup.Refresh");
        EntriesTitle = T("Startup.Entries");
        DisableText = T("Startup.Disable");
        EnabledText = T("Startup.Enabled");
        if (Items.Count > 0)
        {
            Refresh();
        }
    }

    [RelayCommand]
    public void Refresh()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            Items.Clear();
            foreach (var item in _startupService.GetStartupItems())
            {
                Items.Add(new StartupItem
                {
                    Name = item.Name,
                    Command = item.Command,
                    Hive = item.Hive,
                    RegistryKey = item.RegistryKey,
                    ValueName = item.ValueName,
                    IsEnabled = item.IsEnabled,
                    EnabledLabel = EnabledText,
                    DisableLabel = DisableText
                });
            }

            StatusMessage = $"{Items.Count} entries";
            ActivityLogService.Instance.Info("Startup Manager", StatusMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void Disable(StartupItem? item)
    {
        if (item is null || !item.IsEnabled)
        {
            return;
        }

        if (_startupService.DisableStartupItem(item, out var message))
        {
            StatusMessage = message;
            ActivityLogService.Instance.Success("Startup Manager", message);
            Refresh();
            return;
        }

        StatusMessage = message;
        ActivityLogService.Instance.Error("Startup Manager", message);
    }
}
