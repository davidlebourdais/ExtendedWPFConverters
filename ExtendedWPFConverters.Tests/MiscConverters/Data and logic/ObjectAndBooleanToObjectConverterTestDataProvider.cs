using System;
using System.Collections.Generic;
using System.Linq;

namespace EMA.ExtendedWPFConverters.Tests.Data
{
    /// <summary>
    /// Provides data to be used to data-driven tests on ObjectAndBooleanToObjectConverter
    /// with some test logic to verify mathematical operations.
    /// </summary>
    public static class ObjectAndBooleanToObjectConverterTestDataProvider
    {
        #region Test helpers
        public static bool Operate(BooleanOperation operation, bool[] values)
            => BooleanConvertersForMultibindingTestsBase.Operate(operation, values); // redirect to boolean to boolean operations.
        #endregion

        #region Test data
        public static IEnumerable<object[]> ConvertTestDataForMultibinding => GenerateConvertTestDataForMultibinding();

        private static IEnumerable<object[]> InputData => new List<object[]>()
        {
            new object[] { "string value" },
            new object[] { 123 },
            new object[] { TimeSpan.FromDays(1) },
            new object[] { new Random() },
            new object[] { new List<double>() },
            new object[] { null }
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

        private static IEnumerable<object> ValuesForInvalid => new List<object> { "invalid", null };

        private static List<BooleanOperation> Operations { get; } = Enum.GetValues(typeof(BooleanOperation)).Cast<BooleanOperation>().ToList();
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
                            object result = invalid;

                            // Restructure input data to match converter input for multibinding:
                            var rawInputs = new List<object>() { value };
                            foreach (var bool_entry in booleans)
                                rawInputs.Add(bool_entry);

                            // Precalculate result if possible:
                            if (!booleans.Any())
                                result = value;
                            else if (booleans.All(x => x is bool))
                                result = Operate(operation, booleans.Cast<bool>().ToArray()) ? value : null; 

                            toReturn.Add(new object[] { rawInputs, operation, invalid, result });
                        }

            return toReturn;
        }
        #endregion
    }
}
