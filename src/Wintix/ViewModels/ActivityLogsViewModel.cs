using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class ActivityLogsViewModel : LocalizedViewModelBase
{
    [ObservableProperty]
    private string _filterText = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _clearActionText = string.Empty;

    [ObservableProperty]
    private string _refreshText = string.Empty;

    [ObservableProperty]
    private string _filterPlaceholder = string.Empty;

    [ObservableProperty]
    private string _entriesTitle = string.Empty;

    public ActivityLogService LogService => ActivityLogService.Instance;

    public ActivityLogsViewModel() => RefreshLocalizedStrings();

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Logs");
        PageSubtitle = T("Logs.Subtitle");
        ClearActionText = T("Logs.Clear");
        RefreshText = T("Logs.Refresh");
        FilterPlaceholder = T("Logs.FilterPlaceholder");
        EntriesTitle = T("Logs.Entries");
    }

    [RelayCommand]
    public void Refresh()
    {
        OnPropertyChanged(nameof(FilteredEntries));
        StatusMessage = $"{LogService.Entries.Count} entries";
    }

    [RelayCommand]
    public void ClearLogs()
    {
        LogService.Clear();
        Refresh();
    }

    public IEnumerable<Models.ActivityLogEntry> FilteredEntries
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                return LogService.Entries;
            }

            return LogService.Entries.Where(e =>
                e.Category.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                e.Message.Contains(FilterText, StringComparison.OrdinalIgnoreCase));
        }
    }

    partial void OnFilterTextChanged(string value) => OnPropertyChanged(nameof(FilteredEntries));
}
