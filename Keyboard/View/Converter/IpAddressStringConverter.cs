using System.Globalization;
using System.Windows.Data;

namespace Keyboard.View.Converter;

public class IpAddressStringConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => $"Serwer uruchomiony na adresie: {(string)values[0]}:{(string)values[1]}!" +
        $"\nStatus ekranu: {((bool)values[2] ? "Połączony" : "Rozłączony")}";

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
