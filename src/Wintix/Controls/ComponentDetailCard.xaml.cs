using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class ComponentDetailCard : UserControl
{
    public static readonly DependencyProperty CategoryProperty =
        DependencyProperty.Register(nameof(Category), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DetailProperty =
        DependencyProperty.Register(nameof(Detail), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SpecLineProperty =
        DependencyProperty.Register(nameof(SpecLine), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty StatusTextProperty =
        DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty StatusColorHexProperty =
        DependencyProperty.Register(nameof(StatusColorHex), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata("#10B981"));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(ComponentDetailCard), new PropertyMetadata("\uE946"));

    public ComponentDetailCard()
    {
        InitializeComponent();
    }

    public string Category { get => (string)GetValue(CategoryProperty); set => SetValue(CategoryProperty, value); }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Detail { get => (string)GetValue(DetailProperty); set => SetValue(DetailProperty, value); }
    public string SpecLine { get => (string)GetValue(SpecLineProperty); set => SetValue(SpecLineProperty, value); }
    public string StatusText { get => (string)GetValue(StatusTextProperty); set => SetValue(StatusTextProperty, value); }
    public string StatusColorHex { get => (string)GetValue(StatusColorHexProperty); set => SetValue(StatusColorHexProperty, value); }
    public string IconGlyph { get => (string)GetValue(IconGlyphProperty); set => SetValue(IconGlyphProperty, value); }
}
