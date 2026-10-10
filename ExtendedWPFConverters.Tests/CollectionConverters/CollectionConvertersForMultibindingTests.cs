using System;
using System.Linq;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class CollectionConvertersForMultibindingTests
    {
		#region CollectionCountConverter
        public static IEnumerable<object[]> CollectionFirstItemData => new List<object[]>
        {
            new object[] { new object[] { new List<object>() { 1, 2, 3}}, false },
            new object[] { new object[] { new List<object>() { 1, 2, 3}}, true },
            new object[] { new object[] { new List<object>() }, true },
            new object[] { new object[] { "test" }, false },
            new object[] { new object[] { "test" }, true },
            new object[] { new object[] { "test1", "test2", "test3" }, false },
            new object[] { new object[] { new TimeSpan(), 123, 2.0d, "test" }, false },
            new object[] { new object[] { }, false },
            new object[] { new object[] { null }, false },
            new object[] { new object[] { null }, true },
            new object[] { new object[] { null, null, null }, false },
        };

        [Theory]
        [MemberData(nameof(CollectionFirstItemData))]
        public void ConvertsCollectionToFirstItem(object[] inputs, bool asIEnumerable)
        {
            var converter = new CollectionFirstItemConverterForMultiBinding() { AsIEnumerable = asIEnumerable };
            var result = converter.Convert(inputs, typeof(Color), null, null);
            if (!inputs.Any())
            {
                Assert.Null(result);
            }
            else if (!asIEnumerable)
            {
                Assert.Equal(inputs.First(), result);
            }
            else if (inputs.First() is IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                try
                {
                    Assert.Equal(enumerator.MoveNext() ? enumerator.Current : null, result);
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                        disposable.Dispose();
                }
            }
            else
            {
                Assert.Null(result);
            }
        }
        #endregion

        #region CollectionIndexOfConverterForMultiBinding
        public static IEnumerable<object[]> CollectionIndexOfData => new List<object[]>
        {
            // valid:
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 1 }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 2 }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<int>() { 1, 2, 3}, 3 }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 1 }, false, -1, "-1", 0, "error" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 1 }, false, -1, "0", 0, "error" },
            new object[] { new object[] { new List<string>() { "1", "2", "3"}, "3" }, false, -1, "", -1, "" },
            new object[] { new object[] { "test", 's' }, false, -1, "", -1, "" },

            new object[] { new object[] { new List<object>() { 1, 2, 3}, 1 }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 2 }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<int>() { 1, 2, 3}, 3 }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 1 }, true, -1, "-1", 0, "error" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 1 }, true, -1, "0", 0, "error" },
            new object[] { new object[] { new List<string>() { "1", "2", "3"}, "3" }, true, -1, "", -1, "" },
            new object[] { new object[] { "test", 's' }, true, -1, "", -1, "" },
            
            // not found:
            new object[] { new object[] { new List<object>() { 1, 2, 3}, 0 }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, -1 }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, null }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, "invalid" }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>(), 0 }, false, -1, "", -1, "" },
            new object[] { new object[] { new List<object>(3), 1 }, false, -1, "", -1, "" },

            new object[] { new object[] { new List<object>() { 1, 2, 3}, 0 }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, -1 }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, null }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>() { 1, 2, 3}, "invalid" }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>(), 0 }, true, -1, "", -1, "" },
            new object[] { new object[] { new List<object>(3), 1 }, true, -1, "", -1, "" },

            // invalid:
            new object[] { new object[] { null, 0 }, false, -1, "", -1, "" },
            new object[] { new object[] { null, null, null, 0 }, false, -1, "", -1, "" },
            new object[] { new object[] { null, 0 }, false, -1, "-1", 0, "0" },

            new object[] { new object[] { null, 0 }, true, -1, "", -1, "" },
            new object[] { new object[] { null, null, null, 0 }, true, -1, "", -1, "" },
            new object[] { new object[] { null, 0 }, true, -1, "-1", 0, "0" }
        };

        [Theory]
        [MemberData(nameof(CollectionIndexOfData))]
        public void ConvertsCollectionToIndexOf(object[] inputs, bool outputAsString, int valueForNotFound, string valueStringForNotFound, int valueForInvalid, string valueStringForInvalid)
        {
            var converter = new CollectionIndexOfConverterForMultiBinding() 
            { 
                OutputAsString = outputAsString,
                ValueForNotFound = valueForNotFound,
                ValueStringForNotFound = valueStringForNotFound,
                ValueForInvalid = valueForInvalid,
                ValueStringForInvalid = valueStringForInvalid
            };

            var result = converter.Convert(inputs, typeof(Color), null, null);

            if (inputs.First() is IEnumerable asEnumerable && inputs.Length > 1 && inputs[1] != null)
            {
                var count = 0;
                bool stopped;
                var found = false;
                var enumerator = asEnumerable.GetEnumerator();
                enumerator.Reset();
                do
                {
                    stopped = !enumerator.MoveNext();
                    if (!stopped && enumerator.Current?.Equals(inputs[1]) == true)
                        found = true;
                    else count++;
                } while (!stopped && !found);

                if (found)
                {
                    if (outputAsString)
                        Assert.Equal(count.ToString(), result);
                    else Assert.Equal(count, result);                        
                } 
                else
                {
                   if (outputAsString)
                        Assert.Equal(valueStringForNotFound, result);
                    else Assert.Equal(valueForNotFound, result);   
                }
            }
            else 
            {
                if (outputAsString)
                    Assert.Equal(valueStringForInvalid, result);
                else Assert.Equal(valueForInvalid, result);   
            }
        }
        #endregion

        #region CollectionContainsConverterForMultiBinding
        public static IEnumerable<object[]> CollectionContainsData => new List<object[]>
        {
            new object[] { new object[] { new List<object>() { 1, 2, 3 }, 1 }, false, false },
            new object[] { new object[] { new List<object>() { 1, 2, 3 }, 4 }, false, false },
            new object[] { new object[] { new List<object>() { 1, null, 3 }, null }, false, false },
            new object[] { new object[] { "test", 's' }, false, false },
            new object[] { new object[] { "test", 'z' }, false, false },
            new object[] { new object[] { new[] { "alpha", "beta" }.AsEnumerable(), "beta" }, false, false },
            new object[] { new object[] { new[] { "alpha", "beta" }.AsEnumerable(), "gamma" }, false, false },

            new object[] { new object[] { new List<object>() { 1, 2, 3 }, 1 }, true, false },
            new object[] { new object[] { new List<object>() { 1, 2, 3 }, 4 }, true, false },
            new object[] { new object[] { null, 1 }, false, false },
            new object[] { new object[] { 123, 1 }, false, false },
            new object[] { new object[] { new List<object>() { 1, 2, 3 } }, false, true }
        };

        [Theory]
        [MemberData(nameof(CollectionContainsData))]
        public void ConvertsCollectionToContains(object[] inputs, bool trueIfNotContains, bool valueForInvalid)
        {
            var converter = new CollectionContainsConverterForMultiBinding()
            {
                TrueIfNotContains = trueIfNotContains,
                ValueForInvalid = valueForInvalid
            };

            var result = converter.Convert(inputs, typeof(Color), null, null);

            if (inputs.Length < 2 || !(inputs[0] is IEnumerable enumerable))
            {
                Assert.Equal(valueForInvalid, result);
                return;
            }

            var contains = false;
            foreach (var item in enumerable)
            {
                if (Equals(item, inputs[1]))
                {
                    contains = true;
                    break;
                }
            }

            Assert.Equal(trueIfNotContains ? !contains : contains, result);
        }

        public static IEnumerable<object[]> CollectionContainsUnresolvedData
        {
            get
            {
                var unresolved = DependencyProperty.UnsetValue;
                var inputs = new[]
                {
                    new object[] { new[] { 1, 2, 3 }, unresolved },
                    new object[] { Enumerable.Range(1, 3).Where(item => true), unresolved },
                    new object[] { new[] { unresolved }, unresolved },
                    new object[] { unresolved, 1 }
                };

                foreach (var input in inputs)
                    foreach (var trueIfNotContains in new[] { false, true })
                        foreach (var valueForInvalid in new[] { false, true })
                            yield return new object[] { input, trueIfNotContains, valueForInvalid };
            }
        }

        [Theory]
        [MemberData(nameof(CollectionContainsUnresolvedData))]
        public void ReturnsConfiguredInvalidValueForUnresolvedCollectionBindings(object[] inputs, bool trueIfNotContains, bool valueForInvalid)
        {
            var converter = new CollectionContainsConverterForMultiBinding()
            {
                TrueIfNotContains = trueIfNotContains,
                ValueForInvalid = valueForInvalid
            };

            Assert.Equal(valueForInvalid, converter.Convert(inputs, typeof(bool), null, null));
        }
        #endregion
    }
}
