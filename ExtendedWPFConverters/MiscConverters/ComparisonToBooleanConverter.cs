using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Compares an input value to the converter parameter and returns a boolean result.
    /// </summary>
    public class ComparisonToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Gets or sets the comparison operation to apply.
        /// </summary>
        public ComparisonOperation Operation { get; set; } = ComparisonOperation.Equal;

        /// <summary>
        /// Value to be returned when the input values cannot be compared.
        /// </summary>
        public bool ValueForInvalid { get; set; } = false;

        /// <summary>
        /// Compares the input value to the converter parameter.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">The reference value to compare to.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The comparison result.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ComparisonHelper.Evaluate(value, parameter, Operation) ?? ValueForInvalid;
        }

        /// <summary>
        /// Unsupported conversion method.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotSupportedException">Thrown if this method is called.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
