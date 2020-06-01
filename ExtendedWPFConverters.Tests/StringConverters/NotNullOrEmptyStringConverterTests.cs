using Xunit;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class NotNullOrEmptyStringConverterTests
    {
        #region NotNullOrEmptyStringToBooleanConverter
        [Theory]
        [InlineData("not null", true, false)]
        [InlineData("not null", false, true)]
        [InlineData("not null", false, false)]
        [InlineData("not null", true, true)]
        [InlineData(true, true, false)]
        [InlineData(123, true, false)]
        [InlineData(null, true, false)]
        [InlineData(null, false, true)]
        public void ConvertsNotNullOrEmptyStringToBoolean(object input, bool valueForNotNullOrEmpty, bool valueForNullOrEmpty)
        {
            var converter = new NotNullOrEmptyStringToBooleanConverter() { ValueForNotNullOrEmpty = valueForNotNullOrEmpty, ValueForNullOrEmpty = valueForNullOrEmpty };
            var result = converter.Convert(input, typeof(string), null, null);
            if (!string.IsNullOrEmpty(input as string))
                Assert.Equal(valueForNotNullOrEmpty, result);
            else
                Assert.Equal(valueForNullOrEmpty, result);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, false, true)]
        [InlineData("invalid", true, false)]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        public void ConvertsBooleanBackToNotNullOrEmptyString(object input, bool valueForNotNullOrEmpty, bool valueForNullOrEmpty)
        {
            var converter = new NotNullOrEmptyStringToBooleanConverter() { ValueForNotNullOrEmpty = valueForNotNullOrEmpty, ValueForNullOrEmpty = valueForNullOrEmpty };
            var result = converter.ConvertBack(input, typeof(bool), null, null);
            if ((input as bool?) == valueForNotNullOrEmpty)
                Assert.NotEmpty(result as string);
            else
                Assert.Null(result);
        }
        #endregion

        #region NotNullOrEmptyStringToVisibilityConverter
        [Theory]
        [InlineData("not null", Visibility.Visible, Visibility.Collapsed)]
        [InlineData("not null", Visibility.Visible, Visibility.Hidden)]
        [InlineData("not null", Visibility.Collapsed, Visibility.Visible)]
        [InlineData("not null", Visibility.Collapsed, Visibility.Hidden)]
        [InlineData("not null", Visibility.Hidden, Visibility.Visible)]
        [InlineData("not null", Visibility.Hidden, Visibility.Collapsed)]
        [InlineData("not null", Visibility.Visible, Visibility.Visible)]
        [InlineData(true, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(123, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(null, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(null, Visibility.Visible, Visibility.Hidden)]
        [InlineData(null, Visibility.Collapsed, Visibility.Visible)]
        [InlineData(null, Visibility.Collapsed, Visibility.Hidden)]
        [InlineData(null, Visibility.Hidden, Visibility.Visible)]
        [InlineData(null, Visibility.Hidden, Visibility.Collapsed)]
        [InlineData(null, Visibility.Visible, Visibility.Visible)]
        public void ConvertsNotNullOrEmptyStringToVisibility(object input, Visibility valueForNotNullOrEmpty, Visibility valueForNullOrEmpty)
        {
            var converter = new NotNullOrEmptyStringToVisibilityConverter() { ValueForNotNullOrEmpty = valueForNotNullOrEmpty, ValueForNullOrEmpty = valueForNullOrEmpty };
            var result = converter.Convert(input, typeof(string), null, null);
            if (!string.IsNullOrEmpty(input as string))
                Assert.Equal(valueForNotNullOrEmpty, result);
            else
                Assert.Equal(valueForNullOrEmpty, result);
        }

        [Theory]
        [InlineData(Visibility.Visible, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(Visibility.Hidden, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed)]
        [InlineData("invalid", Visibility.Visible, Visibility.Collapsed)]
        [InlineData(null, Visibility.Visible, Visibility.Collapsed)]
        [InlineData(Visibility.Visible, Visibility.Collapsed, Visibility.Visible)]
        [InlineData(Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible)]
        [InlineData(Visibility.Visible, Visibility.Visible, Visibility.Visible)]
        [InlineData(Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed)]
        public void ConvertsVisibilityBackToNotNullOrEmptyString(object input, Visibility valueForNotNullOrEmpty, Visibility valueForNullOrEmpty)
        {
            var converter = new NotNullOrEmptyStringToVisibilityConverter() { ValueForNotNullOrEmpty = valueForNotNullOrEmpty, ValueForNullOrEmpty = valueForNullOrEmpty };
            var result = converter.ConvertBack(input, typeof(Visibility), null, null);
            if ((input as Visibility?) == valueForNotNullOrEmpty)
                Assert.NotEmpty(result as string);
            else
                Assert.Null(result);
        }
        #endregion
    }
}
