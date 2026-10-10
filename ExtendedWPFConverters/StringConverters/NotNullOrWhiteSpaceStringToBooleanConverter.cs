using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that returns the opposite value of <see cref="string.IsNullOrWhiteSpace(string)"/> on a passed string.
    /// </summary>
    public class NotNullOrWhiteSpaceStringToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is not null nor white-space.
        /// </summary>
        public bool ValueForNotNullOrWhiteSpace { get; set; } = true;

        /// <summary>
        /// Value to be applied when converted string is null or white-space.
        /// </summary>
        public bool ValueForNullOrWhiteSpace { get; set; } = false;

        /// <summary>
        /// Returns the opposite value of <see cref="string.IsNullOrWhiteSpace(string)"/> on the passed entry.
        /// </summary>
        /// <param name="value">A string entry to be assessed.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A value indicating if the string entry is null or white-space or not.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value as string) ? ValueForNullOrWhiteSpace : ValueForNotNullOrWhiteSpace;
        }

        /// <summary>
        /// Returns a string for which the state depends on the result of a passed boolean value.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A non-empty string if input maps to the configured positive state, null otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValueForNotNullOrWhiteSpace && value as bool? == true || !ValueForNotNullOrWhiteSpace && value as bool? != true
                ? "not null nor white-space"
                : null;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
