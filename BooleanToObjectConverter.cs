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
        public BooleanOperation Operation { get; set; } = BooleanOperation.None;

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
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool value_bool = false;
            if (value == null || !(value is bool))
                return null;
            value_bool = (bool)value;

            switch (Operation)
            {
                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                case BooleanOperation.Nor:
                    return value_bool ? null : parameter;
                case BooleanOperation.None:
                case BooleanOperation.Equality:
                case BooleanOperation.Or:
                case BooleanOperation.And:
                    return value_bool ? parameter : null;

                case BooleanOperation.Xor:
                    return null;

                case BooleanOperation.Xnor:
                    return parameter;

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToObjectConverter) + ".");
            }
        }

        /// <summary>
        /// Unsupported conversion method. Returns null.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Null.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
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
