using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SimpleCalculator.Converter
{
    public class SolidColorBrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var brush = (SolidColorBrush)value;

            if (brush == null)
                return null;

            return brush.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var color = (Color)value;

            if (color == null)
                return null;

            return new SolidColorBrush(color);
        }
    }
}
