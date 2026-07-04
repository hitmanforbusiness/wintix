using Microsoft.UI.Xaml.Controls;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class SmartCleanerPage : Page
{
    public SmartCleanerViewModel ViewModel { get; } = new();

    public SmartCleanerPage()
    {
        InitializeComponent();
        HeroPanel.PrimaryActionClick += async (_, _) => await ViewModel.ScanCommand.ExecuteAsync(null);
    }
}
