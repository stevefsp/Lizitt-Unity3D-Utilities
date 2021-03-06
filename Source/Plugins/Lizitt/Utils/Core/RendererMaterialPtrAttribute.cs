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

namespace com.lizitt
{
    /// <summary>
    /// Provides user friendly GUI element for <see cref="RendererMaterialPtr"/> fields.
    /// </summary>
    public class RendererMaterialPtrAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// Require all renders to be local to the field's reference object.
        /// </summary>
        public bool RequireLocal { get; private set; }

        /// <summary>
        /// The property search path to use to locate the reference object for local searches.
        /// (Only applicable if <see cref="RequireLocal"/> is true.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// The path must be relative to the field's target object.  This is the root Unity object
        /// the field is attached to.
        /// </para>
        /// <para>
        /// If null, the field's target object will be searched for renderers.  If non-null the
        /// path must refer to an object reference field that references a <c>GameObject</c>
        /// or <c>Component</c>.
        /// </para>
        /// </remarks>
        public string SearchPropertyPath { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RendererMaterialPtrAttribute()
        {
            RequireLocal = false;
        }

        /// <summary>
        /// Constructor to restrict selection to only local renderer's.
        /// </summary>
        /// <remarks>
        /// See <see cref="SearchPropertyPath"/> for information on the search path.
        /// </remarks>
        /// <param name="searchPropertyPath">
        /// The property search path to use to locate the reference object for local searches, 
        /// or null to use the field's target object.
        /// </param>
        public RendererMaterialPtrAttribute(string searchPropertyPath)
        {
            SearchPropertyPath = searchPropertyPath;
            RequireLocal = true;
        }
    }
}
