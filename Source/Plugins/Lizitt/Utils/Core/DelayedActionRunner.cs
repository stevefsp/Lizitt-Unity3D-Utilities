/*
 * Copyright (c) 2015-2016 Stephen A. Pratt
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
    /// Calls the specified delgate after a delay.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To pause the runner, simply stop calling the update method.
    /// </para>
    /// </remarks>
    public class DelayedActionRunner
    {
        #region Pooling

        private static SimpleInfinitePool<DelayedActionRunner> m_Pool;

        private static void CheckForPool(
            int maxPoolSize = 50, int preloadCount = 0, int initCapacity = 10)
        {
            if (m_Pool == null)
            {
                m_Pool = new SimpleInfinitePool<DelayedActionRunner>(maxPoolSize,
                    delegate() { return new DelayedActionRunner(); },
                    delegate(DelayedActionRunner item) { item.Reset(null, 0); }, 
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
        /// [Limit: 0 &lt;= value &lt;= <paramref name="maxPoolSize"/>]
        /// </param>
        /// <param name="initPoolCapacity">
        /// The initial capacity of the pool. 
        /// [Limit: 0 &lt;= value &lt;= <paramref name="maxPoolSize"/>]
        public static void ResetPool(
            int maxPoolSize = 50, int preloadCount = 0, int initCapacity = 10)
        {
            m_Pool = null;
            CheckForPool(maxPoolSize, preloadCount, initCapacity);
        }

        /// <summary>
        /// Gets a runner form the pool with the specified duration.
        /// </summary>
        /// <param name="action">The action to run after the specified delay. (May be null.)</param>
        /// <param name="delay">
        /// The number of seconds to wait before calling the action. [Limit: >=0]
        /// </param>
        /// <returns>A runner from the pool or a new runner if the pool is empty.</returns>
        public static DelayedActionRunner GetFromPool(System.Action action, float delay)
        {
            CheckForPool();

            var result = m_Pool.GetPooledObject();
            result.Reset(action, Mathf.Max(0, delay));  // Don't wan't accidental -1.

            return result;
        }

        /// <summary>
        /// Return a runner to the pool if there is room.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Important: All external references to the runner should be nulled.
        /// </para>
        /// </remarks>
        /// <param name="item">The runner to return.</param>
        /// <returns>
        /// True if the pool had room for the runner.  
        /// False if the runner was reset and released for garbage collection.
        /// </returns>
        public static bool ReturnToPool(DelayedActionRunner item)
        {
            CheckForPool();
            return m_Pool.PoolObject(item);
        }

        #endregion

        private float m_Delay = 1;
        private float m_Time = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action">The action to run after the specified delay. (May be null.)</param>
        /// <param name="delay">
        /// The number of seconds to wait before calling the action. [Limit: >=0]
        /// </param>
        public DelayedActionRunner(System.Action action = null, float delay = 1)
        {
            Action = action;
            Delay = delay;
        }

        /// <summary>
        /// The action to run after the specified delay. (May be null.)
        /// </summary>
        /// <remarks>
        /// <para>The action can only be set using one of the reset methods.</para>
        /// </remarks>
        public System.Action Action { get; private set; }

        /// <summary>
        /// The number of seconds to wait before calling the action. [Limit: >=0]
        /// </summary>
        /// <remarks>
        /// <para>
        /// The delay can only be set using one of the reset methods.
        /// </para>
        /// </remarks>
        public float Delay
        {
            get { return m_Delay; }
            private set { m_Delay = Mathf.Max(0, value); }
        }

        /// <summary>
        /// The number of seconds the runner has been running.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The runner will trigger the action when it reaches the <see cref="Delay"/>.  The value
        /// will continue increasing with each update call, so it can be greater than
        /// <see cref="Delay"/>.
        /// </para>
        /// </remarks>
        public float Time 
        {
            get { return m_Time; }
            private set { m_Time = Mathf.Max(0, value); }
        }

        /// <summary>
        /// True if the action has been executed.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Reset the runner for re-use or to change the delay. 
        /// </summary>
        /// <param name="delay">
        /// The number of seconds to wait before calling the action, or -1 to use the currently
        /// defined delay. [Limit: >=0, or -1]
        /// </param>
        public void Reset(float delay = -1)
        {
            Delay = delay == -1 ? m_Delay : delay;
            //EndTime = Time.time + Delay;
            Time = 0;
            IsComplete = false;
        }

        /// <summary>
        /// Reset the runner for re-use or to change its settings.
        /// </summary>
        /// <param name="action">
        /// The action to execute after the delay. (Can be null.)
        /// </param>
        /// <param name="delay">
        /// The number of seconds to wait before calling the action, or -1 to use the currently
        /// defined delay. [Limit: >=0, or -1]
        /// </param>
        public void Reset(System.Action action, float delay = 1)
        {
            Action = action;
            Reset(delay);
        }

        /// <summary>
        /// Updates the runner using Time.deltaTime and executes the action when the delay 
        /// has been reached.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An update method must be called periodically in order to the runner to detect when to
        /// execute the action.  (E.g. Call during MonoBehaviour.Update(), FixedUpdate(), etc.)
        /// </para>
        /// <para>
        /// It is safe to continue calling this method after the action has been executed. 
        /// It will only execute the action once per reset.
        /// </para>
        /// </remarks>
        /// <returns>
        /// True until the action has been executed, then false.  (I.e. Returns the oposite
        /// of <see cref="IsComplete"/>.)
        /// </returns>
        public bool Update()
        {
            Time += UnityEngine.Time.deltaTime;

            ProcessAction();

            return !IsComplete;
        }

        /// <summary>
        /// Updates the runner using a custom delta time and executes the action when the delay 
        /// has been reached.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Useful when the runner needs to be updated when Time.deltaTime is not valid, such as
        /// within a variable rate coroutine.
        /// </para>
        /// <para>
        /// An update method must be called periodically in order to the runner to detect when to
        /// execute the action.
        /// </para>
        /// <para>
        /// It is safe to continue calling this method after the action has been executed. 
        /// It will only execute the action once per reset.  (Even if the the time has been
        /// decremented using a negative <paramref name="deltaTime"/> value.
        /// </para>
        /// </remarks>
        /// <param name="deltaTime">The number of seconds to increment/decrement the time.</param>
        /// <returns>
        /// True until the action has been executed, then false.  (I.e. Returns the oposite
        /// of <see cref="IsComplete"/>.)
        /// </returns>
        public bool Update(float deltaTime)
        {
            Time += deltaTime;

            ProcessAction();

            return !IsComplete;
        }

        private void ProcessAction()
        {
            if (!IsComplete && m_Time >= m_Delay)
            {
                IsComplete = true;  // Set early in case of an exception.
                if (Action == null)
                    Debug.LogWarning("Action triggered, but no action assigned.");
                else
                    Action();
            }
        }
    }
}
