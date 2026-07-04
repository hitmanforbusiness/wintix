using Microsoft.UI.Xaml.Controls;
using Wintix.Models;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class StartupPage : Page
{
    public StartupViewModel ViewModel { get; } = new();

    public StartupPage()
    {
        InitializeComponent();
        PageHeader.ActionClick += (_, _) => ViewModel.RefreshCommand.Execute(null);
        Loaded += (_, _) => ViewModel.RefreshCommand.Execute(null);
    }

    private void DisableStartup_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Button { Tag: StartupItem item })
        {
            ViewModel.DisableCommand.Execute(item);
        }
    }
}
