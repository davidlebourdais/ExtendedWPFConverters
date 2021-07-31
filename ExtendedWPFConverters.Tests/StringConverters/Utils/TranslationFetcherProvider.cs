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
        private readonly Dictionary<string, string> _keyValuePairs;
        private readonly CultureInfo _culture;
        
        public TranslationFetcherProvider(Dictionary<string, string> keyValuePairs, CultureInfo culture)
        {
            this._keyValuePairs = keyValuePairs;
            this._culture = culture;
        }

        public Func<string, string> FetchMethod 
            => (key) => _keyValuePairs.FirstOrDefault(x => x.Value == key).Key;
        public Func<string, CultureInfo, string> FetchMethodWithCulture 
            => (key, culture) => culture?.Name == _culture.Name ? FetchMethod(key) : "";

        public Dictionary<string, Func<string, string>> FetchDictionary => 
            new Dictionary<string, Func<string, string>>  { { _culture.ThreeLetterISOLanguageName, FetchMethod } };
        
        public Dictionary<CultureInfo, Func<string, string>> FetchDictionaryWithCulture => 
            new Dictionary<CultureInfo, Func<string, string>>  { { _culture, FetchMethod} };

        public Func<string, bool, string> BidirectionalFetchMethod 
            => (key, invert) => invert ? _keyValuePairs[key] : FetchMethod(key);
        public Func<string, CultureInfo, bool, string> BidirectionalFetchMethodWithCulture 
            => (key, culture, invert) => culture?.Name == this._culture.Name ? BidirectionalFetchMethod(key, invert) : "";

        public Dictionary<string, Func<string, bool, string>> BidirectionalFetchDictionary => 
            new Dictionary<string, Func<string, bool, string>>  { { _culture.ThreeLetterISOLanguageName, BidirectionalFetchMethod } };
        
        public Dictionary<CultureInfo, Func<string, bool, string>> BidirectionalFetchDictionaryWithCulture => 
            new Dictionary<CultureInfo, Func<string, bool, string>>  { { _culture, BidirectionalFetchMethod} };

        public Dictionary<string, IDictionary<string, string>> TranslationDictionary => 
            new Dictionary<string, IDictionary<string, string>>  { { _culture.ThreeLetterISOLanguageName, _keyValuePairs } };

        public Dictionary<CultureInfo, IDictionary<string, string>> TranslationDictionaryWithCulture => 
            new Dictionary<CultureInfo, IDictionary<string, string>>  { { _culture, _keyValuePairs } };
    }
}
