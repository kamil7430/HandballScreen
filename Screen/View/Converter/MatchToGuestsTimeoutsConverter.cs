using Keyboard.Model;
using System.Globalization;
using System.Windows.Data;

namespace Screen.View.Converter;

public class MatchToGuestsTimeoutsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var match = (Match)value;

        var timeouts = new List<char>(match.MaxTimeouts);
        for (int i = 0; i < match.GuestsTimeoutsUsed; i++)
            timeouts.Add('x');
        for (int i = 0; i < match.GuestsTimeoutsLeft; i++)
            timeouts.Add('o');

        return timeouts;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
