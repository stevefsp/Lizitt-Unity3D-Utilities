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
    /// Provides a variety of extension methods.
    /// </summary>
    public static partial class Extensions
    {       
        /*
         * Unity Object exentions
         */

        /// <summary>
        /// The clone suffix automatically appended by Unity to instantiated instances of prefabs.
        /// </summary>
        public const string CloneSuffix = "(Clone)";

        /// <summary>
        /// Strips all instances of "(Clone)" from the object name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "FooThing(Clone)(Clone)" will become "FooThing".
        /// </para>
        /// </remarks>
        /// <param name="obj">
        /// The object to strip.
        /// </param>
        public static void StripCloneName(this Object obj)
        {
            obj.name = obj.name.Replace(CloneSuffix, "");
        }

        /// <summary>
        /// Replaces all instances of "(Clone)" in the object name with the value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: If value is "_Baked", then "FooThing(Clone)(Clone)" will become 
        /// "FooThing_Baked_Baked".
        /// </para>
        /// </remarks>
        /// <param name="obj">
        /// The object to perform the replace operation on.
        /// </param>
        /// <param name="value">
        /// The value to replace the clone suffix with.
        /// </param>
        public static void ReplaceCloneName(this Object obj, string value)
        {
            obj.name = obj.name.Replace(CloneSuffix, value);
        }
    }
}