using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class MetricBadge : UserControl
{
    public static readonly DependencyProperty MetricNameProperty =
        DependencyProperty.Register(nameof(MetricName), typeof(string), typeof(MetricBadge), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(MetricBadge), new PropertyMetadata("0"));

    public static readonly DependencyProperty UnitProperty =
        DependencyProperty.Register(nameof(Unit), typeof(string), typeof(MetricBadge), new PropertyMetadata("%"));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(MetricBadge), new PropertyMetadata("\uE9F5"));

    public static readonly DependencyProperty AccentColorHexProperty =
        DependencyProperty.Register(nameof(AccentColorHex), typeof(string), typeof(MetricBadge), new PropertyMetadata("#00D4FF"));

    public static readonly DependencyProperty PercentageProperty =
        DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(MetricBadge), new PropertyMetadata(0d));

    public MetricBadge()
    {
        InitializeComponent();
    }

    public string MetricName
    {
        get => (string)GetValue(MetricNameProperty);
        set => SetValue(MetricNameProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string Unit
    {
        get => (string)GetValue(UnitProperty);
        set => SetValue(UnitProperty, value);
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

    public double Percentage
    {
        get => (double)GetValue(PercentageProperty);
        set => SetValue(PercentageProperty, value);
    }
}
