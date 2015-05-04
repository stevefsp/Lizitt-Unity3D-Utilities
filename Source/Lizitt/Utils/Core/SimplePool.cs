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
using System.Collections.Generic;

namespace com.lizitt.u3d
{
    /// <summary>
    /// <see cref="SimplePool"/> related settings.
    /// </summary>
    [System.Serializable]
    public struct SimplePoolParams
    {
        /// <summary>
        /// Clamps all values to the required limits.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is necessary since the default structure is invalid.
        /// </para>
        /// </remarks>
        /// <param name="settings">The settings to clamp.</param>
        public static SimplePoolParams EnforceLimits(SimplePoolParams settings)
        {
            settings.MaximumPooled = settings.m_MaximumPooled;
            settings.PreloadCount = settings.m_PreloadCount;

            return settings;
        }

        [SerializeField]
        [Tooltip("The maximum items that can be held in the pool. [Limit: >= 1]")]
        [ClampMinimum(1)]
        private int m_MaximumPooled;

        [SerializeField]
        [Tooltip("The number of pooled items that should be pre-instantiated."
            + " [Limits: 0 <= value <= MaximumPooled]")]
        [ClampMinimum(0)]
        private int m_PreloadCount;

        /// <summary>
        /// The maximum items that can be held in the pool. [Limit: >= 1]
        /// </summary>
        public int MaximumPooled
        {
            get { return m_MaximumPooled; }
            set { m_MaximumPooled = Mathf.Max(1, value); }
        }

        /// <summary>
        /// The number of pooled items that should be pre-instantiated. 
        /// [Limits: 0 &lt;= value &lt;= MaximumPooled]
        /// </summary>
        public int PreloadCount
        {
            get { return m_PreloadCount; }
            set { m_PreloadCount = Mathf.Clamp(value, 0, m_MaximumPooled); }
        }
    }

    /// <summary>
    /// A simple object pool.
    /// </summary>
    /// <typeparam name="T">The type of objects in the pool.</typeparam>
    public class SimplePool<T>
        where T : class
    {
        private readonly Stack<T> m_Pool;
        private readonly System.Func<T> m_FactoryMethod;

        private readonly SimplePoolParams m_Settings;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">The pool settings.</param>
        /// <param name="factoryMethod">
        /// The method for creating new instances of the pooled objects.
        /// </param>
        public SimplePool(SimplePoolParams settings, System.Func<T> factoryMethod)
        {
            if (factoryMethod == null)
                throw new System.ArgumentNullException("factoryMethod");

            m_FactoryMethod = factoryMethod;

            m_Settings =  SimplePoolParams.EnforceLimits(m_Settings);

            m_Pool = new Stack<T>(m_Settings.MaximumPooled);

            while (m_Pool.Count <= m_Settings.PreloadCount)
                m_Pool.Push(m_FactoryMethod());
        }

        /// <summary>
        /// The current pool's settings.
        /// </summary>
        public SimplePoolParams Settings
        {
            get { return m_Settings; }
        }

        /// <summary>
        /// The method used to create new instances when the pool is empty.
        /// </summary>
        public System.Func<T> FactoryMethod
        {
            get { return m_FactoryMethod; }
        }

        /// <summary>
        /// Gets an object from the pool, or a new object if the pool is empty.
        /// </summary>
        /// <returns>
        /// An object from the pool, or a new object if the pool is empty.
        /// </returns>
        public T GetItem()
        {
            return m_Pool.Count == 0 ? m_FactoryMethod() : m_Pool.Pop();
        }

        /// <summary>
        /// Returns an object to the pool if there is room available.
        /// </summary>
        /// <param name="item">The object to return to the pool.</param>
        /// <returns>
        /// True if the object was pooled, or false if <paramref name="item"/> is null or there 
        /// is no room in the pool.
        /// </returns>
        public bool ReturnItem(T item)
        {
            if (item != null && m_Pool.Count <= m_Settings.MaximumPooled)
            {
                m_Pool.Push(item);
                return true;
            }

            return false;
        }
    }
}
