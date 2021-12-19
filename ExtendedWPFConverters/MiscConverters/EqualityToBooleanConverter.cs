using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Checks if passed object and parameter value are equal.
    /// </summary>
    public class EqualityToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// If set, the equality converter result is inverted.
        /// </summary>
        public bool TrueIfNotEqual { get; set; } = false;

        /// <summary>
        /// Checks if passed object value is equal to parameter value.
        /// </summary>
        /// <param name="value">An object to be checked.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">A reference value.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>True if objects are equal (or if <see cref="TrueIfNotEqual"/> is set, returns true if are not equal), 
        /// false otherwise (when <see cref="TrueIfNotEqual"/> is set, returns false if objects are equal).</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TrueIfNotEqual ? value == null && parameter != null || value?.Equals(parameter) == false :
                                    value == null && parameter == null || value?.Equals(parameter) == true;
        }

        /// <summary>
        /// Unsupported conversion method.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotSupportedException">Thrown if this method is called.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
