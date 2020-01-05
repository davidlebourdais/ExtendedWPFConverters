using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter between multi booleans and visibility object to be used in multibindings.
    /// </summary>
    public class BooleanToVisibilityConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        public Visibility ValueForTrue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Value to be applied when converted value is false, null or invalid.
        /// </summary>
        public Visibility ValueForFalse { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.None;

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch(Operation)
            {
                case BooleanOperation.Equality:
                    bool first_value = false;
                    if (values.Length > 0)
                        first_value = (values[0] as bool?) != null && ((values[0] as bool?) == true);

                    // Case all true:
                    if (first_value)
                    {
                        if (values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                            return ValueForTrue;
                    }
                    else // case all false:
                    {
                        if (values.All(v => ((v as bool?) == null || ((v as bool?) == false))))
                            return ValueForTrue;
                    }

                    return ValueForFalse;

                case BooleanOperation.None:
                case BooleanOperation.And:
                    if (values.Any(v => ((v as bool?) == null || ((v as bool?) == false))))
                        return ValueForFalse;
                    return ValueForTrue;

                case BooleanOperation.Or:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForTrue;
                    return ValueForFalse;

                case BooleanOperation.Xor:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForTrue;
                    return ValueForFalse;

                case BooleanOperation.Nand:
                    if (values.Any(v => ((v as bool?) == null || ((v as bool?) == false))))
                        return ValueForTrue;
                    return ValueForFalse;

                case BooleanOperation.Nor:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForFalse;
                    return ValueForTrue;

                case BooleanOperation.Xnor:
                    if (values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return ValueForFalse;
                    return ValueForTrue;

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToVisibilityConverterForMultibinding) + ".");
            }
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