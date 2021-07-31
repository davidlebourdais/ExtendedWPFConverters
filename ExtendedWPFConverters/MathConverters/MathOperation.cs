namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A set of mathematical operation that can be 
    /// used as parameter for various converters.
    /// </summary>
    public enum MathOperation
    {
        /// <summary>
        /// No operation
        /// </summary>
        None,
        /// <summary>
        /// Sum operation
        /// </summary>
        Add,
        /// <summary>
        /// Difference operation (outputs negative results) 
        /// </summary>
        Subtract,
        /// <summary>
        /// Difference operation (outputs 0 when result is negative)
        /// </summary>
        SubtractPositiveOnly,
        /// <summary>
        /// Product operation
        /// </summary>
        Multiply,
        /// <summary>
        /// Division operation
        /// </summary>
        Divide,
        /// <summary>
        /// Modulo operation
        /// </summary>
        Modulo,
        /// <summary>
        /// 'Raise to power' operation
        /// </summary>
        Power,
        /// <summary>
        /// Absolute value operation
        /// </summary>
        Absolute
    }
}
