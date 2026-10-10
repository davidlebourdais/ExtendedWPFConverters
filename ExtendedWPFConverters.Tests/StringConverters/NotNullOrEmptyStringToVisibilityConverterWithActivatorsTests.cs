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

        [Theory]
        [InlineData(Visibility.Collapsed)]
        [InlineData(Visibility.Hidden)]
        [InlineData(Visibility.Visible)]
        public void NullInputArrayReturnsConfiguredInvalidVisibility(Visibility valueForInvalid)
        {
            var otherVisibility = valueForInvalid == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            var converter = new NotNullOrEmptyStringToVisibilityConverterWithActivators
            {
                ValueForInvalid = valueForInvalid,
                ValueForNullOrEmpty = otherVisibility,
                ValueForNotNullOrEmpty = otherVisibility
            };

            Assert.Equal(valueForInvalid, converter.Convert(null, typeof(Visibility), null, null));
            Assert.Equal(valueForInvalid, converter.Convert(new object[0], typeof(Visibility), null, null));
            Assert.Equal(otherVisibility, converter.Convert(new object[] { "content" }, typeof(Visibility), null, null));
        }
    }
}
