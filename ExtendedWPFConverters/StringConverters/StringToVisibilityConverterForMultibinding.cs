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
    public class StringToVisibilityConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is null or empty.
        /// </summary>
        public Visibility ValueForNullOrEmpty { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when converted string is not null nor empty.
        /// </summary>
        public Visibility ValueForNotNullOrEmpty { get; set; } = Visibility.Visible;

        /// <summary>
        /// Operation to be used accross the mutliple bound values.
        /// </summary>
        public BooleanOperation OperationWithEnablers { get; set; } = BooleanOperation.And;

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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Fist value must be the string;
            if (values.Length < 1) return ValueForNullOrEmpty;

            if (!(values[0] is string text))
                return ValueForNullOrEmpty;
            if ((values.Length == 1 || OperationWithEnablers == BooleanOperation.And) && string.IsNullOrEmpty(text))
                return ValueForNullOrEmpty;
            else if (values.Length == 1)
                return ValueForNotNullOrEmpty;

            // Other values will be booleans that will actiated or not the output regarding to
            // the boolean operation with enablers that is set:
            var enablers = new List<bool>();
            for(int i = 1; i < values.Length; i++)
                if (values[i] is bool casted)
                    enablers.Add(casted);

            if (enablers.Count == 0)
                return string.IsNullOrEmpty(text) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;

            if (OperationWithEnablers == BooleanOperation.And && enablers.Any(x => x == false))
                return ValueForNullOrEmpty;

            if (OperationWithEnablers == BooleanOperation.Or && string.IsNullOrEmpty(text) && enablers.All(x => x == false))
                return ValueForNullOrEmpty;

            return ValueForNotNullOrEmpty;
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