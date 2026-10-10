using System.Collections.Generic;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ComparisonToBooleanConverterTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { 5, 5, ComparisonOperation.Equal, true },
            new object[] { 5, "5", ComparisonOperation.Equal, true },
            new object[] { 1000m, "1,000", ComparisonOperation.Equal, true },
            new object[] { 1000d, "1,000", ComparisonOperation.Equal, true },
            new object[] { 5, 4, ComparisonOperation.NotEqual, true },
            new object[] { 5, 4, ComparisonOperation.GreaterThan, true },
            new object[] { 5, "4", ComparisonOperation.GreaterThanOrEqual, true },
            new object[] { 5, 6, ComparisonOperation.LessThan, true },
            new object[] { 5, 5, ComparisonOperation.LessThanOrEqual, true },
            new object[] { 5, 5.4d, ComparisonOperation.Equal, false },
            new object[] { 5.4d, 5, ComparisonOperation.Equal, false },
            new object[] { 5, 5.4d, ComparisonOperation.LessThan, true },
            new object[] { 5.4d, 5, ComparisonOperation.GreaterThan, true },
            new object[] { 5, 5.4m, ComparisonOperation.NotEqual, true },
            new object[] { 5.4m, 5, ComparisonOperation.GreaterThanOrEqual, true },
            new object[] { 5, "5.4", ComparisonOperation.Equal, false },
            new object[] { 10, "9.5", ComparisonOperation.LessThan, false },
            new object[] { "9.5", 10, ComparisonOperation.LessThan, true },
            new object[] { "10", "9.5", ComparisonOperation.LessThan, true },
            new object[] { 9007199254740993L, 9007199254740992d, ComparisonOperation.Equal, false },
            new object[] { 9007199254740993L, 9007199254740992d, ComparisonOperation.GreaterThan, true },
            new object[] { 9007199254740992d, 9007199254740993L, ComparisonOperation.LessThan, true },
            new object[] { 9007199254740992L, 9007199254740992d, ComparisonOperation.Equal, true },
            new object[] { ulong.MaxValue, long.MaxValue, ComparisonOperation.GreaterThan, true },
            new object[] { decimal.MaxValue, ulong.MaxValue, ComparisonOperation.GreaterThan, true },
            new object[] { 9007199254740993L, "9007199254740993", ComparisonOperation.Equal, true },
            new object[] { 0, "1e-30", ComparisonOperation.LessThan, true },
            new object[] { 10, "1e100", ComparisonOperation.LessThan, true },
            new object[] { 0.1m, 0.1d, ComparisonOperation.LessThan, true },
            new object[] { 0.1f, 0.1d, ComparisonOperation.GreaterThan, true },
            new object[] { 0.1d, "0.1", ComparisonOperation.Equal, true },
            new object[] { "0.1", 0.1d, ComparisonOperation.Equal, true },
            new object[] { 0.1f, "0.1", ComparisonOperation.Equal, true },
            new object[] { double.Epsilon, 0m, ComparisonOperation.GreaterThan, true },
            new object[] { double.PositiveInfinity, decimal.MaxValue, ComparisonOperation.GreaterThan, true },
            new object[] { double.NegativeInfinity, decimal.MinValue, ComparisonOperation.LessThan, true },
            new object[] { double.NaN, 0, ComparisonOperation.LessThan, true },
            new object[] { "beta", "alpha", ComparisonOperation.GreaterThan, true },
            new object[] { "alpha", "beta", ComparisonOperation.GreaterThan, false },
            new object[] { new List<int>(), 1, ComparisonOperation.GreaterThan, false }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsComparisonToBoolean(object input, object parameter, ComparisonOperation operation, bool expected)
        {
            var converter = new ComparisonToBooleanConverter() { Operation = operation };
            var result = converter.Convert(input, null, parameter, null);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, "invalid")]
        [InlineData("invalid", 10)]
        [InlineData(10, true)]
        [InlineData(true, 10)]
        [InlineData(10, "1e999999999999999999999")]
        public void ReturnsConfiguredInvalidValueForInvalidNumericComparison(object input, object parameter)
        {
            var converter = new ComparisonToBooleanConverter()
            {
                Operation = ComparisonOperation.LessThan,
                ValueForInvalid = true
            };

            Assert.True((bool)converter.Convert(input, typeof(bool), parameter, null));
        }

        [Fact]
        public void ReturnsConfiguredInvalidValueForUnsupportedComparison()
        {
            var converter = new ComparisonToBooleanConverter()
            {
                Operation = ComparisonOperation.GreaterThan,
                ValueForInvalid = true
            };

            var result = converter.Convert(new List<int>(), null, 2, null);
            Assert.True((bool)result);
        }
    }
}
