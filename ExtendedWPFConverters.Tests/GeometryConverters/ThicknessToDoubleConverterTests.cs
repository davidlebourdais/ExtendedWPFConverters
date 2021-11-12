using System;
using Xunit;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ThicknessToDoubleConverterTests
    {
        [Fact]
        public void ConvertsUniformThicknessToDouble()
        {
            var input = new Thickness(15d, 15d, 15d, 15d);
            
            var converter = new ThicknessToDoubleConverter { ThrowOnNonUniformThickness = false };
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(15d, result);
        }
        
        [Fact]
        public void ConvertsNonUniformThicknessToDouble()
        {
            var input = new Thickness(15d, 1d, 5d, 20d);
            
            var converter = new ThicknessToDoubleConverter { ThrowOnNonUniformThickness = false };
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(15d, result);
        }
        
        [Fact]
        public void ThrowWhenConvertsNonUniformThicknessToDouble()
        {
            var input = new Thickness(15d, 1d, 5d, 20d);
            
            var converter = new ThicknessToDoubleConverter { ThrowOnNonUniformThickness = true };

            Assert.Throws<ArgumentException>(() => 
                converter.Convert(input, typeof(object), null, null));
        }
        
        [Fact]
        public void ConvertsDoubleToUniformThickness()
        {
            const double input = 15d;
            
            var converter = new ThicknessToDoubleConverter { ThrowOnNonUniformThickness = true };
            var result = converter.ConvertBack(input, typeof(object), null, null);
            
            Assert.Equal(new Thickness(15d, 15d, 15d, 15d), result);
        }
    }
}
