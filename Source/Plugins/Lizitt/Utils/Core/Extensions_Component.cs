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
    public static partial class Extensions
    {       
        /*
         * Component extensions.
         */

        /// <summary>
        /// Destroys the object in the correct way depending on whether or not the application
        /// is playing.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        public static void SafeDestroy(this Component component)
        {
            if (Application.isPlaying)
                GameObject.Destroy(component);
            else
                GameObject.DestroyImmediate(component);
        }

        /// <summary>
        /// Instantiates a new instance of the component's GameObject and returns the instance's 
        /// new component.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is a shortcut for the following complex command:
        /// <code>((GameObject)GameObject.Instantiate(comp.gameObject)).GetComponent[T]();</code>
        /// </para>
        /// </remarks>
        /// <param name="component">The componet that needs to be duplicated.</param>
        /// <returns>A new instance of the new component.</returns>
        public static T Instantiate<T>(this T component) where T : Component
        {
            return ((GameObject)GameObject.Instantiate(component.gameObject)).GetComponent<T>();
        }
    }
}