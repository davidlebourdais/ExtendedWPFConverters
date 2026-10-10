using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class ThicknessMathConverterTests
    {
        [Theory]
        [InlineData(MathOperation.None, -4d, 2d, 6d, 8d)]
        [InlineData(MathOperation.Add, -2d, 4d, 8d, 10d)]
        [InlineData(MathOperation.Subtract, -6d, 0d, 4d, 6d)]
        [InlineData(MathOperation.SubtractPositiveOnly, 0d, 0d, 4d, 6d)]
        [InlineData(MathOperation.Multiply, -8d, 4d, 12d, 16d)]
        [InlineData(MathOperation.Divide, -2d, 1d, 3d, 4d)]
        [InlineData(MathOperation.Modulo, 0d, 0d, 0d, 0d)]
        [InlineData(MathOperation.Power, 16d, 4d, 36d, 64d)]
        [InlineData(MathOperation.Absolute, 4d, 2d, 6d, 8d)]
        public void AppliesOperationToEverySide(MathOperation operation, double left, double top, double right, double bottom)
        {
            var converter = new ThicknessMathConverter() { Operation = operation };

            var result = converter.Convert(new Thickness(-4d, 2d, 6d, 8d), typeof(Thickness), 2d, null);

            Assert.Equal(new Thickness(left, top, right, bottom), result);
        }

        [Fact]
        public void ComputesModuloSeparatelyForEverySide()
        {
            var converter = new ThicknessMathConverter() { Operation = MathOperation.Modulo };

            var result = converter.Convert(new Thickness(-5d, 4d, 7d, 11d), typeof(Thickness), 3d, null);

            Assert.Equal(new Thickness(-2d, 1d, 1d, 2d), result);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(2d)]
        [InlineData("2")]
        [InlineData("2.0")]
        [InlineData("2,0")]
        public void AcceptsNumericScalarParameters(object parameter)
        {
            var converter = new ThicknessMathConverter() { Operation = MathOperation.Multiply };

            var result = converter.Convert(new Thickness(1d, 2d, 3d, 4d), typeof(Thickness), parameter,
                CultureInfo.GetCultureInfo("fr-FR"));

            Assert.Equal(new Thickness(2d, 4d, 6d, 8d), result);
        }

        [Theory]
        [InlineData(MathOperation.None)]
        [InlineData(MathOperation.Absolute)]
        public void IgnoresParameterForUnaryOperations(MathOperation operation)
        {
            var converter = new ThicknessMathConverter() { Operation = operation };
            var input = new Thickness(-1d, 2d, -3d, 4d);
            var expected = operation == MathOperation.None ? input : new Thickness(1d, 2d, 3d, 4d);

            Assert.Equal(expected, converter.Convert(input, typeof(Thickness), null, null));
            Assert.Equal(expected, converter.Convert(input, typeof(Thickness), new object(), null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("1,2,3,4")]
        [InlineData(2d)]
        public void ReturnsConfiguredFallbackForInvalidInput(object input)
        {
            var converter = new ThicknessMathConverter() { ValueForInvalid = Binding.DoNothing };

            Assert.Same(Binding.DoNothing, converter.Convert(input, typeof(Thickness), null, null));
            Assert.Same(Binding.DoNothing, converter.ConvertBack(input, typeof(Thickness), null, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalid")]
        [InlineData(true)]
        public void ReturnsConfiguredFallbackForInvalidParameter(object parameter)
        {
            var converter = new ThicknessMathConverter()
            {
                Operation = MathOperation.Add,
                ValueForInvalid = Binding.DoNothing
            };
            var input = new Thickness(1d, 2d, 3d, 4d);

            Assert.Same(Binding.DoNothing, converter.Convert(input, typeof(Thickness), parameter, null));
            Assert.Same(Binding.DoNothing, converter.ConvertBack(input, typeof(Thickness), parameter, null));
        }

        [Fact]
        public void RejectsThicknessParameter()
        {
            var converter = new ThicknessMathConverter() { Operation = MathOperation.Add };

            Assert.Null(converter.Convert(new Thickness(1d), typeof(Thickness), new Thickness(2d), null));
            Assert.Null(converter.ConvertBack(new Thickness(1d), typeof(Thickness), new Thickness(2d), null));
        }

        [Theory]
        [InlineData(MathOperation.None)]
        [InlineData(MathOperation.Add)]
        [InlineData(MathOperation.Subtract)]
        [InlineData(MathOperation.Multiply)]
        [InlineData(MathOperation.Divide)]
        public void ConvertsBackToOriginalThickness(MathOperation operation)
        {
            var converter = new ThicknessMathConverter() { Operation = operation };
            var input = new Thickness(-4d, 2d, 6d, 8d);
            var parameter = operation == MathOperation.None ? null : "2";

            var converted = converter.Convert(input, typeof(Thickness), parameter, null);
            var result = converter.ConvertBack(converted, typeof(Thickness), parameter, null);

            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(MathOperation.Multiply)]
        [InlineData(MathOperation.Divide)]
        public void ReturnsConfiguredFallbackWhenInverseParameterIsZero(MathOperation operation)
        {
            var converter = new ThicknessMathConverter()
            {
                Operation = operation,
                ValueForInvalid = Binding.DoNothing
            };

            Assert.Same(Binding.DoNothing,
                converter.ConvertBack(new Thickness(1d, 2d, 3d, 4d), typeof(Thickness), "0", null));
        }

        [Theory]
        [InlineData(MathOperation.Absolute)]
        [InlineData(MathOperation.SubtractPositiveOnly)]
        [InlineData(MathOperation.Modulo)]
        [InlineData(MathOperation.Power)]
        public void RejectsBackwardConversionWithoutUnambiguousInverse(MathOperation operation)
        {
            var converter = new ThicknessMathConverter() { Operation = operation };

            Assert.Throws<NotSupportedException>(() =>
                converter.ConvertBack(new Thickness(1d), typeof(Thickness), 2d, null));
        }

        [Fact]
        public void PreservesDivisionByZeroBehaviorOfMathConverter()
        {
            var converter = new ThicknessMathConverter() { Operation = MathOperation.Divide };

            var result = Assert.IsType<Thickness>(
                converter.Convert(new Thickness(-1d, 0d, 1d, 2d), typeof(Thickness), 0d, null));

            Assert.Equal(double.NegativeInfinity, result.Left);
            Assert.True(double.IsNaN(result.Top));
            Assert.Equal(double.PositiveInfinity, result.Right);
            Assert.Equal(double.PositiveInfinity, result.Bottom);
        }

        [Fact]
        public void RejectsUnsupportedOperation()
        {
            var converter = new ThicknessMathConverter() { Operation = (MathOperation)999 };

            Assert.Throws<NotSupportedException>(() =>
                converter.Convert(new Thickness(1d), typeof(Thickness), 2d, null));
            Assert.Throws<NotSupportedException>(() =>
                converter.ConvertBack(new Thickness(1d), typeof(Thickness), 2d, null));
        }

        [Fact]
        public void UsesNoneAndNullFallbackByDefault()
        {
            var converter = new ThicknessMathConverter();

            Assert.Equal(MathOperation.None, converter.Operation);
            Assert.Null(converter.ValueForInvalid);
            Assert.Null(converter.Convert(null, typeof(Thickness), null, null));
        }

        [Fact]
        public void CanBeCreatedAsInlineMarkupExtension()
        {
            const string xaml = "<Binding xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                "xmlns:extconv=\"clr-namespace:EMA.ExtendedWPFConverters;assembly=ExtendedWPFConverters\" " +
                "Converter=\"{extconv:ThicknessMathConverter Operation=Multiply}\" ConverterParameter=\"2\" />";

            var binding = Assert.IsType<Binding>(XamlReader.Parse(xaml));

            var converter = Assert.IsType<ThicknessMathConverter>(binding.Converter);
            Assert.Equal(MathOperation.Multiply, converter.Operation);
            Assert.Equal("2", binding.ConverterParameter);
            Assert.Same(converter, converter.ProvideValue(null));
        }
    }
}
