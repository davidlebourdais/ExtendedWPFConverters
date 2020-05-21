using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Base class for all converters that takes as input a single <see cref="bool"/> value to
    /// transform it in a <typeparamref name="TResult"/> object.
    /// </summary>
    public class BooleanConverterBase<TResult> : MarkupExtension, IValueConverter
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
        /// Result to be returned when input value is null or not boolean.
        /// </summary>
        public virtual TResult ValueForInvalid { get; set; }

        /// <summary>
        /// Specifies the operation to be applied with the converter.
        /// </summary>
        public ReducedBooleanOperation Operation { get; set; } = ReducedBooleanOperation.None;

        /// <summary>
        /// Makes an association between a boolean entry and a <typeparamref name="TResult"/> object to be returned 
        /// after being processed by the specified <see cref="ReducedBooleanOperation"/> (default is none).
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">An optional value for "true" output (overrides parameterized one nammed <see cref="ValueForTrue"/>).</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A value corresponding to the boolean entry value once processed by the specified <see cref="BooleanConverterBase{TResult}.Operation"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // One can pass value for "true" through parameter:
            var value_for_true = parameter is TResult ? parameter : ValueForTrue;

            if (value is bool castedValue)
            {
                if (Operation == ReducedBooleanOperation.None)
                    return castedValue ? value_for_true : ValueForFalse;
                else if (Operation == ReducedBooleanOperation.Not)
                    return castedValue ? ValueForFalse : value_for_true;
                else
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanConverterBase<TResult>) + ".");
            }
            else return ValueForInvalid;
        }

        /// <summary>
        /// Converts a <typeparamref name="TResult"/> back into a boolean.
        /// </summary>
        /// <param name="value">A <typeparamref name="TResult"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A boolean value that matches best the passed entry.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TResult casted && casted.Equals(Operation == ReducedBooleanOperation.None ? (parameter is TResult ? parameter : ValueForTrue) : ValueForFalse);
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
