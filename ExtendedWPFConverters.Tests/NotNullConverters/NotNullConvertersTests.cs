using Xunit;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class NotNullConvertersTests
    {
        #region NotNullToVisibilityConverter
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
        public void ConvertsNotNullToVisibility(object input, Visibility valueForNotNull, Visibility valueForNull)
        {
            var converter = new NotNullToVisibilityConverter() { ValueForNotNull = valueForNotNull, ValueForNull = valueForNull };
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(input != null ? valueForNotNull : valueForNull, result);
        }
        #endregion
    }
}
