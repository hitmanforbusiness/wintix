using Microsoft.UI.Xaml.Controls;
using Wintix.Models;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class PerformancePage : Page
{
    private readonly Microsoft.UI.Xaml.DispatcherTimer _timer = new();
    public PerformanceViewModel ViewModel { get; } = new();

    public PerformancePage()
    {
        InitializeComponent();
        ViewModel.AttachTimer(_timer);
        PageHeader.ActionClick += (_, _) => ViewModel.RefreshCommand.Execute(null);
        Loaded += PerformancePage_Loaded;
        Unloaded += PerformancePage_Unloaded;
    }

    private void PerformancePage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.RefreshCommand.Execute(null);
    }

    private void PerformancePage_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.DetachTimer();
    }

    private void EndProcess_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Button { Tag: ProcessInfoItem process })
        {
            ViewModel.EndProcessCommand.Execute(process);
        }
    }
}
