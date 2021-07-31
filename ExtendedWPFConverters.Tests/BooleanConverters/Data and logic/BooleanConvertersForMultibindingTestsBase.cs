using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanConvertersForMultibindingTestsBase
    {
        public static bool Operate(BooleanOperation operation, bool[] inputs)
        {
            switch(operation)
            {
                case BooleanOperation.Equality:
                    return inputs.All(x => x == inputs[0]);
                    
                case BooleanOperation.None:
                case BooleanOperation.And:
                    return inputs.All(v => v);

                case BooleanOperation.Or:
                    return inputs.Any(v => v);

                case BooleanOperation.Xor:
                    return inputs.Count(v => v) % 2 == 1;

                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                    return inputs.Any(v => v != true);

                case BooleanOperation.Nor:
                    return inputs.All(v => v != true);

                case BooleanOperation.XNor:
                    return inputs.Count(v => v) % 2 == 0;

                default:
                    throw new NotSupportedException("'" + operation + "' operation is not supported for " + nameof(BooleanToVisibilityConverterForMultibinding) + ".");
            }
        }

        protected static void TestConversion<T>(BooleanConverterBaseForMultibinding<T> converter, object[] inputs, T valueForTrue, T valueForFalse, T valueForInvalid, BooleanOperation operation)
        {
            converter.ValueForTrue = valueForTrue;
            converter.ValueForFalse = valueForFalse;
            converter.ValueForInvalid = valueForInvalid;
            converter.Operation = operation;

            var result = converter.Convert(inputs, typeof(bool), null, null);
            
            if (inputs != null)
            {
                var casted = inputs.Where(x => x is bool).Cast<bool>().ToArray();
                if (casted.Length == inputs.Length)  // if all are booleans.
                {
                    var expected = Operate(operation, casted.ToArray());
                    Assert.Equal(expected ? valueForTrue : valueForFalse, result);
                }
                else Assert.Equal(valueForInvalid, result);
            }
            else Assert.Equal(valueForInvalid, result);
        }

        #region Base for data sets
        public abstract class BooleanConverterBaseForMultibindingTestData<TResult>
        {
            private static readonly List<object[]> _inputs = new List<object[]>
            { 
                new object[] { false },
                new object[] { true },
                new object[] { false, false },
                new object[] { false, true },
                new object[] { true, false },
                new object[] { true, true },
                new object[] { false, false, false },
                new object[] { false, false, true },
                new object[] { false, true, false },
                new object[] { false, true, true },
                new object[] { true, false, false },
                new object[] { true, true, true },
                new object[] { false, false, false, false, false, false },
                new object[] { false, false, false, false, false, false, true },
                new object[] { false, false, true, false, true, false, true, false },

                new object[] { },
                new object[] { null },
                new object[] { "invalid" },
                new object[] { false, null, false, false, false, false, false },
                new object[] { false, "invalid", false, false, false, false, false },
                new object[] { true, null, false, false, false, false, false },
                new object[] { true, "invalid", false, false, false, false, false },
                new object[] { null, false },
                new object[] { "invalid", true },
                new object[] { false, null },
                new object[] { false, "invalid" },
                new object[] { null, null },
                new object[] { "invalid", "invalid" }, 
            };

            private static readonly List<BooleanOperation> _operations = Enum.GetValues(typeof(BooleanOperation)).Cast<BooleanOperation>().ToList();

            public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(null);

            protected static IEnumerable<object[]> GenerateConvertTestData(List<(TResult,TResult,TResult)> defaultValues)
            {
                var toReturn = new List<object[]>();

                foreach (var input in _inputs)
                    foreach (var operation in _operations)
                    {
                        if (defaultValues != null)
                            foreach (var (item1, item2, item3) in defaultValues)
                                toReturn.Add(new object[] { input, item1, item2, item3, operation });

                        // Add null inputs for fun or as default:
                        toReturn.Add(new object[] { input, null, null, null, operation });
                    }

                return toReturn;
            }
        }
        #endregion

    }
}
