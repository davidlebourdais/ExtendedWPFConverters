using System;
using System.Linq;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class CollectionConvertersTests
    {
        #region CollectionCountConverter
        public static IEnumerable<object[]> CollectionCountData => new List<object[]>
        {
            new object[] { new List<object>() { 1, 2, 3}, true, 0, "0" },
            new object[] { new List<object>() { 1, 2, 3}, false, 0, "0" },
            new object[] { new List<object>() { 1, 2, 3}, true, -1, "" },
            new object[] { new List<object>() { 1, 2, 3}, false, -1, "" },
            new object[] { new List<object>(), true, 0, "0" },
            new object[] { new List<object>(), false, 0, "0" },
            new object[] { new List<object>(500), true, 0, "0" },
            new object[] { new List<object>(500), false, 0, "0" },
            new object[] { new List<object>(500).ToArray().AsEnumerable(), true, 0, "0" },
            new object[] { new List<object>(500).ToArray().AsEnumerable(), false, 0, "0" },
            new object[] { new List<bool>(275), true, 0, "0" },
            new object[] { new List<bool>(124), false, 0, "0" },
            new object[] { new List<(bool, double, string, TimeSpan)>(12), true, 0, "0" },
            new object[] { new List<(bool, double, string, TimeSpan)>(2), false, 0, "0" },
            new object[] { null, true, 0, "0" },
            new object[] { null, false, 0, "0" },
            new object[] { "invalid", true, 0, "0" },
            new object[] { "invalid", false, 0, "0" },
            new object[] { null, true, -1, "" },
            new object[] { null, false, -10, "" },
            new object[] { "invalid", true, 10, "" },
            new object[] { "invalid", false, 1, "error" }
        };

        [Theory]
        [MemberData(nameof(CollectionCountData))]
        public void ConvertsCollectionToCount(object input, bool outputsString, int defaultCountValue, string defaultCountValueString)
        {
            var converter = new CollectionCountConverter() { OutputAsString = outputsString, DefaultCountValue = defaultCountValue, DefaultCountValueString = defaultCountValueString };
            var result = converter.Convert(input, typeof(Color), null, null);
            if (input is IEnumerable enumerable)
            {
                var count = 0;
                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())  // seems to be actual implementation for Count properties.
                    count++;

                if (outputsString)
                    Assert.Equal(count.ToString(), result);
                else Assert.Equal(count, result);
            }
            else
                if (outputsString)
                    Assert.Equal(defaultCountValueString, result);
                else Assert.Equal(defaultCountValue, result);
        }
        #endregion

        #region CollectionFirstItemConverter
        public static IEnumerable<object[]> CollectionFirstItemData => new List<object[]>
        {
            new object[] { new List<object>() { 1, 2, 3} },
            new object[] { new List<object>() { 5, 4, 3, 2, 1} },
            new object[] { new List<int>(100) },
            new object[] { new List<object>(100) },
            new object[] { "test" },
            new object[] { new Dictionary<List<int>, int>() { { new List<int>() { 1, 2 }, 3 } } },
            new object[] { new[] { "abc", "def" } },
            new object[] { new List<object>() },
            new object[] { null }
        };

        [Theory]
        [MemberData(nameof(CollectionFirstItemData))]
        public void ConvertsCollectionToFirstItem(object input)
        {
            var converter = new CollectionFirstItemConverter();
            var result = converter.Convert(input, typeof(IEnumerable), null, null);
            if (input is IEnumerable enumerable)
                Assert.Equal(enumerable.GetEnumerator().MoveNext(), result);
            else Assert.Null(result);
        }
        #endregion
    }
}
