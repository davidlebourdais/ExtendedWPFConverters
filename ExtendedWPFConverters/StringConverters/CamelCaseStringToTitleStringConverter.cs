using System;
using System.Globalization;
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
            return (value as string).ToTitleCase();
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
                    result = char.ToLower(result[0]) + result.Substring(1);
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
