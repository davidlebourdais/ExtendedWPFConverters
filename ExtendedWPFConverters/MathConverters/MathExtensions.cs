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

            if (GetGreatestCommonDivisor(a, m) != 1)
                return false;

            var m0 = m; 
            y = 0;
            x = 1; 
    
            if (m != 1)
            {
                while (a > 1) 
                { 
                    // q is quotient 
                    var q = a / m; 
        
                    var t = m; 
        
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
        
        private static int GetGreatestCommonDivisor(int a, int b)
        {
            while (true)
            {
                if (a == 0)
                    return b;
                
                var a1 = a;
                a = b % a;
                b = a1;
            }
        }
    }
}
