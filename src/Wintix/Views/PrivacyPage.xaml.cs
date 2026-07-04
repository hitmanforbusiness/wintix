using Microsoft.UI.Xaml.Controls;
using Wintix.Models;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class PrivacyPage : Page
{
    public PrivacyViewModel ViewModel { get; } = new();

    public PrivacyPage()
    {
        InitializeComponent();
        PageHeader.ActionClick += (_, _) => ViewModel.RefreshCommand.Execute(null);
        Loaded += (_, _) => ViewModel.RefreshCommand.Execute(null);
    }

    private void ActionButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Button { Tag: PrivacyActionItem action })
        {
            ViewModel.ExecuteActionCommand.Execute(action);
        }
    }
}
