using System.Text.RegularExpressions;

namespace ResourcePlanner.Utilities.Regexes
{
    /// <summary>
    /// Provides functionality to validate time strings in the format "HH:mm".
    /// </summary>
    public abstract class TimeValidator
    {
        /// <summary>
        /// The regex pattern for matching time strings in "HH:mm" format.
        /// </summary>
        private static readonly string regexPattern = @"^([0-9]{2}):([0-9]{2})$";

        /// <summary>
        /// Validates whether a given string matches the "HH:mm" time format.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <returns>True if the string matches the time format; otherwise, false.</returns>
        public static bool IsValid(string value)
        {
            var regex = new Regex(regexPattern);
            return regex.IsMatch(value);
        }
    }
}