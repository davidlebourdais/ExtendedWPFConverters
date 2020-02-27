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

        /// <summary>
        /// Converts a boolean entry into an opacity (double between 0.0 and 1.0) value.
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">An optional opacity value to be return when value is true (overrides parameterized one).</param>
        /// <param name="culture">Unused.</param>
        /// <returns>An opacity value corresponding to the entry.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // One can pass dynamic opacity value for "true" through parameter:
            double reduced_opacity = parameter == null ? DefaultOpacityForTrue : System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);

            if (value is bool castedValue)
                return castedValue ? reduced_opacity : DefaultOpacityForFalse;

            return DefaultOpacityForInvalid;
        }

        /// <summary>
        /// Returns a boolean value corresponding to a given opacity.
        /// </summary>
        /// <param name="value">The opacity value to assess.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>True if opacity value matches <see cref="DefaultOpacityForTrue"/>, false otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double reduced_opacity = parameter == null ? DefaultOpacityForTrue : System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return value is double casted && casted == reduced_opacity;
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
