using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanConvertersForMultibindingTestsBase
    {
        protected bool Operate(BooleanOperation operation, bool[] inputs)
        {
            switch(operation)
            {
                case BooleanOperation.Equality:
                    return inputs.All(x => x == inputs[0]);
                    
                case BooleanOperation.None:
                case BooleanOperation.And:
                    return inputs.All(v => v == true);

                case BooleanOperation.Or:
                    return inputs.Any(v => v == true);

                case BooleanOperation.Xor:
                    return inputs.Any(x => x == true) && !inputs.All(x => x == true);

                case BooleanOperation.Not:
                case BooleanOperation.Nand:
                    return !inputs.All(v => v == true);

                case BooleanOperation.Nor:
                    return !inputs.Any(v => v == true);

                case BooleanOperation.Xnor:
                    return !(inputs.Any(x => x == true) && !inputs.All(x => x == true));

                default:
                    throw new NotSupportedException("'" + operation.ToString() + "' operation is not supported for " + nameof(BooleanToVisibilityConverterForMultibinding) + ".");
            }
        }

        protected void TestConversion<T>(BooleanConverterBaseForMultibinding<T> converter, object[] inputs, T valueForTrue, T valueForFalse, T valueForInvalid, BooleanOperation operation)
        {
            converter.ValueForTrue = valueForTrue;
            converter.ValueForFalse = valueForFalse;
            converter.ValueForInvalid = valueForInvalid;
            converter.Operation = operation;

            var result = converter.Convert(inputs, typeof(bool), null, null);
            
            if (inputs != null)
            {
                var casted = inputs.Where(x => x is bool).Cast<bool>();
                if (casted.Count() == inputs.Length)  // if all are bools.
                {
                    var expected = Operate(operation, casted.ToArray());
                    if (expected)
                        Assert.Equal(valueForTrue, result);
                    else Assert.Equal(valueForFalse, result);
                }
                else Assert.Equal(valueForInvalid, result);
            }
            else Assert.Equal(valueForInvalid, result);
        }

        #region Base for data sets
        public abstract class BooleanConverterBaseForMultibindingTestData<TResult>
        {
            public static List<object[]> Inputs = new List<object[]>() 
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
            public static List<BooleanOperation> Operations = Enum.GetValues(typeof(BooleanOperation)).Cast<BooleanOperation>().ToList();

            public static IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(null);

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
        }
        #endregion

    }
}
