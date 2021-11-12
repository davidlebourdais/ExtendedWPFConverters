using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        /// <returns>The <see cref="DescriptionAttribute.Description"/> of the value, or empty string if none is set.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum asEnumValue))
                return null;

            var descriptionAttribute = asEnumValue.GetType()
                                                  .GetMembers(BindingFlags.Public | BindingFlags.Static)
                                                  .Single(x => x.Name == asEnumValue.ToString())
                                                  .GetCustomAttributes(true)
                                                  .OfType<DescriptionAttribute>()
                                                  .FirstOrDefault();

            return descriptionAttribute?.Description ?? string.Empty;
        }
        
        /// <summary>
        /// Returns an <see cref="Enum"/> value that matching a passed description.
        /// </summary>
        /// <param name="value">The string containing the enum value description.</param>
        /// <param name="targetType">The target <see cref="Enum"/> type.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The resulting enum value.</returns>
        /// <exception cref="ArgumentException">Thrown if no <see cref="DescriptionAttribute.Description"/> matches passed string or
        /// if does not exist on the target <see cref="Enum"/> type.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || targetType == null)
                return null;

            foreach (var member in targetType.GetMembers(BindingFlags.Public | BindingFlags.Static))
            {
                var descriptionAttribute = member.GetCustomAttributes(true)
                                                 .OfType<DescriptionAttribute>()
                                                 .FirstOrDefault();
                
                if (descriptionAttribute == null)
                    continue;
                
                if (descriptionAttribute.Description.Equals(value))
                    return Enum.Parse(targetType, member.Name);
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
