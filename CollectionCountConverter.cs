using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Returns how many items a <see cref="IEnumerable"/> stores.
    /// </summary>
    public class CollectionCountConverterExtension : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if the output should be a string or a int.
        /// </summary>
        public bool OutputAsString { get; } = true;

        /// <summary>
        /// Returns the number of items the passed <see cref="IEnumerable"/> has.
        /// </summary>
        /// <param name="value">A <see cref="IEnumerable"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The number of items the collection contains.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable casted)
            {
                var counter = 0;
                foreach (var item in casted)
                    counter++;
                return OutputAsString ? (object)counter.ToString() : counter;
            }
            return OutputAsString ? (object)"0" : 0;

        }

        /// <summary>
        /// Not supported operation.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
