using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter to transform a <see cref="Color"/> into a <see cref="SolidColorBrush"/>.
    /// </summary>
    public class ColorToSolidColorBrushConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Default value to be returned in case color is not valid.
        /// </summary>
        public SolidColorBrush Default { get; set; } = Brushes.Black;

        /// <summary>
        /// Converts a <see cref="Color"/> into a <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="value">A <see cref="Color"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A <see cref="SolidColorBrush"/> implementing the passed <see cref="Color"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Color value_color ? new SolidColorBrush(value_color) : Default;
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush"/> into a <see cref="Color"/>.
        /// </summary>
        /// <param name="value">A <see cref="SolidColorBrush"/> from which to extract a <see cref="Color"/>.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The <see cref="Color"/> contained into the passed entry, or the default color if entry is invalid.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is SolidColorBrush value_solid ? value_solid.Color : Default.Color;
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
