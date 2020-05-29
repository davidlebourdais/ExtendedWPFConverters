namespace EMA.ExtendedWPFConverters.Utils
{
    /// <summary>
    /// A set of mathematical methods to extend the behavior of integers.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Performs a reverse multiplicative modular operation.
        /// </summary>
        /// <params name="a">Base value to invert.</params>
        /// <params name="m">Pivot value against which to find the inverse mod of.</params>
        /// <params name="result">Inverse mod of the passed number a against passed .</params>
        /// <returns>The modular inverse of passed value.</returns>
        /// <remarks>From: https://www.geeksforgeeks.org/multiplicative-inverse-under-modulo-m/</remarks>
        public static bool InverseUnderModulo(this int a, int m, out int result) 
        { 
            result = 0;
            int x = 0, y = 0;

            if (GetGreatestCommonDivisorRecursively(a, m , ref x, ref y) != 1)
                return false;

            int m0 = m; 
            y = 0;
            x = 1; 
    
            if (m != 1)
            {
                while (a > 1) 
                { 
                    // q is quotient 
                    int q = a / m; 
        
                    int t = m; 
        
                    // m is remainder now, process 
                    // same as Euclid's algo 
                    m = a % m; 
                    a = t; 
                    t = y; 
        
                    // Update x and y 
                    y = x - q * y; 
                    x = t; 
                } 
        
                // Make x positive 
                if (x < 0) 
                    x += m0; 
        
                result = x; 
            }
            else result = 0;

            return true;
        }

        /// <summary>
        /// Gets the GCD with a another passed number.
        /// </summary>
        /// <params name="a">Base value.</params>
        /// <params name="m">Facing value to find the GCD from.</params>
        /// <returns>The GCD of both values.</returns>
        
        public static int GetGreatestCommonDivisorWith(this int a, int b) 
        { 
            int x = 0, y = 0;
            return GetGreatestCommonDivisorRecursively(b%a, a, ref x, ref y); 
        } 

        private static int GetGreatestCommonDivisorRecursively(int a, int b, ref int x, ref int y) 
        { 
            // Base Case 
            if (a == 0) 
            { 
                x = 0; y = 1; 
                return b; 
            } 
        
            int x1 = 0, y1 = 0; // To store results of recursive call 
            int gcd = GetGreatestCommonDivisorRecursively(b%a, a, ref x, ref y); 
        
            // Update x and y using results of recursive 
            // call 
            x = y1 - (b/a) * x1; 
            y = x1;
        
            return gcd; 
        } 
    }
}
