using System.IO;
using System.Collections.Generic;
using Xunit;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

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

        private static Bitmap CreateBitmapAtRuntime(int sizeX, int sizeY, Brush brush)
        {
            var image = new Bitmap(sizeX, sizeY);
            Graphics.FromImage(image).FillRectangle(brush, 0, 0, sizeX, sizeY);
            return image;
        }

        private static Stream CreateJpegStreamAtRuntime(int sizeX, int sizeY, Brush brush)
        {
            var image = CreateBitmapAtRuntime(sizeX, sizeY, brush);
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
            if (input is Image image)
            {
                try
                {
                    Assert.IsType<BitmapImage>(result);
                    Assert.Equal(image.Size.Height, ((BitmapImage)result).Height);
                    Assert.Equal(image.Size.Width, ((BitmapImage)result).Width);
                }
                finally
                {
                    image.Dispose();
                }
            }
            else
                Assert.Null(result);
        }
    }
}
