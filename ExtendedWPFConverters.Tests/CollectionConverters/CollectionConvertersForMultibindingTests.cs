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
        public void ConvertsCollectionToFirstItem(object[] inputs, bool as_ienumerable)
        {
            var converter = new EMA.ExtendedWPFConverters.CollectionFirstItemConverterForMultiBinding() { AsIEnumerable = as_ienumerable };
            var result = converter.Convert(inputs, typeof(Color), null, null);
            if (inputs.Any() && (as_ienumerable && inputs.First() is IEnumerable<object> || !as_ienumerable))
            {
                if (as_ienumerable)
                    Assert.Equal((inputs.First() as IEnumerable<object>).First(), result);
                else
                    Assert.Equal(inputs.First(), result);
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
        public void ConvertsCollectionToIndexOf(object[] inputs, bool output_as_string, int value_for_not_found, string value_string_for_not_found, 
                                                                 int value_for_invalid, string value_string_for_invalid)
        {
            var converter = new CollectionIndexOfConverterForMultiBinding() 
            { 
                OutputAsString = output_as_string,
                ValueForNotFound = value_for_not_found,
                ValueStringForNotFound = value_string_for_not_found,
                ValueForInvalid = value_for_invalid,
                ValueStringForInvalid = value_string_for_invalid
            };

            var result = converter.Convert(inputs, typeof(Color), null, null);

            if (inputs.First() is IEnumerable asEnumerable && inputs.Length > 1 && inputs[1] != null)
            {
                var count = 0;
                var stopped = false;
                var found = false;
                var enumerator = asEnumerable.GetEnumerator();
                enumerator.Reset();
                do
                {
                    stopped = !enumerator.MoveNext();
                    if (!stopped && enumerator.Current.Equals(inputs[1]))
                        found = true;
                    else count++;
                } while (!stopped && !found);

                if (found)
                {
                    if (output_as_string)
                        Assert.Equal(count.ToString(), result);
                    else Assert.Equal(count, result);                        
                } 
                else
                {
                   if (output_as_string)
                        Assert.Equal(value_string_for_not_found, result);
                    else Assert.Equal(value_for_not_found, result);   
                }
            }
            else 
            {
                if (output_as_string)
                    Assert.Equal(value_string_for_invalid, result);
                else Assert.Equal(value_for_invalid, result);   
            }
        }
        #endregion
    }
}
