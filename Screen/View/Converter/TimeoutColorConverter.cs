using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Screen.View.Converter;

public class TimeoutColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (char)value switch
        {
            'o' => Brushes.Green,
            'x' => Brushes.Red,
            _ => throw new NotImplementedException()
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
