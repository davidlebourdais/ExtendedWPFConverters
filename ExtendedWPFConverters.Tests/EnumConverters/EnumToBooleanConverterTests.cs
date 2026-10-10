using System.ComponentModel;
using System.Windows.Data;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class EnumToBooleanConverterTests
    {
        public enum TestEnum
        {
            [Description("First")]
            First,
            Second
        }

        [Theory]
        [InlineData(TestEnum.First, TestEnum.First, true)]
        [InlineData(TestEnum.First, TestEnum.Second, false)]
        [InlineData(TestEnum.First, "First", true)]
        [InlineData(TestEnum.Second, "second", true)]
        [InlineData(TestEnum.Second, "First", false)]
        public void ConvertsEnumToBoolean(TestEnum input, object parameter, bool expected)
        {
            var converter = new EnumToBooleanConverter();
            var result = converter.Convert(input, typeof(bool), parameter, null);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnsConfiguredValueWhenEnumConversionIsInvalid()
        {
            var converter = new EnumToBooleanConverter() { ValueForInvalid = true };
            var result = converter.Convert("invalid", typeof(bool), "First", null);
            Assert.True((bool)result);
        }

        [Theory]
        [InlineData(TestEnum.First)]
        [InlineData("Second")]
        public void ConvertsBooleanBackToEnum(object parameter)
        {
            var converter = new EnumToBooleanConverter();
            var result = converter.ConvertBack(true, typeof(TestEnum), parameter, null);
            Assert.Equal(parameter is string ? TestEnum.Second : TestEnum.First, result);
        }

        [Fact]
        public void DoesNothingWhenConvertsFalseBackToEnum()
        {
            var converter = new EnumToBooleanConverter();
            var result = converter.ConvertBack(false, typeof(TestEnum), TestEnum.First, null);
            Assert.Same(Binding.DoNothing, result);
        }

        [Fact]
        public void DoesNothingWhenConvertsInvalidParameterBackToEnum()
        {
            var converter = new EnumToBooleanConverter();
            var result = converter.ConvertBack(true, typeof(TestEnum), "unknown", null);
            Assert.Same(Binding.DoNothing, result);
        }
    }
}
