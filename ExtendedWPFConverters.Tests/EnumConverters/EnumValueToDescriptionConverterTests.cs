using System;
using System.ComponentModel;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class EnumValueToDescriptionConverterTests
    {
        private enum TestEnum
        {
            [Description("With description")]
            WithDescription,
            WithoutDescription
        }
        
        [Fact]
        public void ConvertsEnumValueToDescription()
        {
            const TestEnum input = TestEnum.WithDescription;
            
            var converter = new EnumValueToDescriptionConverter();
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal("With description", result);
        }
        
        [Fact]
        public void ConvertsEnumValueToEmptyString()
        {
            const TestEnum input = TestEnum.WithoutDescription;
            
            var converter = new EnumValueToDescriptionConverter();
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(string.Empty, result);
        }
        
        [Fact]
        public void ConvertsDescriptionToEnumValue()
        {
            const string input = "With description";
            
            var converter = new EnumValueToDescriptionConverter();
            var result = converter.ConvertBack(input, typeof(TestEnum), null, null);
            
            Assert.Equal(TestEnum.WithDescription, result);
        }
        
        [Fact]
        public void ThrowsWhenConvertsDescriptionToEnumValue()
        {
            const string input = "Invalid description";
            
            var converter = new EnumValueToDescriptionConverter();
            
            Assert.Throws<ArgumentException>(() => 
                converter.ConvertBack(input, typeof(TestEnum), null, null));
        }
    }
}
