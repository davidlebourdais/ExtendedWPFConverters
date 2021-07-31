using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Linq;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Base class for all converters that takes multiple <see cref="bool"/> values as input to
    /// transform them in a <typeparamref name="TResult"/> object through a specified <see cref="BooleanOperation"/>.
    /// </summary>
    public class BooleanConverterBaseForMultibinding<TResult> : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Result to be returned when converted value is true.
        /// </summary>
        public virtual TResult ValueForTrue { get; set; }

        /// <summary>
        /// Result to be returned when converted value is false.
        /// </summary>
        public virtual TResult ValueForFalse { get; set; }

        /// <summary>
        /// Result to be returned when input values are not all booleans.
        /// </summary>
        public virtual TResult ValueForInvalid { get; set; }

        /// <summary>
        /// Specifies the operation to be applied with the converter.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.And;

        /// <summary>
        /// Converts several booleans to a single <typeparamref name="TResult"/> object. Booleans are combined through a boolean operation.
        /// </summary>
        /// <param name="values">The array boolean of values that the source bindings in the MultiBinding produces. 
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The  <typeparamref name="TResult"/> result of the boolean operation applied to all boolean inputs.</returns>
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Any(x => !(x is bool)))
                return ValueForInvalid;

            switch(Operation)
            {
                case BooleanOperation.Equality:
                    var firstValue = false;
                    if (values.Length > 0)
                        firstValue = values[0] is bool && values[0] as bool? == true;

                    // Case all true:
                    if (firstValue)
                    {
                        if (values.All(v => v is bool && v as bool? == true))
                            return ValueForTrue;
                    }
                    else // case all false:
                    {
                        if (values.All(v => !(v is bool) || v as bool? == false))
                            return ValueForTrue;
                    }
                    return ValueForFalse;

                case BooleanOperation.None:
                case BooleanOperation.And:
                    return values.Any(v => !(v is bool) || v as bool? == false) ? ValueForFalse : ValueForTrue;

                case BooleanOperation.Or:
                    return values.Any(v => v is bool && v as bool? == true) ? ValueForTrue : ValueForFalse;

                case BooleanOperation.Xor:
                    return values.Count(x => x as bool? == true) % 2 == 1 ? ValueForTrue : ValueForFalse;

                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                    return values.Any(v => !(v is bool) || v as bool? == false) ? ValueForTrue : ValueForFalse;

                case BooleanOperation.Nor:
                    return values.Any(v => v is bool && v as bool? == true) ? ValueForFalse : ValueForTrue;

                case BooleanOperation.XNor:
                    return values.Count(x => x as bool? == true) % 2 == 1 ? ValueForFalse : ValueForTrue;

                default:
                    throw new NotSupportedException(Operation + " is not supported for " + nameof(BooleanConverterBaseForMultibinding<TResult>) + ".");
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
        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
