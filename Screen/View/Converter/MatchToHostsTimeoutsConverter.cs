using Keyboard.Model;
using System.Globalization;
using System.Windows.Data;

namespace Screen.View.Converter;

public class MatchToHostsTimeoutsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var match = (Match)value;

        var timeouts = new List<char>(match.MaxTimeouts);
        for (int i = 0; i < match.HostsTimeoutsUsed; i++)
            timeouts.Add('x');
        for (int i = 0; i < match.HostsTimeoutsLeft; i++)
            timeouts.Add('o');

        return timeouts;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
