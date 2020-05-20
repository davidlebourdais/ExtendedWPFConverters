using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter between multi booleans and visibility object to be used in multibindings.
    /// </summary>
    public class BooleanToVisibilityConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        public Visibility ValueForTrue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Value to be applied when converted value is false, null or invalid.
        /// </summary>
        public Visibility ValueForFalse { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.None;

        /// <summary>
        /// Converts several booleans to a single visibility value.
        /// </summary>
        /// <param name="values">The array of boolean values that the source bindings in the MultiBinding produces. 
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A visibility value based on the result of the boolean operation applied to all boolean inputs.</returns>
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch(Operation)
            {
                case BooleanOperation.Equality:
                    bool first_value = false;
                    if (values.Length > 0)
                        first_value = (values[0] as bool?) != null && ((values[0] as bool?) == true);

                    // Case all true:
                    if (first_value)
                    {
                        if (values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                            return ValueForTrue;
                    }
                    else // case all false:
                    {
                        if (values.All(v => ((v as bool?) == null || ((v as bool?) == false))))
                            return ValueForTrue;
                    }

                    return ValueForFalse;

                case BooleanOperation.None:
                case BooleanOperation.And:
                    if (values.Any(v => ((v as bool?) == null || ((v as bool?) == false))))
                        return ValueForFalse;
                    return ValueForTrue;

                case BooleanOperation.Or:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForTrue;
                    return ValueForFalse;

                case BooleanOperation.Xor:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForTrue;
                    return ValueForFalse;

                case BooleanOperation.Nand:
                    if (values.Any(v => ((v as bool?) == null || ((v as bool?) == false))))
                        return ValueForTrue;
                    return ValueForFalse;

                case BooleanOperation.Nor:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForFalse;
                    return ValueForTrue;

                case BooleanOperation.Xnor:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForFalse;
                    return ValueForTrue;

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToVisibilityConverterForMultibinding) + ".");
            }
        }

        /// <summary>
        /// Unsupported conversion method.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetTypes">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotSupportedException">Thrown if this method is called.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
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