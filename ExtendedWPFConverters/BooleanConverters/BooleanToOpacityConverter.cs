using System;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Converts a boolean value into an opacity level.
    /// </summary>
    public class BooleanToOpacityConverter : BooleanConverterBase<double>
    {
        private double value_for_true = 1.0d;
        private double value_for_false = 0.38d;
        private double value_for_invalid = 1.0d;

        /// <summary>
        /// Value to be applied when converted value is true.
        /// </summary>
        /// <exception cref="ArgumentException">If passed value is negative or >1.</exception>
        public override double ValueForTrue
        {
            get => value_for_true;
            set
            {
                if (value != value_for_true)
                {
                    if (value < 0) 
                        throw new ArgumentException("Cannot set negative values on " + nameof(ValueForTrue) + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    else if (value > 1.0)
                        throw new ArgumentException("Cannot set >1.0 values on " + nameof(ValueForTrue) + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    else value_for_true = value;
                }
            }
        }

        /// <summary>
        /// Value to be applied when converted value is false.
        /// </summary>
        /// <exception cref="ArgumentException">If passed value is negative or >1.</exception>
        public override double ValueForFalse
        {
            get => value_for_false;
            set
            {
                if (value != value_for_false)
                {
                    if (value < 0)
                        throw new ArgumentException("Cannot set negative values on " + nameof(ValueForFalse) + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    else if (value > 1.0)
                        throw new ArgumentException("Cannot set >1.0 values on " + nameof(ValueForFalse) + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    else value_for_false = value;
                }
            }
        }

        /// <summary>
        /// Value to be applied when converted value is null or is not a boolean.
        /// </summary>
        /// <exception cref="ArgumentException">If passed value is negative or >1.</exception>
        public override double ValueForInvalid
        {
            get => value_for_invalid;
            set
            {
                if (value != value_for_invalid)
                {
                    if (value < 0)
                        throw new ArgumentException("Cannot set negative values on " + nameof(ValueForInvalid) + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    else if (value > 1.0)
                        throw new ArgumentException("Cannot set >1.0 values on " + nameof(ValueForInvalid) + " of " + nameof(BooleanToOpacityConverter) + ".", nameof(value));
                    else value_for_invalid = value;
                }
            }
        }
    }
}
