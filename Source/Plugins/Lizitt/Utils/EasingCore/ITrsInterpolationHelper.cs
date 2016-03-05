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
    /// Provides an interface for Transform-based translate/rotate/scale interpolators.
    /// </summary>
    public interface ITrsInterpolationHelper
    {
        /// <summary>
        /// True if interpolation is complete and no further changes to 
        /// <see cref="ItemToTransform"/> will be made. (Even if further calls to update are made.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Whether an interpolator ever completes is dependant on the concrete implementation.
        /// Some interpolators are designed to never complete.  (E.g. Followers.)
        /// </para>
        /// </remarks>
        bool IsComplete { get; }

        /// <summary>
        /// The item the interpolation is being applied to.
        /// </summary>
        Transform ItemToTransform { get; }

        /// <summary>
        /// Resets the interpolator for re-use.
        /// </summary>
        void Reset();

        /// <summary>
        /// Resets the interpolator for re-use with the specified value for <see cref="Space"/>.
        /// </summary>
        /// <param name="space"></param>
        void Reset(Space space);

        /// <summary>
        /// Resets the interpolator for re-use with the specified value for 
        /// <see cref="ItemToTransform"/>.
        /// </summary>
        /// <param name="itemToTransform"></param>
        void Reset(Transform itemToTransform);

        /// <summary>
        /// Resets the interpolator for re-use with the specified values.
        /// </summary>
        /// <param name="itemToTransform"></param>
        /// <param name="space"></param>
        void Reset(Transform itemToTransform, Space space);

        /// <summary>
        /// The Rigidbody object attached to <see cref="ItemToTransform"/>, or null if there is
        /// none.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Unless otherwise noted, interpolators expect this value to be constant for the 
        /// duration of the interpolation.
        /// </para>
        /// </remarks>
        Rigidbody Rigidbody { get; }

        /// <summary>
        /// The space the interpolation is performed in.
        /// </summary>
        Space Space { get; }

        /// <summary>
        /// The item being interpolated to.  (May be null.)
        /// </summary>
        Transform MatchItem { get; set; }

        /// <summary>
        /// The offset being interpolated to.
        /// </summary>
        Vector3 MatchOffset { get; set; }

        /// <summary>
        /// True if the interpolator should apply its interpolation to <see cref="ItemToTransform"/>
        /// during <see cref="Update"/>.  Otherwise, only update interpolation state.
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are use cases where interpolation results need to be applied by a different
        /// component or at a different time in the engine loop that the interpolator is being
        /// updated.  Setting this field to false allows for such scenarios.
        /// </para>
        /// </remarks>
        bool AutoUpdate { get; set; }

        /// <summary>
        /// The current interpolated value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value is undefined until after the first interpolator update.
        /// </para>
        /// </remarks>
        Vector3 InterpolatedValue { get; }

        /// <summary>
        /// Updates the interpolator.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Some components never complete, such as dynamic followers.  Others might complete 
        /// interpolation but continue in a non-interpolation mode.
        /// </para>
        /// </remarks>
        /// <returns>True until no futher updates are required.</returns>
        bool Update();
    }
}
