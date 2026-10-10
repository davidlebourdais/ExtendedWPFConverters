using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace EMA.ExtendedWPFConverters
{
    internal static class ComparisonHelper
    {
        public static bool? Evaluate(object value, object parameter, ComparisonOperation operation)
        {
            switch (operation)
            {
                case ComparisonOperation.Equal:
                    return AreEqual(value, parameter);

                case ComparisonOperation.NotEqual:
                    return !AreEqual(value, parameter);

                case ComparisonOperation.GreaterThan:
                    return TryCompare(value, parameter, out var comparison) ? comparison > 0 : (bool?)null;

                case ComparisonOperation.GreaterThanOrEqual:
                    return TryCompare(value, parameter, out comparison) ? comparison >= 0 : (bool?)null;

                case ComparisonOperation.LessThan:
                    return TryCompare(value, parameter, out comparison) ? comparison < 0 : (bool?)null;

                case ComparisonOperation.LessThanOrEqual:
                    return TryCompare(value, parameter, out comparison) ? comparison <= 0 : (bool?)null;

                default:
                    return null;
            }
        }

        private static bool AreEqual(object value, object parameter)
        {
            if (value == null || parameter == null)
                return value == null && parameter == null;

            if (TryCompare(value, parameter, out var comparison))
                return comparison == 0;

            return value.Equals(parameter);
        }

        private static bool TryCompare(object value, object parameter, out int comparison)
        {
            comparison = 0;
            if (value == null || parameter == null)
                return false;

            if (IsNumeric(value) || IsNumeric(parameter))
                return TryCompareNumbers(value, parameter, out comparison);

            var valueType = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
            var parameterType = Nullable.GetUnderlyingType(parameter.GetType()) ?? parameter.GetType();

            if (TryConvertToType(parameter, valueType, out var convertedParameter))
            {
                parameter = convertedParameter;
            }
            else if (TryConvertToType(value, parameterType, out var convertedValue))
            {
                value = convertedValue;
            }

            if (!(value is IComparable comparableValue))
                return false;

            try
            {
                comparison = comparableValue.CompareTo(parameter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsNumeric(object value)
        {
            if (value == null || value.GetType().IsEnum)
                return false;

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        private static bool TryCompareNumbers(object value, object parameter, out int comparison)
        {
            comparison = 0;
            if (!TryGetNumber(value, parameter.GetType(), out var left, out var leftDivisor, out var leftSpecial) ||
                !TryGetNumber(parameter, value.GetType(), out var right, out var rightDivisor, out var rightSpecial))
                return false;

            if (double.IsNaN(leftSpecial) || double.IsInfinity(leftSpecial) ||
                double.IsNaN(rightSpecial) || double.IsInfinity(rightSpecial))
            {
                comparison = leftSpecial.CompareTo(rightSpecial);
                return true;
            }

            // Compare exact fractions instead of narrowing either operand to the other's type.
            comparison = (left * rightDivisor).CompareTo(right * leftDivisor);
            return true;
        }

        private static bool TryGetNumber(object value, Type otherType, out BigInteger numerator, out BigInteger divisor, out double special)
        {
            numerator = BigInteger.Zero;
            divisor = BigInteger.One;
            special = 0d;

            if (value is string text)
            {
                // Preserve the usual interpretation of string parameters for floating-point inputs.
                if (otherType == typeof(double) || otherType == typeof(float))
                {
                    if (!TryConvertToType(text, otherType, out value))
                        return false;
                }
                else
                {
                    return TryParseNumber(text, out numerator, out divisor);
                }
            }

            if (!IsNumeric(value))
                return false;

            if (value is double || value is float)
            {
                var number = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                if (double.IsNaN(number) || double.IsInfinity(number))
                {
                    special = number;
                    return true;
                }

                // A finite double is an integer significand multiplied by a power of two.
                var bits = BitConverter.DoubleToInt64Bits(number);
                var exponent = (int)((bits >> 52) & 0x7ff);
                numerator = bits & 0xfffffffffffffL;
                if (exponent != 0)
                    numerator += BigInteger.One << 52;
                exponent = exponent == 0 ? -1074 : exponent - 1075;

                if (exponent >= 0)
                    numerator <<= exponent;
                else
                    divisor <<= -exponent;
                if (bits < 0)
                    numerator = -numerator;
                return true;
            }

            // Decimal also represents every supported integral type without loss.
            var decimalBits = decimal.GetBits(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
            numerator = (new BigInteger((uint)decimalBits[2]) << 64) +
                        (new BigInteger((uint)decimalBits[1]) << 32) + (uint)decimalBits[0];
            divisor = BigInteger.Pow(10, (decimalBits[3] >> 16) & 0xff);
            if (decimalBits[3] < 0)
                numerator = -numerator;
            return true;
        }

        private static bool TryParseNumber(string text, out BigInteger numerator, out BigInteger divisor)
        {
            numerator = BigInteger.Zero;
            divisor = BigInteger.One;
            var match = Regex.Match(text.Trim(), @"\A([+-]?)([0-9]*(?:,[0-9]+)*)(?:\.([0-9]*))?(?:[eE]([+-]?[0-9]+))?\z");
            if (!match.Success || match.Groups[2].Length + match.Groups[3].Length == 0)
                return false;

            var exponent = 0;
            if (match.Groups[4].Success &&
                !int.TryParse(match.Groups[4].Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out exponent))
                return false;

            var scale = (long)match.Groups[3].Length - exponent;
            // Bound work for pathological exponent strings rather than allocating enormous integers.
            if (Math.Abs(scale) > 10000)
                return false;

            numerator = BigInteger.Parse(match.Groups[2].Value.Replace(",", "") + match.Groups[3].Value, CultureInfo.InvariantCulture);
            if (scale >= 0)
                divisor = BigInteger.Pow(10, (int)scale);
            else
                numerator *= BigInteger.Pow(10, (int)-scale);
            if (match.Groups[1].Value == "-")
                numerator = -numerator;
            return true;
        }

        private static bool TryConvertToType(object value, Type targetType, out object convertedValue)
        {
            convertedValue = null;
            if (value == null || targetType == null)
                return false;

            var nonNullableTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            var valueType = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();

            if (nonNullableTargetType.IsAssignableFrom(valueType))
            {
                convertedValue = value;
                return true;
            }

            try
            {
                if (nonNullableTargetType.IsEnum && value is string enumString)
                {
                    convertedValue = Enum.Parse(nonNullableTargetType, enumString, true);
                    return true;
                }

                if (value is IConvertible)
                {
                    convertedValue = Convert.ChangeType(value, nonNullableTargetType, CultureInfo.InvariantCulture);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
