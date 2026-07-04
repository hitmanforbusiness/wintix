using Microsoft.UI.Xaml.Controls;
using Wintix.ViewModels;

namespace Wintix.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; } = new();

    public SettingsPage()
    {
        InitializeComponent();
    }
}
