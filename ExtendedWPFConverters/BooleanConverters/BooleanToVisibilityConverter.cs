using System.Windows;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into a <see cref="Visibility"/> object.
    /// </summary>
    public class BooleanToVisibilityConverter : BooleanConverterBase<Visibility>
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
        /// Result to be returned when converted value is null or is not a boolean.
        /// </summary>
        public override Visibility ValueForInvalid { get; set; } = Visibility.Collapsed;
    }
}
