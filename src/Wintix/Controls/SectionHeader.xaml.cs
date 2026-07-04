using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class SectionHeader : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(SectionHeader), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(SectionHeader), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ShowActionProperty =
        DependencyProperty.Register(nameof(ShowAction), typeof(bool), typeof(SectionHeader), new PropertyMetadata(false));

    public static readonly DependencyProperty ActionTextProperty =
        DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(SectionHeader), new PropertyMetadata("Action"));

    public event EventHandler<RoutedEventArgs>? ActionClick;

    public SectionHeader()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public bool ShowAction
    {
        get => (bool)GetValue(ShowActionProperty);
        set => SetValue(ShowActionProperty, value);
    }

    public string ActionText
    {
        get => (string)GetValue(ActionTextProperty);
        set => SetValue(ActionTextProperty, value);
    }

    private void ActionButton_Click(object sender, RoutedEventArgs e) => ActionClick?.Invoke(this, e);
}
