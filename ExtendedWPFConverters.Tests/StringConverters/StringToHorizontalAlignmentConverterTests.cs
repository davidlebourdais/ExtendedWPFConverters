using Xunit;
using System.Windows;
using System.Globalization;
using System.Collections.Generic;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class StringToHorizontalAlignmentConverterTests
    {
        #region Culture base translations for tests
        private static readonly Dictionary<string, string> translations = new Dictionary<string, string>()
        {
            { "Left", "Gauche" },
            { "Right", "Droite" },
            { "Center", "Centre" },
            { "Stretch", "Etire" },
        };
        private static readonly CultureInfo culture = new CultureInfo("fr-FR");
        #endregion

        #region StringToHorizontalAlignmentConverter
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { "Left", HorizontalAlignment.Left, null, null },
            new object[] { "Stretch", HorizontalAlignment.Stretch, null, null },
            new object[] { "invalid", null, null, null },
            new object[] { 1, null, null, null },
            new object[] { null, null, null, null },
            new object[] { "Droite", null, new TranslationFetcherProvider(translations, culture).FetchMethod, null },
            new object[] { "Droite", null, new TranslationFetcherProvider(translations, culture).FetchMethodWithCulture, new CultureInfo("en-US") },
            new object[] { "Droite", HorizontalAlignment.Right, new TranslationFetcherProvider(translations, culture).FetchMethodWithCulture, new CultureInfo("fr-FR") },
            new object[] { "Droite", HorizontalAlignment.Right, new TranslationFetcherProvider(translations, culture).FetchDictionary, new CultureInfo("fr-FR") },
            new object[] { "Droite", HorizontalAlignment.Right, new TranslationFetcherProvider(translations, culture).TranslationDictionary, new CultureInfo("fr-FR") },
            new object[] { "Droite", HorizontalAlignment.Right, new TranslationFetcherProvider(translations, culture).TranslationDictionaryWithCulture, new CultureInfo("fr-FR") },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsStringToHorizontalAlignment(object input, object expected, object parameter, CultureInfo culture)
        {
            var converter = new StringToHorizontalAlignmentConverter();
            var result = converter.Convert(input, typeof(string), parameter, culture);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ConvertBackData => new List<object[]>
        {
            new object[] { HorizontalAlignment.Right, "Right", null, null },
            new object[] { HorizontalAlignment.Center, "Center", null, null },
            new object[] { null, "", null, null },
            new object[] { 123, "", null, null },
            new object[] { HorizontalAlignment.Left, "Gauche", new TranslationFetcherProvider(translations, culture).BidirectionalFetchMethod, null },
            new object[] { HorizontalAlignment.Left, "", new TranslationFetcherProvider(translations, culture).FetchMethod, null },
            new object[] { HorizontalAlignment.Left, "Gauche", new TranslationFetcherProvider(translations, culture).BidirectionalFetchMethodWithCulture, new CultureInfo("fr-FR") },
            new object[] { HorizontalAlignment.Left, "", new TranslationFetcherProvider(translations, culture).BidirectionalFetchMethodWithCulture, new CultureInfo("en-US") },
            new object[] { HorizontalAlignment.Left, "Gauche", new TranslationFetcherProvider(translations, culture).BidirectionalFetchDictionary, new CultureInfo("fr-FR") },
            new object[] { HorizontalAlignment.Left, "Gauche", new TranslationFetcherProvider(translations, culture).BidirectionalFetchDictionaryWithCulture, new CultureInfo("fr-FR") },
            new object[] { HorizontalAlignment.Left, "Gauche", new TranslationFetcherProvider(translations, culture).TranslationDictionary, new CultureInfo("fr-FR") },
            new object[] { HorizontalAlignment.Left, "Gauche", new TranslationFetcherProvider(translations, culture).TranslationDictionaryWithCulture, new CultureInfo("fr-FR") },
        };

        [Theory]
        [MemberData(nameof(ConvertBackData))]
        public void ConvertsHorizontalAlignmentBackToString(object input, string expected, object parameter, CultureInfo culture)
        {
            var converter = new StringToHorizontalAlignmentConverter();
            var result = converter.ConvertBack(input, input?.GetType(), parameter, culture);
            Assert.Equal(expected, result);
        }
        #endregion
    }
}
