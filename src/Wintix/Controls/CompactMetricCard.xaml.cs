using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class CompactMetricCard : UserControl
{
    public static readonly DependencyProperty MetricNameProperty =
        DependencyProperty.Register(nameof(MetricName), typeof(string), typeof(CompactMetricCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(CompactMetricCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty UnitProperty =
        DependencyProperty.Register(nameof(Unit), typeof(string), typeof(CompactMetricCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(CompactMetricCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DetailTextProperty =
        DependencyProperty.Register(nameof(DetailText), typeof(string), typeof(CompactMetricCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(CompactMetricCard), new PropertyMetadata("\uE946"));

    public static readonly DependencyProperty AccentColorHexProperty =
        DependencyProperty.Register(nameof(AccentColorHex), typeof(string), typeof(CompactMetricCard), new PropertyMetadata("#22D3EE"));

    public static readonly DependencyProperty PercentageProperty =
        DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(CompactMetricCard), new PropertyMetadata(0d));

    public static readonly DependencyProperty SparklineProperty =
        DependencyProperty.Register(nameof(Sparkline), typeof(IReadOnlyList<double>), typeof(CompactMetricCard), new PropertyMetadata(null));

    public CompactMetricCard()
    {
        InitializeComponent();
    }

    public string MetricName { get => (string)GetValue(MetricNameProperty); set => SetValue(MetricNameProperty, value); }
    public string Value { get => (string)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public string Unit { get => (string)GetValue(UnitProperty); set => SetValue(UnitProperty, value); }
    public string Description { get => (string)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }
    public string DetailText { get => (string)GetValue(DetailTextProperty); set => SetValue(DetailTextProperty, value); }
    public string IconGlyph { get => (string)GetValue(IconGlyphProperty); set => SetValue(IconGlyphProperty, value); }
    public string AccentColorHex { get => (string)GetValue(AccentColorHexProperty); set => SetValue(AccentColorHexProperty, value); }
    public double Percentage { get => (double)GetValue(PercentageProperty); set => SetValue(PercentageProperty, value); }
    public IReadOnlyList<double>? Sparkline { get => (IReadOnlyList<double>?)GetValue(SparklineProperty); set => SetValue(SparklineProperty, value); }
}
