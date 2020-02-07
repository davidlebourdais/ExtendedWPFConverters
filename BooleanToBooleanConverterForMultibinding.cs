using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter for multiboolean operations to be used with multibindings.
    /// </summary>
    public class BooleanToBooleanConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Specifies the operation to be applied with the converter.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.And;

        /// <summary>
        /// Converts several booleans to a single result through a boolean operation.
        /// </summary>
        /// <param name="values">The array boolean of values that the source bindings in the MultiBinding produces. 
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The boolean result of the boolean operation applied to all boolean inputs.</returns>
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
                        return values.All(v => ((v as bool?) != null && ((v as bool?) == true)));
                    }
                    else // case all false:
                    {
                        return values.All(v => ((v as bool?) == null || ((v as bool?) == false)));
                    }

                case BooleanOperation.None:
                case BooleanOperation.And:
                    return !(values.Any(v => ((v as bool?) == null || ((v as bool?) == false))));

                case BooleanOperation.Or:
                    return values.Any(v => ((v as bool?) != null && ((v as bool?) == true)));

                case BooleanOperation.Xor:
                    return values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true)));

                case BooleanOperation.Nand:
                    return (values.Any(v => ((v as bool?) == null || ((v as bool?) == false))));

                case BooleanOperation.Nor:
                    return !(values.Any(v => ((v as bool?) != null && ((v as bool?) == true))));

                case BooleanOperation.Xnor:
                    return !(values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true))));

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToBooleanConverterForMultibinding) + ".");
            }
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetTypes">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
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