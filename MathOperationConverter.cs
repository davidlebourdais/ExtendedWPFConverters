using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Performs a mathematical operation between a number and another 
    /// one that is passed as parameter of the converter.
    /// </summary>
    /// <remarks>Inspired from WPF converters of http://materialdesigninxaml.net/</remarks>
    public class MathOperationConverter : MarkupExtension, IValueConverter
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(!double.TryParse(value.ToString(),  NumberStyles.Any, CultureInfo.InvariantCulture, out double value1)) 
                return ValueForInvalid;

            // Return input value if no operation to perform:
            if (Operation == MathOperation.None) return value;

            // Returns itself is value2 is invalid:
            if(!double.TryParse(parameter.ToString(),  NumberStyles.Any, CultureInfo.InvariantCulture, out double value2)) return value1;
            
            switch (Operation)
            {
                case MathOperation.Add:
                    return value1 + value2;
                case MathOperation.Substract:
                    return (value1 - value2) > 0 ? (value1 - value2) : 0; 
                case MathOperation.SubstractNegativeAllowed:
                    return value1 - value2; 
                case MathOperation.Multiply:
                    return value1 * value2; 
                case MathOperation.Divide:
                    return value1 / value2; 
                case MathOperation.Modulo:
                    return value1 % value2;
                case MathOperation.Power:
                    return Math.Pow(value1, value2);
                default:
                    return ValueForInvalid;
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double value1 = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                double value2 = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                switch (Operation)
                {
                    case MathOperation.Add:
                        return value1 - value2;
                    case MathOperation.Substract:
                        return value1 + value2;
                    case MathOperation.Multiply:
                        return value1 / value2;
                    case MathOperation.Divide:
                        return value1 * value2;
                    default:
                        throw new NotSupportedException();
                }
            }
            catch
            {
                throw new NotSupportedException();            
            }
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
