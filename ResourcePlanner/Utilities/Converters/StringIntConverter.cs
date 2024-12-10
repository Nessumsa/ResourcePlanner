using System.Globalization;
using System.Windows.Data;

namespace ResourcePlanner.Utilities.Converters
{
    /// <summary>
    /// A value converter that converts between a string and an integer.
    /// </summary>
    public class StringIntConverter : IValueConverter
    {
        /// <summary>
        /// Converts an integer value to a string representation.
        /// </summary>
        /// <param name="value">The value to convert, expected to be an integer.</param>
        /// <returns>A string representation of the integer, or an empty string if the input is not an integer.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return intValue.ToString();

            return string.Empty;
        }

        /// <summary>
        /// Converts a string value back to an integer.
        /// </summary>
        /// <param name="value">The value to convert back, expected to be a string.</param>
        /// <returns>The integer representation of the string, or 0 if conversion fails.</returns>
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