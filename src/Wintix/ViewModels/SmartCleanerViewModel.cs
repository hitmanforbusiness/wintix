using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wintix.Helpers;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class SmartCleanerViewModel : LocalizedViewModelBase
{
    private readonly SmartCleanerService _cleanerService = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _totalReclaimable = "0 B";

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _scanActionText = string.Empty;

    [ObservableProperty]
    private string _metricLabel = string.Empty;

    [ObservableProperty]
    private string _scanButtonText = string.Empty;

    [ObservableProperty]
    private string _cleanButtonText = string.Empty;

    [ObservableProperty]
    private string _categoriesTitle = string.Empty;

    public ObservableCollection<CleanCategory> Categories { get; } = [];

    public SmartCleanerViewModel()
    {
        foreach (var category in _cleanerService.CreateDefaultCategories())
        {
            Categories.Add(category);
        }

        RefreshLocalizedStrings();
    }

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.SmartCleaner");
        PageSubtitle = T("Cleaner.Subtitle");
        ScanActionText = T("Cleaner.ScanNow");
        MetricLabel = T("Cleaner.Reclaimable");
        ScanButtonText = T("Cleaner.Scan");
        CleanButtonText = T("Cleaner.CleanSelected");
        CategoriesTitle = T("Cleaner.Categories");
        StatusMessage = T("Cleaner.Status.Idle");
        LocalizeCategories();
    }

    [RelayCommand]
    public async Task ScanAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        StatusMessage = T("Cleaner.Status.Scanning");

        try
        {
            await _cleanerService.ScanAsync(Categories);
            RefreshCategoryBindings();
            RefreshTotals();
            StatusMessage = T("Cleaner.Status.ScanDone");
            ActivityLogService.Instance.Success("Smart Cleaner", StatusMessage);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            ActivityLogService.Instance.Error("Smart Cleaner", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task CleanAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var selected = Categories.Where(c => c.IsSelected && c.SizeBytes > 0).ToList();
        if (selected.Count == 0)
        {
            StatusMessage = T("Cleaner.Status.NoSelection");
            return;
        }

        IsBusy = true;
        StatusMessage = T("Cleaner.Status.Cleaning");

        try
        {
            var cleaned = await _cleanerService.CleanAsync(selected);
            RefreshCategoryBindings();
            RefreshTotals();
            StatusMessage = T("Cleaner.Status.CleanDone", FormatHelper.FormatBytes(cleaned));
            ActivityLogService.Instance.Success("Smart Cleaner", StatusMessage);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            ActivityLogService.Instance.Error("Smart Cleaner", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LocalizeCategories()
    {
        foreach (var category in Categories)
        {
            category.Name = T($"Cleaner.Cat.{category.Id}.Name");
            category.Description = T($"Cleaner.Cat.{category.Id}.Desc");
        }

        RefreshCategoryBindings();
    }

    private void RefreshCategoryBindings()
    {
        for (var i = 0; i < Categories.Count; i++)
        {
            var item = Categories[i];
            Categories[i] = new CleanCategory
            {
                Id = item.Id,
                Path = item.Path,
                Name = item.Name,
                Description = item.Description,
                IsSelected = item.IsSelected,
                SizeBytes = item.SizeBytes
            };
        }
    }

    private void RefreshTotals()
    {
        var total = Categories.Where(c => c.IsSelected).Sum(c => c.SizeBytes);
        TotalReclaimable = FormatHelper.FormatBytes(total);
    }
}
