using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a uniform <see cref="CornerRadius"/> into a double.
    /// </summary>
    public class CornerRadiusToDoubleConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if a <see cref="Exception"/> must be thrown when trying to convert
        /// a <see cref="CornerRadius"/> that stores non-uniform double values, i.e.
        /// <see cref="CornerRadius.TopLeft"/>, <see cref="CornerRadius.TopRight"/>,
        /// <see cref="CornerRadius.BottomRight"/> or <see cref="CornerRadius.BottomLeft"/> are not equal.
        /// </summary>
        public bool ThrowOnNonUniformCornerRadius { get; set; }
        
        /// <summary>
        /// Returns the uniform value of a <see cref="CornerRadius"/> or the 
        /// <see cref="CornerRadius.TopLeft"/> value.
        /// </summary>
        /// <param name="value">A <see cref="CornerRadius"/> entry to be convverted.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A the uniform <see cref="CornerRadius"/> or the <see cref="CornerRadius.TopLeft"/> value.</returns>
        /// <exception cref="ArgumentException">The <see cref="CornerRadius"/> values were not uniform.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CornerRadius cornerRadius))
                return 0.0d;

            if (!ThrowOnNonUniformCornerRadius)
                return cornerRadius.TopLeft;
            
            if (Math.Abs(cornerRadius.TopLeft - cornerRadius.TopRight) > 0.1d || 
                Math.Abs(cornerRadius.TopLeft - cornerRadius.BottomRight) > 0.1d ||
                Math.Abs(cornerRadius.TopLeft - cornerRadius.BottomLeft) > 0.1d)
                throw new ArgumentException($"Corner radius is not uniform and cannot be converted. Values are {cornerRadius}");
            
            return cornerRadius.TopLeft;
        }

        /// <summary>
        /// Returns a uniform <see cref="CornerRadius"/> based on a <see cref="double"/> value.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A uniform <see cref="CornerRadius"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double asDouble ? new CornerRadius(asDouble, asDouble, asDouble, asDouble) : new CornerRadius();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
