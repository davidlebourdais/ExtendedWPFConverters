using Xunit;
using System.Windows;
using System.Globalization;
using System.Collections.Generic;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class StringToVerticalAlignmentConverterTests
    {
        #region Culture base translations for tests
        private static readonly Dictionary<string, string> _translations = new Dictionary<string, string>()
        {
            { "Top", "Haut" },
            { "Bottom", "Bas" },
            { "Center", "Centre" },
            { "Stretch", "Etire" },
        };
        private static readonly CultureInfo _culture = new CultureInfo("fr-FR");
        #endregion

        #region StringToVerticalAlignmentConverter
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { "Bottom", VerticalAlignment.Bottom, null, null },
            new object[] { "Stretch", VerticalAlignment.Stretch, null, null },
            new object[] { "invalid", null, null, null },
            new object[] { 1, null, null, null },
            new object[] { null, null, null, null },
            new object[] { "Haut", null, new TranslationFetcherProvider(_translations, _culture).FetchMethod, null },
            new object[] { "Haut", null, new TranslationFetcherProvider(_translations, _culture).FetchMethodWithCulture, new CultureInfo("en-US") },
            new object[] { "Haut", VerticalAlignment.Top, new TranslationFetcherProvider(_translations, _culture).FetchMethodWithCulture, new CultureInfo("fr-FR") },
            new object[] { "Haut", VerticalAlignment.Top, new TranslationFetcherProvider(_translations, _culture).FetchDictionary, new CultureInfo("fr-FR") },
            new object[] { "Haut", VerticalAlignment.Top, new TranslationFetcherProvider(_translations, _culture).TranslationDictionary, new CultureInfo("fr-FR") },
            new object[] { "Haut", VerticalAlignment.Top, new TranslationFetcherProvider(_translations, _culture).TranslationDictionaryWithCulture, new CultureInfo("fr-FR") },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsStringToVerticalAlignment(object input, object expected, object parameter, CultureInfo culture)
        {
            var converter = new StringToVerticalAlignmentConverter();
            var result = converter.Convert(input, typeof(string), parameter, culture);
            
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ConvertBackData => new List<object[]>
        {
            new object[] { VerticalAlignment.Top, "Top", null, null },
            new object[] { VerticalAlignment.Center, "Center", null, null },
            new object[] { null, "", null, null },
            new object[] { 123, "", null, null },
            new object[] { VerticalAlignment.Bottom, "Bas", new TranslationFetcherProvider(_translations, _culture).BidirectionalFetchMethod, null },
            new object[] { VerticalAlignment.Bottom, "", new TranslationFetcherProvider(_translations, _culture).FetchMethod, null },
            new object[] { VerticalAlignment.Bottom, "Bas", new TranslationFetcherProvider(_translations, _culture).BidirectionalFetchMethodWithCulture, new CultureInfo("fr-FR") },
            new object[] { VerticalAlignment.Bottom, "", new TranslationFetcherProvider(_translations, _culture).BidirectionalFetchMethodWithCulture, new CultureInfo("en-US") },
            new object[] { VerticalAlignment.Bottom, "Bas", new TranslationFetcherProvider(_translations, _culture).BidirectionalFetchDictionary, new CultureInfo("fr-FR") },
            new object[] { VerticalAlignment.Bottom, "Bas", new TranslationFetcherProvider(_translations, _culture).BidirectionalFetchDictionaryWithCulture, new CultureInfo("fr-FR") },
            new object[] { VerticalAlignment.Bottom, "Bas", new TranslationFetcherProvider(_translations, _culture).TranslationDictionary, new CultureInfo("fr-FR") },
            new object[] { VerticalAlignment.Bottom, "Bas", new TranslationFetcherProvider(_translations, _culture).TranslationDictionaryWithCulture, new CultureInfo("fr-FR") },
        };

        [Theory]
        [MemberData(nameof(ConvertBackData))]
        public void ConvertsVerticalAlignmentBackToString(object input, string expected, object parameter, CultureInfo culture)
        {
            var converter = new StringToVerticalAlignmentConverter();
            var result = converter.ConvertBack(input, input?.GetType(), parameter, culture);
            
            Assert.Equal(expected, result);
        }
        #endregion
    }
}
