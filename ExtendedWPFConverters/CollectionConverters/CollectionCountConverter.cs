using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;
using System.Collections.Generic;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Returns how many items a <see cref="IEnumerable"/> stores.
    /// </summary>
    public class CollectionCountConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Indicates if the output should be a string or a int.
        /// </summary>
        public bool OutputAsString { get; set; } = true;

        /// <summary>
        /// Gets or sets the default value to be returned when passed input value
        /// is not iterable and thus cannot be counted.
        /// </summary>
        public int DefaultCountValue { get; set; } = 0;

        /// <summary>
        /// Gets or sets the default value to be returned when passed input value
        /// is not iterable and thus cannot be counted.
        /// </summary>
        /// <remarks>Active when <see cref="OutputAsString"/> is set.abstract</remarks>
        public string DefaultCountValueString { get; set; } = "0";

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
            switch (value)
            {
                case ICollection castedCollection:
                    return OutputAsString ? (object)castedCollection.Count.ToString() : castedCollection.Count;
                case ICollection<object> castedGenericCollection:
                    return OutputAsString ? (object)castedGenericCollection.Count.ToString() : castedGenericCollection.Count;
            }

            if (!(value is IEnumerable casted))
                return OutputAsString ? (object)DefaultCountValueString : DefaultCountValue;
            
            var counter = 0;
            foreach (var _ in casted)
                counter++;
            
            return OutputAsString ? (object)counter.ToString() : counter;
        }

        /// <summary>
        /// Unsupported conversion method.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotSupportedException">Thrown if this method is called.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
