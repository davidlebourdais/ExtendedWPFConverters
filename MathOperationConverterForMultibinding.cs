using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections.Generic;
using System.Linq;

namespace EMA.View.Converters
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}