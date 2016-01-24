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

namespace com.lizitt
{
    /// <summary>
    /// An interpolator that continuously and smoothly moves a transform toward the match position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interpolator never completes and is optimized for a dynamic match position.
    /// </para>
    /// </remarks>
    public class SmoothFollowPositionHelper
        : TrsSmoothFollowHelper
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SmoothFollowPositionHelper()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="settings"/> is stored as a reference, not a copy.  So changes to
        /// the settings object will be reflected in the interpolator.
        /// </para>
        /// </remarks>
        /// <param name="settings">The settings to use.</param>
        public SmoothFollowPositionHelper(TrsSmoothFollowParams settings)
            : base(settings)
        {
        }

        protected override  void ApplyUpdate(Vector3 position)
        {
            ApplyStandardPositionUpdate(position);
        }

        protected override Vector3 GetCurrentValue()
        {
            return GetStandardFromPosition();
        }

        protected override Vector3 GetTargetValue(Vector3 fromValue)
        {
            return GetStandardToPosition();
        }

        protected override Vector3 Interpolate(Vector3 current, Vector3 target, 
            ref Vector3 velocity, float smoothTime, float maximumSpeed)
        {
            return Vector3.SmoothDamp(current, target, ref velocity, smoothTime, maximumSpeed);
        }
    }
}

