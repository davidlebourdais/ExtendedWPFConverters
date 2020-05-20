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
        /// Difference operation (negative not allowed)
        /// </summary>
        Substract,
        /// <summary>
        /// Difference operation (negative allowed)
        /// </summary>
        SubstractNegativeAllowed,
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
        Power
    }
}
