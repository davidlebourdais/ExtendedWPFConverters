﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that transforms a boolean value into a <see cref="FontWeight"/>.
    /// </summary>
    public class BooleanToFontWeightConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when converted value is true.
        /// </summary>
        public FontWeight ValueForTrue { get; set; } = FontWeights.DemiBold;

        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when converted value is false, null or not boolean.
        /// </summary>
        public FontWeight ValueForFalse { get; set; } = FontWeights.Normal;

        /// <summary>
        /// Returns a <see cref="FontWeight"/> value corresponding to a passed boolean value.
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>The font weight corresponding to the boolean entry.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool value_bool && value_bool ? ValueForTrue : ValueForFalse;
        }

        /// <summary>
        /// Converts a <see cref="FontWeight"/> value back into a boolean.
        /// </summary>
        /// <param name="value">A <see cref="FontWeight"/> entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A boolean value that matches best the passed entry.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is FontWeight casted && casted == ValueForTrue;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}