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
    /// A simple timer that uses deltaTime to track completion.
    /// </summary>
    /// <remarks>
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
    public class SimpleTimer
    {
        [SerializeField]
        [Tooltip("The number of seconds the timer should run before completing.")]
        [ClampMinimum(0)]
        private float m_Duration = 1;

        private float m_Time;

        /// <summary>
        /// Default constructor. (<see cref="Time"/> = 1)
        /// </summary>
        public SimpleTimer()
        {
            Reset(-1);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="duration">The number of seconds the timer will run. [Limit: >= 0]</param>
        public SimpleTimer(float duration)
        {
            Duration = duration;
            Reset(-1);
        }

        /// <summary>
        /// The number of seconds the timer has been running.
        /// </summary>
        public float Time
        {
            get { return m_Time; }
            private set { m_Time = Mathf.Max(0, value); }
        }

        /// <summary>
        /// The duration of the timer, in seconds.
        /// </summary>
        public float Duration
        {
            get { return m_Duration; }
            set { m_Duration = Mathf.Max(0, value); }
        }

        /// <summary>
        /// Reset the timer for re-use.  (<see cref="Time"/> is reset to zero.)
        /// </summary>
        /// <param name="duration">The new duration of the timer, or -1 to use the current
        /// duration.</param>
        public void Reset(float duration = -1)
        {
            Duration = duration == -1 ? m_Duration : duration;
            Time = 0;
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
            Time += UnityEngine.Time.deltaTime;

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
            Time += deltaTime;

            return !IsComplete;
        }
    }
}
