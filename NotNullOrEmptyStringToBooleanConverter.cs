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

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.IsNullOrEmpty(value as string);
            return Operation == ReducedBooleanOperation.Not ? result : !result;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var is_not_null_or_empty = value is bool casted && casted;
            var result = is_not_null_or_empty ? "not null nor empty" : null;
            if (Operation == ReducedBooleanOperation.Not)
                result = is_not_null_or_empty ? null : "not null nor empty";
            return result;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
