using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Compares an input value to the converter parameter and returns a visibility result.
    /// </summary>
    public class ComparisonToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Gets or sets the comparison operation to apply.
        /// </summary>
        public ComparisonOperation Operation { get; set; } = ComparisonOperation.Equal;

        /// <summary>
        /// Gets or sets the visibility returned when the comparison result is true.
        /// </summary>
        public Visibility VisibilityForTrue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Gets or sets the visibility returned when the comparison result is false.
        /// </summary>
        public Visibility VisibilityForFalse { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Gets or sets the visibility returned when the input values cannot be compared.
        /// </summary>
        public Visibility ValueForInvalid { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Compares the input value to the converter parameter.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">The reference value to compare to.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A visibility value corresponding to the comparison result.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var comparisonResult = ComparisonHelper.Evaluate(value, parameter, Operation);
            return comparisonResult.HasValue ? (comparisonResult.Value ? VisibilityForTrue : VisibilityForFalse) : ValueForInvalid;
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
