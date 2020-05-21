using System;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into an <see cref="IComparable"/> one.
    /// </summary>
    public class BooleanToNumberConverter : BooleanConverterBase<IComparable>
    {
        /// <summary>
        /// Result to be returned when converted value is true.
        /// </summary>
        public override IComparable ValueForTrue { get; set; } = 1.0d;

        /// <summary>
        /// Result to be returned when converted value is false.
        /// </summary>
        public override IComparable ValueForFalse { get; set; } = 0.0d;

        /// <summary>
        /// Result to be returned when converted value is null or is not a boolean.
        /// </summary>
        public override IComparable ValueForInvalid { get; set; } = -1.0d;
    }
}
