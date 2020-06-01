using Xunit;
using System.Windows;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class StringToVisibilityConverterForMultibindingTests
    {
        [Theory]
        [MemberData(nameof(NotNullOrEmptyStringToVisibilityTestDataProvider.ConvertTestDataForMultibinding), MemberType= typeof(NotNullOrEmptyStringToVisibilityTestDataProvider))]
        public void ConvertsStringToVisibilityForMultibinding(object[] inputs, BooleanOperation operation, Visibility valueForNotNullOrEmpty, Visibility valueForNullOrEmpty, Visibility valueForInvalid, object expected)
        {
            var converter = new NotNullOrEmptyToVisibilityConverterForMultibinding() 
            { 
                ValueForNotNullOrEmpty = valueForNotNullOrEmpty, 
                ValueForNullOrEmpty = valueForNullOrEmpty, 
                ValueForInvalid = valueForInvalid,
                OperationForEnablers = operation
            };
            var result = converter.Convert(inputs, typeof(object), null, null);
            Assert.Equal(expected, result);
        }
    }
}
