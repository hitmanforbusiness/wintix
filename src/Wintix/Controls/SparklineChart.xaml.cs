using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Wintix.Converters;

namespace Wintix.Controls;

public sealed partial class SparklineChart : UserControl
{
    public static readonly DependencyProperty ValuesProperty =
        DependencyProperty.Register(nameof(Values), typeof(IReadOnlyList<double>), typeof(SparklineChart),
            new PropertyMetadata(null, OnValuesChanged));

    public static readonly DependencyProperty AccentColorHexProperty =
        DependencyProperty.Register(nameof(AccentColorHex), typeof(string), typeof(SparklineChart),
            new PropertyMetadata("#22D3EE"));

    private static readonly HexToBrushConverter BrushConverter = new();

    public SparklineChart()
    {
        InitializeComponent();
    }

    public IReadOnlyList<double>? Values
    {
        get => (IReadOnlyList<double>?)GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    public string AccentColorHex
    {
        get => (string)GetValue(AccentColorHexProperty);
        set => SetValue(AccentColorHexProperty, value);
    }

    private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SparklineChart chart)
        {
            chart.RenderChart();
        }
    }

    private void RenderChart()
    {
        ChartPanel.Children.Clear();
        if (Values is null || Values.Count == 0)
        {
            return;
        }

        var brush = BrushConverter.Convert(AccentColorHex, typeof(Brush), null!, "") as Brush
            ?? new SolidColorBrush(Microsoft.UI.Colors.Cyan);
        const double maxHeight = 36;

        foreach (var value in Values)
        {
            var height = Math.Max(4, value / 100d * maxHeight);
            ChartPanel.Children.Add(new Border
            {
                Width = 5,
                Height = height,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = brush,
                CornerRadius = new CornerRadius(2),
                Opacity = 0.85
            });
        }
    }
}
