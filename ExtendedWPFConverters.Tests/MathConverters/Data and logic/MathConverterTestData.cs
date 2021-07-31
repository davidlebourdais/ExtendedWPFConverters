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
            new object[] { 2, 1 },
            new object[] { 5, 1, 6, 8 },
            new object[] { 2, 2, 1, 9, 20 },

            // Equals:
            new object[] { 1, 1 },
            new object[] { 50, 50 },
            new object[] { 8, 8, 8, 8, 8 },

            // Zeroed:
            new object[] { 1, 0 },
            new object[] { 0, 5 },
            new object[] { 3, 0, 2, 3, 4 },
            new object[] { 0, 0, 0, 0 },

            // Negative:
            new object[] { -1, 2 },
            new object[] { 1, -2 },
            new object[] { -1, -2 },
            new object[] { -2, -10 },
            new object[] { 1, -5, -0 },
            new object[] { -1, -3, -10, 11 },
            
            /* double */
            // Base cases:
            new object[] { 1.2, 2.5 },
            new object[] { 10.25, 2.1235 },
            new object[] { 2.5, 2.3, 1.2, 4.1 },
            new object[] { 0.1255, 10.25, 1.92, 0.25 },

            // Equals:
            new object[] { 1.2, 1.2 },
            new object[] { 10.25, 10.25 },
            new object[] { 2.1, 2.1, 2.1, 2.1 },

            // Zeroed:
            new object[] { 1.85, 0.0d },
            new object[] { 0, 5.1 },
            new object[] { 0.0, 4.1d, 5.8d, 0.0d },
            new object[] { 0.0d, 0.0d, 0.0d, 0.0d, 0.0d },

            // Negative:
            new object[] { -1.2, 2.5 },
            new object[] { -2.5, 1.2 },
            new object[] { -2.5, -1.2 },
            new object[] { 10.25, 2.1235 },
            new object[] { -2.1235, -2.5, -10.25, -1.20 },

            // Exp:
            new object[] { 1E-6, -1E3 },
            new object[] { -1E-5, 3E2 },
            new object[] { 1E-1, -1E1, 10E-1, 10E1 },
            new object[] { 2E9, 23E-9, 12E2 },

            // // Extrema:
            new object[] { double.MinValue, 0.0 },
            new object[] { double.MinValue, 5.2 },
            new object[] { double.MinValue, -6.2 },
            new object[] { 0, double.MinValue},
            new object[] { 5.2, double.MinValue},
            new object[] { -6.2, double.MinValue},
            new object[] { double.MaxValue, 0 },
            new object[] { double.MaxValue, 5.2 },
            new object[] { double.MaxValue, -6.2 },
            new object[] { 0, double.MaxValue},
            new object[] { 5.2, double.MaxValue},
            new object[] { -6.2, double.MaxValue},
            new object[] { double.MaxValue, double.MinValue },
            new object[] { double.MinValue, double.MaxValue },
            new object[] { double.MinValue, double.MinValue, double.MinValue, double.MinValue },
            new object[] { double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue },

            // // Invalid:
            new object[] { 1.2, null },
            new object[] { null, 2.5 },
            new object[] { null, null },
            new object[] { 1.2, "invalid" },
            new object[] { "invalid", 2.5 },
            new object[] { "invalid", "invalid2" },
            new object[] { 12.5, "invalid", 12.6 },
            new object[] { 6.5, null, 6.6 },
            new object[] { null, null, null, null },
        };
    }
}
