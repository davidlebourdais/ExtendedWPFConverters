using System;
using System.Collections.Generic;

namespace EMA.ExtendedWPFConverters.Tests.Data
{
    /// <summary>
    /// A set of numerical and invalid data to be used as bare inputs
    /// for MathConverter tests.
    /// </summary>
    public static class MathConverterTestData
    {
        public static IEnumerable<object[]> Data => new List<object[]>()
        {
            /* Integers */
            // Base cases:
            new object[] { 3, 5, },
            new object[] { 5, 3 },
            new object[] { 1, 2 },
            new object[] { 2, 1 },
            new object[] { 3, 6, 5 },
            new object[] { 5, 1, 6, 8 },
            new object[] { 1, 9, 3, 5 },
            new object[] { 2, 2, 1, 9, 20 },

            // Equals:
            new object[] { 1, 1 },
            new object[] { 50, 50 },
            new object[] { 8, 8, 8, 8, 8 },

            // Zeroed:
            new object[] { 1, 0 },
            new object[] { 0, 5 },
            new object[] { 3, 0, 2, 3, 4 },
            new object[] { 4, 0, 4, 0, 5 },
            new object[] { 0, 0, 0, 0 },

            // Negative:
            new object[] { -1, 2 },
            new object[] { 1, -2 },
            new object[] { -1, -2 },
            new object[] { -2, -10 },
            new object[] { 1, -5, -0 },
            new object[] { 1, -3, 3, -5 },
            new object[] { -1, -3, -10, 11 },
            new object[] { -420, -10, -3, -4, -2 },
            
            /* Double */
            // Base cases:
            new object[] { 1.2, 2.5 },
            new object[] { 2.5, 1.2 },
            new object[] { 10.25, 2.1235 },
            new object[] { 2.1235, 10.25 },
            new object[] { 1.2, 5.3, 2.5, 6.1 },
            new object[] { 2.5, 2.3, 1.2, 4.1 },
            new object[] { 10.25, 5.1231, 2.1235 },
            new object[] { 0.1255, 10.25, 1.92, 0.25 },

            // Equals:
            new object[] { 1.2, 1.2 },
            new object[] { 10.25, 10.25 },
            new object[] { 2.1, 2.1, 2.1, 2.1 },

            // Zeroed:
            new object[] { 1.85, 0.0d },
            new object[] { 0, 5.1 },
            new object[] { 0.0, 4.1d, 5.8d, 0.0d },
            new object[] { 2.95, 0.0d, 6.8d, 4.2d },
            new object[] { 0.0d, 0.0d, 0.0d, 0.0d, 0.0d },

            // Negative:
            new object[] { -1.2, 2.5 },
            new object[] { -2.5, 1.2 },
            new object[] { -2.5, -1.2 },
            new object[] { 10.25, 2.1235 },
            new object[] { -2.1235, -10.25 },
            new object[] { 2.5, -1.2, -1.45 },
            new object[] { -2.1235, -2.5, -10.25, -1.20 },

            // Exp:
            new object[] { 1E-6, -1E3 },
            new object[] { -1E-5, 3E2 },
            new object[] { 1E-6, -1E3 },
            new object[] { 1E-1, -1E1, 10E-1, 10E1 },
            new object[] { 2E9, 23E-9, 12E2 },

            // // Extrema:
            new object[] { Double.MinValue, 0.0 },
            new object[] { Double.MinValue, 5.2 },
            new object[] { Double.MinValue, -6.2 },
            new object[] { 0, Double.MinValue},
            new object[] { 5.2, Double.MinValue},
            new object[] { -6.2, Double.MinValue},
            new object[] { Double.MaxValue, 0 },
            new object[] { Double.MaxValue, 5.2 },
            new object[] { Double.MaxValue, -6.2 },
            new object[] { 0, Double.MaxValue},
            new object[] { 5.2, Double.MaxValue},
            new object[] { -6.2, Double.MaxValue},
            new object[] { Double.MaxValue, Double.MinValue },
            new object[] { Double.MinValue, Double.MaxValue },
            new object[] { Double.MinValue, Double.MinValue, double.MinValue, double.MinValue },
            new object[] { Double.MaxValue, Double.MaxValue, double.MaxValue, double.MaxValue },

            // // Invalid:
            new object[] { 1.2, null },
            new object[] { null, 2.5 },
            new object[] { null, null },
            new object[] { 1.2, "invalid" },
            new object[] { "invalid", 2.5 },
            new object[] { "invalid", "invalid" },
            new object[] { 12.5, "invalid", 12.6 },
            new object[] { "invalid1", "invalid2", "invalid3", "invalid4" },
            new object[] { 6.5, null, 6.6 },
            new object[] { null, null, null, null },
        };
    }
}
