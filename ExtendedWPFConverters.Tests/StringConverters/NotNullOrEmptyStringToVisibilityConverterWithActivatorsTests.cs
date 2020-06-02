using Xunit;
using System.Windows;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class NotNullOrEmptyStringToVisibilityConverterWithActivatorsTests
    {
        [Theory]
        [MemberData(nameof(NotNullOrEmptyStringToVisibilityConverterWithActivatorsTestDataProvider.ConvertTestDataForMultibinding), 
         MemberType= typeof(NotNullOrEmptyStringToVisibilityConverterWithActivatorsTestDataProvider))]
        public void ConvertsStringToVisibilityForMultibinding(object[] inputs, BooleanOperation operation, 
                                                              Visibility valueForNotNullOrEmpty, Visibility valueForNullOrEmpty, Visibility valueForInvalid, 
                                                              object expected)
        {
            var converter = new NotNullOrEmptyStringToVisibilityConverterWithActivators() 
            { 
                ValueForNotNullOrEmpty = valueForNotNullOrEmpty, 
                ValueForNullOrEmpty = valueForNullOrEmpty, 
                ValueForInvalid = valueForInvalid,
                ActivationOperation = operation
            };
            var result = converter.Convert(inputs, typeof(object), null, null);
            Assert.Equal(expected, result);
        }
    }
}
