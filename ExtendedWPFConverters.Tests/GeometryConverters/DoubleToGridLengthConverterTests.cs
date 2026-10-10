using System;
using System.Windows;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class DoubleToGridLengthConverterTests
    {
        [Fact]
        public void ConvertsDoubleToPixelGridLength()
        {
            const double input = 15d;

            var converter = new DoubleToGridLengthConverter() { GridUnitType = GridUnitType.Pixel };
            var result = converter.Convert(input, typeof(object), null, null);

            Assert.Equal(new GridLength(15d, GridUnitType.Pixel), result);
        }

        [Fact]
        public void ConvertsDoubleToStarGridLength()
        {
            const double input = 2d;

            var converter = new DoubleToGridLengthConverter() { GridUnitType = GridUnitType.Star };
            var result = converter.Convert(input, typeof(object), null, null);

            Assert.Equal(new GridLength(2d, GridUnitType.Star), result);
        }

        [Fact]
        public void ConvertsAnyDoubleToAutoGridLength()
        {
            var converter = new DoubleToGridLengthConverter() { GridUnitType = GridUnitType.Auto };
            var result = converter.Convert(15d, typeof(object), null, null);

            Assert.Equal(GridLength.Auto, result);
        }

        [Fact]
        public void ConvertsAbsoluteGridLengthBackToDouble()
        {
            var converter = new DoubleToGridLengthConverter() { ThrowOnNonAbsoluteGridLength = true };
            var result = converter.ConvertBack(new GridLength(15d, GridUnitType.Pixel), typeof(object), null, null);

            Assert.Equal(15d, result);
        }

        [Fact]
        public void ThrowsWhenConvertsNonAbsoluteGridLengthBackToDouble()
        {
            var converter = new DoubleToGridLengthConverter() { ThrowOnNonAbsoluteGridLength = true };

            Assert.Throws<ArgumentException>(() =>
                converter.ConvertBack(new GridLength(1d, GridUnitType.Star), typeof(object), null, null));
        }
    }
}
