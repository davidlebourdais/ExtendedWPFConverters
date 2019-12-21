using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Markup;

namespace EMA.View.Converters
{
    /// Imported and adapted by David from: https://www.codeproject.com/Tips/517457/Simple-Way-to-Bind-an-Image-Class-as-Source-to-Ima

    /// <summary>
    /// Converts an image to an ImageSource for WPF's Image control bindings.
    /// </summary>
    public class ImageToSourceConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Image image)
            {
                var ms = new MemoryStream();
                try
                {
                    image.Save(ms, image.RawFormat);
                }
                catch  // in case the raw format is not supported (no defined encoder with the image)
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                ms.Seek(0, SeekOrigin.Begin);
                var bi = new BitmapImage
                {
                    CacheOption = BitmapCacheOption.OnLoad
                };
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                return bi;
            }
            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
