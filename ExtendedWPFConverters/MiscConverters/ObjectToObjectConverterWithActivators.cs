using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter that returns first object (values[0]) or null regarding to passed boolean values in the next values[1..n] called activators.
    /// This is a way to provide object based on bound conditions using multibinding.
    /// </summary>
    public class ObjectToObjectConverterWithActivators : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Value to be applied when passed boolean activators are invalid (not that providing
        /// no boolean is considered as a valid input).
        /// </summary>
        public object ValueForInvalid { get; set; } = null;

        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public BooleanOperation ActivationOperation { get; set; } = BooleanOperation.And;

        /// <summary>
        /// Returns first passed object depending on a boolean operation applied to remaining boolean entries called 'activators'.
        /// </summary>
        /// <param name="values">Should contain the object to be returned in first position, then a set of boolean entries to be evaluated through a boolean operation.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The first passed object or null depending on the result of the boolean operation.</returns>
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (values.Length)
            {
                case 0:
                    return null;
                case 1:
                    return values[0];
            }

            if (!values.Skip(1).All(x => x is bool))
                return ValueForInvalid;

            var activators = values.Skip(1).Cast<bool>().ToArray();

            switch(ActivationOperation)
            {
                case BooleanOperation.Equality:
                    return activators.All(x => x == activators[0]) ? values[0] : null;

                case BooleanOperation.None:
                case BooleanOperation.And:
                    return activators.Any(x => !x) ? null : values[0];

                case BooleanOperation.Or:
                    return activators.Any(x => x) ? values[0] : null;

                case BooleanOperation.Xor:
                    return activators.Count(x => x) % 2 == 1 ? values[0] : null;

                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                    return activators.Any(x => !x) ? values[0] : null;

                case BooleanOperation.Nor:
                    return activators.Any(x => x) ? null : values[0];

                case BooleanOperation.XNor:
                    return activators.Count(x => x) % 2 == 1 ? null : values[0];

                default:
                    throw new NotSupportedException(ActivationOperation + " is not supported for " + nameof(ObjectToObjectConverterWithActivators) + ".");
            }
        }

        /// <summary>
        /// Unsupported conversion method.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetTypes">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotSupportedException">Thrown if this method is called.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}