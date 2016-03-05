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
    /// Represents a marker with an option to include a rotation directive.
    /// </summary>
    /// <remarks>
    /// <para>This is similar to a traditional actor marker representing a position and
    /// face direction. The range is generally used to communicate how close to the mark
    /// the actor must get.</para>
    /// </remarks>
    [AddComponentMenu(LizittUtil.LizittMenu + "Navigation Marker", LizittUtil.MarkerMenuOrder + 1)]
    public class NavigationMarker
        : BaseNavigationMarker
    {
        #region Marker Members

        [SerializeField]
        [Tooltip("If true, the rotation of the marker is significant.")]
        private bool m_UseRotation = false;

        /// <summary>
        /// If true, the rotation of the marker is significant.
        /// </summary>
        public override bool UseRotation
        {
            get { return m_UseRotation; }
            set { m_UseRotation = true; }
        }

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
        /// <returns>True if the direction vector matches the rotation required by the marker 
        /// within the specified tolerance.</returns>
        public override bool IsAtRotation(Vector3 direction, float tolerance = DirectionTolerance)
        {
            // WARNING: See note in base class if you are getting a mono error for this method.

            if (!m_UseRotation)
                return true;

            return Mathf.Abs(Vector3.Angle(transform.forward, direction)) < tolerance;
        }

        /// <summary>
        /// Determines whether the specified position and direction is considered on mark.
        /// (Performs both the range and rotation checks.)
        /// </summary>
        /// <param name="position">The position to test.</param>
        /// <param name="direction">The direction to test.</param>
        /// <param name="directionTolerance">The angle tolerance in degrees.</param>
        /// <returns>True if the values meet the range and rotation checks.</returns>
        public override bool IsOnMark(
            Vector3 position, Vector3 direction, float directionTolerance = DirectionTolerance)
        {
            if (!IsInRange(position))
                return false;

            return IsAtRotation(direction, directionTolerance);
        }

        /// <summary>
        /// Mutates the transform's position and rotation to meet the markers requirements.
        /// </summary>
        /// <param name="trans">The transform to operate on.</param>
        public override void ApplyTo(Transform trans)
        {
            base.ApplyTo(trans);

            if (!IsAtRotation(trans.forward, DirectionTolerance))
                trans.forward = transform.forward;
        }

        #endregion

        #region Iteration Members

        public override BaseNavigationMarker GetLink(int index)
        {
            throw new System.IndexOutOfRangeException();
        }

        public override int LinkCount
        {
            get { return 0; }
        }

        public override System.Collections.Generic.IEnumerator<BaseNavigationMarker> GetEnumerator()
        {
            return null;
        }

        #endregion

        #region Gizmo Members

        /// <summary>
        /// The color to use for gizmos.
        /// </summary>
        protected override Color GizmoColor
        {
            get { return ColorUtil.Magenta; }
        }

        protected override void OnDrawGizmos()
        {
            if (m_UseRotation)
            {
                AnnotationUtil.DrawArrowGizmo(
                        transform.position, transform.position + transform.forward * Range, 0, 0.1f, GizmoColor);
            }

            base.OnDrawGizmos();
        }

        #endregion
    }
}
