using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wintix.Helpers;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class DashboardViewModel : LocalizedViewModelBase
{
    private readonly ISystemOverviewService _overviewService;
    private DateTime? _lastScanUtc;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isScanning;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private string _healthScore = "--";

    [ObservableProperty]
    private double _healthScoreValue;

    [ObservableProperty]
    private string _healthStatus = string.Empty;

    [ObservableProperty]
    private string _lastScanSummary = string.Empty;

    [ObservableProperty]
    private string _lastScanTime = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _quickScanText = string.Empty;

    [ObservableProperty]
    private string _deepScanText = string.Empty;

    [ObservableProperty]
    private string _lastScanLabel = string.Empty;

    [ObservableProperty]
    private string _systemUsageTitle = string.Empty;

    [ObservableProperty]
    private string _recommendedActionsTitle = string.Empty;

    [ObservableProperty]
    private string _componentsTitle = string.Empty;

    [ObservableProperty]
    private string _healthScoreLabel = string.Empty;

    [ObservableProperty]
    private string _downloadLabel = string.Empty;

    [ObservableProperty]
    private string _uploadLabel = string.Empty;

    [ObservableProperty]
    private string _storageUsedLabel = string.Empty;

    [ObservableProperty]
    private string _storageFreeLabel = string.Empty;

    [ObservableProperty]
    private string _diskUsedText = "--";

    [ObservableProperty]
    private string _diskFreeText = "--";

    public SystemMetric CpuMetric { get; private set; } = CreatePlaceholderMetric();
    public SystemMetric RamMetric { get; private set; } = CreatePlaceholderMetric();
    public SystemMetric DiskMetric { get; private set; } = CreatePlaceholderMetric();
    public SystemMetric NetworkMetric { get; private set; } = CreatePlaceholderMetric();
    public IReadOnlyList<SystemComponent> Components { get; private set; } = [];
    public IReadOnlyList<RecommendedAction> RecommendedActions { get; private set; } = [];

    public DashboardViewModel()
        : this(new SystemOverviewService())
    {
    }

    public DashboardViewModel(ISystemOverviewService overviewService)
    {
        _overviewService = overviewService;
        RefreshLocalizedStrings();
    }

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Dashboard");
        PageSubtitle = T("Dashboard.Subtitle");
        QuickScanText = T("Dashboard.QuickScan");
        DeepScanText = T("Dashboard.DeepScan");
        LastScanLabel = T("Dashboard.LastScan");
        SystemUsageTitle = T("Dashboard.SystemUsage");
        RecommendedActionsTitle = T("Dashboard.RecommendedActions");
        ComponentsTitle = T("Dashboard.Components");
        HealthScoreLabel = T("Dashboard.HealthScore");
        DownloadLabel = T("Dashboard.Metric.Download");
        UploadLabel = T("Dashboard.Metric.Upload");
        StorageUsedLabel = T("Dashboard.Metric.Used");
        StorageFreeLabel = T("Dashboard.Metric.Free");
        LastScanTime = _lastScanUtc.HasValue
            ? T("Dashboard.LastScan.At", FormatLastScan(_lastScanUtc.Value))
            : T("Dashboard.LastScan.Never");
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            await RefreshDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task QuickScanAsync()
    {
        if (IsScanning)
        {
            return;
        }

        IsScanning = true;
        ErrorMessage = null;

        try
        {
            ActivityLogService.Instance.Info("Dashboard", "Quick scan started.");
            var cleaner = new SmartCleanerService();
            var categories = cleaner.CreateDefaultCategories().ToList();
            await cleaner.ScanAsync(categories);
            var reclaimable = categories.Where(c => c.IsSelected).Sum(c => c.SizeBytes);
            MarkScanComplete();
            await RefreshDataAsync();
            LastScanSummary = reclaimable > 0
                ? T("Cleaner.Status.CleanDone", FormatHelper.FormatBytes(reclaimable))
                : T("Cleaner.Status.ScanDone");
            ActivityLogService.Instance.Success("Dashboard", LastScanSummary);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsScanning = false;
        }
    }

    [RelayCommand]
    public async Task DeepScanAsync()
    {
        if (IsScanning)
        {
            return;
        }

        IsScanning = true;
        ErrorMessage = null;

        try
        {
            ActivityLogService.Instance.Info("Dashboard", "Deep scan started.");
            var cleaner = new SmartCleanerService();
            var categories = cleaner.CreateDefaultCategories().ToList();
            foreach (var category in categories)
            {
                category.IsSelected = true;
            }

            await cleaner.ScanAsync(categories);
            await Task.Delay(600);
            MarkScanComplete();
            await RefreshDataAsync();
            LastScanSummary = T("Dashboard.Optimized");
            ActivityLogService.Instance.Success("Dashboard", "Deep scan complete.");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsScanning = false;
        }
    }

    private void MarkScanComplete()
    {
        _lastScanUtc = DateTime.Now;
        LastScanTime = T("Dashboard.LastScan.At", FormatLastScan(_lastScanUtc.Value));
    }

    private static string FormatLastScan(DateTime localTime) =>
        localTime.ToString("g", System.Globalization.CultureInfo.CurrentCulture);

    private async Task RefreshDataAsync()
    {
        var data = await _overviewService.GetOverviewAsync();
        CpuMetric = data.CpuMetric ?? CreatePlaceholderMetric();
        RamMetric = data.RamMetric ?? CreatePlaceholderMetric();
        DiskMetric = data.DiskMetric ?? CreatePlaceholderMetric();
        NetworkMetric = data.NetworkMetric ?? CreatePlaceholderMetric();
        Components = data.Components;
        RecommendedActions = data.RecommendedActions;
        HealthScore = data.HealthScore;
        HealthStatus = data.HealthStatus;
        HealthScoreValue = double.TryParse(data.HealthScore, out var score) ? score : 0;
        LastScanSummary = data.LastScanSummary;

        if (DiskMetric.StorageTotalBytes > 0)
        {
            DiskUsedText = FormatHelper.FormatBytes(DiskMetric.StorageUsedBytes);
            DiskFreeText = FormatHelper.FormatBytes(DiskMetric.StorageFreeBytes);
        }
        else
        {
            DiskUsedText = "--";
            DiskFreeText = "--";
        }

        if (!_lastScanUtc.HasValue)
        {
            LastScanTime = data.LastScanTime;
        }

        OnPropertyChanged(nameof(CpuMetric));
        OnPropertyChanged(nameof(RamMetric));
        OnPropertyChanged(nameof(DiskMetric));
        OnPropertyChanged(nameof(NetworkMetric));
        OnPropertyChanged(nameof(Components));
        OnPropertyChanged(nameof(RecommendedActions));
    }

    private static SystemMetric CreatePlaceholderMetric() => new()
    {
        Name = "--",
        Value = "--",
        Unit = string.Empty,
        IconGlyph = "\uE946",
        AccentColorHex = "#64748B",
        Percentage = 0,
        Description = string.Empty,
        DetailText = string.Empty,
        Sparkline = []
    };
}
