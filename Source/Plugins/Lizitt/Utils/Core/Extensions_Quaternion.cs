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
         * Quaternion extensions.
         */

        /// <summary>
        /// Get the signed angle around the y-axis between the two quaternions where up 
        /// is world up.  [Range: -180 to 180]
        /// </summary>
        /// <remarks>
        /// <para>
        /// Because this algorithm ignores the quaterion up-axis it is not suitable for some
        /// use cases.  If a local-space angle is required, then see <see cref="AimAngles"/>.
        /// </para>
        /// <para>
        /// Warning: The result of this algorithm is undefined if either of the rotations has 
        /// no xz-plane component.  (I.e. The look direction is Vector3.up or Vector3.down.)
        /// </para>
        /// </remarks>
        /// <param name="from">The reference rotation.</param>
        /// <param name="to">The target rotation.</param>
        /// <returns>The signed y-axis angle. [Range: -180 to 180]</returns>
        public static float SignedAngleY(this Quaternion from, Quaternion to)
        {
            // Get the forward vector for each rotation
            Vector3 forwardA = from * Vector3.forward;
            Vector3 forwardB = to * Vector3.forward;

            // Get a numeric angle for each vector, on the X-Z plane, relative to world forward.
            float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
            float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;
            
            return Mathf.DeltaAngle(angleA, angleB);
        }
    }
}