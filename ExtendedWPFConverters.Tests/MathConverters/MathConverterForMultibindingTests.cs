using System.Globalization;
using Xunit;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class MathConverterForMultibindingTests
    {
        [Theory]
        [MemberData(nameof(MathConverterTestDataProvider.ConvertTestDataForMultibinding), MemberType= typeof(MathConverterTestDataProvider))]
        public void ConvertsWithMathOperation(object[] inputs, CultureInfo culture, MathOperation operation, object valueForInvalid, bool outputAsString, object expected)
        {
            var converter = new MathConverterForMultibinding() { Operation = operation, OutputAsString = outputAsString, ValueForInvalid = valueForInvalid };
            var result = converter.Convert(inputs, null, null, culture);
            Assert.Equal(expected, result);
        }
    }
}
