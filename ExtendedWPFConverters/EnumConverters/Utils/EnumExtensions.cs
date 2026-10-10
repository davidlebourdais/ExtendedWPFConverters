using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Provides display text for enum values.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the description of a named enum member, or its string representation when no description exists.
        /// </summary>
        /// <param name="val">An enum value, which may be null.</param>
        /// <returns>The description, enum name, flag list or numeric representation; empty string for null.</returns>
        public static string ToDescriptionOrToString(this Enum val)
        {
            if (val == null)
                return string.Empty;

            var enumText = val.ToString();
            var field = val.GetType().GetField(enumText);
            var description = field?.GetCustomAttributes<DescriptionAttribute>(inherit: false).FirstOrDefault();

            return description != null ? description.Description : enumText;
        }
    }
}
