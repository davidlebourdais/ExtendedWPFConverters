using System;
using System.Linq;
using Xunit;
using System.Collections;
using System.Collections.Generic;
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
            if (inputs.Any() && (asIEnumerable && inputs.First() is IEnumerable<object> || !asIEnumerable))
            {
                Assert.Equal(asIEnumerable ? (inputs.First() as IEnumerable<object>)?.First() : inputs.First(), result);
            }
            else Assert.Null(result);
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
    }
}
