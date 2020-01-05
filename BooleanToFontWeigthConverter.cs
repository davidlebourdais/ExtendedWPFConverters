using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that transforms a boolean value into a <see cref="FontWeight"/>.
    /// </summary>
    public class BooleanToFontWeightConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when converted value is true.
        /// </summary>
        public FontWeight ValueForTrue { get; set; } = FontWeights.DemiBold;
        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when converted value is false, null or not boolean.
        /// </summary>
        public FontWeight ValueForFalse { get; set; } = FontWeights.Normal;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool value_bool && value_bool ? ValueForTrue : ValueForFalse;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is FontWeight casted && casted == ValueForTrue;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}