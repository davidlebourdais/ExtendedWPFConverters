using System.Windows;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into a <see cref="Thickness"/> one.
    /// </summary>
    public class BooleanToThicknessConverter : BooleanConverterBase<Thickness>
    {
        /// <summary>
        /// <see cref="Thickness"/> value to be applied when conversion operation output is true.
        /// </summary>
        public override Thickness ValueForTrue { get; set; } = new Thickness(1);

        /// <summary>
        /// <see cref="Thickness"/> value to be applied when conversion operation output is false.
        /// </summary>
        public override Thickness ValueForFalse { get; set; } = new Thickness(0);

        /// <summary>
        /// <see cref="Thickness"/> value to be applied when input is null or not boolean.
        /// </summary>
        public override Thickness ValueForInvalid { get; set; } = new Thickness(0);
    }
}
