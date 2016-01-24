/*
 * Note: The easing equations have an additional license.
 * 
 * Copyright (c) 2015 Stephen A. Pratt
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
 * 
 * *************************************************************************************************
 * 
 * TERMS OF USE - EASING EQUATIONS
 * 
 * Open source under the BSD License.
 * 
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted
 * provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions
 * and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials provided
 * with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse or
 * promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS
 * OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
 * AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER 
 * OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 * 
 * *************************************************************************************************
 * 
 * Lizitt Notes: 
 * 
 * While labeled as the BSD License, the above license is more commonly known as the Modified BSD 
 * License.
 * 
 * The section of source that includes code covered by the above license is contained in the region
 * labeled "Easing Curves".  Much of the code within this section is significantly modified.  
 * So it is not the original code and any ulgliness is not necessarily from the original author.
 * 
 */
using UnityEngine;

namespace com.lizitt
{
    /// <summary>
    /// A standard easing function.
    /// </summary>
    /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
    /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
    /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
    /// <returns>The interpolated value.</returns>
    public delegate float EaseFunction(float start, float end, float time);

    /// <summary>
    /// Provides various easing agorithms.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Most of these functions are similar or the same as the common easing functions found in 
    /// CSS3, Javascript, jQuery, etc. Examples of most of the algorithms can be found at 
    /// <a href="http://easings.net/">easings.net</a>.  You can hover over the example graphs 
    /// to see a demonstration the algorithm.  More details and links can be found at
    /// <a href="http://www.robertpenner.com/easing/">Robert Penner's Easing Site</a>.  
    /// </para>
    /// <para>
    /// The general naming convention uses 'in' to mean 'special behavior at the start' and 
    /// 'out' to mean 'special behavior change at the end'.  For example, an algorithm may
    /// have slower change at the beginning, special occilations at the end, etc.
    /// </para>
    /// </remarks>
    public static class Easing
    {
        /// <summary>
        /// Retrieves the function for the specified ease type.
        /// </summary>
        /// <param name="type">The function type to retrieve.</param>
        /// <returns>The function for the specified ease type.</returns>
        public static EaseFunction GetFunction(EaseType type)
        {
            switch (type)
            {
                case EaseType.Linear:
                    return Mathf.Lerp;
                case EaseType.SmoothStep:
                    return Mathf.SmoothStep;
                case EaseType.EaseInQuad:
                    return EaseInQuad;
                case EaseType.EaseOutQuad:
                    return EaseOutQuad;
                case EaseType.EaseInOutQuad:
                    return EaseInOutQuad;
                case EaseType.EaseInCubic:
                    return EaseInCubic;
                case EaseType.EaseOutCubic:
                    return EaseOutCubic;
                case EaseType.EaseInOutCubic:
                    return EaseInOutCubic;
                case EaseType.EaseInQuart:
                    return EaseInQuart;
                case EaseType.EaseOutQuart:
                    return EaseOutQuart;
                case EaseType.EaseInOutQuart:
                    return EaseInOutQuart;
                case EaseType.EaseInQuint:
                    return EaseInQuint;
                case EaseType.EaseOutQuint:
                    return EaseOutQuint;
                case EaseType.EaseInOutQuint:
                    return EaseInOutQuint;
                case EaseType.EaseInSine:
                    return EaseInSine;
                case EaseType.EaseOutSine:
                    return EaseOutSine;
                case EaseType.EaseInOutSine:
                    return EaseInOutSine;
                case EaseType.EaseInExpo:
                    return EaseInExpo;
                case EaseType.EaseOutExpo:
                    return EaseOutExpo;
                case EaseType.EaseInOutExpo:
                    return EaseInOutExpo;
                case EaseType.EaseInCirc:
                    return EaseInCirc;
                case EaseType.EaseOutCirc:
                    return EaseOutCirc;
                case EaseType.EaseInOutCirc:
                    return EaseInOutCirc;
                case EaseType.Spring:
                    return Spring;
                case EaseType.EaseInBounce:
                    return EaseInBounce;
                case EaseType.EaseOutBounce:
                    return EaseOutBounce;
                case EaseType.EaseInOutBounce:
                    return EaseInOutBounce;
                case EaseType.EaseInBack:
                    return EaseInBack;
                case EaseType.EaseOutBack:
                    return EaseOutBack;
                case EaseType.EaseInOutBack:
                    return EaseInOutBack;
                case EaseType.EaseInElastic:
                    return EaseInElastic;
                case EaseType.EaseOutElastic:
                    return EaseOutElastic;
                case EaseType.EaseInOutElastic:
                    return EaseInOutElastic;
                case EaseType.CircularLerp:
                    return Clerp;
            }
             
            throw new System.InvalidOperationException(
                "Internal error: Unrecognized ease type: " + type);
        }

