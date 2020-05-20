using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Markup;

namespace EMA.ExtendedWPFConverters
{
    /// Imported and adapted by David from: https://www.codeproject.com/Tips/517457/Simple-Way-to-Bind-an-Image-Class-as-Source-to-Ima

    /// <summary>
    /// Converts an image to an ImageSource for WPF's Image control bindings.
    /// </summary>
    public class ImageToSourceConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Image"/> into a <see cref="BitmapImage"/>.
        /// </summary>
        /// <param name="value">A <see cref="Image"/> source entry.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>A <see cref="BitmapImage"/> ready to be rendered.</returns>
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

        /// <summary>
        /// Unsupported conversion method.
        /// </summary>
        /// <param name="value">Unused.</param>
        /// <param name="targetType">Unused.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotSupportedException">Thrown if this method is called.</exception>
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
