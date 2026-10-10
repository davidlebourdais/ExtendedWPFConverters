using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a <see cref="GridLength"/> into a double.
    /// </summary>
    public class GridLengthToDoubleConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if an <see cref="Exception"/> must be thrown when trying to convert
        /// a non-absolute <see cref="GridLength"/>.
        /// </summary>
        public bool ThrowOnNonAbsoluteGridLength { get; set; }

        /// <summary>
        /// Gets or sets the resulting <see cref="GridUnitType"/> used during back conversion.
        /// </summary>
        public GridUnitType GridUnitType { get; set; } = GridUnitType.Pixel;

        /// <summary>
        /// Returns the numeric value of a <see cref="GridLength"/>.
        /// </summary>
        /// <param name="value">A <see cref="GridLength"/> entry to be converted.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The <see cref="GridLength.Value"/> of the grid length.</returns>
        /// <exception cref="ArgumentException">The <see cref="GridLength"/> is not absolute while strict mode is enabled.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is GridLength gridLength))
                return 0.0d;

            if (ThrowOnNonAbsoluteGridLength && !gridLength.IsAbsolute)
                throw new ArgumentException($"GridLength is not absolute and cannot be converted. Value is {gridLength}");

            return gridLength.Value;
        }

        /// <summary>
        /// Returns a <see cref="GridLength"/> based on a <see cref="double"/> value.
        /// </summary>
        /// <param name="value">A double value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A <see cref="GridLength"/> corresponding to the configured <see cref="GridUnitType"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double asDouble))
                return new GridLength(0);

            return GridUnitType == GridUnitType.Auto ? GridLength.Auto : new GridLength(asDouble, GridUnitType);
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
