using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanConverterBaseTests
    {
        protected bool Operate(ReducedBooleanOperation operation, bool input) 
            => operation == ReducedBooleanOperation.None ? input 
                                                         : operation == ReducedBooleanOperation.Not ? !input 
                                                                                                    : throw new NotSupportedException("Unknown " + nameof(ReducedBooleanOperation));

        protected void TestConversion<T>(BooleanConverterBase<T> converter, object input, T valueForTrue, T valueForFalse, T valueForInvalid, ReducedBooleanOperation operation)
        {
            converter.ValueForTrue = valueForTrue;
            converter.ValueForFalse = valueForFalse;
            converter.ValueForInvalid = valueForInvalid;
            converter.Operation = operation;

            var result = converter.Convert(input, typeof(bool), null, null);

            if (input is bool)
            {
                var expected = Operate(operation, (input as bool?) == true);
                if (expected)
                    Assert.Equal(valueForTrue, result);
                else Assert.Equal(valueForFalse, result);
            }
            else Assert.Equal(valueForInvalid, result);
        }

        protected void TestConversionBack<T>(BooleanConverterBase<T> converter, object input, T valueForTrue, T valueForFalse, T valueForInvalid, ReducedBooleanOperation operation)
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
            public static List<object> Inputs = new List<object>() { false, true, null, "invalid" };
            public static List<ReducedBooleanOperation> Operations = Enum.GetValues(typeof(ReducedBooleanOperation)).Cast<ReducedBooleanOperation>().ToList();

            public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(null);

            public static IEnumerable<object[]> ConvertBackTestData => GenerateConvertBackTestData();

            protected static IEnumerable<object[]> GenerateConvertTestData(List<(TResult,TResult,TResult)> defaultValues)
            {
                var toReturn = new List<object[]>();

                foreach (var input in Inputs)
                    foreach (var operation in Operations)
                    {
                        if (defaultValues != null)
                            foreach (var defaultValueSet in defaultValues)
                                toReturn.Add(new object[] { input, defaultValueSet.Item1, defaultValueSet.Item2, defaultValueSet.Item3, operation });

                        // Add null inputs for fun or as default:
                        toReturn.Add(new object[] { input, null, null, null, operation });
                    }

                return toReturn;
            }

            protected static IEnumerable<object[]> GenerateConvertBackTestData()
            {
                var toReturn = new List<object[]>();

                // Take data and toggle input/result:
                foreach (var dataline in ConvertTestData)
                {
                    if ((dataline[0] as bool?) == true)
                        dataline[0] = dataline[1];
                    else if ((dataline[0] as bool?) == false)
                        dataline[0] = dataline[2];
                    else dataline[0] = dataline[3];

                    if (toReturn.All(x => x.Except(dataline).Any()))  // only add if combination is not already existing.
                        toReturn.Add(dataline);
                }

                // Add invalid inputs (note that they might be valid depending on target type, then they shall not be processed separately):
                foreach (var operation in Operations)
                    foreach (var dataline in ConvertTestData.Select(x => ValueTuple.Create(x[1], x[2], x[3])).Distinct())
                    {
                        toReturn.Add(new object[] { null, dataline.Item1, dataline.Item2, dataline.Item3, operation });
                        toReturn.Add(new object[] { "invalid", dataline.Item1, dataline.Item2, dataline.Item3, operation });
                    }

                return toReturn;
            }
        }
        #endregion

    }
}
