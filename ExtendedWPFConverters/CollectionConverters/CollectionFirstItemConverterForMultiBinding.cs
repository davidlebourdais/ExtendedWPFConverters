using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Returns the first item of a collection or the first value of a multibinding.
    /// </summary>
    /// <remarks>Useful to force refreshment of a target property based on a non-notifiable bound property.
    /// Refresh is then triggered by one or many other bound properties.</remarks>
    public class CollectionFirstItemConverterForMultiBinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Gets or sets a value indicating if item is
        /// a <see cref="IEnumerable"/> for which the first item must be returned.
        /// </summary>
        public bool AsIEnumerable { get; set; } = true;

        /// <summary>
        /// Gets and returns the first passed value or the first item of the collection passed
        /// as first item when <see cref="AsIEnumerable"/> is activated.
        /// </summary>
        /// <param name="values">A set of values.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The first value of the passed set of values, or null if none exists.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0)
                return null;

            if (!AsIEnumerable)
                return values[0];

            if (!(values[0] is IEnumerable enumerable))
                return null;

            var enumerator = enumerable.GetEnumerator();
            try
            {
                return enumerator.MoveNext() ? enumerator.Current : null;
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
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
