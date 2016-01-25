/*
 * Copyright (c) 2016 Stephen A. Pratt
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

namespace com.lizitt
{
    /// <summary>
    /// A list of Unity Objects that must be of a give type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Does not support the addition/assignment of null or duplicate entries.  But the list may
    /// contain null entries if objects are destroyed while in the list.
    /// </para>
    /// <para>
    /// This class is useful for serialization of objects that are known to be Unity objects
    /// but present a non-Unity object interface during normal use.  E.g. A list of Unity objects
    /// that all implement the IAgent interface.
    /// </para>
    /// <para>
    /// Use in conjunction with the <see cref="ObjectListDrawer"/> to present a user friendly
    /// GUI element for object lists.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The required type.</typeparam>
    public abstract class ObjectList<T>
        where T : class
    {
        [SerializeField]
        private List<Object> m_Items;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bufferSize">The initial size of the object buffer.</param>
        public ObjectList(int bufferSize)
        {
            m_Items = new List<Object>(Mathf.Max(0, bufferSize));
        }

        /// <summary>
        /// The current size of the object buffer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Because Unity objects can be destroyed, objects added to this list may become
        /// null after they are added.
        /// </para>
        /// </remarks>
        public int BufferSize
        {
            get { return m_Items.Count; }
        }

        /// <summary>
        /// The item at the specified index, or null.
        /// </summary>
        /// <param name="index">
        /// Item index. [Limit: 0 &lt;= value &lt; <see cref="BufferSize"/>]
        /// </param>
        /// <returns>The item at the specified index, or null.</returns>
        public T this[int index]
        {
            get { return GetItem(index); }
            set { SetItem(index, value); }
        }

        /// <summary>
        /// Get the item as the specified index, or null if there is no valid reference.
        /// (E.g. The object was destroyed.)
        /// </summary>
        /// <param name="index">The item index.</param>
        /// <returns>The item as the specified index, or null if there is no valid reference.
        /// (E.g. The object was destroyed.)</returns>
        public T GetItem(int index)
        {
            return m_Items[index] ? m_Items[index] as T : null;
        }

        /// <summary>
        /// Set the value at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">A non-null Unity Object. (Required)</param>
        /// <param name="refObj">
        /// An object reference to use for more friendly error reporting. (Optional)
        /// </param>
        public void SetItem(int index, T value, Object refObj = null)
        {
            var obj = value as Object;

            if (!obj)
            {
                Debug.LogError("Item is null or not a Unity Object. Assignment ignored.", refObj);
                return;
            }

            m_Items[index] = obj;
        }

        /// <summary>
        /// Add an item to the list.  
        /// </summary>
        /// <remarks>
        /// <para>
        /// Duplicates are not allowed.  If an attempt to add a duplicate is detected, its
        /// current index location will be returned.
        /// </para>
        /// </remarks>
        /// <param name="item">A non-null Unity Object. (Required)</param>
        /// <param name="refObj">
        /// An object reference to use for more friendly error reporting. (Optional)
        /// </param>
        /// <returns>The index where the items was stored, or -1 if the item was rejected.</returns>
        public int Add(T item, Object refObj = null)
        {
            var obj = item as Object;
            if (!obj)
            {
                Debug.LogError(typeof(T).Name + " item is not a Unity Object: " + item, refObj);
                return -1;
            }

            var i = m_Items.IndexOf(obj);
            if (i != -1)
                return i;

            m_Items.Add(obj);
            return m_Items.Count - 1;
        }

        /// <summary>
        /// Remove the specifed item from the list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was found in the list and removed.</returns>
        public bool Remove(T item)
        {
            var obj = item as Object;
            if (!obj)
                return false;

            return m_Items.Remove(obj);
        }

        /// <summary>
        /// Remove all items from the list.
        /// </summary>
        public void Clear()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// Remove all null items from the list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// While null items can't be added to the list, objects can be destroyed by Unity, causing
        /// them to become effectively null.
        /// </para>
        /// </remarks>
        public void PurgeNulls()
        {
            for (int i = m_Items.Count - 1; i >= 0; i--)
            {
                if (!m_Items[i])
                    m_Items.RemoveAt(i);
            }
        }

        /// <summary>
        /// True if the list contains the specified object.
        /// </summary>
        /// <param name="item">The object.</param>
        /// <returns>True if the list contains the specified object.</returns>
        public bool Contains(T item)
        {
            var obj = item as Object;
            if (!obj)
                return false;

            return m_Items.Contains(obj);
        }
    }
}