/*
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
 */
using UnityEngine;

namespace com.lizitt.u3d
{
    /// <summary>
    /// Interpolates a float over time using an easing function.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To pause the interpolator, simply stop calling the update methods.
    /// </para>
    /// </remarks>
    public class FloatInterpolator
    {
        private float m_Duration = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">The ease mode to use for the interpolation.</param>
        public FloatInterpolator(EaseType mode)
        {
            Function = Easing.GetFunction(mode);
            Duration = 1;
            End = 1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">The ease mode to use for the interpolation.</param>
        /// <param name="from">The value at time 0.</param>
        /// <param name="to">
        /// The value at <see cref="Time"/> >= <see cref="Duration"/>.
        /// </param>
        /// <param name="duration">
        /// The duration of the interpolation in seconds. [Limit: >= 0]
        /// </param>
        public FloatInterpolator(EaseType mode, float from, float to, float duration)
        {
            Function = Easing.GetFunction(mode);
            Reset(from, to, duration);
        }

        /// <summary>
        /// The ease mode being used.
        /// </summary>
        public EaseType Mode { get; private set; }

        /// <summary>
        /// The ease function being used.
        /// </summary>
        public EaseFunction Function { get; private set; }

        /// <summary>
        /// The value at time 0.
        /// </summary>
        public float Start { get; set; }

        /// <summary>
        /// The value at <see cref="Time"/> >= <see cref="Duration"/>.
        /// </summary>
        public float End { get; set; }

        /// <summary>
        /// The normalized time. [Limits: 0 &lt;= value &lt;= 1]
        /// </summary>
        public float NormalizedTime { get; private set; }

        /// <summary>
        /// The current time.  [Limits: 0 &lt;= value]
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value will continue incrementing as long as the update method is called with a
        /// positive deltaTime.  It does not stop at <see cref="Duration"/>.
        /// </para>
        /// </remarks>
        public float Time { get; private set; }

        /// <summary>
        /// The interpolated value associated with <see cref="Time"/>.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// True when the interpolation is complete. (<see cref="Value"/> equals
        /// <see cref="End"/> and <see cref="Time"/> >= <see cref="Duration"/>.
        /// </summary>
        public bool IsComplete
        {
            get { return Time >= Duration; }
        }

        /// <summary>
        /// The duration of the interpolator in seconds. [Limit: >= 0]
        /// </summary>
        public float Duration
        {
            get { return m_Duration; }
            set { m_Duration = Mathf.Max(0, value); }
        }

        /// <summary>
        /// Resets the <see cref="Time"/> to zero.
        /// </summary>
        public void Reset()
        {
            Time = 0f;
            Value = Start;
        }

        /// <summary>
        /// Resets the <see cref="Time"/> to zero with new <paramref name="from"/>
        /// and <paramref name="to"/> values.
        /// </summary>
        /// <param name="from">The value at time 0.</param>
        /// <param name="to">
        /// The value at <see cref="Time"/> >= <see cref="Duration"/>.
        /// </param>
        public void Reset(float from, float to)
        {
            Start = from;
            End = to;

            Reset();
        }

        /// <summary>
        /// Resets the <see cref="Time"/> to zero with the new parameter values.
        /// </summary>
        /// <param name="from">The value at time 0.</param>
        /// <param name="to">
        /// The value at <see cref="Time"/> >= <see cref="Duration"/>.
        /// </param>
        /// <param name="duration">
        /// The duration of the interpolation in seconds. [Limit: >= 0]
        /// </param>
        public void Reset(float from, float to, float duration)
        {
            Duration = duration;

            Reset(from, to);
        }

        /// <summary>
        /// Updates <see cref="Time"/> using UnityEngine.Time.deltaTime.
        /// </summary>
        /// <returns>
        /// The value of <see cref="Value"/> ater UnityEngine.Time.deltaTime is applied.
        /// </returns>
        public float Update()
        {
            return Update(UnityEngine.Time.deltaTime);
        }

        /// <summary>
        /// Updates <see cref="Time"/> using a custom <paramref name="deltaTime"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when the update is perform at a non-standard frequency, such as
        /// within a coroutine.
        /// </para>
        /// <para>
        /// While a negative <paramref name="deltaTime"/> is valid, <see cref="Time"/> 
        /// will never decrement below zero.
        /// </para>
        /// </remarks>
        /// <param name="deltaTime">The time to apply to <see cref="Time"/>.</param>
        /// <returns>
        /// The value of <see cref="Value"/> ater deltaTime is applied.
        /// </returns>
        public float Update(float deltaTime)
        {
            Time = Mathf.Max(0, Time + deltaTime);

            if (m_Duration == 0)
            {
                Value = End;
                return Value;
            }

            float NormalizedTime = Time / m_Duration;

            Value = (NormalizedTime >= 1) ? End : Function(Start, End, NormalizedTime);
            
            return Value;
        }
    }
}

