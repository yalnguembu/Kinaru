using System.Globalization;

namespace Kinaru.Converters;

public class BoolToLayoutOptionsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFromCurrentUser)
        {
            return isFromCurrentUser ? LayoutOptions.End : LayoutOptions.Start;
        }
        return LayoutOptions.Start;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
