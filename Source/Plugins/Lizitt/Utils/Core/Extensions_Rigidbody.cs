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
         * Rigidbody extensions.
         */

        /// <summary>
        /// Applies the position apropriately based on the state of the rigid body.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Takes into account whether the rigidbody is kinematic or not.
        /// </para>
        /// </remarks>
        /// <param name="rigidBody">The rigidbody to move.</param>
        /// <param name="position">The position to move to.</param>
        /// <param name="space">The space to move in.</param>
        public static void MoveTo(
            this Rigidbody rigidBody, Vector3 position, Space space = Space.World)
        {
            var trans = rigidBody.transform;

            if (!rigidBody.isKinematic)
            {
                if (space == Space.Self)
                {
                    var original = trans.position;

                    // HACK: Use transform to convert from local to world space.
                    trans.localPosition = position;
                    position = trans.position;

                    trans.position = original;
                }

                rigidBody.MovePosition(position);
            }
            else
                trans.MoveTo(position, space);
        }
    }
}