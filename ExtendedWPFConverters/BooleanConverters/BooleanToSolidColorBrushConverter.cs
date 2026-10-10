using System.Windows.Media;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into a <see cref="SolidColorBrush"/> one.
    /// </summary>
    public class BooleanToSolidColorBrushConverter : BooleanConverterBase<SolidColorBrush>
    {
        /// <summary>
        /// <see cref="SolidColorBrush"/> value to be applied when conversion operation output is true.
        /// </summary>
        public override SolidColorBrush ValueForTrue { get; set; } = Brushes.Black;

        /// <summary>
        /// <see cref="SolidColorBrush"/> value to be applied when conversion operation output is false.
        /// </summary>
        public override SolidColorBrush ValueForFalse { get; set; } = Brushes.Transparent;

        /// <summary>
        /// <see cref="SolidColorBrush"/> value to be applied when input is null or not boolean.
        /// </summary>
        public override SolidColorBrush ValueForInvalid { get; set; } = Brushes.Transparent;
    }
}
