using System.Globalization;

namespace Kinaru.Converters;

public class BoolToTextColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFromCurrentUser)
        {
            return isFromCurrentUser ? Colors.White : Color.FromArgb("#1F2937");
        }
        return Color.FromArgb("#1F2937");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
