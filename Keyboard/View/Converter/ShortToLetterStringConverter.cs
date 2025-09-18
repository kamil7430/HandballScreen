using System.Globalization;
using System.Windows.Data;

namespace Keyboard.View.Converter;

public class ShortToLetterStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => new string(((string)parameter)[0], (short)value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
