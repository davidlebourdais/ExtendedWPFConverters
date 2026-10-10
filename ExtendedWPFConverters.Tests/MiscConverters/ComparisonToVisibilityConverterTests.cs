using System.Collections.Generic;
using System.Windows;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ComparisonToVisibilityConverterTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { 5, 4, ComparisonOperation.GreaterThan, true },
            new object[] { 5, "5", ComparisonOperation.Equal, true },
            new object[] { 5, 6, ComparisonOperation.GreaterThanOrEqual, false },
            new object[] { 5, 5.4d, ComparisonOperation.Equal, false },
            new object[] { 5.4d, 5, ComparisonOperation.Equal, false },
            new object[] { 5, 5.4m, ComparisonOperation.LessThan, true },
            new object[] { 10, "9.5", ComparisonOperation.LessThan, false },
            new object[] { "9.5", 10, ComparisonOperation.LessThan, true },
            new object[] { 9007199254740993L, 9007199254740992d, ComparisonOperation.GreaterThan, true },
            new object[] { "alpha", "beta", ComparisonOperation.LessThan, true },
            new object[] { "alpha", "beta", ComparisonOperation.GreaterThan, false }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsComparisonToVisibility(object input, object parameter, ComparisonOperation operation, bool expected)
        {
            var converter = new ComparisonToVisibilityConverter()
            {
                Operation = operation,
                VisibilityForTrue = Visibility.Visible,
                VisibilityForFalse = Visibility.Hidden
            };

            var result = converter.Convert(input, null, parameter, null);
            Assert.Equal(expected ? Visibility.Visible : Visibility.Hidden, result);
        }

        [Theory]
        [InlineData(10, "invalid")]
        [InlineData("invalid", 10)]
        public void ReturnsConfiguredInvalidVisibilityForInvalidNumericComparison(object input, object parameter)
        {
            var converter = new ComparisonToVisibilityConverter()
            {
                Operation = ComparisonOperation.GreaterThan,
                VisibilityForTrue = Visibility.Visible,
                VisibilityForFalse = Visibility.Collapsed,
                ValueForInvalid = Visibility.Hidden
            };

            Assert.Equal(Visibility.Hidden, converter.Convert(input, typeof(Visibility), parameter, null));
        }

        [Fact]
        public void ReturnsConfiguredInvalidVisibilityForUnsupportedComparison()
        {
            var converter = new ComparisonToVisibilityConverter()
            {
                Operation = ComparisonOperation.GreaterThan,
                ValueForInvalid = Visibility.Hidden
            };

            var result = converter.Convert(new List<int>(), null, 1, null);
            Assert.Equal(Visibility.Hidden, result);
        }
    }
}
