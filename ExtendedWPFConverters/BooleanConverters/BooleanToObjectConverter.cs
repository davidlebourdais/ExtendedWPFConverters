using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Returns an object set as parameter if when main boolean condition is satisfied.
    /// </summary>
    public class BooleanToObjectConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public ReducedBooleanOperation Operation { get; set; } = ReducedBooleanOperation.None;

        /// <summary>
        /// Returns an object passed as a parameter regarding to the boolean entry, associated to 
        /// a boolean operation.
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">The object to be returned when the operation result is positive.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The object passed as parameter or null depending on the boolean 
        /// operation result applied on the boolean entry.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool value_bool = false;
            if (value == null || !(value is bool))
                return null;
            value_bool = (bool)value;

            return Operation == ReducedBooleanOperation.Not ? 
              (value_bool ? null : parameter) // not operation
            : (value_bool ? parameter : null);  // normal operation
        }

        /// <summary>
        /// Returns true or false depending on wether the 
        /// input is null or not, and depending on the current operation.
        /// </summary>
        /// <param name="value">The object value to be assessed.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>True if the object is not null (not null if inverse operation is set), the opposite value otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Operation == ReducedBooleanOperation.Not ? value == null : value != null;
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
