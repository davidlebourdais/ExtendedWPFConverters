using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using EMA.ExtendedWPFConverters.Utils;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Performs a mathematical operation between a number and another 
    /// one that is passed as parameter of the converter.
    /// </summary>
    /// <remarks>Inspired from WPF converters of http://materialdesigninxaml.net/</remarks>
    public class MathConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Mathematics operation to be applied.
        /// </summary>
        public MathOperation Operation { get; set; }

        /// <summary>
        /// Value to be applied when converted value is null or is not a boolean.
        /// </summary>
        public object ValueForInvalid { get; set; } = null;

        /// <summary>
        /// Indicates if the output should be a double or a string.
        /// </summary>
        public bool OutputAsString { get; set; } = false;

        /// <summary>
        /// Private. Indicates if the input is a double or a string.
        /// </summary>
        /// <remarks>Used only for convert back method.</remarks>
        private bool InputAsString { get; set; }

        /// <summary>
        /// Performs a mathematical operation between entry and parameter and returns the result.
        /// </summary>
        /// <param name="value">A numerical value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">A numerical value.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The result of the parameterized mathematical operation between entry and the parameter.</returns>
        /// <exception cref="NotSupportedException">Thrown if the math operation is not supported.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            InputAsString = value is string;

            if(value == null || !double.TryParse(value.ToString()?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var value1)) 
                return ValueForInvalid;

            // Returns invalid is value2 is invalid, except if not going to use it:
            var value2 = 0.0d;
            if(Operation != MathOperation.None && Operation != MathOperation.Absolute && 
                (parameter == null || !double.TryParse(parameter.ToString()?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value2))) 
                return ValueForInvalid;
            
            var result = value1;
            switch (Operation)
            {
                case MathOperation.None:
                    // (this one actually acts as a string to double or string to string converter if needed)
                    break;
                case MathOperation.Add:
                    result = value1 + value2;
                    break;
                case MathOperation.Subtract:
                    result = value1 - value2; 
                    break;
                case MathOperation.SubtractPositiveOnly:
                    result = (value1 - value2) > 0 ? (value1 - value2) : 0; 
                    break;
                case MathOperation.Multiply:
                    result = value1 * value2; 
                    break;
                case MathOperation.Divide:
                    result = value2 != 0 ? value1 / value2 : (value1 != 0 ? value1 < 0 ? double.NegativeInfinity : double.PositiveInfinity : double.NaN); 
                    break;
                case MathOperation.Modulo:
                    result = value1 % value2;
                    break;
                case MathOperation.Power:
                    result = Math.Pow(value1, value2);
                    break;
                case MathOperation.Absolute:
                    result = Math.Abs(value1);
                    break;
                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(MathConverter) + ".");
            }

            return !OutputAsString ? (object)result : result.ToString(culture ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an inverse mathematical operation on the entry and a passed parameter and returns the result.
        /// </summary>
        /// <param name="value">A numerical value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">A numerical value.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The inverse result of what should be obtained on the passed entries if the <see cref="Convert"/> method was called.</returns>
        /// <exception cref="NotSupportedException">Thrown if the math operation is not supported for convert back.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return ValueForInvalid;

            double value1;
            try
            {
                value1 = System.Convert.ToDouble(value, culture ?? CultureInfo.InvariantCulture);
            }
            catch
            {
                return ValueForInvalid;
            }

            var value2 = 0.0d;
            if(Operation != MathOperation.None && Operation != MathOperation.Absolute)
            {
                if(parameter == null)
                    return ValueForInvalid;
                try
                {
                    value2 = System.Convert.ToDouble(parameter, culture ?? CultureInfo.InvariantCulture);
                }
                catch
                {
                    return ValueForInvalid;
                }
            } 

            var result = (double?)null;
            switch (Operation)
            {
                case MathOperation.None:
                case MathOperation.Absolute:
                    result = value1;
                    break;
                case MathOperation.Add:
                    result = value1 - value2;
                    break;
                case MathOperation.Subtract:
                case MathOperation.SubtractPositiveOnly:
                    result = value1 + value2;
                    break;
                case MathOperation.Multiply:
                    result = value1 / value2;
                    break;
                case MathOperation.Modulo:
                    if (((int)value1).InverseUnderModulo((int)value2, out int resultAsInt))
                        result = (double)resultAsInt;
                    break;
                case MathOperation.Divide:
                    result = value1 * value2;
                    break;
                case MathOperation.Power:
                    result = Math.Pow(value1, value2 != 0 ? 1 / value2 : double.PositiveInfinity);
                    break;
                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(MathConverter) + ".");
            }

            if (result != null)
                return InputAsString ? result.Value.ToString(culture ?? CultureInfo.InvariantCulture) : (object)result;
            return ValueForInvalid;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
