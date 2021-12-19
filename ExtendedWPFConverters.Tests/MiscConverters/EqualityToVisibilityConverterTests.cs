using System.Collections.Generic;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class EqualityToVisibilityConverterTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { 'a', 'a', true },
            new object[] { "string value", "string value", true },
            new object[] { 123, 123, true },
            new object[] { null, null, true },

            new object[] { 'a', 'b', false },
            new object[] { "string value", "other value", false },
            new object[] { 123, 5, false },
            new object[] { 123, "a", false },
            new object[] { null, 'a', false },
            new object[] { new List<double>(), new List<bool>(), false },
               new object[] { new List<double>(), new List<bool>(), false },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsEqualityToVisibility(object input, object parameter, bool expected)
        {
            var converter = new EqualityToVisibilityConverter();
            var expectedVisibility = expected ? converter.VisibilityForTrue : converter.VisibilityForFalse;
            var result = converter.Convert(input, null, parameter, null);
            Assert.Equal(expectedVisibility, result);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsInequalityToBoolean(object input, object parameter, bool expected)
        {
            var converter = new EqualityToVisibilityConverter() { TrueIfNotEqual = true };
            var expectedVisibility = expected ? converter.VisibilityForFalse : converter.VisibilityForTrue;
            var result = converter.Convert(input, null, parameter, null);
            Assert.Equal(expectedVisibility, result);
        }
    }
}
