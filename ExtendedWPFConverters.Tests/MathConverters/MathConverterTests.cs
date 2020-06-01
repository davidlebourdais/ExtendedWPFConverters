using System.Globalization;
using Xunit;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class MathConverterTests
    {
        [Theory]
        [MemberData(nameof(MathConverterTestDataProvider.ConvertTestData), MemberType= typeof(MathConverterTestDataProvider))]
        public void ConvertsWithMathOperation(object input, object parameter, CultureInfo culture, MathOperation operation, object valueForInvalid, bool output_as_string, object expected)
        {
            var converter = new MathConverter() { Operation = operation, OutputAsString = output_as_string, ValueForInvalid = valueForInvalid };
            var result = converter.Convert(input, input?.GetType(), parameter, culture);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(MathConverterTestDataProvider.ConvertBackTestData), MemberType= typeof(MathConverterTestDataProvider))]
        public void ConvertsBackWithMathOperation(object input, object parameter, CultureInfo culture, MathOperation operation, object valueForInvalid, object expected)
        {
            var converter = new MathConverter() { Operation = operation, ValueForInvalid = valueForInvalid };
            var result = converter.ConvertBack(input, input?.GetType(), parameter, culture);
            Assert.Equal(expected, result);
        }
    }
}
