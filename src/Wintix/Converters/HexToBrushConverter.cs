using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Wintix.Converters;

public sealed class HexToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not string colorString || string.IsNullOrWhiteSpace(colorString))
        {
            return new SolidColorBrush(Colors.Cyan);
        }

        colorString = colorString.Trim().TrimStart('#');
        if (colorString.Length < 6)
        {
            return new SolidColorBrush(Colors.Cyan);
        }

        try
        {
            var r = System.Convert.ToByte(colorString[..2], 16);
            var g = System.Convert.ToByte(colorString[2..4], 16);
            var b = System.Convert.ToByte(colorString[4..6], 16);
            return new SolidColorBrush(ColorHelper.FromArgb(255, r, g, b));
        }
        catch
        {
            return new SolidColorBrush(Colors.Cyan);
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException();
}
