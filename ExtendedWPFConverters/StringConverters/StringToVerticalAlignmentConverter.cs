﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a string into a <see cref="VerticalAlignment"/> value.
    /// </summary>
    public class StringToVerticalAlignmentConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a string into a <see cref="VerticalAlignment"/> value.
        /// </summary>
        /// <param name="value">A string that is a direct representation of an alignment enum.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Optional function or a dictionary that contains function used for translation of the value in the 
        /// current culture's language. See <see cref="StringTranslationHelper.CheckFetcherFormat(object, bool)"/> for more details.</param>
        /// <param name="culture">Optionally used by parameter to retrieve the right translation for the given input.</param>
        /// <returns>The <see cref="VerticalAlignment"/> that matches the string entry.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string asString))
                return null;
            
            if (StringTranslationHelper.CheckFetcherFormat(parameter, true))
                if (StringTranslationHelper.TryTranslateValue(asString, parameter, culture, out var translated))
                    asString = translated;
                        
            if (Enum.TryParse(asString, ignoreCase:true, out VerticalAlignment vertical))
                return vertical;
            
            return null;
        }

        /// <summary>
        /// Converts a <see cref="VerticalAlignment"/> value into a string.
        /// </summary>
        /// <param name="value">The <see cref="VerticalAlignment"/> value to be converted back into string.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Optional function or a dictionary that contains function used for translation of the value in the 
        /// current culture's language. See <see cref="StringTranslationHelper.CheckFetcherFormat(object, bool)"/> for more details.</param>
        /// <param name="culture">Optionally used by parameter to retrieve the right translation for the given input.</param>
        /// <returns>A string value matching the <see cref="VerticalAlignment"/> entry.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is VerticalAlignment casted))
                return string.Empty;
            
            if (StringTranslationHelper.CheckFetcherFormat(parameter)) // if parameter contains valid translation table
            {
                if (StringTranslationHelper.TryTranslateValueBack(casted.ToString(), parameter, culture, out var translated)) // convert
                    return translated;
            }
            else
                return casted.ToString();  // else return raw string value.
            
            return string.Empty;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
