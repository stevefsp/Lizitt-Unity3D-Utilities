/*
 * Copyright (c) 2015 Stephen A. Pratt
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

namespace com.lizitt.easing
{
    /// <summary>
    /// Interpolates a transform's euler angles using a standard easing function.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All rotation axes are interpolated using the same ease mode.
    /// </para>
    /// <para>
    /// The <see cref="InterpolatedValue"/> is guarenteed to match the <see cref="To"/> value 
    /// at completion.  After completion, if the match item is dynamic, the 
    /// <see cref="InterpolatedValue"/> will stay locked to the <see cref="To"/> value. 
    /// (No further interpolation, lock step behavior.)
    /// </para>
    /// <para>
    /// Because of the guarentee that the interpolation will complete within the specified
    /// duration, this class is generally designed for static use.  It works best when the 
    /// settings and the <see cref="MatchItem"/> remain static for during of the interpolation.  
    /// Any changes during the the interpolation will warp the easing behavior.
    /// </para>
    /// </remarks>
    public class EaseRotateToHelper
        : TrsEaseHelper
    {
        /*
         * TODO: EVAL: Would an axis/angle based interpolation be better?
         * The current method of interpolating all axes separately can result in gimple lock
         * and less than optimal behavior for certain functions for certain rotations.  Maybe 
         * the algorithm should be switched to one that determines an angle of rotation then
         * interpolates the rotation around that axis.
         */

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="easeMode">The ease mode.</param>
        public EaseRotateToHelper(EaseType easeMode)
            : base(easeMode)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">The settings to use.  (Uses the reference, not a copy.)</param>
        public EaseRotateToHelper(TrsEaseParams settings)
            : base(settings)
        {
        }

        protected override void ApplyUpdate(Vector3 eulerAngles)
        {
            ApplyStandardRotationUpdate(eulerAngles);
        }

        protected override Vector3 GetFromValue()
        {
            return GetStandardFromRotation();
        }

        protected override Vector3 GetToValue(Vector3 fromValue)
        {
            return GetStandardToRotation(fromValue);
        }
    }
}

