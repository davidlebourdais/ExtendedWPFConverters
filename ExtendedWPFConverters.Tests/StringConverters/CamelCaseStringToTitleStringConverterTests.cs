using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class CamelCaseStringToTitleStringConverterTests
    {
        [Theory]
        [InlineData("SomeText", "Some Text")]
        [InlineData("someText", "Some Text")]
        [InlineData("Some", "Some")]
        [InlineData("   SomeText", "Some Text")]
        [InlineData("   Some  Text", "Some Text")]
        [InlineData("Some12Text", "Some 12 Text")]
        [InlineData("12SomeText", "12 Some Text")]
        [InlineData("", "")]
        [InlineData("  ", "")]
        [InlineData(null, null)]
        public void ConvertsCameCaseToTitleCase(string value, string expected)
        {
            var converter = new CamelCaseStringToTitleStringConverter();
            var result = converter.Convert(value, typeof(object), null, null);
            
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData("Some Text", "SomeText")]
        [InlineData("Some", "Some")]
        [InlineData("   Some Text", "SomeText")]
        [InlineData( "Some 12 Text", "Some12Text")]
        [InlineData("12 Some Text", "12SomeText")]
        [InlineData("", "")]
        [InlineData("  ", "")]
        [InlineData(null, null)]
        public void ConvertsTitleCaseToCameCase(string value, string expected)
        {
            var converter = new CamelCaseStringToTitleStringConverter();
            var result = converter.ConvertBack(value, typeof(object), null, null);
            
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData("Some Text", "someText")]
        [InlineData("Some", "some")]
        [InlineData("   Some Text", "someText")]
        [InlineData( "Some 12 Text", "some12Text")]
        [InlineData("12 Some Text", "12SomeText")]
        [InlineData("", "")]
        [InlineData("  ", "")]
        [InlineData(null, null)]
        public void ConvertsTitleCaseToCameCaseWithLowerCaseFirstLetter(string value, string expected)
        {
            var converter = new CamelCaseStringToTitleStringConverter { FirstLetterIsLowerCase = true };
            var result = converter.ConvertBack(value, typeof(object), null, null);
            
            Assert.Equal(expected, result);
        }
    }
}
