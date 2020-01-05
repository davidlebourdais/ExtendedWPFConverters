using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter for multiboolean operations to be used in multibindings.
    /// </summary>
    public class StringToVisibilityConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Value to be applied when converted string is null or empty.
        /// </summary>
        public Visibility ValueForNullOrEmpty { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when converted string is not null nor empty.
        /// </summary>
        public Visibility ValueForNotNullOrEmpty { get; set; } = Visibility.Visible;

        /// <summary>
        /// Operation to be used accross the mutliple bound values.
        /// </summary>
        public BooleanOperation OperationWithEnablers { get; set; } = BooleanOperation.And;

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Fist value must be the string;
            if (values.Length < 1) return ValueForNullOrEmpty;

            if (!(values[0] is string text))
                return ValueForNullOrEmpty;
            if ((values.Length == 1 || OperationWithEnablers == BooleanOperation.And) && string.IsNullOrEmpty(text))
                return ValueForNullOrEmpty;
            else if (values.Length == 1)
                return ValueForNotNullOrEmpty;

            // Other values will be booleans that will actiated or not the output regarding to
            // the boolean operation with enablers that is set:
            var enablers = new List<bool>();
            for(int i = 1; i < values.Length; i++)
                if (values[i] is bool casted)
                    enablers.Add(casted);

            if (enablers.Count == 0)
                return string.IsNullOrEmpty(text) ? ValueForNullOrEmpty : ValueForNotNullOrEmpty;

            if (OperationWithEnablers == BooleanOperation.And && enablers.Any(x => x == false))
                return ValueForNullOrEmpty;

            if (OperationWithEnablers == BooleanOperation.Or && string.IsNullOrEmpty(text) && enablers.All(x => x == false))
                return ValueForNullOrEmpty;

            return ValueForNotNullOrEmpty;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}