using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a string coupled to a set of optional booleans (enablers) to a visibility value.
    /// </summary>
    public class NotNullOrEmptyToVisibilityConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is not null nor empty.
        /// </summary>
        public Visibility ValueForNotNullOrEmpty { get; set; } = Visibility.Visible;

        /// <summary>
        /// Value to be applied when converted string is null or empty.
        /// </summary>
        public Visibility ValueForNullOrEmpty { get; set; } = Visibility.Collapsed;
        
        /// <summary>
        /// Value to be applied when passed boolean enablers are invalid (not that providing
        /// no boolean is considered as a valid input).
        /// </summary>
        public Visibility ValueForInvalid { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Operation to be used accross the multiple bound values.
        /// </summary>
        public BooleanOperation OperationForEnablers { get; set; } = BooleanOperation.And;

        /// <summary>
        /// Converts a string and passed booleans to a visibility value regarding to whether the string is null or empty and the 
        /// boolean operation result applied to boolean entries is verified.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the MultiBinding produces. 
        /// First value must be the string to assess, while remaining values are booleans. 
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A visibility value based on the string state and the result of the boolean operation applied to boolean inputs.</returns>
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // There must be a first value:
            if (values.Length < 1) return ValueForInvalid;

            // Other values will be booleans that will activate or not the output regarding to
            // the boolean operation that is set:
            var enablers = new List<bool>();
            for(int i = 1; i < values.Length; i++)
            {
                if (values[i] is bool casted)
                    enablers.Add(casted);
                else return ValueForInvalid;
            }

            // Return result if first item is a null or empty string:
            if (string.IsNullOrEmpty(values[0] as string))
                return ValueForNullOrEmpty;

            // Do not process anymore if no inputs were provided.
            if (enablers.Count == 0)  
                return ValueForNotNullOrEmpty; 

            // Fully process otherwise:
            switch (OperationForEnablers)
            {
                case  BooleanOperation.Equality:
                    return enablers.Skip(1).Any(x => x != enablers.First()) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                case BooleanOperation.None:
                case BooleanOperation.And:
                    return enablers.Any(x => x == false) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                case BooleanOperation.Not:
                case  BooleanOperation.Nand:
                    return enablers.All(x => x == true) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                case BooleanOperation.Or:
                    return enablers.All(x => x == false) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                case BooleanOperation.Nor:
                    return enablers.Any(x => x == true) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                case BooleanOperation.Xor:
                    return (enablers.Count(x => x == true) % 2 == 0) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                case BooleanOperation.Xnor:
                    return (enablers.Count(x => x == true) % 2 == 1) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
                default:
                    throw new NotSupportedException(OperationForEnablers.ToString() + " is not supported for " + nameof(NotNullOrEmptyToVisibilityConverterForMultibinding) + ".");
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
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}