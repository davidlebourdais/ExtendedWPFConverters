using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections.Generic;
using System.Linq;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Performs a mathematical operation on a passed set of numbers.
    /// </summary>
    public class MathConverterForMultibinding: MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Mathematic operation to be applied.
        /// </summary>
        public MathOperation Operation { get; set; }

        /// <summary>
        /// Value to be applied when converted value is null or is not a boolean.
        /// </summary>
        public object ValueForInvalid { get; set; } = Binding.DoNothing;

        /// <summary>
        /// Indicates if the output should be a double or a string.
        /// </summary>
        public bool OutputAsString { get; set; } = false;

        /// <summary>
        /// Performs a mathematical operation on passed values.
        /// </summary>
        /// <param name="values">The array of numerical values that the source bindings in the MultiBinding produces. 
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A numercial value based on the result of the mathematical operation applied to all numerical inputs.</returns>
        /// <exception cref="NotSupportedException">Thrown if the math operation is not supported.</exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 1 || values[0] == null
                || (Operation != MathOperation.None && Operation != MathOperation.Absolute) && values.Any(x => x == null)) return ValueForInvalid;

            var values_array = new List<double>();
            foreach (var value in values)
            {
                // If we cannot deduce a number from one of the inputs, return invalid:
                if (!(double.TryParse(value.ToString().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double casted)))
                    return ValueForInvalid;
                else 
                    values_array.Add(casted);

                // Stop processing if we got the first value while not needing any more:
                if (Operation == MathOperation.None || Operation == MathOperation.Absolute)
                    break;
            }

            if (values_array.Count == 0) return ValueForInvalid;

            var result = values_array.First();
            switch (Operation)
            {
                case MathOperation.None:
                    break;
                case MathOperation.Add:
                    result = values_array.Aggregate((x, y) => x + y);
                    break;
                case MathOperation.Substract:
                    result = values_array.Aggregate((x, y) => x - y);
                    break;
                case MathOperation.SubstractPositiveOnly:
                    result = values_array.Aggregate((x, y) => x - y);
                    result = result > 0.0d ? result : 0.0d;
                    break;
                case MathOperation.Divide:
                    result = values_array.Aggregate((x, y) => y != 0 ? x / y : (x != 0 ? x < 0 ? double.NegativeInfinity : double.PositiveInfinity : double.NaN)); 
                    break;
                case MathOperation.Multiply:
                    result = values_array.Aggregate((x, y) => x * y);
                    break;
                case MathOperation.Modulo:
                    result = values_array.Aggregate((x, y) => x % y);
                    break;
                case MathOperation.Power:
                    result = values_array.Aggregate((x, y) => Math.Pow(x, y));
                    break;
                case MathOperation.Absolute:
                    result = Math.Abs(result);
                    break;
                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(MathConverterForMultibinding) + ".");
            }

            return !OutputAsString ? (object)result : result.ToString(culture ?? CultureInfo.InvariantCulture);
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