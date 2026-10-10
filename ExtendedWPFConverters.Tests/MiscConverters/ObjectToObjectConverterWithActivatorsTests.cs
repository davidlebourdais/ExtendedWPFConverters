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
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NullInputArrayReturnsDefaultOrConfiguredInvalidFallback()
        {
            var converter = new ObjectToObjectConverterWithActivators();
            Assert.Null(converter.Convert(null, typeof(object), null, null));

            var fallback = new object();
            converter.ValueForInvalid = fallback;
            Assert.Same(fallback, converter.Convert(null, typeof(object), null, null));

            // An empty array keeps its existing null result, distinct from a missing array.
            Assert.Null(converter.Convert(new object[0], typeof(object), null, null));
        }
    }
}
