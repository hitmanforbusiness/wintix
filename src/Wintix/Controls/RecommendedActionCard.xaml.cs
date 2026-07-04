using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class RecommendedActionCard : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(RecommendedActionCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(RecommendedActionCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(RecommendedActionCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ActionTextProperty =
        DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(RecommendedActionCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(RecommendedActionCard), new PropertyMetadata("\uE946"));

    public static readonly DependencyProperty AccentColorHexProperty =
        DependencyProperty.Register(nameof(AccentColorHex), typeof(string), typeof(RecommendedActionCard), new PropertyMetadata("#00D4FF"));

    public event EventHandler<RoutedEventArgs>? ActionClick;

    public RecommendedActionCard()
    {
        InitializeComponent();
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Description { get => (string)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }
    public string Value { get => (string)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public string ActionText { get => (string)GetValue(ActionTextProperty); set => SetValue(ActionTextProperty, value); }
    public string IconGlyph { get => (string)GetValue(IconGlyphProperty); set => SetValue(IconGlyphProperty, value); }
    public string AccentColorHex { get => (string)GetValue(AccentColorHexProperty); set => SetValue(AccentColorHexProperty, value); }

    private void ActionButton_Click(object sender, RoutedEventArgs e) => ActionClick?.Invoke(this, e);
}
