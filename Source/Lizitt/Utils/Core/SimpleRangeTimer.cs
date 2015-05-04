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
    /// A simple timer with a variable duration that uses deltaTime to track completion.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A new random duration between <see cref="Minimum"/> and <see cref="Maximum"/> will be
    /// selected whenever the timer is reset.
    /// </para>
    /// <para>
    /// <see cref="Update"/> uses Time.deltaTime to track duration since start.  If 
    /// Time.deltaTime can't be used, such as in a variable rate co-routine, then the alternate 
    /// <see cref="Update(float)"/> method must be used.
    /// </para>
    /// <para>
    /// To pause the timer, simply stop calling the update method.
    /// </para>
    /// </remarks>
    [System.Serializable]
    public class SimpleRangeTimer
    {
        [SerializeField]
        [Tooltip("The minimum number of seconds the timer will run before completing.")]
        [ClampMinimum(0)]
        private float m_Minimum = 1;

        [SerializeField]
        [Tooltip("The maximum number of seconds the timer will run before completing.")]
        [ClampMinimum(0)]
        private float m_Maximum = 2;

        private float m_Duration;
        private float m_Time;

        /// <summary>
        /// Constructor (<see cref="Minimum"/> = 1, <see cref="Maximum"/> = 2)
        /// </summary>
        public SimpleRangeTimer()
        {
            Reset();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <paramref name="minimum"/> is less than <paramref name="maximum"/> it will be 
        /// auto-clamped to <paramref name="maximum"/>.
        /// </para>
        /// </remarks>
        /// <param name="minimum">
        /// The minimum number of seconds to run. 
        /// [Limits: 0 &lt;= value &lt;= <paramref name="maximum"/>.]
        /// </param>
        /// <param name="maximum">The maximum number of seconds to run. [Limit: >= 0]</param>
        public SimpleRangeTimer(float minimum, float maximum)
        {
            // Clamp here to avoid -1 misinterpretation.
            Reset(Mathf.Max(0, minimum), Mathf.Max(0, maximum));
        }

        /// <summary>
        /// The time the timer has been running.
        /// </summary>
        public float Time
        {
            get { return m_Time; }
        }

        /// <summary>
        /// The current duration of the timer.
        /// [Limits: <see cref="Minimum"/> &lt;= value &lt;= <see cref="Maximum"/>]
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is updated to a new value whenever the timer is reset.
        /// </para>
        /// </remarks>
        public float Duration
        {
            get { return m_Duration; }
        }

        /// <summary>
        /// The minimum number of seconds the timer will run before completing.
        /// [Limits: 0 &lt;= value &lt;= <see cref="Maximum"/>.]
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value will be only be auto-clamped to &lt;= <see cref="Maximum"/> during a
        /// timer reset.  It can hold an invalid value until then.
        /// </para>
        /// </remarks>
        public float Minimum
        {
            get { return m_Minimum; }
            set { m_Minimum = Mathf.Max(0, value); }
        }

        /// <summary>
        /// The maximum number of seconds the timer will run before completing.
        /// [Limits: >= 0]
        /// </summary>
        public float Maximum
        {
            get { return m_Maximum; }
            set { m_Maximum = Mathf.Max(0, value); }
        }

        /// <summary>
        /// Resets the timer for re-use. (<see cref="Time"/> is reset to zero and a new duration
        /// is derived.)
        /// </summary>
        /// <param name="minimum">
        /// The new minimum time, or -1 to use the current minimum time.
        /// </param>
        /// <param name="maximum">
        /// The new maximum time, or -1 to use the current maximum time.
        /// </param>
        public void Reset(float minimum = -1, float maximum = -1)
        {
            // Clamp early to allow for exception check.
            if (minimum != -1)
                minimum = Mathf.Max(0, minimum);
            if (maximum != -1)
                maximum = Mathf.Max(0, maximum);

            Minimum = minimum == -1 ? m_Minimum : minimum;
            Maximum = maximum == -1 ? m_Maximum : maximum;

            if (Minimum > Maximum)
            {
                Minimum = Maximum;
                Debug.LogWarning("Minimum is greater than maximum. Auto-clamped to maximum.");
                return;
            }

            m_Duration = Random.Range(m_Minimum, m_Maximum);

            m_Time = 0;
        }

        /// <summary>
        /// True if the timer is complete. (<see cref="Time"/> >= <see cref="Duration"/>) 
        /// </summary>
        public bool IsComplete
        {
            get { return m_Time >= m_Duration; }
        }

        /// <summary>
        /// Update the timer using Time.deltaTime.
        /// </summary>
        /// <returns>False when the timer is complete, otherwise true.</returns>
        public bool Update()
        {
            m_Time += UnityEngine.Time.deltaTime;

            return !IsComplete;
        }

        /// <summary>
        /// Update the timer using a custom delta time.
        /// </summary>
        /// <param name="deltaTime">
        /// The delta time in seconds since the last timer update. (Value can be negative.)
        /// </param>
        /// <returns>False when the timer is complete, otherwise true.</returns>
        public bool Update(float deltaTime)
        {
            m_Time += deltaTime;

            return !IsComplete;
        }
    }
}
