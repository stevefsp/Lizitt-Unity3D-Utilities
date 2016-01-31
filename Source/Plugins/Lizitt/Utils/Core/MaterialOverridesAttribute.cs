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
    /// Display the <see cref="MaterialOverrideGroup"/> field as a user friendly reorderable list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute is only compatible with non-array <see cref="MaterialOverrideGroup"/> fields.
    /// E.g. It can't be used with an array of <see cref="MaterialOverrideGroup"/> objects.
    /// </para>
    /// </remarks>
    public class MaterialOverrideGroupAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The path of the object reference property that contains the target for local searches.
        /// (Optional)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only applicable if <see cref="LocalOnly"/> is true.  If null, the fields target
        /// object is used.
        /// </para>
        /// <para>
        /// The path must be non-relative.  I.e. From the root of the field's taret object.
        /// </para>
        /// </remarks>
        public string SearchPropertyPath { get; set; }

        /// <summary>
        /// If true, then only permit selection of local components.  (E.g. Components on or 
        /// under the search target.)  Otherwise allow any renderer to be assigned.
        /// </summary>
        public bool LocalOnly { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchPropertyPath">
        /// The path of the object reference property that contains the target for local searches.
        /// (If applicable.)
        /// </param>
        /// <param name="localOnly">
        /// If true, then only permit selection of local components.  (E.g. Components on or 
        /// under the search target.)  Otherwise allow any renderer to be assigned.
        /// </param>
        public MaterialOverrideGroupAttribute(string searchPropertyPath = null, bool localOnly = false)
        {
            SearchPropertyPath = searchPropertyPath;
            LocalOnly = localOnly;
        }
    }
}