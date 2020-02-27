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

        /// <summary>
        /// Makes an association between a boolean entry and a number to be returned.
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">An optional value for "true" output (overrives parameterized one).</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A number corresponding to the boolean entry value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // One can pass dynamic value for "true" through parameter:
            var value_for_true = parameter ?? ValueForTrue;

            if (value is bool castedValue)
                return castedValue ? value_for_true : ValueForFalse;

            return ValueForInvalid;
        }

        /// <summary>
        /// Converts a number back into a boolean value.
        /// </summary>
        /// <param name="value">A numerical value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A boolean value that is equivalent to the passed number.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is IComparable casted && casted == (parameter ?? ValueForTrue);
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
