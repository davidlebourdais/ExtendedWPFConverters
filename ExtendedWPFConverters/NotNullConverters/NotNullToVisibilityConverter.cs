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
        /// Value to be applied when converted value is null, DependencyProperty.UnsetValue or Binding.DoNothing.
        /// </summary>
        public Visibility ValueForNull { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when converted value is a non-null object rather than a marker for unresolved or suppressed bindings.
        /// </summary>
        public Visibility ValueForNotNull { get; set; } = Visibility.Visible;

        /// <summary>
        /// Returns ValueForNull for null or markers for unresolved or suppressed bindings; otherwise returns ValueForNotNull.
        /// </summary>
        /// <param name="value">An entry that can be null, DependencyProperty.UnsetValue or Binding.DoNothing.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The <see cref="Visibility"/> value corresponding to the entry state.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || ReferenceEquals(value, DependencyProperty.UnsetValue) || ReferenceEquals(value, Binding.DoNothing))
                return ValueForNull;

            return ValueForNotNull;
        }

        /// <summary>
        /// Returns a null or non-null object depending on a passed visibility value.
        /// </summary>
        /// <param name="value">A <see cref="Visibility"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>An object that will be non-null depending on the passed value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility casted && casted == ValueForNull ? null : "not null";
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
