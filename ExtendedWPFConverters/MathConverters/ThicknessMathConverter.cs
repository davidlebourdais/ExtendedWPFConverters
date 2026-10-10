using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Applies a mathematical operation with a scalar parameter to each side of a <see cref="Thickness"/>.
    /// </summary>
    public class ThicknessMathConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Mathematical operation to be applied to each side.
        /// </summary>
        public MathOperation Operation { get; set; }

        /// <summary>
        /// Value to be returned when the input is not a thickness, the scalar parameter is invalid,
        /// or backward conversion requires a nonzero parameter and receives zero.
        /// </summary>
        public object ValueForInvalid { get; set; } = null;

        /// <summary>
        /// Applies the selected operation to all four sides of the input thickness.
        /// </summary>
        /// <param name="value">The thickness to convert.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">A numeric value or numeric string, ignored for None and Absolute.</param>
        /// <param name="culture">The conversion culture.</param>
        /// <returns>The resulting thickness, or <see cref="ValueForInvalid"/> for invalid input.</returns>
        /// <exception cref="NotSupportedException">Thrown if the mathematical operation is unsupported.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertThickness(value, parameter, culture, Operation);
        }

        /// <summary>
        /// Applies the inverse of None, Add, Subtract, Multiply or Divide to all four sides.
        /// </summary>
        /// <param name="value">The thickness to convert back.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">The original scalar parameter. Must be nonzero for Multiply and Divide.</param>
        /// <param name="culture">The conversion culture.</param>
        /// <returns>The original thickness, or <see cref="ValueForInvalid"/> for invalid input.</returns>
        /// <exception cref="NotSupportedException">Thrown for operations without an unambiguous inverse.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MathOperation inverseOperation;
            switch (Operation)
            {
                case MathOperation.None:
                    inverseOperation = MathOperation.None;
                    break;
                case MathOperation.Add:
                    inverseOperation = MathOperation.Subtract;
                    break;
                case MathOperation.Subtract:
                    inverseOperation = MathOperation.Add;
                    break;
                case MathOperation.Multiply:
                    inverseOperation = MathOperation.Divide;
                    break;
                case MathOperation.Divide:
                    inverseOperation = MathOperation.Multiply;
                    break;
                default:
                    throw new NotSupportedException(Operation + " does not have an unambiguous inverse for " + nameof(ThicknessMathConverter) + ".");
            }

            return ConvertThickness(value, parameter, culture, inverseOperation,
                Operation == MathOperation.Multiply || Operation == MathOperation.Divide);
        }

        private object ConvertThickness(object value, object parameter, CultureInfo culture,
            MathOperation operation, bool requireNonZeroParameter = false)
        {
            if (!(value is Thickness thickness))
                return ValueForInvalid;

            var mathConverter = new MathConverter();
            var scalar = 0.0d;
            if (operation != MathOperation.None && operation != MathOperation.Absolute)
            {
                var parsedParameter = mathConverter.Convert(parameter, typeof(double), null, culture);
                if (!(parsedParameter is double number) || (requireNonZeroParameter && number == 0))
                    return ValueForInvalid;

                scalar = number;
            }

            mathConverter.Operation = operation;
            var left = mathConverter.Convert(thickness.Left, typeof(double), scalar, culture);
            var top = mathConverter.Convert(thickness.Top, typeof(double), scalar, culture);
            var right = mathConverter.Convert(thickness.Right, typeof(double), scalar, culture);
            var bottom = mathConverter.Convert(thickness.Bottom, typeof(double), scalar, culture);

            if (left is double leftValue && top is double topValue &&
                right is double rightValue && bottom is double bottomValue)
                return new Thickness(leftValue, topValue, rightValue, bottomValue);

            return ValueForInvalid;
        }

        /// <summary>
        /// Returns this converter as the value of the markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper for the markup extension.</param>
        /// <returns>This converter.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
