using System.Globalization;
using System.Windows.Data;

namespace Keyboard.View.Converter;

public class DecysecondsToTimeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        long ms = (long)value;
        long mili = ms % 10;
        long sec = ms / 10 % 60;
        long min = ms / 600;
        return $"{min:00}:{sec:00}.{mili:0}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
