using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Gets a given instance and returns its type.
    /// </summary>
    public class EqualityToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// If set, the equality converter result is inverted.
        /// </summary>
        public bool TrueIfNotEqual { get; set; } = false;

        /// <summary>
        /// Gets or sets the visibility to be returned when values are equal 
        /// (or when values are not equal when <see cref="TrueIfNotEqual"/> is set).
        /// </summary>
        public Visibility VisibilityForTrue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Gets or sets the visibility to be returned when values are equal 
        /// (or when values are not equal when <see cref="TrueIfNotEqual"/> is set).
        /// </summary>
        public Visibility VisibilityForFalse { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Checks if passed object value is equal to parameter value and returns a visibility value.
        /// </summary>
        /// <param name="value">An object to be checked.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">A reference value.</param>
        /// <param name="culture">Unused.</param>
        /// <returns><see cref="VisibilityForTrue"/> if objects are equal (or when are not equal if <see cref="TrueIfNotEqual"/> is set), 
        /// <see cref="VisibilityForFalse"/> otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null && parameter == null || value?.Equals(parameter) == true ? (TrueIfNotEqual ? VisibilityForFalse : VisibilityForTrue) :
                                                                                            (TrueIfNotEqual ? VisibilityForTrue : VisibilityForFalse);
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
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
