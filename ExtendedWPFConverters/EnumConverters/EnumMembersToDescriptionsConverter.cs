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
    /// Converts an <see cref="Enum"/> type to an array of string based on each member <see cref="DescriptionAttribute.Description"/> value if any.
    /// </summary>
    public class EnumMembersToDescriptionsConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// If set, will include the name of the member in result even
        /// if no description attribute is found. If unset, members without
        /// attributes are not included in result.
        /// </summary>
        public bool GetMembersWithNoDescription { get; set; } = true;
        
        /// <summary>
        /// If set, will format result into 'Title case'.
        /// </summary>
        public bool ToTitleCase { get; set; }
        
        /// <summary>
        /// Converts an <see cref="Enum"/> type or value into a list of enum member's descriptions.
        /// </summary>
        /// <param name="value">The <see cref="Enum"/> type on which to extract member descriptions,
        /// or a enum value on which the type will be extracted.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>An array of enum member descriptions based on the type.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var type = value as Type ?? value.GetType();
            if (!typeof(Enum).IsAssignableFrom(type))
                return null;
            
            var descriptionAttributes = type.GetMembers(BindingFlags.Public | BindingFlags.Static)
                                            .Select(x => x.GetCustomAttributes(true)
                                                          .OfType<DescriptionAttribute>()
                                                          .FirstOrDefault()?.Description ?? (GetMembersWithNoDescription ? x.Name : null));
            
            return descriptionAttributes?.Where(x => GetMembersWithNoDescription || !string.IsNullOrEmpty(x))
                                         .Select(x => ToTitleCase ? x.ToTitleCase() : x)
                                         .ToArray();
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
