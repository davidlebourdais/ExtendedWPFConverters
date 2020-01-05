using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that converts a string state into a <see cref="Visibility"/> value.
    /// </summary>
    public class NullOrEmptyStringToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is null or empty.
        /// </summary>
        public Visibility ValueForNullOrEmpty { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when converted string is not null nor empty.
        /// </summary>
        public Visibility ValueForNotNullOrEmpty { get; set; } = Visibility.Visible;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility casted && casted == ValueForNullOrEmpty ? null : "not null";
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
