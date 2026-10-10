using System;
using System.Windows;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class GridLengthToDoubleConverterTests
    {
        [Fact]
        public void ConvertsAbsoluteGridLengthToDouble()
        {
            var converter = new GridLengthToDoubleConverter() { ThrowOnNonAbsoluteGridLength = true };
            var result = converter.Convert(new GridLength(15d, GridUnitType.Pixel), typeof(object), null, null);

            Assert.Equal(15d, result);
        }

        [Fact]
        public void ConvertsNonAbsoluteGridLengthToValueWhenAllowed()
        {
            var converter = new GridLengthToDoubleConverter() { ThrowOnNonAbsoluteGridLength = false };
            var result = converter.Convert(new GridLength(2d, GridUnitType.Star), typeof(object), null, null);

            Assert.Equal(2d, result);
        }

        [Fact]
        public void ThrowsWhenConvertsAutoGridLengthToDoubleInStrictMode()
        {
            var converter = new GridLengthToDoubleConverter() { ThrowOnNonAbsoluteGridLength = true };

            Assert.Throws<ArgumentException>(() =>
                converter.Convert(GridLength.Auto, typeof(object), null, null));
        }

        [Fact]
        public void ConvertsDoubleBackToConfiguredGridLength()
        {
            var converter = new GridLengthToDoubleConverter() { GridUnitType = GridUnitType.Star };
            var result = converter.ConvertBack(3d, typeof(object), null, null);

            Assert.Equal(new GridLength(3d, GridUnitType.Star), result);
        }

        [Fact]
        public void ConvertsDoubleBackToAutoGridLength()
        {
            var converter = new GridLengthToDoubleConverter() { GridUnitType = GridUnitType.Auto };
            var result = converter.ConvertBack(3d, typeof(object), null, null);

            Assert.Equal(GridLength.Auto, result);
        }
    }
}
