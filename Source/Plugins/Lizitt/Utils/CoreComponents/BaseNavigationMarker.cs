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
using UnityEngine;

namespace com.lizitt
{
    /// <summary>
    /// The base class for marker's that include rotation information.
    /// </summary>
    public abstract class BaseNavigationMarker
        : Marker, IEnumerable<BaseNavigationMarker>
    {
        /*
         * Design notes:
         * 
         * This base class allows navigation markers of all typs to be linked together.
         * 
         * Bug:
         * 
         * There is a bug as of Unity 5.3.1 where this abstract class shows up in the editor Gizmos list, but 
         * there is no effect.
         */

        /// <summary>
        /// The standard tolerance to use when matching rotation.
        /// </summary>
        public const float DirectionTolerance = 5.0f;

        #region Marker members

        /// <summary>
        /// If true, the rotation of the marker is significant.
        /// </summary>
        public abstract bool UseRotation { get; set; }

        /// <summary>
        /// Determines if a direction vector matches the rotation required by the marker within
        /// the specified tolerance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Will always return true if <see cref="UseRotation"/> is false.
        /// </para>
        /// </remarks>
        /// <param name="direction">The direction to test.</param>
        /// <param name="tolerance">The angle tolerance in degrees.</param>
        /// <returns>
        /// True if the direction vector matches the rotation required by the marker within the specified tolerance.
        /// </returns>
        public abstract bool IsAtRotation(Vector3 direction, float tolerance = DirectionTolerance);

        /// <summary>
        /// Determines whether the specified position and direction is considered on mark.
        /// (Performs both the range and rotation checks.)
        /// </summary>
        /// <param name="position">The position to test.</param>
        /// <param name="direction">The direction to test.</param>
        /// <param name="directionTolerance">The angle tolerance in degrees.</param>
        /// <returns>True if the values meet the range and rotation checks.</returns>
        public abstract bool IsOnMark(Vector3 position, Vector3 direction, float directionTolerance);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // HACK: Unity 5.3.1: Workaround for Mono's optional parameter key duplication bug.

        /// <summary>
        /// Determines whether the specified position and direction is considered on mark using 
        /// <see cref="DirectionTolerance"/>. (Performs both the range and rotation checks.)
        /// </summary>
        /// <param name="position">The position to test.</param>
        /// <param name="direction">The direction to test.</param>
        /// <returns>True if the values meet the range and rotation checks.</returns>
        public bool IsOnMark(Vector3 position, Vector3 direction)
        {
            return IsOnMark(position, direction, DirectionTolerance);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #endregion

        #region Iteration Members

        /// <summary>
        /// Get the link at the specified index.
        /// </summary>
        /// <param name="index">The index. [0 &lt;= value &lt; <see cref="LinkCount"/></param>
        /// <returns>The link, or null if there is none.</returns>
        public abstract BaseNavigationMarker GetLink(int index);

        /// <summary>
        /// The number of exit node links.
        /// </summary>
        public abstract int LinkCount { get; }

        /// <summary>
        /// An enumerator of the exit links for the node.
        /// </summary>
        /// <returns>An enumerator of the exit links for the node.</returns>
        public abstract IEnumerator<BaseNavigationMarker> GetEnumerator();

        /// <summary>
        /// An enumerator of the exit links for the node.
        /// </summary>
        /// <returns>An enumerator of the exit links for the node.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
