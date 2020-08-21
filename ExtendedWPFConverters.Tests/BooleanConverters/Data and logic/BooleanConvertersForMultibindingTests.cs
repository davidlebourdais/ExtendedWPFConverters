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
