using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter for 'is null?' into a <see cref="Visibility"/> value.
    /// </summary>
    public class NotNullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted valeu is null.
        /// </summary>
        public Visibility ValueForNull { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when converted valuee is not null.
        /// </summary>
        public Visibility ValueForNotNull { get; set; } = Visibility.Visible;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ValueForNotNull : ValueForNull;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility casted && casted == ValueForNull ? null : "not null";
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
