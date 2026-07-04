using Microsoft.UI.Xaml.Controls;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class TicketsPage : Page
{
    public TicketsViewModel ViewModel { get; } = new();

    public TicketsPage()
    {
        InitializeComponent();
        Loaded += TicketsPage_Loaded;
    }

    private async void TicketsPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCommand.ExecuteAsync(null);
    }
}
