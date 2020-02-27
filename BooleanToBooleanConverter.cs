using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter to perform operations over a single boolean value.
    /// </summary>
    public class BooleanToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Specifies the operation to be applied with the converter.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.None;

        /// <summary>
        /// Performs a binary operation over a single boolean.
        /// </summary>
        /// <param name="value">Must be a boolean.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The boolean result of the boolean operation applied to the boolean input.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool value_bool = false;
            if (value is bool)
                value_bool = (bool)value;

            switch (Operation)
            {
                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                case BooleanOperation.Nor:
                    return !value_bool;

                case BooleanOperation.None:
                case BooleanOperation.Equality:
                case BooleanOperation.Or:
                case BooleanOperation.And:
                    return value_bool;

                case BooleanOperation.Xor:
                    return false;

                case BooleanOperation.Xnor:
                    return true;

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToBooleanConverter) + ".");

            }
        }

        /// <summary>
        /// Converts a boolean value into another boolean one through the boolean operation.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The inverted operation result over the passed entry.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueBoolean)
            {
                switch (Operation)
                {
                    case BooleanOperation.Not:
                    case BooleanOperation.Nand:
                    case BooleanOperation.Nor:
                        return !valueBoolean;

                    case BooleanOperation.None:
                    case BooleanOperation.Equality:
                    case BooleanOperation.Or:
                    case BooleanOperation.And:
                        return valueBoolean;

                    case BooleanOperation.Xor:
                    case BooleanOperation.Xnor:
                        return valueBoolean;

                    default:
                        throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToBooleanConverter) + " convert back.");
                }
            }

            return false;
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
