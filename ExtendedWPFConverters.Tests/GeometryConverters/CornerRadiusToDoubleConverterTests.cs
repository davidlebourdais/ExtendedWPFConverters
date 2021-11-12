using System;
using Xunit;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class CornerRadiusToDoubleConverterTests
    {
        [Fact]
        public void ConvertsUniformCornerRadiusToDouble()
        {
            var input = new CornerRadius(15d, 15d, 15d, 15d);
            
            var converter = new CornerRadiusToDoubleConverter { ThrowOnNonUniformCornerRadius = false };
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(15d, result);
        }
        
        [Fact]
        public void ConvertsNonUniformCornerRadiusToDouble()
        {
            var input = new CornerRadius(15d, 1d, 5d, 20d);
            
            var converter = new CornerRadiusToDoubleConverter { ThrowOnNonUniformCornerRadius = false };
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(15d, result);
        }
        
        [Fact]
        public void ThrowWhenConvertsNonUniformCornerRadiusToDouble()
        {
            var input = new CornerRadius(15d, 1d, 5d, 20d);
            
            var converter = new CornerRadiusToDoubleConverter { ThrowOnNonUniformCornerRadius = true };

            Assert.Throws<ArgumentException>(() => 
                converter.Convert(input, typeof(object), null, null));
        }
        
        [Fact]
        public void ConvertsDoubleToUniformCornerRadius()
        {
            const double input = 15d;
            
            var converter = new CornerRadiusToDoubleConverter { ThrowOnNonUniformCornerRadius = true };
            var result = converter.ConvertBack(input, typeof(object), null, null);
            
            Assert.Equal(new CornerRadius(15d, 15d, 15d, 15d), result);
        }
    }
}
