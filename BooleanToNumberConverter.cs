using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into an <see cref="IComparable"/> one.
    /// </summary>
    public class BooleanToNumberConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        public IComparable ValueForTrue { get; set; } = 0.4d;

        /// <summary>
        /// Value to be applied when converted value is false.
        /// </summary>
        public IComparable ValueForFalse { get; set; } = 1.0d;

        /// <summary>
        /// Value to be applied when converted value is null or is not a boolean.
        /// </summary>
        public IComparable ValueForInvalid { get; set; } = 1.0d;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // One can pass dynamic value for "true" through parameter:
            var value_for_true = parameter ?? ValueForTrue;

            if (value is bool castedValue)
                return castedValue ? value_for_true : ValueForFalse;

            return ValueForInvalid;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is IComparable casted && casted == (parameter ?? ValueForTrue);
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
