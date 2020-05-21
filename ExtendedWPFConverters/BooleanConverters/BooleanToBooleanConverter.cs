namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter to perform operations over a single boolean value.
    /// </summary>
    public class BooleanToBooleanConverter : BooleanConverterBase<bool>
    { 
        /// <summary>
        /// Result to be returned when converted value is true.
        /// </summary>
        public override bool ValueForTrue { get; set; } = true;

        /// <summary>
        /// Result to be returned when converted value is false.
        /// </summary>
        public override bool ValueForFalse { get; set; } = false;

        /// <summary>
        /// Result to be returned when input value is null or not boolean.
        /// </summary>
        public override bool ValueForInvalid { get; set; } = false;
    }
}
