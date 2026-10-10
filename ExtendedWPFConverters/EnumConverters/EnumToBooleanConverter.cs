using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Checks if an enum value matches a given enum parameter.
    /// </summary>
    public class EnumToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value returned when input or parameter cannot be interpreted as enum values.
        /// </summary>
        public bool ValueForInvalid { get; set; } = false;

        /// <summary>
        /// Checks if an enum value matches the converter parameter.
        /// </summary>
        /// <param name="value">An enum value to check.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">The enum value to compare to, either as enum instance or string.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>True if both enum values match, false otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum asEnumValue))
                return ValueForInvalid;

            return TryResolveEnumValue(parameter, asEnumValue.GetType(), out var referenceValue)
                ? Equals(asEnumValue, referenceValue)
                : ValueForInvalid;
        }

        /// <summary>
        /// Returns the enum value matching the converter parameter when the boolean input is true.
        /// </summary>
        /// <param name="value">A boolean value.</param>
        /// <param name="targetType">The target enum type.</param>
        /// <param name="parameter">The enum value to return, either as enum instance or string.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The enum value matching the converter parameter, or <see cref="Binding.DoNothing"/> when unchecked or invalid.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as bool? != true)
                return Binding.DoNothing;

            var enumType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (enumType == null || !enumType.IsEnum)
                return Binding.DoNothing;

            return TryResolveEnumValue(parameter, enumType, out var referenceValue)
                ? referenceValue
                : Binding.DoNothing;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        private static bool TryResolveEnumValue(object parameter, Type enumType, out object referenceValue)
        {
            referenceValue = null;
            if (parameter == null || enumType == null || !enumType.IsEnum)
                return false;

            if (parameter.GetType() == enumType)
            {
                referenceValue = parameter;
                return true;
            }

            if (parameter is string stringValue)
            {
                try
                {
                    referenceValue = Enum.Parse(enumType, stringValue, true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}
