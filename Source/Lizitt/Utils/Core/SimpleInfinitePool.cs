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
    /// <see cref="SimpleInfinitePool{T}"/> related settings.
    /// </summary>
    [System.Serializable]
    public struct SimpleInfinitePoolParams
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
        public static SimpleInfinitePoolParams EnforceLimits(SimpleInfinitePoolParams settings)
        {
            settings.MaximumPoolSize = settings.m_MaximumPoolSize;
            settings.PreloadCount = settings.m_PreloadCount;
            settings.InitialPoolCapacity = settings.m_InitialPoolCapacity;

            return settings;
        }

        [SerializeField]
        [Tooltip("The maximum items that can be held in the pool. [Limit: >= 1]")]
        [ClampMinimum(1)]
        private int m_MaximumPoolSize;

        /// <summary>
        /// The maximum items that can be held in the pool. [Limit: >= 1]
        /// </summary>
        public int MaximumPoolSize
        {
            get { return m_MaximumPoolSize; }
            set { m_MaximumPoolSize = Mathf.Max(1, value); }
        }

        [SerializeField]
        [Tooltip("The number of objects that should be immediately instantiated and stored"
            + " in the pool on its creation. [Limits: 0 <= value <= MaximumPoolSize]")]
        [ClampMinimum(0)]
        private int m_PreloadCount;

        /// <summary>
        /// The number of pooled items that should be pre-instantiated. 
        /// [Limit: 0 &lt;= value &lt= <paramref name="MaximumPoolSize"/>]
        /// </summary>
        public int PreloadCount
        {
            get { return m_PreloadCount; }
            set { m_PreloadCount = Mathf.Clamp(value, 0, m_MaximumPoolSize); }
        }

        [SerializeField]
        [Tooltip("The initial capacity of the pool. [Limits: 0 <= value <= MaximumPoolSize >= 0]")]
        private int m_InitialPoolCapacity;

        /// <summary>
        /// The initial capacity of the pool. 
        /// [Limit: 0 &lt;= value &lt= <paramref name="MaximumPoolSize"/>]
        /// </summary>
        public int InitialPoolCapacity
        {
            get { return m_InitialPoolCapacity; }
            set { m_InitialPoolCapacity = Mathf.Max(0, value); }
        }
    }

    /// <summary>
    /// A pool of objects that will instantiate new objects as needed when the pool is empty.
    /// </summary>
    /// <typeparam name="T">The type of object the pool will contain.</typeparam>
    public class SimpleInfinitePool<T>
    {
        private readonly int m_MaximumPoolSize;
        private readonly System.Collections.Generic.Queue<T> m_Pool;
        private readonly System.Func<T> m_FactoryMethod;

        /// <summary>
        /// The factor method used to instantiate new pooled objected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Using this method to create a new object instance will have no impact on 
        /// the contents of the pool.
        /// </para>
        /// </remarks>
        public System.Func<T> FactoryMethod
        {
            get { return m_FactoryMethod; }
        }

        private readonly System.Action<T> m_ResetMethod;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxPoolSize">
        /// The maximum number of objects that can be stored in the pool. [Limit: >= 1]
        /// </param>
        /// <param name="factoryMethod">
        /// The method used to create new instances of the object. [Required]
        /// </param>
        /// <param name="preloadCount">
        /// The number of objects that will be immediately instantiated and stored in the pool.
        /// [Limit: 0 &lt;= value &lt= <paramref name="maxPoolSize"/>]
        /// </param>
        /// <param name="resetMethod">
        /// The method used to set the object to a "not in use" status prior to storage in the pool.
        /// [Optional]
        /// </param>
        /// <param name="initialPoolCapacity">
        /// The initial capacity of the pool. 
        /// [Limit: 0 &lt;= value &lt= <paramref name="maxPoolSize"/>]
        /// </param>
        public SimpleInfinitePool(int maxPoolSize,
            System.Func<T> factoryMethod, System.Action<T> resetMethod = null,
            int preloadCount = 0, int initialPoolCapacity = 10)
        {
            if (maxPoolSize <= 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    "maxSize", "Can't have a zero size pool.");
            }
            m_MaximumPoolSize = maxPoolSize;

            if (factoryMethod == null)
                throw new System.ArgumentNullException("factorMethod");
            m_FactoryMethod = factoryMethod;

            m_ResetMethod = resetMethod;

            m_Pool = new System.Collections.Generic.Queue<T>(
                Mathf.Clamp(initialPoolCapacity, 0, m_MaximumPoolSize));

            preloadCount = Mathf.Min(m_MaximumPoolSize, preloadCount);
            for (int i = 0; i < preloadCount; i++)
                m_Pool.Enqueue(factoryMethod());
        }

        /// <summary>
        /// Get an object reference the pool, or a new object if no objects are available in the pool.
        /// </summary>
        /// <returns>An instance of the object.</returns>
        public T GetPooledObject()
        {
            return m_Pool.Count == 0 ? m_FactoryMethod() : m_Pool.Dequeue();
        }

        /// <summary>
        /// Adds the object to the pool, or discards it if the pool is full.
        /// (The object is always reset.)
        /// </summary>
        /// <param name="item">The object to return to the pool.</param>
        /// <returns>True if the object was successfully stored in the pool. False if 
        /// the parameter is null or the pool is full.</returns>
        public bool PoolObject(T item)
        {
            if (item != null)
            {
                if (m_ResetMethod != null)
                    m_ResetMethod(item);

                if (m_Pool.Count <= m_MaximumPoolSize)
                {
                    m_Pool.Enqueue(item);
                    return true;
                }
            }

            return false;
        }
    }
}
