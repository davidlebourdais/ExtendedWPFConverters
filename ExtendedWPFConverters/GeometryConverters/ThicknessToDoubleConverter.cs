using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a uniform <see cref="Thickness"/> into a double.
    /// </summary>
    public class ThicknessToDoubleConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if a <see cref="Exception"/> must be thrown when trying to convert
        /// a <see cref="Thickness"/> that stores non-uniform double values, i.e.
        /// <see cref="Thickness.Left"/>, <see cref="Thickness.Top"/>,
        /// <see cref="Thickness.Right"/> or <see cref="Thickness.Bottom"/> are not equal.
        /// </summary>
        public bool ThrowOnNonUniformThickness { get; set; }
        
        /// <summary>
        /// Returns the uniform value of a <see cref="Thickness"/> or the 
        /// <see cref="Thickness.Left"/> value.
        /// </summary>
        /// <param name="value">A <see cref="Thickness"/> entry to be converted.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A the uniform <see cref="Thickness"/> or the <see cref="Thickness.Left"/> value.</returns>
        /// <exception cref="ArgumentException">The <see cref="Thickness"/> values were not uniform.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Thickness thickness))
                return 0.0d;

            if (!ThrowOnNonUniformThickness)
                return thickness.Left;
            
            if (Math.Abs(thickness.Left - thickness.Top) > 0.1d || 
                Math.Abs(thickness.Left - thickness.Right) > 0.1d ||
                Math.Abs(thickness.Left - thickness.Bottom) > 0.1d)
                throw new ArgumentException($"Thickness is not uniform and cannot be converted. Values are {thickness}");
            
            return thickness.Left;
        }

        /// <summary>
        /// Returns a uniform <see cref="Thickness"/> based on a <see cref="double"/> value.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A uniform <see cref="Thickness"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double asDouble ? new Thickness(asDouble, asDouble, asDouble, asDouble) : new Thickness();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
