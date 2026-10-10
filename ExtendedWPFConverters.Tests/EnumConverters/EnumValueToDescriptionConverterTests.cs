using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class EnumValueToDescriptionConverterTests
    {
        private enum TestEnum
        {
            [Description("With description")]
            WithDescription,
            WithoutDescription,
            [Description("")]
            EmptyDescription,
            [Description(nameof(WithDescription))]
            DescriptionMatchingAnotherName
        }

        [Flags]
        private enum TestFlags
        {
            None = 0,
            [Description("Read access")]
            Read = 1,
            [Description("Write access")]
            Write = 2,
            [Description("Combined access")]
            ReadWrite = Read | Write,
            Execute = 4
        }

        private enum ByteEnum : byte
        {
            [Description("Tiny value")]
            One = 1
        }

        public static IEnumerable<object[]> DescriptionData => new[]
        {
            new object[] { TestEnum.WithDescription, "With description" },
            new object[] { TestEnum.WithoutDescription, "WithoutDescription" },
            new object[] { TestEnum.EmptyDescription, "" },
            new object[] { (TestEnum)999, "999" },
            new object[] { (TestEnum)(-1), "-1" },
            new object[] { TestFlags.None, "None" },
            new object[] { TestFlags.Read, "Read access" },
            new object[] { TestFlags.ReadWrite, "Combined access" },
            new object[] { TestFlags.Execute, "Execute" },
            new object[] { TestFlags.Read | TestFlags.Execute, "Read, Execute" },
            new object[] { (TestFlags)17, "17" },
            new object[] { (ByteEnum)255, "255" }
        };

        public static IEnumerable<object[]> ReverseDescriptionData
        {
            get
            {
                foreach (var row in DescriptionData)
                {
                    var enumType = row[0].GetType();
                    yield return new[] { row[1], enumType, row[0] };
                    yield return new[] { row[1], typeof(Nullable<>).MakeGenericType(enumType), row[0] };
                }
            }
        }
        
        [Fact]
        public void ConvertsEnumValueToDescription()
        {
            const TestEnum input = TestEnum.WithDescription;
            
            var converter = new EnumValueToDescriptionConverter();
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal("With description", result);
        }
        
        [Fact]
        public void ConvertsEnumValueWithoutDescriptionToItsName()
        {
            const TestEnum input = TestEnum.WithoutDescription;
            
            var converter = new EnumValueToDescriptionConverter();
            var result = converter.Convert(input, typeof(object), null, null);
            
            Assert.Equal(nameof(TestEnum.WithoutDescription), result);
        }
        
        [Fact]
        public void ConvertsDescriptionToEnumValue()
        {
            const string input = "With description";
            
            var converter = new EnumValueToDescriptionConverter();
            var result = converter.ConvertBack(input, typeof(TestEnum), null, null);
            
            Assert.Equal(TestEnum.WithDescription, result);
        }
        
        [Theory]
        [MemberData(nameof(DescriptionData))]
        public void DescribesDefinedUndefinedAndFlagValues(object input, string expected)
        {
            var converter = new EnumValueToDescriptionConverter();

            Assert.Equal(expected, ((Enum)input).ToDescriptionOrToString());
            Assert.Equal(expected, converter.Convert(input, typeof(string), null, null));
        }

        [Theory]
        [MemberData(nameof(ReverseDescriptionData))]
        public void ConvertsDescriptionsAndFallbacksBackToEnumAndNullableEnum(string input, Type targetType, object expected)
        {
            var converter = new EnumValueToDescriptionConverter();

            Assert.Equal(expected, converter.ConvertBack(input, targetType, null, null));
        }

        [Fact]
        public void PreservesHelperAndConverterNullContracts()
        {
            Assert.Equal("", EnumExtensions.ToDescriptionOrToString(null));
            var converter = new EnumValueToDescriptionConverter();
            foreach (var value in new object[] { null, 42, "With description", typeof(TestEnum), DependencyProperty.UnsetValue, Binding.DoNothing })
                Assert.Null(converter.Convert(value, typeof(string), null, null));

            Assert.Null(converter.ConvertBack(null, typeof(TestEnum?), null, null));
            Assert.Null(converter.ConvertBack("With description", null, null, null));
            TestEnum? nullableValue = TestEnum.WithDescription;
            Assert.Equal("With description", converter.Convert(nullableValue, typeof(string), null, null));
        }

        [Fact]
        public void DescriptionsTakePrecedenceOverMatchingEnumNames()
        {
            var converter = new EnumValueToDescriptionConverter();

            Assert.Equal(TestEnum.DescriptionMatchingAnotherName,
                converter.ConvertBack(nameof(TestEnum.WithDescription), typeof(TestEnum?), null, null));
        }

        [Theory]
        [InlineData("Invalid description", typeof(TestEnum?))]
        [InlineData("256", typeof(ByteEnum?))]
        [InlineData("With description", typeof(int?))]
        [InlineData(42, typeof(TestEnum?))]
        public void RejectsUnresolvableInputsAndNonEnumTargets(object input, Type targetType)
        {
            var converter = new EnumValueToDescriptionConverter();

            Assert.Throws<ArgumentException>(() => converter.ConvertBack(input, targetType, null, null));
        }

        [Fact]
        public void ThrowsWhenConvertsDescriptionToEnumValue()
        {
            const string input = "Invalid description";
            
            var converter = new EnumValueToDescriptionConverter();
            
            Assert.Throws<ArgumentException>(() => 
                converter.ConvertBack(input, typeof(TestEnum), null, null));
        }
    }
}
