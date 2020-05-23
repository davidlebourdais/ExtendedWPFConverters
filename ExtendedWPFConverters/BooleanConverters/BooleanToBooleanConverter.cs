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
        /// <remarks>Cannot be edited.</remarks>
        public override bool ValueForTrue { get => true; set { } }

        /// <summary>
        /// Result to be returned when converted value is false.
        /// </summary>
        /// <remarks>Cannot be edited.</remarks>
        public override bool ValueForFalse { get => false; set { } }

        /// <summary>
        /// Result to be returned when input value is null or not boolean.
        /// </summary>
        public override bool ValueForInvalid { get; set; } = false;
    }
}
