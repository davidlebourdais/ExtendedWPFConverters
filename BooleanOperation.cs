using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EMA.View.Converters
{
    /// <summary>
    /// A set of boolean operation that can be 
    /// used as parameter for various converters.
    /// </summary>
    public enum BooleanOperation
    {
        None,
        Not,
        Equality,
        And,
        Or,
        Xor,
        Nand,
        Nor,
        Xnor
    }

}
