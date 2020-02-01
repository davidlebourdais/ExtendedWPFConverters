using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Returns an object set as parameter if when main boolean condition is satisfied.
    /// </summary>
    public class BooleanToObjectConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.None;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool value_bool = false;
            if (value == null || !(value is bool))
                return null;
            value_bool = (bool)value;

            switch (Operation)
            {
                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                case BooleanOperation.Nor:
                    return value_bool ? null : parameter;
                case BooleanOperation.None:
                case BooleanOperation.Equality:
                case BooleanOperation.Or:
                case BooleanOperation.And:
                    return value_bool ? parameter : null;

                case BooleanOperation.Xor:
                    return null;

                case BooleanOperation.Xnor:
                    return parameter;

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToObjectConverter) + ".");
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
