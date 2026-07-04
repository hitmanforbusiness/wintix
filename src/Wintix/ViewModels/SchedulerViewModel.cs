using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class SchedulerViewModel : LocalizedViewModelBase
{
    private readonly SchedulerService _schedulerService = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _loadTasksText = string.Empty;

    [ObservableProperty]
    private string _openSchedulerText = string.Empty;

    [ObservableProperty]
    private string _tasksTitle = string.Empty;

    [ObservableProperty]
    private string _runTaskText = string.Empty;

    public ObservableCollection<ScheduledTaskItem> Tasks { get; } = [];

    public SchedulerViewModel() => RefreshLocalizedStrings();

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Scheduler");
        PageSubtitle = T("Scheduler.Subtitle");
        LoadTasksText = T("Scheduler.LoadTasks");
        OpenSchedulerText = T("Scheduler.Open");
        TasksTitle = T("Scheduler.Tasks");
        RunTaskText = T("Scheduler.Run");
        if (Tasks.Count > 0)
        {
            _ = RefreshAsync();
        }
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            Tasks.Clear();
            foreach (var task in await _schedulerService.GetTasksAsync())
            {
                Tasks.Add(new ScheduledTaskItem
                {
                    Name = task.Name,
                    Status = task.Status,
                    NextRun = task.NextRun,
                    Schedule = task.Schedule,
                    RunLabel = RunTaskText
                });
            }

            StatusMessage = $"{Tasks.Count} tasks";
            ActivityLogService.Instance.Info("Scheduler", StatusMessage);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void OpenTaskScheduler()
    {
        if (_schedulerService.OpenTaskScheduler())
        {
            ActivityLogService.Instance.Info("Scheduler", "Task Scheduler opened.");
        }
    }

    [RelayCommand]
    public void RunTask(ScheduledTaskItem? task)
    {
        if (task is null)
        {
            return;
        }

        if (_schedulerService.RunTaskNow(task.Name, out var message))
        {
            StatusMessage = message;
            ActivityLogService.Instance.Success("Scheduler", message);
            return;
        }

        StatusMessage = message;
        ActivityLogService.Instance.Error("Scheduler", message);
    }
}
