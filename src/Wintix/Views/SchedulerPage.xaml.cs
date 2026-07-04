using Microsoft.UI.Xaml.Controls;
using Wintix.Models;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class SchedulerPage : Page
{
    public SchedulerViewModel ViewModel { get; } = new();

    public SchedulerPage()
    {
        InitializeComponent();
        PageHeader.ActionClick += async (_, _) => await ViewModel.RefreshCommand.ExecuteAsync(null);
        Loaded += async (_, _) => await ViewModel.RefreshCommand.ExecuteAsync(null);
    }

    private void RunTask_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Button { Tag: ScheduledTaskItem task })
        {
            ViewModel.RunTaskCommand.Execute(task);
        }
    }
}
