using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class StatCard : UserControl
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(StatCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(StatCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(StatCard), new PropertyMetadata("\uE9F5"));

    public static readonly DependencyProperty AccentColorHexProperty =
        DependencyProperty.Register(nameof(AccentColorHex), typeof(string), typeof(StatCard), new PropertyMetadata("#00D4FF"));

    public StatCard()
    {
        InitializeComponent();
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    public string AccentColorHex
    {
        get => (string)GetValue(AccentColorHexProperty);
        set => SetValue(AccentColorHexProperty, value);
    }

    private void CardBorder_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        CardBorder.BorderBrush = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["WintixAccentBrush"];
    }

    private void CardBorder_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        CardBorder.BorderBrush = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["WintixBorderSubtleBrush"];
    }
}
