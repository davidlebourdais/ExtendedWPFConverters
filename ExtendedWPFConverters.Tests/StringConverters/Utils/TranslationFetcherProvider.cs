using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace EMA.ExtendedWPFConverters.Tests
{
    /// <summary>
    /// Provides methods and structures for a passed key-value pairs dictionary containing 
    /// strings keys and their translation for a passed culture, and to be used with classes
    /// that deal with <see cref="StringTranslationHelper"/> methods.
    /// </summary>
    public class TranslationFetcherProvider
    {
        private readonly Dictionary<string, string> keyValuePairs;
        private readonly CultureInfo culture;
        
        public TranslationFetcherProvider(Dictionary<string, string> keyValuePairs, CultureInfo culture)
        {
            this.keyValuePairs = keyValuePairs;
            this.culture = culture;
        }

        public Func<string, string> FetchMethod 
            => (Func<string, string>)((key) => keyValuePairs.FirstOrDefault(x => x.Value == key).Key);
        public Func<string, CultureInfo, string> FetchMethodWithCulture 
            => (Func<string, CultureInfo, string>)((key, culture) => culture?.Name == this.culture.Name ? FetchMethod(key) : "");

        public Dictionary<string, Func<string, string>> FetchDictionary => 
            new Dictionary<string, Func<string, string>>  { { culture.ThreeLetterISOLanguageName, FetchMethod } };
        
        public Dictionary<CultureInfo, Func<string, string>> FetchDictionaryWithCulture => 
            new Dictionary<CultureInfo, Func<string, string>>  { { culture, FetchMethod} };

        public Func<string, bool, string> BidirectionalFetchMethod 
            => (Func<string, bool, string>)((key, invert) => invert ? keyValuePairs[key] : FetchMethod(key));
        public Func<string, CultureInfo, bool, string> BidirectionalFetchMethodWithCulture 
            => (Func<string, CultureInfo, bool, string>)((key, culture, invert) => culture?.Name == this.culture.Name ? BidirectionalFetchMethod(key, invert) : "");

        public Dictionary<string, Func<string, bool, string>> BidirectionalFetchDictionary => 
            new Dictionary<string, Func<string, bool, string>>  { { culture.ThreeLetterISOLanguageName, BidirectionalFetchMethod } };
        
        public Dictionary<CultureInfo, Func<string, bool, string>> BidirectionalFetchDictionaryWithCulture => 
            new Dictionary<CultureInfo, Func<string, bool, string>>  { { culture, BidirectionalFetchMethod} };

        public Dictionary<string, IDictionary<string, string>> TranslationDictionary => 
            new Dictionary<string, IDictionary<string, string>>  { { culture.ThreeLetterISOLanguageName, keyValuePairs } };

        public Dictionary<CultureInfo, IDictionary<string, string>> TranslationDictionaryWithCulture => 
            new Dictionary<CultureInfo, IDictionary<string, string>>  { { culture, keyValuePairs } };
    }
}
