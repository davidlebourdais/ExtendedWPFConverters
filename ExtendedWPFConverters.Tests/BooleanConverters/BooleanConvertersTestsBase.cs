using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanConvertersTestsBase
    {
        protected static bool Operate(ReducedBooleanOperation operation, bool input) 
            => operation == ReducedBooleanOperation.None ? input 
                                                         : operation == ReducedBooleanOperation.Not ? !input 
                                                                                                    : throw new NotSupportedException("Unknown " + nameof(ReducedBooleanOperation));

        protected static void TestConversion<T>(BooleanConverterBase<T> converter, object input, T valueForTrue, T valueForFalse, T valueForInvalid, ReducedBooleanOperation operation)
        {
            converter.ValueForTrue = valueForTrue;
            converter.ValueForFalse = valueForFalse;
            converter.ValueForInvalid = valueForInvalid;
            converter.Operation = operation;

            var result = converter.Convert(input, typeof(bool), null, null);

            if (input is bool)
            {
                var expected = Operate(operation, (input as bool?) == true);
                Assert.Equal(expected ? valueForTrue : valueForFalse, result);
            }
            else Assert.Equal(valueForInvalid, result);
        }

        protected static void TestConversionBack<T>(BooleanConverterBase<T> converter, object input, T valueForTrue, T valueForFalse, T valueForInvalid, ReducedBooleanOperation operation)
        {
            converter.ValueForTrue = valueForTrue;
            converter.ValueForFalse = valueForFalse;
            converter.ValueForInvalid = valueForInvalid;
            converter.Operation = operation;
            
            var resultBack = converter.ConvertBack(input, typeof(T), null, null);
            
            if (input is T && (input.Equals(valueForTrue) || input.Equals(valueForFalse) || input.Equals(valueForInvalid)))
            {
                var expected = Operate(operation, input.Equals(valueForTrue));
                Assert.Equal(expected, resultBack);
            }
            else Assert.Equal(false, resultBack);
        }

        #region Base for data sets
        public abstract class BooleanConverterBaseTestData<TResult>
        {
            private static readonly List<object> _inputs = new List<object>() { false, true, null, "invalid" };
            private static readonly List<ReducedBooleanOperation> _operations = Enum.GetValues(typeof(ReducedBooleanOperation)).Cast<ReducedBooleanOperation>().ToList();

            public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(null);

            public static IEnumerable<object[]> ConvertBackTestData => GenerateConvertBackTestData();

            protected static IEnumerable<object[]> GenerateConvertTestData(List<(TResult,TResult,TResult)> defaultValues)
            {
                var toReturn = new List<object[]>();

                foreach (var input in _inputs)
                    foreach (var operation in _operations)
                    {
                        if (defaultValues != null)
                            foreach (var (item1, item2, item3) in defaultValues)
                                toReturn.Add(new[] { input, item1, item2, item3, operation });

                        // Add null inputs for fun or as default:
                        toReturn.Add(new[] { input, null, null, null, operation });
                    }

                return toReturn;
            }

            protected static IEnumerable<object[]> GenerateConvertBackTestData()
            {
                var toReturn = new List<object[]>();

                // Take data and toggle input/result:
                foreach (var dataLine in ConvertTestData)
                {
                    if ((dataLine[0] as bool?) == true)
                        dataLine[0] = dataLine[1];
                    else if ((dataLine[0] as bool?) == false)
                        dataLine[0] = dataLine[2];
                    else dataLine[0] = dataLine[3];

                    if (toReturn.All(x => x.Except(dataLine).Any()))  // only add if combination is not already existing.
                        toReturn.Add(dataLine);
                }

                // Add invalid inputs (note that they might be valid depending on target type, then they shall not be processed separately):
                foreach (var operation in _operations)
                    foreach (var (item1, item2, item3) in ConvertTestData.Select(x => ValueTuple.Create(x[1], x[2], x[3])).Distinct())
                    {
                        toReturn.Add(new[] { null, item1, item2, item3, operation });
                        toReturn.Add(new[] { "invalid", item1, item2, item3, operation });
                    }

                return toReturn;
            }
        }
        #endregion

    }
}
