using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class PerformanceViewModel : LocalizedViewModelBase
{
    private readonly PerformanceService _performanceService = new();
    private Microsoft.UI.Xaml.DispatcherTimer? _timer;

    [ObservableProperty]
    private bool _isMonitoring;

    [ObservableProperty]
    private int _memoryLoadPercent;

    [ObservableProperty]
    private string _totalMemory = "--";

    [ObservableProperty]
    private string _usedMemory = "--";

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _refreshActionText = string.Empty;

    [ObservableProperty]
    private string _memoryLoadLabel = string.Empty;

    [ObservableProperty]
    private string _memorySummary = string.Empty;

    [ObservableProperty]
    private string _processesTitle = string.Empty;

    [ObservableProperty]
    private string _endProcessText = string.Empty;

    public string MonitorButtonText => IsMonitoring ? T("Performance.StopMonitor") : T("Performance.StartMonitor");

    public string MemoryLoadDisplay => $"{MemoryLoadPercent}%";

    public ObservableCollection<ProcessInfoItem> Processes { get; } = [];

    public PerformanceViewModel()
    {
        RefreshLocalizedStrings();
    }

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Performance");
        PageSubtitle = T("Performance.Subtitle");
        RefreshActionText = T("Performance.Refresh");
        MemoryLoadLabel = T("Performance.MemoryLoad");
        ProcessesTitle = T("Performance.TopProcesses");
        EndProcessText = T("Performance.End");
        StatusMessage = T("Performance.Status.Idle");
        OnPropertyChanged(nameof(MonitorButtonText));
        OnPropertyChanged(nameof(MemoryLoadDisplay));
        UpdateMemorySummary();
        if (Processes.Count > 0)
        {
            Refresh();
        }
    }

    partial void OnEndProcessTextChanged(string value)
    {
        if (Processes.Count > 0)
        {
            Refresh();
        }
    }

    [RelayCommand]
    public void Refresh()
    {
        var memory = _performanceService.GetMemorySnapshot();
        MemoryLoadPercent = memory.MemoryLoadPercent;
        TotalMemory = memory.TotalMemory;
        UsedMemory = memory.UsedMemory;
        UpdateMemorySummary();

        Processes.Clear();
        foreach (var process in _performanceService.GetTopProcesses())
        {
            Processes.Add(new ProcessInfoItem
            {
                ProcessId = process.ProcessId,
                Name = process.Name,
                MemoryBytes = process.MemoryBytes,
                FormattedMemory = process.FormattedMemory,
                Status = process.Status,
                ActionLabel = EndProcessText
            });
        }

        StatusMessage = $"Updated {DateTime.Now:HH:mm:ss} · {Processes.Count} processes";
        ActivityLogService.Instance.Info("Performance", "Snapshot refreshed.");
    }

    [RelayCommand]
    public void ToggleMonitoring()
    {
        if (IsMonitoring)
        {
            StopMonitoring();
            return;
        }

        StartMonitoring();
    }

    [RelayCommand]
    public void EndProcess(ProcessInfoItem? process)
    {
        if (process is null)
        {
            return;
        }

        if (_performanceService.TryKillProcess(process.ProcessId, out var message))
        {
            StatusMessage = message;
            ActivityLogService.Instance.Warning("Performance", message);
            Refresh();
            return;
        }

        StatusMessage = message;
        ActivityLogService.Instance.Error("Performance", message);
    }

    public void AttachTimer(Microsoft.UI.Xaml.DispatcherTimer timer)
    {
        _timer = timer;
        _timer.Interval = TimeSpan.FromSeconds(2);
        _timer.Tick += (_, _) => Refresh();
    }

    partial void OnIsMonitoringChanged(bool value) => OnPropertyChanged(nameof(MonitorButtonText));

    partial void OnMemoryLoadPercentChanged(int value) => OnPropertyChanged(nameof(MemoryLoadDisplay));

    partial void OnUsedMemoryChanged(string value) => UpdateMemorySummary();
    partial void OnTotalMemoryChanged(string value) => UpdateMemorySummary();

    private void UpdateMemorySummary() => MemorySummary = T("Performance.UsedOf", UsedMemory, TotalMemory);

    private void StartMonitoring()
    {
        if (_timer is null)
        {
            return;
        }

        IsMonitoring = true;
        _timer.Start();
        Refresh();
    }

    private void StopMonitoring()
    {
        _timer?.Stop();
        IsMonitoring = false;
        StatusMessage = T("Performance.StopMonitor");
    }

    public void DetachTimer()
    {
        _timer?.Stop();
        _timer = null;
    }
}
