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
    /// Converts a type to an array of string based on each public property's <see cref="DescriptionAttribute.Description"/> value if any.
    /// </summary>
    public class TypePropertiesToDescriptionsConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// If set, will include the name of the property in result even
        /// if no description attribute is found. If unset, members without
        /// attributes are not included in result.
        /// </summary>
        public bool GetMembersWithNoDescription { get; set; } = true;
        
        /// <summary>
        /// If set, will format result into 'Title case'.
        /// </summary>
        public bool ToTitleCase { get; set; }
        
        /// <summary>
        /// Extracts a list of property descriptions from a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The type on which to extract property descriptions.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>An array of property descriptions based on the type.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Type type))
                return null;
            
            var descriptionAttributes = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                            .Select(x => x.GetCustomAttributes(true)
                                                          .OfType<DescriptionAttribute>()
                                                          .FirstOrDefault()?.Description ?? (GetMembersWithNoDescription ? x.Name : null));
            
            return descriptionAttributes.Where(x => GetMembersWithNoDescription || !string.IsNullOrEmpty(x))
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
