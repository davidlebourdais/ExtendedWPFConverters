﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converter between single boolean and visibility object.
    /// </summary>
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        public Visibility ValueForTrue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Value to be applied when converted value is false.
        /// </summary>
        public Visibility ValueForFalse { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Value to be applied when input value null or invalid.
        /// </summary>
        public Visibility ValueForInvalid { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Boolean operation to be applied during conversion.
        /// </summary>
        public ReducedBooleanOperation Operation { get; set; } = ReducedBooleanOperation.None;

        /// <summary>
        /// Converts a boolean entry into a visibility value.
        /// </summary>
        /// <param name="value">A boolean entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>An visibility value corresponding to the entry.</returns>
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool value_bool = false;
            if (value == null || !(value is bool))
                return ValueForInvalid;
            value_bool = (bool)value;

            switch (Operation)
            {
                case ReducedBooleanOperation.Not:
                    return value_bool ? ValueForFalse : ValueForTrue;
                case ReducedBooleanOperation.None:
                    return value_bool ? ValueForTrue : ValueForFalse;

                default:
                    throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToVisibilityConverter) + ".");
            }
        }

        /// <summary>
        /// Converts a visibility value back into a boolean through a boolean operation.
        /// </summary>
        /// <param name="value">A <see cref="Visibility"/> instance to assess.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A boolean value that matches visibility entry.</returns>
        /// <exception cref="NotSupportedException">Thrown if the boolean operation is not supported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility valueVisbility)
            {
                switch (Operation)
                {
                    case ReducedBooleanOperation.Not:
                        return valueVisbility == ValueForTrue ? false : true;
                    case ReducedBooleanOperation.None:
                        return valueVisbility == ValueForTrue ? true : false;

                    default:
                        throw new NotSupportedException(Operation.ToString() + " is not supported for " + nameof(BooleanToVisibilityConverter) + ".");
                }
            }

            return false;
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
