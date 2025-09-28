using System.Globalization;
using System.Windows.Data;

namespace Keyboard.View.Converter;

public class SuspensionTimeLeftConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var endTime = (long)values[0];
        var actualTime = (long)values[1];
        var timeLeft = (endTime - actualTime) / 10;
        return $"{timeLeft / 60:D2}:{timeLeft % 60:D2}";
    }

    object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
