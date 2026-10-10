using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a string state into a <see cref="Visibility"/> value based on white-space awareness.
    /// </summary>
    public class NotNullOrWhiteSpaceStringToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is not null nor white-space.
        /// </summary>
        public Visibility ValueForNotNullOrWhiteSpace { get; set; } = Visibility.Visible;

        /// <summary>
        /// Value to be applied when converted string is null or white-space.
        /// </summary>
        public Visibility ValueForNullOrWhiteSpace { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Returns a <see cref="Visibility"/> value corresponding to a string entry state.
        /// </summary>
        /// <param name="value">A string entry that can be null or white-space.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The <see cref="Visibility"/> value corresponding to the string entry state.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value as string) ? ValueForNullOrWhiteSpace : ValueForNotNullOrWhiteSpace;
        }

        /// <summary>
        /// Returns a string for which the state depends on the result of a passed visibility value.
        /// </summary>
        /// <param name="value">A <see cref="Visibility"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A non-empty string if input maps to the configured visible state, null otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility casted && casted == ValueForNotNullOrWhiteSpace ? "not null nor white-space" : null;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
