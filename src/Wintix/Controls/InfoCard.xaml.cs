using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class InfoCard : UserControl
{
    public static readonly DependencyProperty CategoryProperty =
        DependencyProperty.Register(nameof(Category), typeof(string), typeof(InfoCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DetailProperty =
        DependencyProperty.Register(nameof(Detail), typeof(string), typeof(InfoCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(InfoCard), new PropertyMetadata("\uE964"));

    public InfoCard()
    {
        InitializeComponent();
    }

    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Detail
    {
        get => (string)GetValue(DetailProperty);
        set => SetValue(DetailProperty, value);
    }

    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    private void CardBorder_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        CardBorder.BorderBrush = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["WintixAccentBrush"];
        CardBorder.Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["WintixSurfaceHoverBrush"];
    }

    private void CardBorder_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        CardBorder.BorderBrush = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["WintixBorderSubtleBrush"];
        CardBorder.Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["WintixSurfaceElevatedBrush"];
    }
}
