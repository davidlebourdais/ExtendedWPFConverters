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
                    foreach (var value in values.Skip(1))
                        result += value;
                    return result;
                case MathOperation.Substract:
                    foreach (var value in values.Skip(1))
                        result -= value;
                    return result;
                case MathOperation.SubstractPositiveOnly:
                    foreach (var value in values.Skip(1))
                        result -= value;
                    return result > 0.0d ? result : 0.0d; 
                case MathOperation.Multiply:
                    foreach (var value in values.Skip(1))
                        result *= value;
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
                    foreach (var value in values.Skip(1))
                        result %= value;
                    return result;
                case MathOperation.Power:
                    foreach (var value in values.Skip(1))
                        result = Math.Pow(result, value);
                    return result;
                case MathOperation.Absolute:
                    return Math.Abs(result);
                default:
                    throw new NotSupportedException(operation.ToString() + " is not supported " + ".");
            }
        } 

        public static bool OperateBack(MathOperation operation, double value1, double value2, out double result)
        {
            if (operation == MathOperation.None)
                result = value1;
            else if (operation == MathOperation.Add)
                result = value1 - value2;
            else if (operation == MathOperation.Substract || operation == MathOperation.SubstractPositiveOnly)
                result = value1 + value2;
            else if (operation == MathOperation.Multiply)
                result = value2 == 0.0d ? value1 == 0 ? double.NaN : (value1 < 0 ? double.NegativeInfinity : double.PositiveInfinity) : value1 / value2;
            else if (operation == MathOperation.Divide)
                result = value1 * value2;
            else if (operation == MathOperation.Modulo)
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
            }
            else if (operation == MathOperation.Power)
            {
                if (value2 == 0)
                    result = Math.Pow(value1, double.PositiveInfinity);
                else
                    result = Math.Pow(value1, 1 / value2);
            }
            else if (operation == MathOperation.Absolute)
                result = value1;
            else
                throw new NotSupportedException(operation.ToString() + " is not supported " + ".");
            
            return true;
        } 
        #endregion

        #region Test data
        public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData();

        public static IEnumerable<object[]> ConvertTestDataForMultibinding => GenerateConvertTestData(false);

        public static IEnumerable<object[]> ConvertBackTestData => GenerateConvertBackTestData();

        private static IEnumerable<object[]> NumberData => MathConverterTestData.Data;

        private static IEnumerable<object> ValuesForInvalid => new List<object> { 0, "0", null };

        private static List<MathOperation> Operations { get; } = Enum.GetValues(typeof(MathOperation)).Cast<MathOperation>().ToList();

        private static List<CultureInfo> Cultures { get; } = new List<CultureInfo>() 
        { CultureInfo.InvariantCulture, new CultureInfo("fr-FR"), new CultureInfo("en-US"), new CultureInfo("zh-CHS") };
        #endregion

        #region Test data generator
        private static IEnumerable<object[]> GenerateConvertTestData(bool dual_values_only = true)
        {
            var toReturn = new List<object[]>();
            var asStrings = new bool[2] { false, true };

            foreach (var inputs in NumberData)
                foreach (var operation in Operations)
                    foreach (var invalid in ValuesForInvalid)
                        foreach (var as_string in asStrings)
                            foreach (var culture in Cultures)
                            {
                                // Precalculate result:
                                var result = invalid;
                                var reducedInputs = dual_values_only ? inputs.Take(2) : inputs;  // take only first 2 inputs if only two values are required.
                                if (reducedInputs.First().IsNumeric() && (IsSingleValueOperation(operation) || reducedInputs.All(x => x.IsNumeric())))
                                {
                                    var castedInputs = IsSingleValueOperation(operation) ? 
                                                        new List<double>() { Convert.ToDouble(reducedInputs.First()) }.ToArray() 
                                                        : reducedInputs.Select(x => Convert.ToDouble(x)).ToArray();
                                    result = Operate(operation, castedInputs);
                                    result = as_string ? ((double)result).ToString(culture ?? CultureInfo.InvariantCulture) : result;
                                }

                                if (dual_values_only)
                                {
                                    var castedReducedInputs = reducedInputs.ToList();
                                    var entry = castedReducedInputs[0];
                                    var parameter = castedReducedInputs[1];
                                    toReturn.Add(new object[] { entry, parameter, culture, operation, invalid, as_string, result });  // raw input + as parameter + operation + expected result.
                                    toReturn.Add(new object[] { entry?.ToString(), parameter?.ToString() , culture, operation, invalid, as_string, result });  // string input + string parameter + operation + expected result.
                                }
                                else
                                {
                                    toReturn.Add(new object[] { reducedInputs, culture, operation, invalid, as_string, result });  // raw inputs + operation + expected result.
                                    toReturn.Add(new object[] { reducedInputs.Select(x => x != null ? x.ToString() : null), culture, operation, invalid, as_string, result });  // string inputs + operation + expected result.
                                }
                            }

            return toReturn;
        }        

        private static IEnumerable<object[]> GenerateConvertBackTestData()  // for dual values only
        {
            var toReturn = new List<object[]>();
            var asStrings = new bool[2] { false, true };

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
                                double asDouble;
                                if (OperateBack(operation, Convert.ToDouble(value1), IsSingleValueOperation(operation) ? 0.0d : Convert.ToDouble(value2), out asDouble))
                                    result = asDouble;                                        
                            }
                            toReturn.Add(new object[] { value1, value2, culture, operation, invalid, result });
                            toReturn.Add(new object[] { value1.IsNumeric() ? Convert.ToString(value1, culture) : value1, value2.IsNumeric() ? Convert.ToString(value2, culture) : value2, culture, operation, invalid, result });
                        }

            return toReturn;
        } 
        #endregion
    }
}