        /// <summary>
        /// Circular lerp - Liner interpolation with wrapping at the 0 and 360 boundaries.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Useful for interpolating angles such as Transform.eulerAngles so easing will
        /// occur in the correct (shortest) direction.
        /// </para>
        /// </remarks>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to - Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float Clerp(float start, float end, float value)
        {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);
            float retval = 0.0f;
            float diff = 0.0f;
            if ((end - start) < -half)
            {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;
            return retval;
        }

        #region Easing Curves  (Covered by the Easing Equations License)

        // Note: The structure of these methods (names and parameters) have been altered from 
        // their original form.

        /// <summary>
        /// Spring easing.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float Spring(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) 
                * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        /// <summary>
        /// Quadratic easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInQuad(float start, float end, float value)
        {
            end -= start;
            return end * value * value + start;
        }

        /// <summary>
        /// Quadratic easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutQuad(float start, float end, float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        /// <summary>
        /// Quadratic easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutQuad(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value + start;
            value--;
            return -end / 2 * (value * (value - 2) - 1) + start;
        }

        /// <summary>
        /// Cubic easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInCubic(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value + start;
        }


        /// <summary>
        /// Cubic easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutCubic(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        /// <summary>
        /// Cubic easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutCubic(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value * value + start;
            value -= 2;
            return end / 2 * (value * value * value + 2) + start;
        }

        /// <summary>
        /// Quartic easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInQuart(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value * value + start;
        }

        /// <summary>
        /// Quartic easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutQuart(float start, float end, float value)
        {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        /// <summary>
        /// Quartic easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutQuart(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value * value * value + start;
            value -= 2;
            return -end / 2 * (value * value * value * value - 2) + start;
        }

        /// <summary>
        /// Quintic easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInQuint(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        /// <summary>
        /// Quintic easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutQuint(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        /// <summary>
        /// Quintic easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutQuint(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * value * value * value * value * value + start;
            value -= 2;
            return end / 2 * (value * value * value * value * value + 2) + start;
        }

        /// <summary>
        /// Sinusoidal (Sine) easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInSine(float start, float end, float value)
        {
            end -= start;
            return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
        }

        /// <summary>
        /// Sinusoidal (Sine) easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutSine(float start, float end, float value)
        {
            end -= start;
            return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
        }

        /// <summary>
        /// Sinusoidal (Sine) easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutSine(float start, float end, float value)
        {
            end -= start;
            return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
        }

        /// <summary>
        /// Exponential easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInExpo(float start, float end, float value)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
        }

        /// <summary>
        /// Exponential easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutExpo(float start, float end, float value)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
        }

        /// <summary>
        /// Exponential easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutExpo(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        /// <summary>
        /// Circular easing - Slower at start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInCirc(float start, float end, float value)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        /// <summary>
        /// Circular easing - Slower at end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutCirc(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        /// <summary>
        /// Circular easing - Slower at start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutCirc(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        /// <summary>
        /// Bounce easing - Small bounces nearer to start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        /// <summary>
        /// Bounce easing - Small bounces nearer to end, and inverted.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutBounce(float start, float end, float value)
        {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        /// <summary>
        /// Bounce easing - <see cref="EaseInBounce"/> appended to <see cref="EaseOutBounce"/>.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            if (value < d / 2) return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        /// <summary>
        /// Ease back - Overshoot just after the start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInBack(float start, float end, float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        /// <summary>
        /// Ease back -  - Overshoot just before the end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value / 1) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        /// <summary>
        /// Ease back - Overshoot near the start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        /// <summary>
        /// Elastic ease - Bouncing with overshoot near the start.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) 
                * (2 * Mathf.PI) / p)) + start;
        }

        /// <summary>
        /// Elastic ease - Bouncing with overshoot near the end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) 
                * (2 * Mathf.PI) / p) + end + start);
        }

        /// <summary>
        /// Elastic ease - Bouncing with overshoot near the start and end.
        /// </summary>
        /// <param name="start">The value to ease from. (Value at <see cref="time"/> 0.0)</param>
        /// <param name="end">The value to ease to.  (Value at <see cref="time"/> 1.0)</param>
        /// <param name="time">The normalized time. [Limits: 0 &lt;= 0 &lt;= 1]</param>
        /// <returns>The interpolated value.</returns>
        public static float EaseInOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d / 2) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1)
            {
                return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1))
                    * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
            }

            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) 
                * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }

        #endregion
    }
}

