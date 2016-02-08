﻿/*
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
    public class ObjectListAttribute
        : PropertyAttribute
    {
        public System.Type RequiredType { get; private set; }
        public string HeaderTitle { get; private set; }
        public string ListPropertyPath { get; private set; }

        public ObjectListAttribute(string headerTitle, System.Type requiredType, string listPropertyPath = "m_Items")
        {
            RequiredType = requiredType;
            HeaderTitle = headerTitle;
            ListPropertyPath = listPropertyPath;
        }
    }

    /// <summary>
    /// A list of distinct Unity Objects of the specified type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Does not support the addition/assignment of null, destroyed, or duplicate entries.  But the list may
    /// contain null entries if objects are destroyed while in the list.
    /// </para>
    /// <para>
    /// This list will never return destroyed object references.  If a stored object is destroyed, a true null
    /// reference will be returned.
    /// </para>
    /// <para>
    /// This class is useful for serialization of objects that are known to be Unity objects
    /// but present a non-Unity Object interface during normal use.  E.g. A list of Unity objects
    /// that all implement the IAgent interface.
    /// </para>
    /// <para>
    /// Extend both this class and <see cref="ObjectListDrawer"/> to present a user friendly GUI element for 
    /// object lists.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The required type.</typeparam>
    public class ObjectList<T>
        where T : class
    {
        /*
         * Design notes:
         * 
         * One of the primary design requirements of this class is to make it Editor GUI friendly.  That is why it
         * doesn't try to directly extend the List class.  (Need 'array' inside of a class.)
         */

        [SerializeField]
        private List<Object> m_Items;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bufferSize">The initial size of the object buffer.</param>
        public ObjectList(int bufferSize)
        {
            m_Items = new List<Object>(bufferSize);
        }

        /// <summary>
        /// The number of items in the list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Because Unity objects can be destroyed, objects added to this list may become
        /// null after they are added.  The count includes destroyed objects.
        /// </para>
        /// </remarks>
        public int Count
        {
            get { return m_Items.Count; }
        }

        /// <summary>
        /// The item at the specified index, or null if it has been destroyed.
        /// </summary>
        /// <param name="index">
        /// The index. [Limit: 0 &lt;= value &lt; <see cref="Count"/>]
        /// </param>
        /// <returns>The item at the specified index, or null if it has been destroyed.</returns>
        public T this[int index]
        {
            get { return GetItem(index); }
        }

        /// <summary>
        /// Get the item as the specified index, or null if it has been destroyed.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item item as the specified index, or null if it has been destroyed.</returns>
        public T GetItem(int index)
        {
            return m_Items[index] ? m_Items[index] as T : null;
        }

        /// <summary>
        /// Set the value of the item at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value must be a non-null, non-destroyed Unity Object that is not already in the list.
        /// </para>
        /// </remarks>
        /// <param name="index">The index.</param>
        /// <param name="value">A valid Unity Object that is not already in the list. (Required)</param>
        /// <param name="refObj">An object reference to use for more error reporting. (Optional)</param>
        /// <returns>True if the item was successfully added.</returns>
        public bool SetItem(int index, T value, Object refObj = null)
        {
            var obj = value as Object;
            if (!obj)
            {
                Debug.LogError("Item is null, destroyed, or not a Unity Object.", refObj);
                return false;
            }
            if (m_Items.IndexOf(obj) != -1)
            {
                Debug.LogError("Item is already in the list.  Assignment rejected.", refObj);
                return false;
            }

            m_Items[index] = obj;

            return true;
        }

        /// <summary>
        /// Add an item to the list.  
        /// </summary>
        /// <remarks>
        /// <para>
        /// The item must be a non-null, non-destroyed Unity Object
        /// </para>
        /// <para>
        /// Duplicates are not allowed.  If an attempt to add a duplicate is detected, its current index will 
        /// be returned.
        /// </para>
        /// </remarks>
        /// <param name="item">A valid Unity Object. (Required)</param>
        /// <param name="refObj">An object reference to use for error reporting. (Optional)</param>
        /// <returns>The index where the items was stored, or -1 if the item was rejected.</returns>
        public int Add(T item, Object refObj = null)
        {
            var obj = item as Object;
            if (!obj)
            {
                Debug.LogErrorFormat(typeof(T).Name + " item is not a Unity Object: " + item, refObj);
                return -1;
            }

            var i = m_Items.IndexOf(obj);
            if (i == -1)
            {
                m_Items.Add(obj);
                i = m_Items.Count - 1;
            }

            return i;
        }

        /// <summary>
        /// Add the items to the list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All items must be non-null, non-destroyed Unity Objects.  All items will be rejected if there are any 
        /// invalid items.
        /// </para>
        /// <para>
        /// Duplicates are not allowed and will be ignored.  The return value will be the count of newly added items.
        /// </para>
        /// </remarks>
        /// <param name="refObj">An object reference to use for error reporting. (Optional)</param>
        /// <param name="items">An array of valid Unity objects.</param>
        /// <returns>The number of items added.  (Not including duplicates.)</returns>
        public int AddItems(Object refObj, params T[] items)
        {
            if (items == null || items.Length == 0)
                return 0;

            for (int i = 0; i < items.Length; i++)
            {
                var obj = items[i] as Object;
                if (!obj)
                {
                    Debug.LogErrorFormat(refObj, 
                        "{0} item at index {1} is null, destroyed, or not a Unity Object", typeof(T).Name, i);
                    return 0;
                }
            }

            int count = 0;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i] as Object;
                if (m_Items.IndexOf(item) == -1)
                {
                    m_Items.Add(item);
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Remove the specifed item from the list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Unlike most other methods that reject destroyed objects as arguments, this method can be used to
        /// remove a known destroyed object.
        /// </para>
        /// </remarks>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was found in the list and removed.</returns>
        public bool Remove(T item)
        {
            // Designed to allow removal of a destroyed item.

            if (item is Object)
                return m_Items.Remove(item as Object);

            return false;
        }

        /// <summary>
        /// Remove all items from the list.
        /// </summary>
        public void Clear()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// Remove all destroyed items from the list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// While null/destroyed items can't be added to the list, objects can be destroyed by Unity, causing
        /// them to become effectively null.
        /// </para>
        /// </remarks>
        public void PurgeDestroyed()
        {
            m_Items.PurgeNulls();
        }

        /// <summary>
        /// True if the list contains the specified undestroyed object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method treats a destroyed object as null.  So it will never return true for a destroyed object.
        /// <see cref="Remove"/> should be used to check for and remove destroyed objects if that is the intention.
        /// </para>
        /// </remarks>
        /// <param name="item">The object.</param>
        /// <returns>True if the list contains the specified undestroyed object.</returns>
        public bool Contains(T item)
        {
            var obj = item as Object;
            if (!obj)
                return false;

            return m_Items.Contains(obj);
        }
    }
}