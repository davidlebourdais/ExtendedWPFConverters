using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into a <see cref="Thickness"/> value.
    /// </summary>
    public class BooleanToThicknessConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public ReducedBooleanOperation Operation { get; set; } = ReducedBooleanOperation.None;

        /// <summary>
        /// <see cref="Thickness"/> value to be applied when conversion operation output is true..
        /// </summary>
        public Thickness ValueForTrue { get; set; } = new Thickness(1);

        /// <summary>
        /// <see cref="Thickness"/> value to be applied when conversion operation output is false.
        /// </summary>
        public Thickness ValueForFalse { get; set; } = new Thickness(0);

        /// <summary>
        /// <see cref="Thickness"/> value to be applied when input is null or not boolean.
        /// </summary>
        public Thickness ValueForInvalid { get; set; } = new Thickness(0);

        /// <summary>
        /// Returns a thickness value that depends on the boolean entry processed through the boolean operation.
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">The object to be returned when the operation result is positive.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A <see cref="Thickness"/> value matching the conversion output.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (value as bool?) == true;
            var operated = Operation == ReducedBooleanOperation.Not ? !result : result;
            return operated ? ValueForTrue : ValueForFalse;
        }

        /// <summary>
        /// Returns true or false depending on wether the input is a
        /// known thickness value, and depending on the current operation.
        /// </summary>
        /// <param name="value">The object value to be assessed.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>True if the object matches thickness for true values (false if inverse operation is set), 
        /// and returns the opposite value otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Operation == ReducedBooleanOperation.None ? (value as Thickness?) == ValueForTrue : (value as Thickness?) != ValueForTrue;
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
