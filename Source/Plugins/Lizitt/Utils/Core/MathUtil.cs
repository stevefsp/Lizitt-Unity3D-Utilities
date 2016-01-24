/*
 * Copyright (c) 2010-2015 Stephen A. Pratt
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

namespace com.lizitt
{
    /// <summary>
    /// Provides various math related constants and utility features.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Static methods are thread safe.
    /// </para>
    /// </remarks>
    public static class MathUtil 
    {
        /// <summary>
        /// A default epsilon value.  (Minimum positive value.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Useful as a minimum for things like editors.
        /// </para>
        /// </remarks>
        public const float Epsilon = 0.00001f;

        /// <summary>
        /// A default comparison tolerance.
        /// </summary>
        public const float Tolerance = 0.0001f;
        
        /// <summary>
        /// Determines whether the values are within the specified tolerance of each other.
        /// </summary>
        /// <param name="a">The a-value to compare against the b-value.</param>
        /// <param name="b">The b-value to compare against the a-value.</param>
        /// <param name="tolerance">The tolerance to use for the comparison.</param>
        /// <returns>True if the values are within the specified tolerance of each other.</returns>
        public static bool SloppyEquals(this float a, float b, float tolerance = Tolerance)
        {
            return !(b < a - tolerance || b > a + tolerance);
        }
    }
}
