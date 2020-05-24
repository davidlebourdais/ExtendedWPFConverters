using System.Windows;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts boolean entries into a <see cref="Visibility"/> object through a specified <see cref="BooleanOperation"/>.
    /// </summary>
    public class BooleanToVisibilityConverterForMultibinding : BooleanConverterBaseForMultibinding<Visibility>
    {
        /// <summary>
        /// Result to be returned when converted value is true.
        /// </summary>
        public override Visibility ValueForTrue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Result to be returned when converted value is false.
        /// </summary>
        public override Visibility ValueForFalse { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Result to be returned when input values are not all booleans.
        /// </summary>
        public override Visibility ValueForInvalid { get; set; } = Visibility.Collapsed;
    }
}