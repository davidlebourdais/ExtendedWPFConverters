using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.View.Converters
{
    /// <summary>
    /// Gets a given instance and returns its type.
    /// </summary>
    public class InstanceToTypeConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.GetType();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}