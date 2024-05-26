using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Returns a visibility based on if a <see cref="IEnumerable"/> stores any items.
    /// </summary>
    public class CollectionNotEmptyToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Result to be returned when collection has at least an item.
        /// </summary>
        public Visibility ValueWhenNotEmpty { get; set; } = Visibility.Visible;

        /// <summary>
        /// Result to be returned when collection is null or has no item.
        /// </summary>
        public Visibility ValueForNullOrEmpty { get; set; } = Visibility.Collapsed;
        
        /// <summary>
        /// Returns the a visibility based on whether ab <see cref="IEnumerable"/> stored items or not.
        /// </summary>
        /// <param name="value">A <see cref="IEnumerable"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A visibility matching the items count.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ICollection castedCollection:
                    return castedCollection.Count > 0 ? ValueWhenNotEmpty : ValueForNullOrEmpty;
                case ICollection<object> castedGenericCollection:
                    return castedGenericCollection.Count > 0 ? ValueWhenNotEmpty : ValueForNullOrEmpty;
            }

            if (!(value is IEnumerable casted))
                return ValueForNullOrEmpty;

            var enumerator = casted.GetEnumerator();
            var hasItems = enumerator.MoveNext();
            if (enumerator is IDisposable disposable)
                disposable.Dispose();
            
            return hasItems ? ValueWhenNotEmpty : ValueForNullOrEmpty; }

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
