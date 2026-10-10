using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Checks whether a collection passed as first input contains the value passed as second input.
    /// </summary>
    public class CollectionContainsConverterForMultiBinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Indicates if the result should be inverted.
        /// </summary>
        public bool TrueIfNotContains { get; set; }

        /// <summary>
        /// Value to be returned when the input values are invalid or a source binding is unresolved.
        /// </summary>
        public bool ValueForInvalid { get; set; } = false;

        /// <summary>
        /// Checks whether a collection contains a given value.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the MultiBinding produces.
        /// First item is the collection, second item is the item to look for.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>True when the collection contains the item, false otherwise.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || !(values[0] is IEnumerable collection) ||
                ReferenceEquals(values[1], DependencyProperty.UnsetValue))
                return ValueForInvalid;

            bool contains;
            if (collection is IList list)
            {
                contains = list.Contains(values[1]);
            }
            else
            {
                contains = false;
                foreach (var item in collection)
                {
                    if (Equals(item, values[1]))
                    {
                        contains = true;
                        break;
                    }
                }
            }

            return TrueIfNotContains ? !contains : contains;
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
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
