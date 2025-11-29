using System.Globalization;

namespace Kinaru.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFromCurrentUser)
        {
            return isFromCurrentUser ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F3F4F6");
        }
        return Color.FromArgb("#F3F4F6");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
