namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// A set of boolean operation that can be 
    /// used as parameter for various converters.
    /// </summary>
    public enum BooleanOperation
    {
        /// <summary>
        /// Do nothing operation
        /// </summary>
        None,
        /// <summary>
        /// Boolean inversion operation
        /// </summary>
        Not,
        /// <summary>
        /// Boolean equality check operation
        /// </summary>
        Equality,
        /// <summary>
        /// Boolean 'AND' operation
        /// </summary>
        And,
        /// <summary>
        /// Boolean 'OR' operation
        /// </summary>
        Or,
        /// <summary>
        /// Boolean 'Exlusive OR' operation
        /// </summary>
        Xor,
        /// <summary>
        /// Boolean 'Not AND' operation
        /// </summary>
        Nand,
        /// <summary>
        /// Boolean 'Not OR' operation
        /// </summary>
        Nor,
        /// <summary>
        /// Boolean 'Exclusive NOR' operation
        /// </summary>
        Xnor
    }
}
