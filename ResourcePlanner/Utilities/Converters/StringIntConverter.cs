using System.Globalization;
using System.Windows.Data;

namespace ResourcePlanner.Utilities.Converters
{
    public class StringIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return intValue.ToString();

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out int result))
                    return result;

                return 0;
            }

            return 0;
        }
    }
}
