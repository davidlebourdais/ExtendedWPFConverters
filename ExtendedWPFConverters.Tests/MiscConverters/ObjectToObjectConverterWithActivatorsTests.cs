using Xunit;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ObjectToObjectConverterWithActivatorsTests
    {
        [Theory]
        [MemberData(nameof(ObjectToObjectWithActivatorsTestDataProvider.ConvertTestDataForMultibinding), MemberType= typeof(ObjectToObjectWithActivatorsTestDataProvider))]
        public void ConvertsStringToVisibilityForMultibinding(object[] inputs, BooleanOperation operation, object valueForInvalid, object expected)
        {
            var converter = new ObjectToObjectConverterWithActivators() 
            { 
                ValueForInvalid = valueForInvalid,
                ActivationOperation = operation
            };
            var result = converter.Convert(inputs, typeof(object), null, null);
            Assert.Equal(expected, result
            );
        }
    }
}
