using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that returns directly the opposite value of <see cref="string.IsNullOrEmpty(string)"/> on a passed string. 
    /// A 'None' operation can be set to return the actual value of the method.
    /// </summary>
    public class NotNullOrEmptyStringToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Specifies the operation to be applied with the converter.
        /// </summary>
        public ReducedBooleanOperation Operation { get; set; } = ReducedBooleanOperation.None;

        /// <summary>
        /// Returns the opposite value of <see cref="string.IsNullOrEmpty(string)"/> on the passed entry.
        /// A boolean operation can be set to invert result.
        /// </summary>
        /// <param name="value">A string entry to be assessed.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A value indicating if the string entry is null or empty or not.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.IsNullOrEmpty(value as string);
            return Operation == ReducedBooleanOperation.Not ? result : !result;
        }

        /// <summary>
        /// Returns a string for which the states depends on the result of a passed boolean value.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A string which will not be null or empty depending on the passed value and the current operation.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var is_not_null_or_empty = value is bool casted && casted;
            var result = is_not_null_or_empty ? "not null nor empty" : null;
            if (Operation == ReducedBooleanOperation.Not)
                result = is_not_null_or_empty ? null : "not null nor empty";
            return result;
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
