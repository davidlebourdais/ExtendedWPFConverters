using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EMA.ExtendedWPFConverters.Tests.Data
{
    /// <summary>
    /// Provides data to be used to data-driven tests on NotNullOrEmptyString converters
    /// with some test logic to verify mathematical operations.
    /// </summary>
    public static class NotNullOrEmptyStringToVisibilityConverterWithActivatorsTestDataProvider
    {
        #region Test helpers
        public static bool Operate(BooleanOperation operation, bool[] values)
            => BooleanConvertersForMultibindingTestsBase.Operate(operation, values); // redirect to boolean to boolean operations.
        #endregion

        #region Test data
        public static IEnumerable<object[]> ConvertTestDataForMultibinding => GenerateConvertTestDataForMultibinding();

        private static IEnumerable<object[]> InputData => new List<object[]>()
        {
                         /* input, ValueForNotNullOrEmpty, ValueForNullOrEmpty */
            new object[] { "not null", Visibility.Visible, Visibility.Collapsed },
            new object[] { "not null", Visibility.Visible, Visibility.Hidden },
            new object[] { "not null", Visibility.Collapsed, Visibility.Visible },
            new object[] { "not null", Visibility.Collapsed, Visibility.Hidden },
            new object[] { "not null", Visibility.Hidden, Visibility.Visible },
            new object[] { "not null", Visibility.Hidden, Visibility.Collapsed },
            new object[] { "not null", Visibility.Visible, Visibility.Visible },
            new object[] { true, Visibility.Visible, Visibility.Collapsed },
            new object[] { 123, Visibility.Visible, Visibility.Collapsed },
            new object[] { null, Visibility.Visible, Visibility.Collapsed },
            new object[] { null, Visibility.Visible, Visibility.Hidden },
            new object[] { null, Visibility.Collapsed, Visibility.Visible },
            new object[] { null, Visibility.Collapsed, Visibility.Hidden },
            new object[] { null, Visibility.Hidden, Visibility.Visible },
            new object[] { null, Visibility.Hidden, Visibility.Collapsed },
            new object[] { null, Visibility.Visible, Visibility.Visible },
        };

        private static IEnumerable<object[]> BooleanData => new List<object[]>()
        {
            new object[] { },
            new object[] { false },
            new object[] { true },
            new object[] { false, false },
            new object[] { false, true },
            new object[] { true, false },
            new object[] { true, true },
            new object[] { false, false, false },
            new object[] { false, true, false },
            new object[] { true, true, true },
            new object[] { false, false, true, true, true },
            new object[] { true, true, true, true, true },
            new object[] { false, null, false },
            new object[] { false, false, "invalid" },
            new object[] { null, null },
            new object[] { true, null, "invalid" },
        };

        private static IEnumerable<Visibility> ValuesForInvalid => new List<Visibility> { Visibility.Visible, Visibility.Collapsed };

        private static IEnumerable<BooleanOperation> Operations { get; } = Enum.GetValues(typeof(BooleanOperation)).Cast<BooleanOperation>().ToList();
        #endregion

        #region Test data generator
        private static IEnumerable<object[]> GenerateConvertTestDataForMultibinding()
        {
            var toReturn = new List<object[]>();

            foreach (var inputs in InputData)
                foreach (var booleans in BooleanData)
                    foreach (var operation in Operations)
                        foreach (var invalid in ValuesForInvalid)
                        {
                            var value = inputs[0];
                            var valueForNotNullOrEmpty = inputs[1];
                            var valueForNullOrEmpty = inputs[2];
                            object result = invalid;

                            // Restructure input data to match converter input for multibinding:
                            var rawInputs = new List<object>() { value };
                            rawInputs.AddRange(booleans);

                            // Precalculate result if possible:
                            var isNotNullOREmpty = !string.IsNullOrEmpty(value as string);
                            if (!booleans.Any())
                                result = isNotNullOREmpty ? valueForNotNullOrEmpty : valueForNullOrEmpty;
                            else if (booleans.All(x => x is bool))
                                result = Operate(operation, booleans.Cast<bool>().ToArray()) && isNotNullOREmpty ? valueForNotNullOrEmpty : valueForNullOrEmpty; 

                            toReturn.Add(new[] { rawInputs, operation, valueForNotNullOrEmpty, valueForNullOrEmpty, invalid, result });
                        }

            return toReturn;
        }
        #endregion
    }
}
