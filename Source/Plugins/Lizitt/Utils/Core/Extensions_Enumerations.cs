﻿/*
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
         * Emumeration extensions.
         */

        /// <summary>
        /// True if the collider status represents a rigidbody status.  (Either kinematic or non-kinematic.)
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>True if the status a rigidbody status.</returns>
        public static bool IsRigidBody(this ColliderBehavior status)
        {
            switch (status)
            {
                case ColliderBehavior.KinematicCollider:
                case ColliderBehavior.KinematicTrigger:
                case ColliderBehavior.RigidbodyCollider:
                case ColliderBehavior.RigidbodyTrigger:

                    return true;
            }

            return false;
        }

        /// <summary>
        /// True if the collider status represents a static status.  (Non-rigidbody)
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>True if the collider status represents a static status.</returns>
        public static bool IsStatic(this ColliderBehavior status)
        {
            switch (status)
            {
                case ColliderBehavior.StaticCollider:
                case ColliderBehavior.StaticTrigger:

                    return true;
            }

            return false;
        }
    }
}