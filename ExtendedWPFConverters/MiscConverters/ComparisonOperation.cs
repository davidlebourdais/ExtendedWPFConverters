namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Defines comparison operations supported by comparison converters.
    /// </summary>
    public enum ComparisonOperation
    {
        /// <summary>
        /// Checks whether both values are equal.
        /// </summary>
        Equal,

        /// <summary>
        /// Checks whether both values are different.
        /// </summary>
        NotEqual,

        /// <summary>
        /// Checks whether the input value is greater than the reference value.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Checks whether the input value is greater than or equal to the reference value.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Checks whether the input value is less than the reference value.
        /// </summary>
        LessThan,

        /// <summary>
        /// Checks whether the input value is less than or equal to the reference value.
        /// </summary>
        LessThanOrEqual
    }
}
