using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class ModuleHeroPanel : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata("\uE946"));

    public static readonly DependencyProperty MetricLabelProperty =
        DependencyProperty.Register(nameof(MetricLabel), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty MetricValueProperty =
        DependencyProperty.Register(nameof(MetricValue), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty StatusTextProperty =
        DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty PrimaryActionTextProperty =
        DependencyProperty.Register(nameof(PrimaryActionText), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SecondaryActionTextProperty =
        DependencyProperty.Register(nameof(SecondaryActionText), typeof(string), typeof(ModuleHeroPanel), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ShowPrimaryActionProperty =
        DependencyProperty.Register(nameof(ShowPrimaryAction), typeof(bool), typeof(ModuleHeroPanel), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowSecondaryActionProperty =
        DependencyProperty.Register(nameof(ShowSecondaryAction), typeof(bool), typeof(ModuleHeroPanel), new PropertyMetadata(false));

    public event EventHandler<RoutedEventArgs>? PrimaryActionClick;
    public event EventHandler<RoutedEventArgs>? SecondaryActionClick;

    public ModuleHeroPanel()
    {
        InitializeComponent();
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Subtitle { get => (string)GetValue(SubtitleProperty); set => SetValue(SubtitleProperty, value); }
    public string IconGlyph { get => (string)GetValue(IconGlyphProperty); set => SetValue(IconGlyphProperty, value); }
    public string MetricLabel { get => (string)GetValue(MetricLabelProperty); set => SetValue(MetricLabelProperty, value); }
    public string MetricValue { get => (string)GetValue(MetricValueProperty); set => SetValue(MetricValueProperty, value); }
    public string StatusText { get => (string)GetValue(StatusTextProperty); set => SetValue(StatusTextProperty, value); }
    public string PrimaryActionText { get => (string)GetValue(PrimaryActionTextProperty); set => SetValue(PrimaryActionTextProperty, value); }
    public string SecondaryActionText { get => (string)GetValue(SecondaryActionTextProperty); set => SetValue(SecondaryActionTextProperty, value); }
    public bool ShowPrimaryAction { get => (bool)GetValue(ShowPrimaryActionProperty); set => SetValue(ShowPrimaryActionProperty, value); }
    public bool ShowSecondaryAction { get => (bool)GetValue(ShowSecondaryActionProperty); set => SetValue(ShowSecondaryActionProperty, value); }

    private void PrimaryActionButton_Click(object sender, RoutedEventArgs e) => PrimaryActionClick?.Invoke(this, e);
    private void SecondaryActionButton_Click(object sender, RoutedEventArgs e) => SecondaryActionClick?.Invoke(this, e);
}
