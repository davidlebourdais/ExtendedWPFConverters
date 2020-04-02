using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// From a collection passed as a first argument, returns the index of a passed second element in this 
    /// collection if existing.
    /// </summary>
    public class IndexOfConverterForMultiBinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Indicates if the output should be a string or a int.
        /// </summary>
        public bool OutputAsString { get; } = true;

        /// <summary>
        /// From a collection passed as a first argument, returns the index of a passed second element 
        /// in this collection if existing.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the MultiBinding produces. 
        /// First item is the collection, second item is the index to retrieve.
        /// The value UnsetValue indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The index of the item in the given collection, if any.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || values[1] == null) return null;

            if (!(values[0] is IEnumerable collection)) return null;

            // If collection is a list, deal with index:
            if (values[0] is IList list)
            {
                if (list.Contains(values[1]))
                {
                    var result = list.IndexOf(values[1]);
                    return OutputAsString ? (object)result.ToString() : result;
                }
                else return OutputAsString ? (object)string.Empty : - 1;
            }

            // Else try to guess by enumerating:
            int index = 0;
            foreach (var item in collection)
            {
                if (item == values[1])
                {
                    var result = index++;
                    return OutputAsString ? (object)result.ToString() : result;
                }
                else index++;
            }

            return OutputAsString ? (object)string.Empty : -1;
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