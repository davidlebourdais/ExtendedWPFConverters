using System;
using System.Windows;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class DoubleToThicknessConverterTests
    {
        [Fact]
        public void ConvertsDoubleToUniformThickness()
        {
            const double input = 15d;

            var converter = new DoubleToThicknessConverter();
            var result = converter.Convert(input, typeof(object), null, null);

            Assert.Equal(new Thickness(15d, 15d, 15d, 15d), result);
        }

        [Fact]
        public void ConvertsInvalidValueToEmptyThickness()
        {
            var converter = new DoubleToThicknessConverter();
            var result = converter.Convert("invalid", typeof(object), null, null);

            Assert.Equal(new Thickness(), result);
        }

        [Fact]
        public void ConvertsUniformThicknessBackToDouble()
        {
            var input = new Thickness(15d, 15d, 15d, 15d);

            var converter = new DoubleToThicknessConverter() { ThrowOnNonUniformThickness = true };
            var result = converter.ConvertBack(input, typeof(object), null, null);

            Assert.Equal(15d, result);
        }

        [Fact]
        public void ThrowsWhenConvertsNonUniformThicknessBackToDouble()
        {
            var input = new Thickness(15d, 10d, 5d, 1d);

            var converter = new DoubleToThicknessConverter() { ThrowOnNonUniformThickness = true };

            Assert.Throws<ArgumentException>(() => converter.ConvertBack(input, typeof(object), null, null));
        }
    }
}
