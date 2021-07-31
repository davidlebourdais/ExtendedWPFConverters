using System;
using System.Runtime.CompilerServices;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into an opacity level.
    /// </summary>
    public class BooleanToOpacityConverter : BooleanConverterBase<double>
    {
        private double _valueForTrue = 1.0d;
        private double _valueForFalse = 0.38d;
        private double _valueForInvalid = 1.0d;

        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        /// <exception cref="ArgumentException">If passed value is negative or >1.</exception>
        public override double ValueForTrue
        {
            get => _valueForTrue;
            set
            {
                if (value == _valueForTrue)
                    return;

                CheckValueOrThrow(value);
                
                _valueForTrue = value;
            }
        }

        /// <summary>
        /// Value to be applied when converted value is false.
        /// </summary>
        /// <exception cref="ArgumentException">If passed value is negative or >1.</exception>
        public override double ValueForFalse
        {
            get => _valueForFalse;
            set
            {
                if (value == _valueForFalse)
                    return;
                
                CheckValueOrThrow(value);
                
                _valueForFalse = value;
            }
        }

        /// <summary>
        /// Value to be applied when converted value is null or is not a boolean.
        /// </summary>
        /// <exception cref="ArgumentException">If passed value is negative or >1.</exception>
        public override double ValueForInvalid
        {
            get => _valueForInvalid;
            set
            {
                if (value == _valueForInvalid)
                    return;
                
                CheckValueOrThrow(value);
                    
                _valueForInvalid = value;
            }
        }

        private static void CheckValueOrThrow(double value, [CallerMemberName] string propertyName = null)
        {
            if (value < 0)
                throw new ArgumentException("Cannot set negative values on " + propertyName + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    
            if (value > 1.0)
                throw new ArgumentException("Cannot set >1.0 values on " + propertyName + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
        }
    }
}
