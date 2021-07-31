using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that returns the opposite value of <see cref="string.IsNullOrEmpty(string)"/> on a passed string (when
    /// default values for <see cref="ValueForNotNullOrEmpty"/> and <see cref="ValueForNullOrEmpty"/> are not changed.
    /// </summary>
    public class NotNullOrEmptyStringToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is not null nor empty.
        /// </summary>
        public bool ValueForNotNullOrEmpty { get; set; } = true;

        /// <summary>
        /// Value to be applied when converted string is null or empty.
        /// </summary>
        public bool ValueForNullOrEmpty { get; set; } = false;

        /// <summary>
        /// Returns the opposite value of <see cref="string.IsNullOrEmpty(string)"/> on the passed entry.
        /// A boolean operation can be set to invert result.
        /// </summary>
        /// <param name="value">A string entry to be assessed.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A value indicating if the string entry is null or empty or not.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
        }

        /// <summary>
        /// Returns a string for which the states depends on the result of a passed boolean value.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A string which will not be null or empty depending on the passed value and the current operation.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValueForNotNullOrEmpty && value as bool? == true || !ValueForNotNullOrEmpty && value as bool? != true ? "not null nor empty" : null;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
