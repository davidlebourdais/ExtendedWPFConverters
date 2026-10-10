using System.Windows;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class NotNullOrWhiteSpaceStringConverterTests
    {
        #region NotNullOrWhiteSpaceStringToBooleanConverter
        [Theory]
        [InlineData("not null", true, false)]
        [InlineData("  ", true, false)]
        [InlineData("\t", true, false)]
        [InlineData("\r\n", false, true)]
        [InlineData(" text ", false, true)]
        [InlineData(null, true, false)]
        [InlineData(123, true, false)]
        public void ConvertsNotNullOrWhiteSpaceStringToBoolean(object input, bool valueForNotNullOrWhiteSpace, bool valueForNullOrWhiteSpace)
        {
            var converter = new NotNullOrWhiteSpaceStringToBooleanConverter()
            {
                ValueForNotNullOrWhiteSpace = valueForNotNullOrWhiteSpace,
                ValueForNullOrWhiteSpace = valueForNullOrWhiteSpace
            };

            var result = converter.Convert(input, typeof(string), null, null);

            Assert.Equal(!string.IsNullOrWhiteSpace(input as string) ? valueForNotNullOrWhiteSpace : valueForNullOrWhiteSpace, result);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, false, true)]
        [InlineData("invalid", true, false)]
        public void ConvertsBooleanBackToNotNullOrWhiteSpaceString(object input, bool valueForNotNullOrWhiteSpace, bool valueForNullOrWhiteSpace)
        {
            var converter = new NotNullOrWhiteSpaceStringToBooleanConverter()
            {
                ValueForNotNullOrWhiteSpace = valueForNotNullOrWhiteSpace,
                ValueForNullOrWhiteSpace = valueForNullOrWhiteSpace
            };

            var result = converter.ConvertBack(input, typeof(bool), null, null);
            if (input as bool? == valueForNotNullOrWhiteSpace)
            {
                Assert.NotNull(result as string);
                Assert.NotEmpty(result as string);
            }
            else
            {
                Assert.Null(result);
            }
        }
        #endregion

        #region NotNullOrWhiteSpaceStringToVisibilityConverter
        [Theory]
        [InlineData("not null", Visibility.Visible, Visibility.Collapsed)]
        [InlineData("  ", Visibility.Visible, Visibility.Collapsed)]
        [InlineData("\t", Visibility.Hidden, Visibility.Collapsed)]
        [InlineData(null, Visibility.Visible, Visibility.Hidden)]
        [InlineData(123, Visibility.Visible, Visibility.Hidden)]
        public void ConvertsNotNullOrWhiteSpaceStringToVisibility(object input, Visibility valueForNotNullOrWhiteSpace, Visibility valueForNullOrWhiteSpace)
        {
            var converter = new NotNullOrWhiteSpaceStringToVisibilityConverter()
            {
                ValueForNotNullOrWhiteSpace = valueForNotNullOrWhiteSpace,
                ValueForNullOrWhiteSpace = valueForNullOrWhiteSpace
            };

            var result = converter.Convert(input, typeof(string), null, null);

            Assert.Equal(!string.IsNullOrWhiteSpace(input as string) ? valueForNotNullOrWhiteSpace : valueForNullOrWhiteSpace, result);
        }

        [Theory]
        [InlineData(Visibility.Visible, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(Visibility.Hidden, Visibility.Hidden, Visibility.Collapsed)]
        [InlineData(Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed)]
        [InlineData("invalid", Visibility.Visible, Visibility.Collapsed)]
        public void ConvertsVisibilityBackToNotNullOrWhiteSpaceString(object input, Visibility valueForNotNullOrWhiteSpace, Visibility valueForNullOrWhiteSpace)
        {
            var converter = new NotNullOrWhiteSpaceStringToVisibilityConverter()
            {
                ValueForNotNullOrWhiteSpace = valueForNotNullOrWhiteSpace,
                ValueForNullOrWhiteSpace = valueForNullOrWhiteSpace
            };

            var result = converter.ConvertBack(input, typeof(Visibility), null, null);
            if (input as Visibility? == valueForNotNullOrWhiteSpace)
            {
                Assert.NotNull(result as string);
                Assert.NotEmpty(result as string);
            }
            else
            {
                Assert.Null(result);
            }
        }
        #endregion
    }
}
