using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into an opacity level.
    /// </summary>
    public class BooleanToOpacityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        public double DefaultOpacityForTrue { get; set; } = 0.4d;

        /// <summary>
        /// Value to be applied when converted value is false.
        /// </summary>
        public double DefaultOpacityForFalse { get; set; } = 1.0d;

        /// <summary>
        /// Value to be applied when converted value is null or is not a boolean.
        /// </summary>
        public double DefaultOpacityForInvalid { get; set; } = 1.0d;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // One can pass dynamic opacity value for "true" through parameter:
            double reduced_opacity = parameter == null ? DefaultOpacityForTrue : System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);

            if (value is bool castedValue)
                return castedValue ? reduced_opacity : DefaultOpacityForFalse;

            return DefaultOpacityForInvalid;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double reduced_opacity = parameter == null ? DefaultOpacityForTrue : System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);

            return value is double casted && casted == reduced_opacity;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
