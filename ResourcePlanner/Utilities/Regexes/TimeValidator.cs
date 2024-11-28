using System.Text.RegularExpressions;

namespace ResourcePlanner.Utilities.Regexes
{
    public abstract class TimeValidator
    {
        private static readonly string regexPattern = @"^([0-9]{2}):([0-9]{2})$";

        public static bool IsValid(string value)
        {
            var regex = new Regex(regexPattern);
            return regex.IsMatch(value);
        }
    }
}
