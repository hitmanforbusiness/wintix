using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel { get; } = new();

    public DashboardPage()
    {
        InitializeComponent();
        Loaded += DashboardPage_Loaded;
        ActionsRepeater.ElementPrepared += ActionsRepeater_ElementPrepared;
    }

    private async void DashboardPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCommand.ExecuteAsync(null);
    }

    private async void QuickScanButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.QuickScanCommand.ExecuteAsync(null);
    }

    private async void DeepScanButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.DeepScanCommand.ExecuteAsync(null);
    }

    private void ActionsRepeater_ElementPrepared(Microsoft.UI.Xaml.Controls.ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
    {
        if (args.Element is Controls.RecommendedActionCard card && args.Index < ViewModel.RecommendedActions.Count)
        {
            var action = ViewModel.RecommendedActions[args.Index];
            card.ActionClick -= RecommendedActionCard_ActionClick;
            card.Tag = action.NavigationKey;
            card.ActionClick += RecommendedActionCard_ActionClick;
        }
    }

    private void RecommendedActionCard_ActionClick(object? sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Controls.RecommendedActionCard { Tag: string navigationKey })
        {
            App.MainWindow?.NavigateToModule(navigationKey);
        }
    }
}
