using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter that returns first object or null regarding to passed boolean values.
    /// </summary>
    // <remarks>If first object is null and bool operation is true, null object is returned.</remarks>
    public class BooleanToNullOrObjectConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Boolean operation to be applied during conversion on booleans.
        /// Positive output will return passed object, negative operation output will return null.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.None;

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // First value will be our object:
            if (values.Length == 0) return null;
            if (values.Length == 1) return values[0];

            // Next values will be booleans:
            var new_values = values.Skip(1);

            switch(Operation)
            {
                case BooleanOperation.Equality:
                    bool first_value = false;
                    if (new_values.Count() > 0)
                        first_value = (new_values.First() as bool?) != null && ((new_values.First() as bool?) == true);

                    // Case all true:
                    if (first_value)
                    {
                        if (new_values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                            return values[0];
                    }
                    else // case all false:
                    {
                        if (new_values.All(v => ((v as bool?) == null || ((v as bool?) == false))))
                            return values[0];
                    }

                    return null;

                case BooleanOperation.None:
                case BooleanOperation.And:
                    if (new_values.Any(v => ((v as bool?) == null || ((v as bool?) == false))))
                        return null;
                    return values[0];

                case BooleanOperation.Or:
                    if (new_values.Any(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return values[0];
                    return null;

                case BooleanOperation.Xor:
                    if (new_values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !new_values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return values[0];
                    return null;

                case BooleanOperation.Nand:
                    if (new_values.Any(v => ((v as bool?) == null || ((v as bool?) == false))))
                        return values[0];
                    return null;

                case BooleanOperation.Nor:
                    if (new_values.Any(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return null;
                    return values[0];

                case BooleanOperation.Xnor:
                    if (new_values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !new_values.All(v => ((v as bool?) != null && ((v as bool?) == true))))
                        return null;
                    return values[0];

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToNullOrObjectConverterForMultibinding) + ".");
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