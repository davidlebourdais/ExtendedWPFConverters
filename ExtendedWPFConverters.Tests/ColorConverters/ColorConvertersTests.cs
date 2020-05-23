using Xunit;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ColorConvertersTests
    {
        #region ColorToSolidColorBrushConverter
        public static IEnumerable<object[]> ColorToSolidColorBrushData => new List<object[]>
        {
            new object[] { Colors.Blue, Brushes.Transparent },
            new object[] { Colors.Red, Brushes.Transparent },
            new object[] { Colors.Green, Brushes.Transparent },
            new object[] { Colors.Transparent, Brushes.Transparent },
            new object[] { Colors.Blue, Brushes.Red },
            new object[] { Colors.Red, Brushes.Green },
            new object[] { Colors.Green, Brushes.Blue },
            new object[] { Colors.Transparent, Brushes.Transparent },
            new object[] { null, Brushes.Transparent },
            new object[] { "invalid", Brushes.Transparent },
            new object[] { null, Brushes.Blue },
            new object[] { "invalid", Brushes.Blue },
        };

        [Theory]
        [MemberData(nameof(ColorToSolidColorBrushData))]
        public void ConvertsColorToSolidColorBrush(object input, SolidColorBrush defaultBrush)
        {
            var converter = new ColorToSolidColorBrushConverter() { Default = defaultBrush };
            var result = converter.Convert(input, typeof(Color), null, null);
            if (input is Color)
                Assert.Equal(input as Color?, (result as SolidColorBrush).Color);
            else
                Assert.Equal(defaultBrush, result);
        }

        [Fact]
        public void ConvertsAnySolidColorBrushBackToColor()
        {
            var converter = new ColorToSolidColorBrushConverter(); 
            typeof(Brushes).GetProperties().Select(x => x.GetValue(null) as SolidColorBrush).ToList().ForEach(brush => 
            {
                var result = converter.ConvertBack(brush, typeof(SolidColorBrush), null, null);
                Assert.Equal(brush.Color, result);
            });
        }
        #endregion
    }
}
