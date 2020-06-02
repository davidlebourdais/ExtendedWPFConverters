using System.IO;
using System.Collections.Generic;
using Xunit;
using System.Drawing;
using System.Drawing.Imaging;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ImageToBitmapImageConverterTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { CreateBitmapAtRuntime(20, 10, Brushes.Blue) },
            new object[] { CreateBitmapAtRuntime(15, 15, Brushes.Green) },
            new object[] { Image.FromStream(CreateJpegStreamAtRuntime(10, 10, Brushes.Red)) },
            new object[] { "invalid" },
            new object[] { null }
        };

        private static Bitmap CreateBitmapAtRuntime(int size_x, int size_y, Brush brush)
        {
            var image = new Bitmap(size_x, size_y);
            Graphics.FromImage(image).FillRectangle(brush, 0, 0, size_x, size_y);
            return image;
        }

        private static Stream CreateJpegStreamAtRuntime(int size_x, int size_y, Brush brush)
        {
            var image = CreateBitmapAtRuntime(size_x, size_y, brush);
            var ms = new MemoryStream();  // no 'using' here, make sure stream is disposed during test.
            image.Save(ms, ImageFormat.Jpeg);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsImageToBitmapImage(object input)
        {
            var converter = new ImageToBitmapImageConverter();
            var result = converter.Convert(input, input?.GetType(), null, null);
            if (input is Image)
            {
                try
                {
                    Assert.IsType<System.Windows.Media.Imaging.BitmapImage>(result);
                    Assert.Equal((input as Image).Size.Height, (result as System.Windows.Media.Imaging.BitmapImage).Height);
                    Assert.Equal((input as Image).Size.Width, (result as System.Windows.Media.Imaging.BitmapImage).Width);
                }
                finally
                {
                    (input as Image).Dispose();
                }
            }
            else Assert.Null(result);
        }
    }
}
