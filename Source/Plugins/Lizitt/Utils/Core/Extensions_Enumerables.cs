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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.lizitt
{
    public static partial class Extensions
    {
        /*
         * Array, list, and extensions to enumerables in general.
         */

        #region Randomize

        /// <summary>
        /// Creates an array containing the randomized content of the enumerable object.
        /// </summary>
        /// <param name="items">
        /// The enumerable object that provides the items to be randomized.
        /// </param>
        /// <returns>
        /// An array containing the randomized content of the enumerable object.
        /// </returns>
        public static T[] CreateRandomized<T>(this IEnumerable<T> items)
        {
            var result = items.ToArray<T>();

            int n = result.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = result[k];
                result[k] = result[n];
                result[n] = value;
            }

            return result;
        }

        /// <summary>
        /// Randomizes the contents of the array.   (In-place randomiziaton.)
        /// </summary>
        /// <param name="items">The array to randomize.</param>
        public static void Randomize<T>(this T[] items)
        {
            int n = items.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = items[k];
                items[k] = items[n];
                items[n] = value;
            }
        }

        /// <summary>
        /// Randomizes the contents of the list.  (In-place randomiziaton.)
        /// </summary>
        /// <param name="items">The list to randomize.</param>
        public static void Randomize<T>(this List<T> items)
        {
            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = items[k];
                items[k] = items[n];
                items[n] = value;
            }
        }

        #endregion

        #region Null Handling

        /// <summary>
        /// Compresses an array by removing all null values.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only valid for use with arrays of reference types.  Properly detects destroyed 
        /// Unity objects.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="items">The array to compress.</param>
        /// <returns>
        /// A new array if compression was needed, or the original array if no compression
        /// was needed.
        /// </returns>
        public static T[] GetCompressed<T>(this T[] items)
            where T : class
        {
            if (items == null)
                return null;

            if (items.Length == 0)
                return items;

            int count = 0;

            foreach (T item in items)
            {
                if (item is Object)
                    count += (item as Object) ? 1 : 0;
                else
                    count += (item == null) ? 0 : 1;
            }

            if (count == items.Length)
                return items;

            var result = new T[count];

            if (count == 0)
                return result;

            count = 0;
            foreach (T item in items)
            {
                if (item is Object)
                {
                    if ((item as Object))
                        result[count++] = item;
                }
                else if (item != null)
                    result[count++] = item;
            }

            return result;
        }

        /// <summary>
        /// Removes all null and destroyed Unity objects from the list.
        /// </summary>
        /// <param name="items">The object list.</param>
        public static void PurgeNulls<T>(this List<T> items)
            where T : UnityEngine.Object
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (!items[i])
                    items.RemoveAt(i);
            }
        }

        #endregion

        #region Add New Items

        /// <summary>
        /// Compress the taget array and add non-dulplicate new elements to the end of the array.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All null and destroyed elements will be removed from the target array.  All non-null, 
        /// non-destroyed elements that don't already exist in the target array will be added to 
        /// the end of the array.
        /// </para>
        /// <para>
        /// This method is optimizited to minimize allocations, properly detect destroyed
        /// Unity Objects, and maintain the order of the target array.  It is especially useful 
        /// for refreshing component lists. E.g. Perform a component search then add 
        /// only the newly found components to an existing component list, without disrupting 
        /// the order.
        /// </para>
        /// <para>
        /// This method does not attempt to detect duplicates in the target array.  Only
        /// potential new items are checked.
        /// </para>
        /// <para>
        /// Setting <paramref name="destructive"/> to true will potentially save an allocation
        /// at the cost of altering the content of the <paramref name="from"/> array by
        /// setting duplicate elements to null.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="to">The array to compress and add new items to.</param>
        /// <param name="destructive">
        /// If true, all elements in the <paramref name="from"/> array that are duplicates 
        /// of an element in the target array will be set to null.  Otherwise a copy of 
        /// <paramref name="from"/> will be used to detect duplicates.
        /// </param>
        /// <param name="from">An array of the potential items to add to the target array.</param>
        /// <returns>
        /// A reference to the <paramref name="to"/> array if no changes were required,
        /// otherwise a reference to a new array.</returns>
        public static T[] CompressAndAddDistinct<T>(
            this T[] to, bool destructive, params T[] from)
            where T : class
        {
            if (from == null || from.Length == 0)
                return to.GetCompressed();
            
            var ncount = 0;  // New count.
            for (int i = 0; i < from.Length; i++)
            {
                if (from[i] == null || from[i].IsUnityNull())
                {
                    // Need this check before Contains()
                    continue;
                }
                else if (to.Contains(from[i]))
                {
                    if (!destructive)
                    {
                        destructive = true;
                        from = (T[])from.Clone();
                    }

                    from[i] = null;
                }
                else
                    ncount++;
            }

            if (ncount == 0)
                return to.GetCompressed();

            int ecount = 0;  // Existing count.
            for (int i = 0; i < to.Length; i++)
            {
                if (!(to[i] == null || to[i].IsUnityNull()))
                    ecount++;
            }

            var nitems = new T[ecount + ncount];

            var j = 0;

            if (ecount > 0)
            {
                for (int i = 0; i < to.Length; i++)
                {
                    if (!(to[i] == null || to[i].IsUnityNull()))
                        nitems[j++] = to[i];
                }
            }

            for (int i = 0; i < from.Length; i++)
            {
                if (!(from[i] == null || from[i].IsUnityNull())) // Dups were set to null.
                    nitems[j++] = from[i];
            }

            return nitems;
        }

        #endregion
    }
}