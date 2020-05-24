using System.Linq;
using System;
using Xunit;
using System.Collections.Generic;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanConvertersForMultiBindingTests : BooleanConvertersForMultibindingTestsBase
    {
        #region BooleanToBooleanConverterForMultibinding
        public class BooleanTestData : BooleanConverterBaseForMultibindingTestData<bool>
        {            
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(bool,bool,bool)>()
            {
                ValueTuple.Create(true, false, false),
                ValueTuple.Create(true, false, true)
            });
        }

        [Theory]
        [MemberData(nameof(BooleanTestData.ConvertTestData), MemberType= typeof(BooleanTestData))]
        public void ConvertsBooleanToBooleanForMultiBinding(object[] inputs, bool value_for_true, bool value_for_false, bool value_for_invalid, BooleanOperation operation)
        {
            if (value_for_true == true && value_for_false == false)  // TestConversion will fail otherwise as changing these values has no effect (see next test).
                base.TestConversion(new BooleanToBooleanConverterForMultibinding(), inputs, value_for_true, value_for_false, value_for_invalid, operation);
        }

        [Fact]
        public void BooleanToBooleanConvertersCannotChangeDefaults()
        {
            var converter = new BooleanToBooleanConverterForMultibinding();

            // As designed, cannot set value for true or value for false
            // as there is not point in switching them for bools:
            var previous = converter.ValueForTrue;
            converter.ValueForTrue = !previous;
            Assert.Equal(previous, converter.ValueForTrue);
            previous = converter.ValueForFalse;
            converter.ValueForFalse = !previous;
            Assert.Equal(previous, converter.ValueForFalse);

            // Can set value for invalid though:
            previous = converter.ValueForInvalid;
            converter.ValueForInvalid = !previous;
            Assert.NotEqual(previous, converter.ValueForInvalid);
        }
        #endregion

        #region BooleanToNullOrObjectConverterForMultibinding
        public class NullOrObjectTestData
        {            
            private static IEnumerable<object> TestObjects = new List<object>() 
            {
                "test", 123, true, TimeSpan.FromDays(1), new List<string>(), null
            };

            public static IEnumerable<object[]> ConvertTestData => MakeConvertTestData();

            private static List<object[]> MakeConvertTestData()
            {
                var toReturn = new List<object[]>();
                foreach (var operation in BooleanConverterBaseForMultibindingTestData<object>.Operations)  // combine operations
                    foreach (var objectInput in TestObjects)  // with test oject at first position
                        foreach (var input in BooleanConverterBaseForMultibindingTestData<object>.Inputs) // then bool inputs
                        {
                            var data = new List<object>();
                            data.Add(objectInput);
                            foreach (var subinput in input)
                                data.Add(subinput);
                            toReturn.Add(new object[] {data, operation });
                        } 
                return toReturn;
            }
        }

        [Theory]
        [MemberData(nameof(NullOrObjectTestData.ConvertTestData), MemberType= typeof(NullOrObjectTestData))]
        public void ConvertsNullOrObjectAndBooleanToNullOrObject(object[] inputs, BooleanOperation operation)
        {
            var converter = new BooleanToNullOrObjectConverterForMultibinding() { Operation = operation };
            var result = converter.Convert(inputs, typeof(object), null, null);
            
            if (inputs.Length > 0)
            {
                var boolInputs = inputs.Skip(1);
                if (boolInputs.All(x => x is bool))
                {
                    var subResult = base.Operate(operation, boolInputs.Where(x => x is bool).Cast<bool>().ToArray());
                    if (subResult)
                        Assert.Equal(inputs[0], result);
                    else Assert.Null(result);
                } else Assert.Null(result);
            }
            else Assert.Null(result);
        }
        #endregion

        #region BooleanToVisibilityConverterForMultibinding
        public class VisibilityTestData : BooleanConverterBaseForMultibindingTestData<Visibility>
        {            
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(Visibility,Visibility,Visibility)>()
            {
                ValueTuple.Create(Visibility.Visible, Visibility.Hidden, Visibility.Collapsed),
                ValueTuple.Create(Visibility.Collapsed, Visibility.Visible, Visibility.Visible),
                ValueTuple.Create(Visibility.Visible, Visibility.Collapsed, Visibility.Visible),
                ValueTuple.Create(Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed),
                ValueTuple.Create(Visibility.Hidden, Visibility.Visible, Visibility.Collapsed)
            });
        }

        [Theory]
        [MemberData(nameof(VisibilityTestData.ConvertTestData), MemberType= typeof(VisibilityTestData))]
        public void ConvertsBooleanToVisibilityForMultiBinding(object[] inputs, Visibility valueForTrue, Visibility valueForFalse, Visibility valueForInvalid, BooleanOperation operation)
                => base.TestConversion(new BooleanToVisibilityConverterForMultibinding(), inputs, valueForTrue, valueForFalse, valueForInvalid, operation);
        #endregion
   }
}
