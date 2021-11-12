using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a string content from CamelCase to Title Case.
    /// </summary>
    public class CamelCaseStringToTitleStringConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// If set, will set an lower case letter when converting back to camelCase
        /// (will be CamelCase if unset).
        /// </summary>
        public bool FirstLetterIsLowerCase { get; set; }
        
        /// <summary>
        /// Converts a string content from CamelCase to Title Case.
        /// </summary>
        /// <param name="value">A string in the camel case format.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A string in the title case format.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string asString))
                return null;
            
            return SplitCamelCase(asString);
        }

        private static string SplitCamelCase(string toSplit)
        {
            if (toSplit.Length > 0)
                toSplit = char.ToUpper(toSplit[0]) + toSplit[1..];
            return Regex.Replace(toSplit.Replace(" ", ""), "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
        }

        /// <summary>
        /// Sets a string content to CamelCase.
        /// </summary>
        /// <param name="value">The string to be converted to camel case format.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A string in the camel case format.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string asString))
                return null;

            var result = asString.Replace(" ", "");
            
            if (FirstLetterIsLowerCase)
            {
                if (result.Length > 0)
                    result = char.ToLower(result[0]) + result[1..];
            }
            
            return result;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
