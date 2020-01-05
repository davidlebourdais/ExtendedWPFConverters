using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter to transform a color into a solid color brush.
    /// </summary>
    public class ColorToSolidColorBrushConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Default value to be returned in case color is not valid.
        /// </summary>
        public SolidColorBrush Default { get; set; } = new SolidColorBrush(Color.FromArgb(255, 43, 116, 240));

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Color value_color ? new SolidColorBrush(value_color) : Default;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is SolidColorBrush value_solid ? value_solid.Color : Default.Color;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
