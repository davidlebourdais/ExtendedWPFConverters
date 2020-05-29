using System.Globalization;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using EMA.ExtendedWPFConverters.Utils;
using EMA.ExtendedWPFConverters.Tests.Utils;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class MathConverterTests
    {
        #region MathConverter
        public class MathConverterDataProvider
        {
            protected static bool IsSingleValueOperation(MathOperation operation) => operation == MathOperation.None || operation == MathOperation.Abs;

            protected static double Operate(MathOperation operation, double value1, double value2)
            {
                switch (operation)
                {
                    case MathOperation.None:
                        return value1;
                    case MathOperation.Add:
                        return value1 + value2;
                    case MathOperation.Substract:
                        return value1 - value2; 
                    case MathOperation.SubstractPositiveOnly:
                        return (value1 - value2) > 0.0d ? (value1 - value2) : 0.0d; 
                    case MathOperation.Multiply:
                        return value1 * value2; 
                    case MathOperation.Divide:
                        return value2 == 0.0d ? value1 / value2 : (value1 < 0.0d ? double.NegativeInfinity : double.PositiveInfinity); 
                    case MathOperation.Modulo:
                        return value1 % value2;
                    case MathOperation.Power:
                        return Math.Pow(value1, value2);
                    case MathOperation.Abs:
                        return Math.Abs(value1);
                    default:
                        throw new NotSupportedException(operation.ToString() + " is not supported " + ".");
                }
            } 

            protected static bool OperateBack(MathOperation operation, double value1, double value2, out double result)
            {
                if (operation == MathOperation.None)
                    result = value1;
                else if (operation == MathOperation.Add)
                    result = value1 - value2;
                else if (operation == MathOperation.Substract || operation == MathOperation.SubstractPositiveOnly)
                    result = value1 + value2;
                else if (operation == MathOperation.Multiply)
                    result = value2 == 0.0d ? (value1 < 0 ? double.NegativeInfinity : double.PositiveInfinity) : value1 / value2;
                else if (operation == MathOperation.Divide)
                    result = value1 * value2;
                else if (operation == MathOperation.Modulo)
                {
                    var asInt1 = Convert.ToInt32(value1);
                    var asInt2 = Convert.ToInt32(value2);
                    if (asInt1.InverseUnderModulo(asInt2, out int resultInt))
                        result = Convert.ToDouble(resultInt);
                    else
                    {
                        result = 0;
                        return false;
                    }
                }
                else if (operation == MathOperation.Power)
                    result = Math.Pow(value1, 1 / value2);
                else if (operation == MathOperation.Abs)
                    result = value1;
                else
                    throw new NotSupportedException(operation.ToString() + " is not supported " + ".");
                
                return true;
            } 

            public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData();

            public static IEnumerable<object[]> ConvertBackTestData => GenerateConvertBackTestData();

            protected static IEnumerable<object[]> NumberData => new List<object[]>
            {
                /* Integers */
                // Base cases:
                new object[] { 3, 5, },
                new object[] { 5, 3 },
                new object[] { 1, 2 },
                new object[] { 2, 1 },

                // Equals:
                new object[] { 1, 1 },
                new object[] { 5, 5 },

                // Zeroed:
                new object[] { 1, 0 },
                new object[] { 0, 5 },
                new object[] { 0, 0 },

                // Negative:
                new object[] { -1, 2 },
                new object[] { 1, -2 },
                new object[] { -1, -2 },
                new object[] { -2, -10 },
                
                // Extrema:
                new object[] { Int32.MinValue, 0 },
                new object[] { Int32.MinValue, 5 },
                new object[] { Int32.MinValue, -6 },
                new object[] { 0, Int32.MinValue},
                new object[] { 5, Int32.MinValue},
                new object[] { -6, Int32.MinValue},
                new object[] { Int32.MaxValue, 0 },
                new object[] { Int32.MaxValue, 5 },
                new object[] { Int32.MaxValue, -6 },
                new object[] { 0, Int32.MaxValue},
                new object[] { 5, Int32.MaxValue},
                new object[] { -6, Int32.MaxValue},
                new object[] { Int32.MaxValue, Int32.MinValue },
                new object[] { Int32.MinValue, Int32.MaxValue },

                /* Double */
                // Base cases:
                new object[] { 1.2, 2.5 },
                new object[] { 2.5, 1.2 },
                new object[] { 10.25, 2.1235 },
                new object[] { 2.1235, 10.25 },

                // Equals:
                new object[] { 1.2, 1.2 },
                new object[] { 10.25, 10.25 },

                // Zeroed:
                new object[] { 1.85, 0.0d },
                new object[] { 0, 5.1 },
                new object[] { 0.0d, 0.0d },
                new object[] { 0.0, 0.0 },

                // Negative:
                new object[] { -1.2, 2.5 },
                new object[] { -2.5, 1.2 },
                new object[] { -2.5, -1.2 },
                new object[] { 10.25, 2.1235 },
                new object[] { -2.1235, -10.25 },

                // Exp:
                new object[] { 1E-6, -1E3 },
                new object[] { -1E-5, 3E2 },
                new object[] { 1E-6, -1E3 },

                // // Extrema:
                new object[] { Double.MinValue, 0.0 },
                new object[] { Double.MinValue, 5.2 },
                new object[] { Double.MinValue, -6.2 },
                new object[] { 0, Double.MinValue},
                new object[] { 5.2, Double.MinValue},
                new object[] { -6.2, Double.MinValue},
                new object[] { Double.MaxValue, 0 },
                new object[] { Double.MaxValue, 5.2 },
                new object[] { Double.MaxValue, -6.2 },
                new object[] { 0, Double.MaxValue},
                new object[] { 5.2, Double.MaxValue},
                new object[] { -6.2, Double.MaxValue},
                new object[] { Double.MaxValue, Double.MinValue },
                new object[] { Double.MinValue, Double.MaxValue },

                // // Invalid:
                new object[] { 1.2, null },
                new object[] { null, 2.5 },
                new object[] { null, null },
                new object[] { 1.2, "invalid" },
                new object[] { "invalid", 2.5 },
                new object[] { "invalid", "invalid" },
            };

            protected static IEnumerable<object> ValuesForInvalid => new List<object> { 0, "0", null };

            public static List<MathOperation> Operations { get; } = Enum.GetValues(typeof(MathOperation)).Cast<MathOperation>().ToList();

            public static List<CultureInfo> Cultures { get; } = new List<CultureInfo>() 
            { CultureInfo.InvariantCulture, new CultureInfo("fr-FR"), new CultureInfo("en-US"), new CultureInfo("zh-CHS") }; 

            protected static IEnumerable<object[]> GenerateConvertTestData()
            {
                var toReturn = new List<object[]>();
                var asStrings = new bool[2] { false, true };

                foreach (var input in NumberData)
                    foreach (var operation in Operations)
                        foreach (var invalid in ValuesForInvalid)
                            foreach (var as_string in asStrings)
                                foreach (var culture in Cultures)
                                {
                                    // Precalculate result:
                                    var result = invalid;
                                    if (input.First().IsNumeric() && (input.Last().IsNumeric() || IsSingleValueOperation(operation)))
                                    {
                                        result = Operate(operation, Convert.ToDouble(input.First()), IsSingleValueOperation(operation) ? 0.0d : Convert.ToDouble(input.Last()));
                                        result = as_string ? ((double)result).ToString(culture ?? CultureInfo.InvariantCulture) : result;
                                    }
                                    toReturn.Add(new object[] { input.First(), input.Last(), culture, operation, invalid, as_string, result });  // raw input + operation + expected result.
                                    toReturn.Add(new object[] { input.First()?.ToString(), input.Last()?.ToString(), culture, operation, invalid, as_string, result });  // string input + operation + expected result.
                                }

                return toReturn;
            }        
            protected static IEnumerable<object[]> GenerateConvertBackTestData()
            {
                var toReturn = new List<object[]>();
                var asStrings = new bool[2] { false, true };

                foreach (var input in NumberData.Take(5))
                    foreach (var operation in Operations)
                        foreach (var invalid in ValuesForInvalid)
                            foreach (var culture in Cultures)
                            {
                                // Precalculate result:
                                var result = invalid;
                                var value1 = input.First();
                                var value2 = input.Last();
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
        }

        [Theory]
        [MemberData(nameof(MathConverterDataProvider.ConvertTestData), MemberType= typeof(MathConverterDataProvider))]
        public void ConvertsWithMathOperation(object input, object parameter, CultureInfo culture, MathOperation operation, object valueForInvalid, bool output_as_string, object expected)
        {
            var converter = new MathConverter() { Operation = operation, OutputAsString = output_as_string, ValueForInvalid = valueForInvalid };
            var result = converter.Convert(input, input?.GetType(), parameter, culture);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(MathConverterDataProvider.ConvertBackTestData), MemberType= typeof(MathConverterDataProvider))]
        public void ConvertsBackWithMathOperation(object input, object parameter, CultureInfo culture, MathOperation operation, object valueForInvalid, object expected)
        {
            var converter = new MathConverter() { Operation = operation, ValueForInvalid = valueForInvalid };
            var result = converter.ConvertBack(input, input?.GetType(), parameter, culture);
            Assert.Equal(expected, result);
        }
        #endregion
    }
}
