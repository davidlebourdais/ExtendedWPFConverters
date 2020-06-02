using System.Collections.Generic;
using System;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class InstanceToTypeConverterTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { "string value" },
            new object[] { 123 },
            new object[] { TimeSpan.FromDays(1) },
            new object[] { new Random() },
            new object[] { new List<double>() },
            new object[] { null }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertsInstanceToType(object input)
        {
            var converter = new InstanceToTypeConverter();
            var result = converter.Convert(input, input?.GetType(), null, null);
            Assert.Equal(input?.GetType(), result);
        }
    }
}
