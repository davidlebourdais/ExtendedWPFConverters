using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace EMA.ExtendedWPFConverters
{
    /// <summary>
    /// Groups methods to be used for string translation through various types
    /// of fetchers to be provided with converter calls.
    /// </summary>
    public static class StringTranslationHelper
    {
        /// <summary>
        /// Checks if passed translation retriever object type is supported by the helper methods.
        /// </summary>
        /// <param name="translationGetter">A object to check for compatibility.</param>
        /// <param name="bidirectionalOnly">If set to true, filter out types that support one-way translations.</param>
        /// <returns>True if passed object type matches supported types in this helper class.</returns>
        /// <remarks>The translation getter could be a one-way fetcher when its type is:
        /// - a Func{string, string} that accepts the value as input and returns the corresponding translation
        /// - a Func{string, CultureInfo, string} that accepts the value and the current CultureInfo as input and returns the corresponding translation
        /// - a IDict{string culture, Func{string, string}} which contains the current culture's TwoLetterISOLanguageName or ThreeLetterISOLanguageName as culture key
        ///   and points to a function that accepts the value as input and returns the corresponding translation 
        /// - a IDict{CultureInfo culture, Func{string, string}} which contains the current culture information as culture key and points to a function that accepts the 
        ///   value as input and returns the corresponding translation 
        /// Or translation getter can be a bidirectional converter with an additional bool parameter which inverts the translation process if set to true:
        /// - a Func{string, bool, string} that accepts the value as input and returns the corresponding translation when bool is true, do the opposite if false
        /// - a Func{string, CultureInfo, bool, string} that accepts the value and the current CultureInfo as input and returns the corresponding translation
        /// - a IDict{string culture, Func{string, bool, string}} which contains the current culture's TwoLetterISOLanguageName or ThreeLetterISOLanguageName as culture key
        ///   and points to a function that accepts the value as input and returns the corresponding translation 
        /// - a IDict{CultureInfo culture, Func{string, bool, string}} which contains the current culture information as culture key and points to a function that accepts the 
        ///   value as input and returns the corresponding translation 
        /// Or translation getter can be a dictionary of key-values per culture, in which case it is considered bidirectional by default:
        /// - a IDict{string culture, IDict{string key, string value}} which contains, for a given culture's TwoLetterISOLanguageName or ThreeLetterISOLanguageName as culture, 
        /// a dictionary containing the translations. Forward conversion will use the value to translate as key.
        /// - a IDict{string culture, IDict{string key, string value}} which contains, for a given culture, a dictionary containing the translations. Forward conversion will 
        /// use the value to translate as key.
        /// </remarks>
        public static bool CheckFetcherFormat(object translationGetter, bool bidirectionalOnly = false)
            => (!bidirectionalOnly && translationGetter is Func<string, string> 
                    || translationGetter is Func<string, CultureInfo, string> 
                    || translationGetter is IDictionary<string, Func<string, string>> 
                    || translationGetter is IDictionary<CultureInfo, Func<string, string>>)           
                    || translationGetter is Func<string, bool, string> 
                    || translationGetter is Func<string, CultureInfo, bool, string> 
                    || translationGetter is IDictionary<string, Func<string, bool, string>> 
                    || translationGetter is IDictionary<CultureInfo, Func<string, bool, string>>
                    || translationGetter is IDictionary<string, IDictionary<string, string>>
                    || translationGetter is IDictionary<CultureInfo, IDictionary<string, string>>;

        /// <summary>
        /// Tries to fetch a translation for a source value and from a method or a dictionary that are passed as parameter for a given culture.
        /// </summary>
        /// <param name="value">The value to be translated, used as a key for method calls and dictionary fetches.</param>
        /// <param name="translationGetter">Compatible object to be used for translation retrieving. See <see cref="CheckFetcherFormat"/> for more information.</param>
        /// <param name="culture">Current culture information.</param>
        /// <param name="translated">Result of the translation.</param>
        /// <returns>True if translation succeeded, false otherwise.</returns>
        public static bool TryTranslateValue(string value, object translationGetter, CultureInfo culture, out string translated)
        {
            translated = default;
            if (translationGetter == null) 
                return false;

            // If translationGetter is a Func<string, string>:
            if (translationGetter is Func<string, string> fetcher)
            {
                translated = fetcher.Invoke(value);
                return true;
            }

            if (translationGetter is Func<string, bool, string> reversibleFetcher)
            {
                translated = reversibleFetcher.Invoke(value, false);
                return true;
            }

            if (culture == null)
                return false; // no result here.
            
            // If translationGetter is a Func<string, CultureInfo, string>:
            if (translationGetter is Func<string, CultureInfo, string> fetcherWithCulture)
            {
                translated = fetcherWithCulture.Invoke(value, culture);
                return true;
            }
            
            if (translationGetter is Func<string, CultureInfo, bool, string> reversibleFetcherWithCulture)
            {
                translated = reversibleFetcherWithCulture.Invoke(value, culture, false);
                return true;
            }
            
            // If translationGetter is a Dict<string, Func<string, string>>:
            if (translationGetter is IDictionary<string, Func<string, string>> fetchDict)
            {
                if (fetchDict.TryGetValue(culture.ThreeLetterISOLanguageName, out var dictFetcher1))
                {
                    translated = dictFetcher1.Invoke(value);
                    return true;
                }
                
                if (fetchDict.TryGetValue(culture.TwoLetterISOLanguageName, out var dictFetcher2))
                {
                    translated = dictFetcher2.Invoke(value);
                    return true;
                }
            }
            else if (translationGetter is IDictionary<string, Func<string, bool, string>> reversibleFetchDict)
            {
                if (reversibleFetchDict.TryGetValue(culture.ThreeLetterISOLanguageName, out var reversibleDictFetcher1))
                {
                    translated = reversibleDictFetcher1.Invoke(value, false);
                    return true;
                }
                
                if (reversibleFetchDict.TryGetValue(culture.TwoLetterISOLanguageName, out var reversibleDictFetcher2))
                {
                    translated = reversibleDictFetcher2.Invoke(value, false);
                    return true;
                }
            }
            // If translationGetter is a Dict<CultureInfo, Func<string, string>>:
            else if (translationGetter is IDictionary<CultureInfo, Func<string, string>> fetchDictWithCulture)
            {
                if (fetchDictWithCulture.TryGetValue(culture, out var dictFetcherWithCulture))
                {
                    translated = dictFetcherWithCulture.Invoke(value);
                    return true;
                }
            }
            else if (translationGetter is IDictionary<CultureInfo, Func<string, bool, string>> reversibleFetchDictWithCulture)
            {
                if (reversibleFetchDictWithCulture.TryGetValue(culture, out var reversibleDictFetcherWithCulture))
                {
                    translated = reversibleDictFetcherWithCulture.Invoke(value, false);
                    return true;
                }
            }
            // If translationGetter is a Dict<string, Dict<string, string>> then take key by value on target dict:
            else if (translationGetter is IDictionary<string, IDictionary<string, string>> refDict)
            {
                if (refDict.TryGetValue(culture.ThreeLetterISOLanguageName, out var refDictionary1))
                {
                    if ((translated = refDictionary1.FirstOrDefault(x => x.Value == value).Key) != null)
                        return true;
                }
                else if (refDict.TryGetValue(culture.TwoLetterISOLanguageName, out var refDictionary2))
                {
                    if ((translated = refDictionary2.FirstOrDefault(x => x.Value == value).Key) != null)
                        return true;
                }
            }
            // If translationGetter is a Dict<CultureInfo, IDictionary<string, string>> then take key by value on target dict:
            else if (translationGetter is IDictionary<CultureInfo, IDictionary<string, string>> refDictWithCulture)
            {
                if (refDictWithCulture.TryGetValue(culture, out var refDictWithCulture1))
                {
                    if ((translated = refDictWithCulture1.FirstOrDefault(x => x.Value == value).Key) != null)
                        return true;
                }
            }

            return false; // no result here.
        }

        /// <summary>
        /// Tries to fetch the original value from a translated value and from a method or a dictionary that are passed as parameter for a given culture.
        /// </summary>
        /// <param name="value">The already translated value to be converted back, used as a key for method calls and as value for dictionary fetches.</param>
        /// <param name="translationGetter">Compatible object to be used for translation retrieving. See <see cref="CheckFetcherFormat"/> for more information.</param>
        /// <param name="culture">Current culture information.</param>
        /// <param name="originalValue">Original value matching the entry.</param>
        /// <returns>True if translate back operation succeeded, false otherwise.</returns>
        public static bool TryTranslateValueBack(string value, object translationGetter, CultureInfo culture, out string originalValue)
        {
            originalValue = default;
            if (translationGetter == null)
                return false;

            // If translationGetter is a Func<string, bool, string>:
            if (translationGetter is Func<string, bool, string> reversibleFetcher)
            {
                originalValue = reversibleFetcher.Invoke(value, true);
                return true;
            }
            else if (culture != null)
            {
                // If translationGetter is a Func<string, CultureInfo, bool, string>:
                if (translationGetter is Func<string, CultureInfo, bool, string> reversibleFetcherWithCulture)
                {
                    originalValue = reversibleFetcherWithCulture.Invoke(value, culture, true);
                    return true;
                }
                // If translationGetter is a Dict<string, Func<string, bool, string>>:
                else if (translationGetter is IDictionary<string, Func<string, bool, string>> reversibleFetchDict)
                {
                    if (reversibleFetchDict.TryGetValue(culture.ThreeLetterISOLanguageName, out var reversibleDictFetcher1))
                    {
                        originalValue = reversibleDictFetcher1.Invoke(value, true);
                        return true;
                    }
                    else if (reversibleFetchDict.TryGetValue(culture.TwoLetterISOLanguageName, out var reversibleDictFetcher2))
                    {
                        originalValue = reversibleDictFetcher2.Invoke(value, true);
                        return true;
                    }
                }
                // If translationGetter is a Dict<CultureInfo, Func<string, string>>:
                else if (translationGetter is IDictionary<CultureInfo, Func<string, bool, string>> reversibleFetchDictWithCulture)
                {
                    if (reversibleFetchDictWithCulture.TryGetValue(culture, out var reversibleDictFetcherWithCulture))
                    {
                        originalValue = reversibleDictFetcherWithCulture.Invoke(value, true);
                        return true;
                    }
                }
                // If translationGetter is a Dict<string, Dict<string, string>>:
                else if (translationGetter is IDictionary<string, IDictionary<string, string>> refDict)
                {
                    if (refDict.TryGetValue(culture.ThreeLetterISOLanguageName, out var refDict1))
                        return refDict1.TryGetValue(value, out originalValue);
                    else if (refDict.TryGetValue(culture.TwoLetterISOLanguageName, out var refDict2))
                        return refDict2.TryGetValue(value, out originalValue);
                }
                // If translationGetter is a Dict<CultureInfo, IDictionary<string, string>>:
                else if (translationGetter is IDictionary<CultureInfo, IDictionary<string, string>> refDictWithCulture)
                {
                    if (refDictWithCulture.TryGetValue(culture, out var refDictWithCulture1))
                    {
                        return refDictWithCulture1.TryGetValue(value, out originalValue);
                    }
                }
            }

            return false; // no result here.
        }
    }
}