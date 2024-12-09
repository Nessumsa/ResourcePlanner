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
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If SelectedItem is null, return Collapsed; otherwise, Visible.
            if (value is string stringValue)
            {
                // Return Collapsed if the string is null or empty, Visible otherwise.
                return string.IsNullOrEmpty(stringValue) ? Visibility.Collapsed : Visibility.Visible;
            }
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != string.Empty)
            {
                return (string)value != string.Empty ? Visibility.Visible : Visibility.Collapsed;
            }
            return value != null ?  Visibility.Visible : Visibility.Collapsed;
        }
    }
}
