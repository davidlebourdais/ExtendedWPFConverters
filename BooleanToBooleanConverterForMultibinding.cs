using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter for multiboolean operations to be used with multibindings.
    /// </summary>
    public class BooleanToBooleanConverterForMultibinding : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Specifies the operation to be applied with the converter.
        /// </summary>
        public BooleanOperation Operation { get; set; } = BooleanOperation.And;

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
                        return values.All(v => ((v as bool?) != null && ((v as bool?) == true)));
                    }
                    else // case all false:
                    {
                        return values.All(v => ((v as bool?) == null || ((v as bool?) == false)));
                    }

                case BooleanOperation.None:
                case BooleanOperation.And:
                    return !(values.Any(v => ((v as bool?) == null || ((v as bool?) == false))));

                case BooleanOperation.Or:
                    return values.Any(v => ((v as bool?) != null && ((v as bool?) == true)));

                case BooleanOperation.Xor:
                    return values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true)));

                case BooleanOperation.Nand:
                    return (values.Any(v => ((v as bool?) == null || ((v as bool?) == false))));

                case BooleanOperation.Nor:
                    return !(values.Any(v => ((v as bool?) != null && ((v as bool?) == true))));

                case BooleanOperation.Xnor:
                    return !(values.Any(v => ((v as bool?) != null && ((v as bool?) == true))) && !values.All(v => ((v as bool?) != null && ((v as bool?) == true))));

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToBooleanConverterForMultibinding) + ".");
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