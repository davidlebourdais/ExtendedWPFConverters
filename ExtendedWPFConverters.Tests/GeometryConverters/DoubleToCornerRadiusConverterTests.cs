using System;
using System.Windows;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class DoubleToCornerRadiusConverterTests
    {
        [Fact]
        public void ConvertsDoubleToUniformCornerRadius()
        {
            const double input = 15d;

            var converter = new DoubleToCornerRadiusConverter();
            var result = converter.Convert(input, typeof(object), null, null);

            Assert.Equal(new CornerRadius(15d, 15d, 15d, 15d), result);
        }

        [Fact]
        public void ConvertsInvalidValueToEmptyCornerRadius()
        {
            var converter = new DoubleToCornerRadiusConverter();
            var result = converter.Convert("invalid", typeof(object), null, null);

            Assert.Equal(new CornerRadius(), result);
        }

        [Fact]
        public void ConvertsUniformCornerRadiusBackToDouble()
        {
            var input = new CornerRadius(15d, 15d, 15d, 15d);

            var converter = new DoubleToCornerRadiusConverter() { ThrowOnNonUniformCornerRadius = true };
            var result = converter.ConvertBack(input, typeof(object), null, null);

            Assert.Equal(15d, result);
        }

        [Fact]
        public void ThrowsWhenConvertsNonUniformCornerRadiusBackToDouble()
        {
            var input = new CornerRadius(15d, 10d, 5d, 1d);

            var converter = new DoubleToCornerRadiusConverter() { ThrowOnNonUniformCornerRadius = true };

            Assert.Throws<ArgumentException>(() => converter.ConvertBack(input, typeof(object), null, null));
        }
    }
}
