using Xunit;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

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

        public static IEnumerable<object[]> NullOrBindingSentinelData
        {
            get
            {
                foreach (var input in new[] { null, DependencyProperty.UnsetValue, Binding.DoNothing })
                    foreach (var valueForNull in new[] { Visibility.Collapsed, Visibility.Hidden, Visibility.Visible })
                        yield return new object[] { input, valueForNull };
            }
        }

        [Theory]
        [MemberData(nameof(NullOrBindingSentinelData))]
        public void NullAndBindingSentinelsUseConfiguredNullVisibility(object input, Visibility valueForNull)
        {
            var converter = new NotNullToVisibilityConverter
            {
                ValueForNull = valueForNull,
                ValueForNotNull = valueForNull == Visibility.Visible ? Visibility.Hidden : Visibility.Visible
            };

            Assert.Equal(valueForNull, converter.Convert(input, typeof(Visibility), null, null));
        }

        [Fact]
        public void BindingSentinelsAreCollapsedByDefault()
        {
            var converter = new NotNullToVisibilityConverter();

            Assert.Equal(Visibility.Collapsed, converter.Convert(DependencyProperty.UnsetValue, typeof(Visibility), null, null));
            Assert.Equal(Visibility.Collapsed, converter.Convert(Binding.DoNothing, typeof(Visibility), null, null));
        }

        [Fact]
        public void OrdinaryObjectsAreNotSentinelsEvenWhenTheirEqualsMethodMatchesEverything()
        {
            var converter = new NotNullToVisibilityConverter
            {
                ValueForNull = Visibility.Hidden,
                ValueForNotNull = Visibility.Visible
            };

            Assert.Equal(Visibility.Visible, converter.Convert(new AlwaysEqualObject(), typeof(Visibility), null, null));
        }

        private sealed class AlwaysEqualObject
        {
            public override bool Equals(object obj) => true;
            public override int GetHashCode() => 0;
        }
        #endregion
    }
}
