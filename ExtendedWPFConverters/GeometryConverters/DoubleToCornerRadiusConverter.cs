using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a double into a uniform <see cref="CornerRadius"/>.
    /// </summary>
    public class DoubleToCornerRadiusConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if an <see cref="Exception"/> must be thrown when trying to convert back
        /// a non-uniform <see cref="CornerRadius"/>.
        /// </summary>
        public bool ThrowOnNonUniformCornerRadius { get; set; }

        /// <summary>
        /// Returns a uniform <see cref="CornerRadius"/> based on a <see cref="double"/> value.
        /// </summary>
        /// <param name="value">A double value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A uniform <see cref="CornerRadius"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double asDouble ? new CornerRadius(asDouble, asDouble, asDouble, asDouble) : new CornerRadius();
        }

        /// <summary>
        /// Returns the uniform value of a <see cref="CornerRadius"/> or the <see cref="CornerRadius.TopLeft"/> value.
        /// </summary>
        /// <param name="value">A <see cref="CornerRadius"/> entry to be converted.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The uniform corner radius value or the top-left radius value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new CornerRadiusToDoubleConverter() { ThrowOnNonUniformCornerRadius = ThrowOnNonUniformCornerRadius }
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
