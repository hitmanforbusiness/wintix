using Microsoft.UI.Xaml.Controls;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class NetworkPage : Page
{
    public NetworkViewModel ViewModel { get; } = new();

    public NetworkPage()
    {
        InitializeComponent();
        PageHeader.ActionClick += (_, _) => ViewModel.RefreshAdaptersCommand.Execute(null);
        Loaded += (_, _) => ViewModel.RefreshAdaptersCommand.Execute(null);
    }
}
