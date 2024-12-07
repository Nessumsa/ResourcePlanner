using System.Globalization;
using System.Windows.Data;

namespace ResourcePlanner.Utilities
{
    internal class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeAsString = (string)value;
            return timeAsString.Insert(2, ":");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeFormatted = (string)value;
            return timeFormatted.Replace(":", "");
        }
    }
}
