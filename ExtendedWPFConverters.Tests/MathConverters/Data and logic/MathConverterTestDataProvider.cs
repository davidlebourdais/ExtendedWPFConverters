using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using EMA.ExtendedWPFConverters.Utils;
using EMA.ExtendedWPFConverters.Tests.Utils;

namespace EMA.ExtendedWPFConverters.Tests.Data
{
    /// <summary>
    /// Provides data to be used to data-driven tests on MathConverters 
    /// and test logic to verify mathematical operations.
    /// </summary>
    public static class MathConverterTestDataProvider
    {
        #region Test helpers
        public static bool IsSingleValueOperation(MathOperation operation) => operation == MathOperation.None || operation == MathOperation.Absolute;

        public static double Operate(MathOperation operation, double[] values)
        {
            var result = values[0];
            switch (operation)
            {
                case MathOperation.None:
                    return result;
                case MathOperation.Add:
                    result += values.Skip(1).Sum();
                    return result;
                case MathOperation.Subtract:
                    result = values.Skip(1).Aggregate(result, (current, value) => current - value);
                    return result;
                case MathOperation.SubtractPositiveOnly:
                    result = values.Skip(1).Aggregate(result, (current, value) => current - value);
                    return result > 0.0d ? result : 0.0d; 
                case MathOperation.Multiply:
                    result = values.Skip(1).Aggregate(result, (current, value) => current * value);
                    return result;
                case MathOperation.Divide:
                    foreach (var value in values.Skip(1))
                    {
                        if (value == 0.0d)
                        {
                            if (result == 0.0d)
                                result = double.NaN;
                            else result = result < 0.0d ? double.NegativeInfinity : double.PositiveInfinity;
                        }
                        else result /= value;
                    }
                    return result;
                case MathOperation.Modulo:
                    result = values.Skip(1).Aggregate(result, (current, value) => current % value);
                    return result;
                case MathOperation.Power:
                    result = values.Skip(1).Aggregate(result, Math.Pow);
                    return result;
                case MathOperation.Absolute:
                    return Math.Abs(result);
                default:
                    throw new NotSupportedException(operation + " is not supported " + ".");
            }
        } 

        public static bool OperateBack(MathOperation operation, double value1, double value2, out double result)
        {
            switch (operation)
            {
                case MathOperation.None:
                    result = value1;
                    break;
                case MathOperation.Add:
                    result = value1 - value2;
                    break;
                case MathOperation.Subtract:
                case MathOperation.SubtractPositiveOnly:
                    result = value1 + value2;
                    break;
                case MathOperation.Multiply:
                    result = value2 == 0.0d ? value1 == 0 ? double.NaN : (value1 < 0 ? double.NegativeInfinity : double.PositiveInfinity) : value1 / value2;
                    break;
                case MathOperation.Divide:
                    result = value1 * value2;
                    break;
                case MathOperation.Modulo:
                {
                    var asInt1 = Convert.ToInt32((int)value1);
                    var asInt2 = Convert.ToInt32((int)value2);
                    if (asInt1.InverseUnderModulo(asInt2, out int resultInt))
                        result = Convert.ToDouble(resultInt);
                    else
                    {
                        result = 0;
                        return false;
                    }

                    break;
                }
                case MathOperation.Power when value2 == 0:
                    result = Math.Pow(value1, double.PositiveInfinity);
                    break;
                case MathOperation.Power:
                    result = Math.Pow(value1, 1 / value2);
                    break;
                case MathOperation.Absolute:
                    result = value1;
                    break;
                default:
                    throw new NotSupportedException(operation.ToString() + " is not supported " + ".");
            }

            return true;
        } 
        #endregion

        #region Test data
        public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData();

        public static IEnumerable<object[]> ConvertTestDataForMultibinding => GenerateConvertTestData(false);

        public static IEnumerable<object[]> ConvertBackTestData => GenerateConvertBackTestData();

        private static IEnumerable<object[]> NumberData => MathConverterTestData.Data;

        private static IEnumerable<object> ValuesForInvalid => new List<object> { 0, null };

        private static IEnumerable<MathOperation> Operations { get; } = Enum.GetValues(typeof(MathOperation)).Cast<MathOperation>().ToList();

        private static IEnumerable<CultureInfo> Cultures { get; } = new List<CultureInfo>() 
        { CultureInfo.InvariantCulture, new CultureInfo("fr-FR"), new CultureInfo("en-US") };
        #endregion

        #region Test data generator
        private static IEnumerable<object[]> GenerateConvertTestData(bool dualValuesOnly = true)
        {
            var toReturn = new List<object[]>();
            var asStrings = new[] { false, true };

            foreach (var inputs in NumberData)
                foreach (var operation in Operations)
                    foreach (var invalid in ValuesForInvalid)
                        foreach (var asString in asStrings)
                            foreach (var culture in Cultures)
                            {
                                // Precalculate result:
                                var result = invalid;
                                var reducedInputs = dualValuesOnly ? inputs.Take(2).ToArray() : inputs.ToArray();  // take only first 2 inputs if only two values are required.
                                if (reducedInputs[0].IsNumeric() && (IsSingleValueOperation(operation) || reducedInputs.All(x => x.IsNumeric())))
                                {
                                    var castedInputs = IsSingleValueOperation(operation) ? 
                                                        new List<double>() { Convert.ToDouble(reducedInputs.First()) }.ToArray() 
                                                        : reducedInputs.Select(Convert.ToDouble).ToArray();
                                    result = Operate(operation, castedInputs);
                                    result = asString ? ((double)result).ToString(culture ?? CultureInfo.InvariantCulture) : result;
                                }

                                if (dualValuesOnly)
                                {
                                    var castedReducedInputs = reducedInputs.ToList();
                                    var entry = castedReducedInputs[0];
                                    var parameter = castedReducedInputs[1];
                                    toReturn.Add(new[] { entry, parameter, culture, operation, invalid, asString, result });  // raw input + as parameter + operation + expected result.
                                    toReturn.Add(new[] { entry?.ToString(), parameter?.ToString() , culture, operation, invalid, asString, result });  // string input + string parameter + operation + expected result.
                                }
                                else
                                {
                                    toReturn.Add(new[] { reducedInputs, culture, operation, invalid, asString, result });  // raw inputs + operation + expected result.
                                    toReturn.Add(new[] { reducedInputs.Select(x => x?.ToString()), culture, operation, invalid, asString, result });  // string inputs + operation + expected result.
                                }
                            }

            return toReturn;
        }        

        private static IEnumerable<object[]> GenerateConvertBackTestData()  // for dual values only
        {
            var toReturn = new List<object[]>();

            foreach (var input in NumberData)
                foreach (var operation in Operations)
                    foreach (var invalid in ValuesForInvalid)
                        foreach (var culture in Cultures)
                        {
                            // Precalculate result:
                            var result = invalid;
                            var value1 = input[0];
                            var value2 = input[1];
                            if (value1.IsNumeric() && (value2.IsNumeric() || IsSingleValueOperation(operation)))
                            {
                                if (OperateBack(operation, Convert.ToDouble(value1), IsSingleValueOperation(operation) ? 0.0d : Convert.ToDouble(value2), out var asDouble))
                                    result = asDouble;                                        
                            }
                            toReturn.Add(new[] { value1, value2, culture, operation, invalid, result });
                            toReturn.Add(new[] 
                            { 
                                value1.IsNumeric() ? Convert.ToString(value1, culture) : value1,
                                value2.IsNumeric() ? Convert.ToString(value2, culture) : value2, culture, operation, invalid, result 
                            });
                        }

            return toReturn;
        } 
        #endregion
    }
}
