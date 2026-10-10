using System.Windows;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into a <see cref="GridLength"/> one.
    /// </summary>
    public class BooleanToGridLengthConverter : BooleanConverterBase<GridLength>
    {
        /// <summary>
        /// <see cref="GridLength"/> value to be applied when conversion operation output is true.
        /// </summary>
        public override GridLength ValueForTrue { get; set; } = GridLength.Auto;

        /// <summary>
        /// <see cref="GridLength"/> value to be applied when conversion operation output is false.
        /// </summary>
        public override GridLength ValueForFalse { get; set; } = new GridLength(0);

        /// <summary>
        /// <see cref="GridLength"/> value to be applied when input is null or not boolean.
        /// </summary>
        public override GridLength ValueForInvalid { get; set; } = new GridLength(0);
    }
}
