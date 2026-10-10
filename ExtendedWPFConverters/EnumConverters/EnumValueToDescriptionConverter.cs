using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts an <see cref="Enum"/> value to its <see cref="DescriptionAttribute.Description"/> value if any.
    /// </summary>
    public class EnumValueToDescriptionConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Extracts the description of an <see cref="Enum"/> value.
        /// </summary>
        /// <param name="value">A <see cref="Enum"/> value to process.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The description or enum string representation when no description exists; null for non-enum input.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum asEnumValue))
                return null;

            return asEnumValue.ToDescriptionOrToString();
        }
        
        /// <summary>
        /// Returns an <see cref="Enum"/> value matching a description or enum string representation.
        /// </summary>
        /// <param name="value">A description, enum name, flag list or numeric representation.</param>
        /// <param name="targetType">The target enum or nullable enum type.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The resulting enum value.</returns>
        /// <exception cref="ArgumentException">Thrown if the target is not an enum type or the input cannot be resolved.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || targetType == null)
                return null;

            var enumType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (!enumType.IsEnum)
                throw new ArgumentException("The target type must be an enum or nullable enum.", nameof(targetType));

            foreach (var member in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var descriptionAttribute = member.GetCustomAttributes(true)
                                                 .OfType<DescriptionAttribute>()
                                                 .FirstOrDefault();
                
                if (descriptionAttribute == null)
                    continue;
                
                if (Equals(descriptionAttribute.Description, value))
                    return Enum.Parse(enumType, member.Name);
            }

            if (value is string enumText)
            {
                try
                {
                    return Enum.Parse(enumType, enumText);
                }
                catch (ArgumentException)
                {
                    // Neither a description nor an enum representation.
                }
                catch (OverflowException)
                {
                    // The numeric representation does not fit the enum's underlying type.
                }
            }

            throw new ArgumentException($"Cannot convert back from description to enum value for string {value}");
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
