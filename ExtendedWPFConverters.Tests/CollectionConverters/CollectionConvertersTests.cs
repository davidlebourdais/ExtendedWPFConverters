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
            new object[] { new List<object>() { 1, 2, 3}, true, 0 },
            new object[] { new List<object>() { 1, 2, 3}, false, 0 },
            new object[] { new List<object>() { 1, 2, 3}, true, -1 },
            new object[] { new List<object>() { 1, 2, 3}, false, -1 },
            new object[] { new List<object>(), true, 0 },
            new object[] { new List<object>(), false, 0 },
            new object[] { new List<object>(500), true, 0 },
            new object[] { new List<object>(500), false, 0 },
            new object[] { new List<object>(500).ToArray().AsEnumerable(), true, 0 },
            new object[] { new List<object>(500).ToArray().AsEnumerable(), false, 0 },
            new object[] { new List<bool>(275), true, 0 },
            new object[] { new List<bool>(124), false, 0 },
            new object[] { new List<(bool, double, string, TimeSpan)>(12), true, 0 },
            new object[] { new List<(bool, double, string, TimeSpan)>(2), false, 0 },
            new object[] { null, true, 0 },
            new object[] { null, false, 0 },
            new object[] { "invalid", true, 0 },
            new object[] { "invalid", false, 0 },
            new object[] { null, true, -1 },
            new object[] { null, false, -10 },
            new object[] { "invalid", true, 10 },
            new object[] { "invalid", false, 1 }
        };

        [Theory]
        [MemberData(nameof(CollectionCountData))]
        public void ConvertsCollectionToCount(object input, bool outputs_string, int default_count_value)
        {
            var converter = new CollectionCountConverter() { OutputAsString = outputs_string, DefaultCountValue = default_count_value };
            var result = converter.Convert(input, typeof(Color), null, null);
            if (input is IEnumerable)
            {
                var count = 0;
                var enumerator = (input as IEnumerable).GetEnumerator();
                while (enumerator.MoveNext())  // seems to be actual implementation for Count properties.
                    count++;

                if (outputs_string)
                    Assert.Equal(count.ToString(), result);
                else Assert.Equal(count, result);
            }
            else
                if (outputs_string)
                    Assert.Equal(default_count_value.ToString(), result);
                else Assert.Equal(default_count_value, result);
        }
        #endregion
    }
}
