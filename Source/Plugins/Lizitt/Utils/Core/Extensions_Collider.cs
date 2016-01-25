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
    /// Provides a variety of extension methods.
    /// </summary>
    public static partial class Extensions
    {
        /*
         * Collider extensions
         * 
         * Design notes:
         * 
         * The Extensions class encapulates a large variety of extension methods
         * in one class so they don't clutter the namespaces.  E.g. Want to avoid GameObjectExt, 
         * TranformExt, etc.  
         * 
         * Only when a particular type deserves its own utility class will extensions be
         * considered for movement.  (See: AnimatorUtil, ColorUtil, etc.)
         */

        /// <summary>
        /// Returns the Rigidbody currently attached to the collider or the one that will be
        /// attached.  Otherwise null.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If Collider.attachedRigidbody is not assigned, then this method will return the
        /// result of a parent search.  This is helpful when it is not know if the collider has
        /// been fully initialized.  (E.g. When trying to locate a collider's the rigidbody 
        /// outside of play mode.
        /// </para>
        /// </remarks>
        /// <param name="collider">The collider to check.</param>
        /// <returns>
        /// Returns the Rigidbody currently attached to the collider or the one that will be
        /// attached.  Otherwise null.
        /// </returns>
        public static Rigidbody GetAssociatedRigidBody(this Collider collider)
        {
            return collider.attachedRigidbody
                ? collider.attachedRigidbody
                : collider.GetComponentInParent<Rigidbody>();
        }
    }
}