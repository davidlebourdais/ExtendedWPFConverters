using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a double into a uniform <see cref="Thickness"/>.
    /// </summary>
    public class DoubleToThicknessConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if an <see cref="Exception"/> must be thrown when trying to convert back
        /// a non-uniform <see cref="Thickness"/>.
        /// </summary>
        public bool ThrowOnNonUniformThickness { get; set; }

        /// <summary>
        /// Returns a uniform <see cref="Thickness"/> based on a <see cref="double"/> value.
        /// </summary>
        /// <param name="value">A double value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A uniform <see cref="Thickness"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double asDouble ? new Thickness(asDouble, asDouble, asDouble, asDouble) : new Thickness();
        }

        /// <summary>
        /// Returns the uniform value of a <see cref="Thickness"/> or the <see cref="Thickness.Left"/> value.
        /// </summary>
        /// <param name="value">A <see cref="Thickness"/> entry to be converted.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The uniform thickness value or the left thickness value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ThicknessToDoubleConverter() { ThrowOnNonUniformThickness = ThrowOnNonUniformThickness }
                .Convert(value, targetType, parameter, culture);
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
