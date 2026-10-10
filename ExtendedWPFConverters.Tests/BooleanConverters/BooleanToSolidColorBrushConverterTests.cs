using System.Windows;
using System.Windows.Media;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class BooleanToSolidColorBrushConverterTests
    {
        [Theory]
        [InlineData(true, ReducedBooleanOperation.None, true)]
        [InlineData(false, ReducedBooleanOperation.None, false)]
        [InlineData(true, ReducedBooleanOperation.Not, false)]
        [InlineData(false, ReducedBooleanOperation.Not, true)]
        public void ConvertsBooleanToDefaultBrush(bool input, ReducedBooleanOperation operation, bool expectedBlack)
        {
            var converter = new BooleanToSolidColorBrushConverter() { Operation = operation };

            var result = converter.Convert(input, typeof(SolidColorBrush), null, null);

            Assert.Same(expectedBlack ? Brushes.Black : Brushes.Transparent, result);
        }

        [Theory]
        [InlineData(ReducedBooleanOperation.None)]
        [InlineData(ReducedBooleanOperation.Not)]
        public void ReturnsConfiguredBrushInstances(ReducedBooleanOperation operation)
        {
            var trueBrush = new SolidColorBrush(Colors.Red);
            var falseBrush = new SolidColorBrush(Colors.Blue);
            var converter = new BooleanToSolidColorBrushConverter()
            {
                ValueForTrue = trueBrush,
                ValueForFalse = falseBrush,
                Operation = operation
            };
            var inverted = operation == ReducedBooleanOperation.Not;

            Assert.Same(inverted ? falseBrush : trueBrush, converter.Convert(true, typeof(SolidColorBrush), null, null));
            Assert.Same(inverted ? trueBrush : falseBrush, converter.Convert(false, typeof(SolidColorBrush), null, null));
            Assert.Equal(!inverted, converter.ConvertBack(trueBrush, typeof(bool), null, null));
            Assert.Equal(inverted, converter.ConvertBack(falseBrush, typeof(bool), null, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalid")]
        [InlineData(1)]
        public void ReturnsConfiguredBrushForInvalidInput(object input)
        {
            var converter = new BooleanToSolidColorBrushConverter() { Operation = ReducedBooleanOperation.Not };

            Assert.Same(Brushes.Transparent, converter.Convert(input, typeof(SolidColorBrush), null, null));
            converter.ValueForInvalid = Brushes.Gray;
            Assert.Same(Brushes.Gray, converter.Convert(input, typeof(SolidColorBrush), null, null));
            Assert.Same(Brushes.Gray, converter.Convert(DependencyProperty.UnsetValue, typeof(SolidColorBrush), null, null));
        }

        [Theory]
        [InlineData(ReducedBooleanOperation.None)]
        [InlineData(ReducedBooleanOperation.Not)]
        public void ParameterOverridesTrueBrushForBothConversionDirections(ReducedBooleanOperation operation)
        {
            var converter = new BooleanToSolidColorBrushConverter() { Operation = operation };
            var input = operation == ReducedBooleanOperation.None;

            Assert.Same(Brushes.Red, converter.Convert(input, typeof(SolidColorBrush), Brushes.Red, null));
            Assert.Same(Brushes.Transparent, converter.Convert(!input, typeof(SolidColorBrush), Brushes.Red, null));
            Assert.Equal(input, converter.ConvertBack(Brushes.Red, typeof(bool), Brushes.Red, null));
            Assert.Equal(!input, converter.ConvertBack(Brushes.Transparent, typeof(bool), Brushes.Red, null));
            Assert.Same(Brushes.Black, converter.Convert(input, typeof(SolidColorBrush), Colors.Red, null));
        }

        [Fact]
        public void ConvertBackUsesBrushIdentityRatherThanColorEquality()
        {
            var converter = new BooleanToSolidColorBrushConverter();

            Assert.Equal(true, converter.ConvertBack(Brushes.Black, typeof(bool), null, null));
            Assert.Equal(false, converter.ConvertBack(new SolidColorBrush(Colors.Black), typeof(bool), null, null));
            Assert.Equal(false, converter.ConvertBack(null, typeof(bool), null, null));
        }
    }
}
