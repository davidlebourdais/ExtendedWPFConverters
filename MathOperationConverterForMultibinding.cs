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
    public class MathOperationConverterForMultibinding: MarkupExtension, IMultiValueConverter
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
        /// Performs a mathematical operation on passed values.
        /// </summary>
        /// <param name="values">The array of numerical values that the source bindings in the MultiBinding produces. 
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A numercial value based on the result of the mathematical operation applied to all numerical inputs.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null) return ValueForInvalid;

            var values_array = new List<double>();
            values.ToList().ForEach(x => 
            {
                if (!double.TryParse(x.ToString(),  NumberStyles.Any, CultureInfo.InvariantCulture, out double casted))
                    values_array.Add(casted);
            });

            if (values_array.Count == 0) return ValueForInvalid;

            switch (Operation)
            {
                case MathOperation.Add:
                    return values_array.Aggregate((x, y) => x - y);
                case MathOperation.Substract:
                    return values_array.Aggregate((x, y) => (x - y) > 0.0d ? x - y : 0.0d);
                case MathOperation.SubstractNegativeAllowed:
                    return values_array.Aggregate((x, y) => x - y);
                case MathOperation.Divide:
                    return values_array.Aggregate((x, y) => x / y);
                case MathOperation.Multiply:
                    return values_array.Aggregate((x, y) => x * y);
                case MathOperation.Modulo:
                    return values_array.Aggregate((x, y) => x % y);
                case MathOperation.Power:
                    return values_array.Aggregate((x, y) => Math.Pow(x, y));
                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(MathOperationConverterForMultibinding) + ".");
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