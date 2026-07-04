using Microsoft.UI.Xaml.Controls;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class ActivityLogsPage : Page
{
    public ActivityLogsViewModel ViewModel { get; } = new();

    public ActivityLogsPage()
    {
        InitializeComponent();
        PageHeader.ActionClick += (_, _) => ViewModel.ClearLogsCommand.Execute(null);
        Loaded += ActivityLogsPage_Loaded;
    }

    private void ActivityLogsPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.RefreshCommand.Execute(null);
    }
}
