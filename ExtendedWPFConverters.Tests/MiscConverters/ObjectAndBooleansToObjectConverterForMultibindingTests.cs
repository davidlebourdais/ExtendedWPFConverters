using Xunit;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ObjectAndBooleansToObjectConverterForMultibindingTests
    {
        [Theory]
        [MemberData(nameof(ObjectAndBooleanToObjectConverterTestDataProvider.ConvertTestDataForMultibinding), MemberType= typeof(ObjectAndBooleanToObjectConverterTestDataProvider))]
        public void ConvertsStringToVisibilityForMultibinding(object[] inputs, BooleanOperation operation, object valueForInvalid, object expected)
        {
            var converter = new ObjectAndBooleansToObjectConverterForMultibinding() 
            { 
                ValueForInvalid = valueForInvalid,
                OperationForEnablers = operation
            };
            var result = converter.Convert(inputs, typeof(object), null, null);
            Assert.Equal(expected, result
            );
        }
    }
}
