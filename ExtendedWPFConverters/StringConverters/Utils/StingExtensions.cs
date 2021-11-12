using System.Text.RegularExpressions;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Groups methods to be used for string translation through various types
    /// of fetchers to be provided with converter calls.
    /// </summary>
    public static class StingExtensions
    {
        /// <summary>
        /// Converts a string from 'camelCase' or 'CamelCase' to 'Title Case'.
        /// </summary>
        /// <param name="toSplit">The string to process.</param>
        /// <returns>A string in the 'Title Case' format.</returns>
        public static string ToTitleCase(this string toSplit)
        {
            if (toSplit == null)
                return null;
            
            if (string.IsNullOrWhiteSpace(toSplit))
                return string.Empty;
            
            toSplit = char.ToUpper(toSplit[0]) + toSplit.Substring(1);
            toSplit = Regex.Replace(toSplit, @"\s+", " ").TrimStart().TrimEnd();
            
            return Regex.Replace(toSplit, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
        }
    }
}