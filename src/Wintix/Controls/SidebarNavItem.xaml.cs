using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace Wintix.Controls;

public sealed partial class SidebarNavItem : UserControl
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(SidebarNavItem), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(SidebarNavItem), new PropertyMetadata("\uE80F"));

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(SidebarNavItem), new PropertyMetadata(false, OnVisualStateChanged));

    public static readonly DependencyProperty NavigationKeyProperty =
        DependencyProperty.Register(nameof(NavigationKey), typeof(string), typeof(SidebarNavItem), new PropertyMetadata(string.Empty));

    public SidebarNavItem()
    {
        InitializeComponent();
        Loaded += (_, _) => UpdateVisualState();
        RootGrid.Tapped += OnTapped;
    }

    public event EventHandler<TappedRoutedEventArgs>? ItemClick;

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public string NavigationKey
    {
        get => (string)GetValue(NavigationKeyProperty);
        set => SetValue(NavigationKeyProperty, value);
    }

    private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SidebarNavItem item)
        {
            item.UpdateVisualState();
        }
    }

    private void UpdateVisualState()
    {
        VisualStateManager.GoToState(this, IsActive ? "Active" : "Inactive", true);

        var accent = (Brush)Application.Current.Resources["WintixAccentBrush"];
        var secondary = (Brush)Application.Current.Resources["WintixTextSecondaryBrush"];
        var primary = (Brush)Application.Current.Resources["WintixTextPrimaryBrush"];

        NavIcon.Foreground = IsActive ? accent : secondary;
        NavLabel.Foreground = IsActive ? primary : secondary;
        NavLabel.FontWeight = IsActive ? Microsoft.UI.Text.FontWeights.SemiBold : Microsoft.UI.Text.FontWeights.Normal;
    }

    private void OnTapped(object sender, TappedRoutedEventArgs e) => ItemClick?.Invoke(this, e);
}
