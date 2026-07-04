using Microsoft.UI.Xaml.Controls;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class EventsPage : Page
{
    public EventsViewModel ViewModel { get; } = new();

    public EventsPage()
    {
        InitializeComponent();
        Loaded += EventsPage_Loaded;
    }

    private async void EventsPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCommand.ExecuteAsync(null);
    }
}
