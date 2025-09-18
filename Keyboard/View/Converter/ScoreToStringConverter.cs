using System.Globalization;
using System.Windows.Data;

namespace Keyboard.View.Converter;

public class ScoreToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var score = ((short, short))value;
        return $"{score.Item1} - {score.Item2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
