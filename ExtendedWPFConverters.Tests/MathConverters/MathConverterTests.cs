using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Xunit;
using EMA.ExtendedWPFConverters.Tests.Data;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class MathConverterTests
    {
        [Theory]
        [MemberData(nameof(MathConverterTestDataProvider.ConvertTestData), MemberType= typeof(MathConverterTestDataProvider))]
        public void ConvertsWithMathOperation(object input, object parameter, CultureInfo culture, MathOperation operation, object valueForInvalid, bool outputAsString, object expected)
        {
            var converter = new MathConverter() { Operation = operation, OutputAsString = outputAsString, ValueForInvalid = valueForInvalid };
            var result = converter.Convert(input, input?.GetType(), parameter, culture);
            
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(MathConverterTestDataProvider.ConvertBackTestData), MemberType= typeof(MathConverterTestDataProvider))]
        public void ConvertsBackWithMathOperation(object input, object parameter, CultureInfo culture, MathOperation operation, object valueForInvalid, object expected)
        {
            var converter = new MathConverter() { Operation = operation, ValueForInvalid = valueForInvalid };
            // ConvertBack targets the source property type, regardless of the incoming value's type.
            var result = converter.ConvertBack(input, typeof(double), parameter, culture);
            
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("", true)]
        [InlineData("fr-FR", false)]
        [InlineData("fr-FR", true)]
        public void ReverseOutputUsesSourceTypeRegardlessOfPreviousForwardCalls(string cultureName, bool outputAsString)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            var converter = new MathConverter { Operation = MathOperation.Add, OutputAsString = outputAsString };
            var input = 7.5d.ToString(culture);

            // Reverse conversion also works before any forward conversion has occurred.
            Assert.Equal(5.5d.ToString(culture), Assert.IsType<string>(converter.ConvertBack(input, typeof(string), 2d, culture)));
            Assert.Equal(5.5d, Assert.IsType<double>(converter.ConvertBack(input, typeof(double), 2d, culture)));

            foreach (var forwardInput in new object[] { "9.5", 9.5d, null, "invalid" })
            {
                converter.Convert(forwardInput, typeof(object), 2d, culture);
                Assert.Equal(5.5d.ToString(culture), Assert.IsType<string>(converter.ConvertBack(input, typeof(string), 2d, culture)));
                Assert.Equal(5.5d, Assert.IsType<double>(converter.ConvertBack(input, typeof(double), 2d, culture)));
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(object))]
        [InlineData(typeof(double))]
        [InlineData(typeof(double?))]
        [InlineData(typeof(int))]
        public void OtherReverseTargetsRetainDoubleOutput(Type targetType)
        {
            var converter = new MathConverter();
            converter.Convert("5.5", typeof(double), null, null);

            Assert.Equal(5.5d, Assert.IsType<double>(converter.ConvertBack("5.5", targetType, null, null)));
        }

        [Theory]
        [InlineData(typeof(double), false, null)]
        [InlineData(typeof(string), false, "fr-FR")]
        [InlineData(typeof(string), true, null)]
        [InlineData(typeof(object), true, "fr-FR")]
        public void ModuloReverseAlwaysReturnsBindingDoNothing(Type targetType, bool outputAsString, string cultureName)
        {
            var converter = new MathConverter { Operation = MathOperation.Modulo, OutputAsString = outputAsString };
            var culture = cultureName == null ? null : CultureInfo.GetCultureInfo(cultureName);
            var inputs = new[]
            {
                new object[] { 3d, 5d }, new object[] { 2d, 4d }, new object[] { -3d, -5d },
                new object[] { 1.5d, 2.5d }, new object[] { 3d, 0d },
                new object[] { double.NaN, double.PositiveInfinity }, new object[] { double.MaxValue, 5d },
                new object[] { "1,5", "2,5" }, new object[] { null, 5d }, new object[] { 3d, null },
                new object[] { "invalid", "invalid" }, new object[] { new object(), new object() },
                new object[] { DependencyProperty.UnsetValue, Binding.DoNothing }
            };

            foreach (var operands in inputs)
                Assert.Same(Binding.DoNothing, converter.ConvertBack(operands[0], targetType, operands[1], culture));

            converter.ValueForInvalid = new object();
            foreach (var operands in inputs)
                Assert.Same(Binding.DoNothing, converter.ConvertBack(operands[0], targetType, operands[1], culture));
        }

        [Fact]
        public void ModuloStillComputesForwardRemaindersAndUsesConfiguredInvalidFallback()
        {
            var fallback = new object();
            var converter = new MathConverter { Operation = MathOperation.Modulo, ValueForInvalid = fallback };

            Assert.Equal(3d, Assert.IsType<double>(converter.Convert(8d, typeof(double), 5d, null)));
            Assert.Equal(3d, Assert.IsType<double>(converter.Convert(13d, typeof(double), 5d, null)));
            Assert.Equal(1.5d, Assert.IsType<double>(converter.Convert(6.5d, typeof(double), 2.5d, null)));
            Assert.Same(fallback, converter.Convert(null, typeof(double), 5d, null));
            Assert.Same(fallback, converter.Convert(8d, typeof(double), "invalid", null));
        }

        [Fact]
        public void SharedConverterKeepsEachBindingsReverseTypeAndParameter()
        {
            var converter = new MathConverter { Operation = MathOperation.Add };
            converter.Convert("5.5", typeof(double), 2d, null);
            converter.Convert(10d, typeof(double), 3d, null);

            Assert.Equal("6.5", Assert.IsType<string>(converter.ConvertBack(8.5d, typeof(string), 2d, null)));

            converter.Convert("6.5", typeof(double), 2d, null);
            Assert.Equal(11d, Assert.IsType<double>(converter.ConvertBack(14d, typeof(double), 3d, null)));
        }
    }
}
