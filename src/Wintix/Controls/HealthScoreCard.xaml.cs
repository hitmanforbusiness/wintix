using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Wintix.Controls;

public sealed partial class HealthScoreCard : UserControl
{
    public static readonly DependencyProperty ScoreProperty =
        DependencyProperty.Register(nameof(Score), typeof(string), typeof(HealthScoreCard), new PropertyMetadata("--"));

    public static readonly DependencyProperty StatusTextProperty =
        DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(HealthScoreCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(HealthScoreCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SummaryProperty =
        DependencyProperty.Register(nameof(Summary), typeof(string), typeof(HealthScoreCard), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ScoreValueProperty =
        DependencyProperty.Register(nameof(ScoreValue), typeof(double), typeof(HealthScoreCard), new PropertyMetadata(0d, OnScoreChanged));

    public HealthScoreCard()
    {
        InitializeComponent();
    }

    public string Score { get => (string)GetValue(ScoreProperty); set => SetValue(ScoreProperty, value); }
    public string StatusText { get => (string)GetValue(StatusTextProperty); set => SetValue(StatusTextProperty, value); }
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }
    public string Summary { get => (string)GetValue(SummaryProperty); set => SetValue(SummaryProperty, value); }

    public double ScoreValue
    {
        get => (double)GetValue(ScoreValueProperty);
        set => SetValue(ScoreValueProperty, value);
    }

    private static void OnScoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HealthScoreCard card && e.NewValue is double value)
        {
            card.ScoreProgress.Value = value;
        }
    }
}
