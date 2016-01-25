using System.Collections.Generic;
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

namespace com.lizitt
{
    /// <summary>
    /// Defines a group of material overrides.  (Materials and which render's to apply them to.)
    /// </summary>
    /// <remarks>
    /// <para>
    /// Designed for use as a field in a Unity component.  When used with 
    /// <see cref="MaterialOverridesAttribute"/>, it provides a better editor experience than 
    /// is available with arrays.
    /// </para>
    /// <para>
    /// Warning: Not suitable for use with arrays of arrays.  E.g. An array of 
    /// <see cref="MaterialOverrides"/> objects, or an array of objects that contain
    /// a <see cref="MaterialOverrides"/> field.  Basically, if there is more than one of overrides
    /// group in a single serialized object the inspector GUI will not behave correctly.
    /// </para>
    /// </remarks>
    [System.Serializable]
    public struct MaterialOverrides
    {
        /*
         * Design notes:
         * 
         * This design uses an anti-pattern.  Unity doesn't support custom drawers for array fields.  
         * Drawers apply to the elements in the array, not the array itself.  This design solves 
         * the problem.
         * 
         */

        [SerializeField]
        private MaterialOverride[] m_Items;
        private MaterialOverride[] Items
        {
            get 
            {
                if (m_Items == null)
                    m_Items = new MaterialOverride[0];
                return m_Items; 
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">The number of accessories.</param>
        public MaterialOverrides(int size)
        {
            m_Items = new MaterialOverride[Mathf.Max(0, size)];
        }

        /// <summary>
        /// The accessory at the specified index.
        /// </summary>
        /// <param name="index">
        /// Accessory index. [Limit: 0 &lt;= value &lt; <see cref="Count"/>]
        /// </param>
        /// <returns>The accessory at the specified index.</returns>
        public MaterialOverride this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        /// <summary>
        /// The number of elements in the group. [Limit: >= 0]
        /// </summary>
        public int Count
        {
            get { return Items.Length; }
        }
    }
}