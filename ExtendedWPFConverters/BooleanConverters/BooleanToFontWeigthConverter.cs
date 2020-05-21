using System.Windows;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter that transforms a boolean value into a <see cref="FontWeight"/>.
    /// </summary>
    public class BooleanToFontWeightConverter : BooleanConverterBase<FontWeight>
    {
        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when converted value is true.
        /// </summary>
        public override FontWeight ValueForTrue { get; set; } = FontWeights.DemiBold;

        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when converted value is false.
        /// </summary>
        public override FontWeight ValueForFalse { get; set; } = FontWeights.Normal;

        /// <summary>
        /// <see cref="FontWeight"/> value to be applied when input value is invalid (null or not boolean).
        /// </summary>
        public override FontWeight ValueForInvalid { get; set; } = FontWeights.Normal;
    }
}