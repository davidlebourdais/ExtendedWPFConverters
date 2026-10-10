using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// From a collection passed as a first argument, returns an element in this 
    /// collection that is given as second argument if existing. This is similar to IList.IndexOf(item) method.
    /// </summary>
    public class CollectionIndexOfConverterForMultiBinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Indicates if the output should be a string or a int.
        /// </summary>
        public bool OutputAsString { get; set; } = true;

        /// <summary>
        /// Gets or sets the default value to be returned when passed
        /// item to retrieve the index of cannot be found in the collection.
        /// </summary>
        public int ValueForNotFound { get; set; } = -1;

        /// <summary>
        /// Gets or sets the default value to be returned when passed
        /// item to retrieve the index of cannot be found in the collection.
        /// </summary>
        /// <remarks>Used when <see cref="OutputAsString"/> is set.null</remarks>
        public string ValueStringForNotFound { get; set; } = "";

        /// <summary>
        /// Gets or sets the default value to be returned when the collection or input array
        /// is invalid, or a source binding is unresolved.
        /// </summary>
        public int ValueForInvalid { get; set; } = -1;
        
        /// <summary>
        /// Gets or sets the default value to be returned when the collection or input array
        /// is invalid, or a source binding is unresolved.
        /// <remarks>Used when <see cref="OutputAsString"/> is set.null</remarks>
        /// </summary>
        public string ValueStringForInvalid { get; set; } = "";

        /// <summary>
        /// From a collection passed as a first argument, returns the index of a passed second element 
        /// in this collection if existing.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the MultiBinding produces. 
        /// First item is the collection, second item is the item to retrieve the index of.
        /// A null search item is valid and matches the first null element, if present.
        /// UnsetValue in either source binding returns the configured invalid-input fallback.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The index of the item in the given collection, if any.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return OutputAsString ? (object)ValueStringForInvalid : ValueForInvalid;

            if (!(values[0] is IEnumerable collection) || ReferenceEquals(values[1], DependencyProperty.UnsetValue))
                return OutputAsString ? (object)ValueStringForInvalid : ValueForInvalid;

            // If collection is a list, deal with index:
            if (values[0] is IList list)
            {
                if (!list.Contains(values[1]))
                    return OutputAsString ? (object)ValueStringForNotFound : ValueForNotFound;
                
                var result = list.IndexOf(values[1]);
                return OutputAsString ? (object)result.ToString() : result;

            }

            // Else try to guess by enumerating:
            var index = 0;
            foreach (var item in collection)
            {
                if (Equals(item, values[1]))
                    return OutputAsString ? (object)index.ToString() : index;

                index++;
            }

            return OutputAsString ? (object)ValueStringForNotFound : ValueForNotFound;
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
