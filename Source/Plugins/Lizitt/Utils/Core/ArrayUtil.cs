/*
 * Copyright (c) 2011-2015 Stephen A. Pratt
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
using System.Collections.Generic;

namespace com.lizitt.u3d
{
    /// <summary>
    /// Provides array related utility methods.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Static methods are thread safe.
    /// </para>
    /// </remarks>
    public static class ArrayUtil
    {
        /// <summary>
        /// Compresses an array by removing all null values.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only valid for use with arrays of reference types.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="items">The array to compress.</param>
        public static T[] Compress<T>(T[] items)
            where T : class
        {
            if (items == null)
                return null;

            if (items.Length == 0)
                return items;

            int count = 0;

            foreach (T item in items)
                count += (item == null) ? 0 : 1;

            if (count == items.Length)
                return items;

            var result = new T[count];

            if (count == 0)
                return result;

            count = 0;
            foreach (T item in items)
            {
                if (item != null)
                    result[count++] = item;
            }

            return result;
        }

        /// <summary>
        /// Removes the element at the specified index from the array.
        /// Limit use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the UnityEditor is available, use UnityEditor.RemoveAt() instead.
        /// </para>
        /// </remarks>
        /// <param name="source">The array to remove the element from.</param>
        /// <param name="index">The index of the element to remove.</param>
        public static T[] RemoveAt<T>(T[] source, int index)
        {
            var result = new T[source.Length - 1];

            System.Array.Copy(source, result, index);

            if (index < result.Length)
                System.Array.Copy(source, index + 1, result, index, result.Length - index);

            return result;
        }

        /// <summary>
        /// Removes all null or disposed Unity objects from the list.
        /// </summary>
        /// <param name="items">The list of objects to operate on.</param>
        public static void PurgeNulls<T>(this List<T> items)
            where T : UnityEngine.Object
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (!items[i])
                    items.RemoveAt(i);
            }
        }
    }
}
