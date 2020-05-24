namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A converter for multiboolean operations to be used with multibindings.
    /// </summary>
    public class BooleanToBooleanConverterForMultibinding : BooleanConverterBaseForMultibinding<bool>
    {
        /// <summary>
        /// Result to be returned when converted value is true.
        /// </summary>
        /// <remarks>Cannot be edited.</remarks>
        public override bool ValueForTrue { get => true; set { } }  // note: cannot change.

        /// <summary>
        /// Result to be returned when converted value is false.
        /// </summary>
        /// <remarks>Cannot be edited.</remarks>
        public override bool ValueForFalse { get => false; set { } }  // note: cannot change.

        /// <summary>
        /// Result to be returned when input values are not all booleans.
        /// </summary>
        public override bool ValueForInvalid { get; set; } = false;
    }
}