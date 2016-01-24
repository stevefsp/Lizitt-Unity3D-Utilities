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

namespace com.lizitt
{
    /// <summary>
    /// A simple timer that uses deltaTime to track completion.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Update()"/> uses Time.deltaTime to track duration since start.  If 
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
        #region Pooling

        private static SimpleInfinitePool<SimpleTimer> m_Pool;

        private static void CheckForPool(
            int maxPoolSize = 50, int preloadCount = 0, int initCapacity = 10)
        {
            if (m_Pool == null)
            {
                m_Pool = new SimpleInfinitePool<SimpleTimer>(maxPoolSize, 
                    delegate() { return new SimpleTimer(); }, 
                    delegate(SimpleTimer item) { item.Reset(0); },   // Force to zero duration.
                    preloadCount, initCapacity);
            }
        }

        /// <summary>
        /// Resets the pool with the specified configuration.
        /// </summary>
        /// <param name="maxPoolSize">
        /// The maximum number of objects that can be stored in the pool. [Limit: >= 1]
        /// </param>
        /// <param name="preloadCount">
        /// The number of objects that will be immediately instantiated and stored in the pool.
        /// [Limit: 0 &lt;= value &lt= <paramref name="maxPoolSize"/>]
        /// </param>
        /// <param name="initPoolCapacity">
        /// The initial capacity of the pool. 
        /// [Limit: 0 &lt;= value &lt= <paramref name="maxPoolSize"/>]
        public static void ResetPool(
            int maxPoolSize = 50, int preloadCount = 0, int initCapacity = 10)
        {
            m_Pool = null;
            CheckForPool(maxPoolSize, preloadCount, initCapacity);
        }

        /// <summary>
        /// Gets a timer form the pool with the specified duration.
        /// </summary>
        /// <param name="duration">The timer duration. (Seconds)</param>
        /// <returns>A timer reference the pool or a new timer if the pool is empty.</returns>
        public static SimpleTimer GetFromPool(float duration)
        {
            CheckForPool();

            var result = m_Pool.GetPooledObject();
            result.Reset(Mathf.Max(0, duration));  // Don't wan't accidental -1.

            return result;
        }

        /// <summary>
        /// Return a timer to the pool if there is room.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Important: All external references to the timer should be nulled.
        /// </para>
        /// </remarks>
        /// <param name="item">The timer to return.</param>
        /// <returns>
        /// True if the pool had room for the timer.  
        /// False if the timer was reset and released for garbage collection.
        /// </returns>
        public static bool ReturnToPool(SimpleTimer item)
        {
            CheckForPool();
            return m_Pool.PoolObject(item);
        }

        #endregion

        [SerializeField]
        [Tooltip("The number of seconds the timer should run before completing.")]
        [ClampMinimum(0)]
        private float m_Duration = 1;

        /// <summary>
        /// The duration of the timer, in seconds.
        /// </summary>
        public float Duration
        {
            get { return m_Duration; }
            set { m_Duration = Mathf.Max(0, value); }
        }

        private float m_Time;

        /// <summary>
        /// The number of seconds the timer has been running.
        /// </summary>
        public float Time
        {
            get { return m_Time; }
            private set { m_Time = Mathf.Max(0, value); }
        }

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
        /// Reset the timer for re-use.  (<see cref="Time"/> is reset to zero.)
        /// </summary>
        /// <param name="duration">
        /// The new duration of the timer, or -1 to use the current duration.</param>
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
        /// <remarks>
        /// <para>
        /// A negative <paramref name="deltaTime"/> will never result in a 
        /// negative <see cref="Time"/>.  But it can result in the timer transitioning
        /// reference complete to incomplete.
        /// </para>
        /// </remarks>
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
