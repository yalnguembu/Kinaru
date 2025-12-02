using Kinaru.Shared.Enums;
using System.Globalization;

namespace Kinaru.Converters;

public class PropertyStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is PropertyStatus status)
        {
            return status switch
            {
                PropertyStatus.Disponible => Color.FromArgb("#10B981"),
                PropertyStatus.Vendu => Color.FromArgb("#EF4444"),
                PropertyStatus.Loue => Color.FromArgb("#F59E0B"),
                _ => Color.FromArgb("#6B7280")
            };
        }
        return Color.FromArgb("#6B7280");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
