using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ResourcePlanner.Utilities.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // Convert from bool to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed; // Default value if input is not a bool
        }

        // Convert back from Visibility to bool (optional, rarely used)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false; // Default value if input is not Visibility
        }
    }
}
