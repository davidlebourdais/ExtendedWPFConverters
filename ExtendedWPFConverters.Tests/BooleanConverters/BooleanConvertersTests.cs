using System;
using Xunit;
using System.Collections.Generic;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanConvertersTests : BooleanConvertersTestsBase
    {
        #region BooleanToBooleanConverter
        [Theory]
        [InlineData(false, ReducedBooleanOperation.None)]
        [InlineData(true, ReducedBooleanOperation.None)]
        [InlineData(false, ReducedBooleanOperation.Not)]
        [InlineData(true, ReducedBooleanOperation.Not)]
        public void ConvertsBooleanToBoolean(bool input, ReducedBooleanOperation operation)
        {
            var converter = new BooleanToBooleanConverter() { Operation = operation };
            var result = converter.Convert(input, typeof(bool), null, null);
            Assert.Equal(Operate(operation, input), result);
        }

        [Theory]
        [InlineData(false, ReducedBooleanOperation.None)]
        [InlineData(true, ReducedBooleanOperation.None)]
        [InlineData(false, ReducedBooleanOperation.Not)]
        [InlineData(true, ReducedBooleanOperation.Not)]
        public void ConvertsBooleanBackToBoolean(bool input, ReducedBooleanOperation operation)
        {
            var converter = new BooleanToBooleanConverter() { Operation = operation };
            var result = converter.ConvertBack(input, typeof(bool), null, null);
            Assert.Equal(Operate(operation, input), result);
        }

        [Fact]
        public void BooleanToBooleanConvertersCannotChangeDefaults()
        {
            var converter = new BooleanToBooleanConverter();

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

        #region BooleanToFontWeightConverter
        public class FontWeightTestData : BooleanConverterBaseTestData<FontWeight>
        {            
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(FontWeight,FontWeight,FontWeight)>()
            {
                ValueTuple.Create(FontWeights.Normal, FontWeights.DemiBold, FontWeights.Normal),
                ValueTuple.Create(FontWeights.DemiBold, FontWeights.Heavy, FontWeights.Normal),
                ValueTuple.Create(FontWeights.Bold, FontWeights.Thin, FontWeights.DemiBold),
                ValueTuple.Create(FontWeights.Normal, FontWeights.Normal, FontWeights.Normal),
            });
        }

        [Theory]
        [MemberData(nameof(FontWeightTestData.ConvertTestData), MemberType= typeof(FontWeightTestData))]
        public void ConvertsBooleanToFontWeight(object input, FontWeight valueForTrue, FontWeight valueForFalse, FontWeight valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversion(new BooleanToFontWeightConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        
        [Theory]
        [MemberData(nameof(FontWeightTestData.ConvertBackTestData), MemberType= typeof(FontWeightTestData))]
        public void ConvertsFontWeightBackToBoolean(object input, FontWeight valueForTrue, FontWeight valueForFalse, FontWeight valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversionBack(new BooleanToFontWeightConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        #endregion

        #region BooleanToNumberConverter
        public class NumberTestData : BooleanConverterBaseTestData<IComparable>
        {
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(IComparable,IComparable,IComparable)>()
            {
                ValueTuple.Create(1.0, 0.0, 0.0),
                ValueTuple.Create(10, 1, -1),
                ValueTuple.Create(10d, 1d, -1d),
                ValueTuple.Create(0.38, 1, 0.8),
                ValueTuple.Create(-8E2, 1E2, 1E2),
                ValueTuple.Create(1, 3, 2),
                ValueTuple.Create((byte)2, (byte)255, (byte)0),
                ValueTuple.Create((long)-2000, (long)255E2, (long)0),
            });
        }

        [Theory]
        [MemberData(nameof(NumberTestData.ConvertTestData), MemberType= typeof(NumberTestData))]
        public void ConvertsBooleanToNumber(object input, IComparable valueForTrue, IComparable valueForFalse, IComparable valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversion(new BooleanToNumberConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        
        [Theory]
        [MemberData(nameof(NumberTestData.ConvertBackTestData), MemberType= typeof(NumberTestData))]
        public void ConvertsNumberBackToBoolean(object input, IComparable valueForTrue, IComparable valueForFalse, IComparable valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversionBack(new BooleanToNumberConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        #endregion

        #region BooleanToOpacityConverter
        public class OpacityTestData : BooleanConverterBaseTestData<double>
        {
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(double,double,double)>()
            {
                ValueTuple.Create(1.0, 0.0, 0.0),
                ValueTuple.Create(0.0, 1.0, 0.0),
                ValueTuple.Create(1.0, 0.48, 0d),
                ValueTuple.Create(10.0, 0.48, 0d),
                ValueTuple.Create(1.0, -0.48, 0d),
            });
        }
        [Theory]
        [MemberData(nameof(OpacityTestData.ConvertTestData), MemberType= typeof(OpacityTestData))]
        public void ConvertsBooleanToOpacity(object input, double valueForTrue, double valueForFalse, double valueForInvalid, ReducedBooleanOperation operation)
        {
            if (valueForTrue < 0 || valueForTrue > 1.0 || valueForFalse < 0 || valueForFalse > 1.0 || valueForInvalid < 0 || valueForInvalid > 1.0)
                Assert.Throws<ArgumentException>(() => base.TestConversion(new BooleanToOpacityConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation));
            else base.TestConversion(new BooleanToOpacityConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        }
        
        [Theory]
        [MemberData(nameof(NumberTestData.ConvertBackTestData), MemberType= typeof(NumberTestData))]
        public void ConvertsOpacityBackToBoolean(object input, double valueForTrue, double valueForFalse, double valueForInvalid, ReducedBooleanOperation operation)
        {
            if (valueForTrue < 0 || valueForTrue > 1.0 || valueForFalse < 0 || valueForFalse > 1.0 || valueForInvalid < 0 || valueForInvalid > 1.0)
                Assert.Throws<ArgumentException>(() => base.TestConversionBack(new BooleanToOpacityConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation));
            else base.TestConversionBack(new BooleanToOpacityConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation); 
        }
        #endregion

        #region BooleanToThicknessConverter
        public class ThicknessTestData : BooleanConverterBaseTestData<Thickness>
        {
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(Thickness,Thickness,Thickness)>()
            {
                ValueTuple.Create(new Thickness(1), new Thickness(0), new Thickness(0)),
                ValueTuple.Create(new Thickness(0.2), new Thickness(1), new Thickness(-1)),
                ValueTuple.Create(new Thickness(10), new Thickness(10), new Thickness(0)),
                ValueTuple.Create(new Thickness(5), new Thickness(5), new Thickness(5))
            });
        }

        [Theory]
        [MemberData(nameof(ThicknessTestData.ConvertTestData), MemberType= typeof(ThicknessTestData))]
        public void ConvertsBooleanToThickness(object input, Thickness valueForTrue, Thickness valueForFalse, Thickness valueForInvalid, ReducedBooleanOperation operation)
           => base.TestConversion(new BooleanToThicknessConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        
        [Theory]
        [MemberData(nameof(ThicknessTestData.ConvertBackTestData), MemberType= typeof(ThicknessTestData))]
        public void ConvertsThicknessBackToBoolean(object input, Thickness valueForTrue, Thickness valueForFalse, Thickness valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversionBack(new BooleanToThicknessConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        #endregion

        #region BooleanToVisibilityConverter
        public class VisibilityTestData : BooleanConverterBaseTestData<Visibility>
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
        public void ConvertsBooleanToVisibility(object input, Visibility valueForTrue, Visibility valueForFalse, Visibility valueForInvalid, ReducedBooleanOperation operation)
           => base.TestConversion(new BooleanToVisibilityConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        
        [Theory]
        [MemberData(nameof(VisibilityTestData.ConvertBackTestData), MemberType= typeof(VisibilityTestData))]
        public void ConvertsVisibilityBackToBoolean(object input, Visibility valueForTrue, Visibility valueForFalse, Visibility valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversionBack(new BooleanToVisibilityConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        #endregion
    
        #region BooleanToObjectConverter
        public class ObjectTestData : BooleanConverterBaseTestData<object>
        {
            public static new IEnumerable<object[]> ConvertTestData => GenerateConvertTestData(new List<(object,object,object)>()
            {
                ValueTuple.Create(string.Empty, string.Empty, string.Empty),
                ValueTuple.Create(Int32.MaxValue, default(Int32), (object)null),
                ValueTuple.Create(DateTime.Now, (object)null, (object)null),
                ValueTuple.Create(DateTime.Now, string.Empty, Double.MinValue)
            });
        }

        [Theory]
        [MemberData(nameof(ObjectTestData.ConvertTestData), MemberType= typeof(ObjectTestData))]
        public void ConvertsBooleanToObject(object input, object valueForTrue, object valueForFalse, object valueForInvalid, ReducedBooleanOperation operation)
           => base.TestConversion(new BooleanToObjectConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        
        [Theory]
        [MemberData(nameof(ObjectTestData.ConvertBackTestData), MemberType= typeof(ObjectTestData))]
        public void ConvertsObjectBackToBoolean(object input, object valueForTrue, object valueForFalse, object valueForInvalid, ReducedBooleanOperation operation)
            => base.TestConversionBack(new BooleanToObjectConverter(), input, valueForTrue, valueForFalse, valueForInvalid, operation);
        #endregion
    }
}
