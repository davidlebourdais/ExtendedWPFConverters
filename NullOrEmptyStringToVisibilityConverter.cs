using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a string state into a <see cref="Visibility"/> value.
    /// </summary>
    public class NullOrEmptyStringToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is null or empty.
        /// </summary>
        public Visibility ValueForNullOrEmpty { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when converted string is not null nor empty.
        /// </summary>
        public Visibility ValueForNotNullOrEmpty { get; set; } = Visibility.Visible;

        /// <summary>
        /// Returns <see cref="Visibility"/> value that correspond to a string entry state (null or empty so not visible, not null so visible).
        /// </summary>
        /// <param name="value">An string entry that can be null or empty.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The <see cref="Visibility"/> value corresponding to the string entry state.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
        }

        /// <summary>
        /// Returns a string for which the states depends on the result of a passed visibility value.
        /// </summary>
        /// <param name="value">A <see cref="Visibility"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A string which will not be null or empty depending on the passed value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility casted && casted == ValueForNullOrEmpty ? null : "not null";
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
