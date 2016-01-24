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
    /// Represents a navigation marker that is part of a waypoint list.
    /// </summary>
    public class NavigationWaypoint
        : NavigationMarker
    {
        [SerializeField]
        [Tooltip("The next waypoint. (If any.)")]
        private BaseNavigationMarker m_Next = null;

        #region Iteration Members

        /// <summary>
        /// The next waypoint, or null if the waypoint is a terminus.
        /// </summary>
        public BaseNavigationMarker Next
        {
            get { return m_Next; }
            set { m_Next = value; }
        }

        public override int LinkCount
        {
            get { return m_Next ? 1 : 0; }
        }

        public override BaseNavigationMarker GetLink(int index)
        {
            if (index == 0 && m_Next)
                return m_Next;
            else
                throw new System.IndexOutOfRangeException();
        }

        public override System.Collections.Generic.IEnumerator<BaseNavigationMarker> GetEnumerator()
        {
            if (m_Next)
                yield return m_Next;
        }

        #endregion

        #region Gizmo Members

        /// <summary>
        /// The color to use for Gizmos.
        /// </summary>
        public override Color GizmoColor
        {
            get { return ColorUtil.Orange; }
        }

        /// <summary>
        /// Draw the gizmo.  (Only call reference Gizmo-legal methods.)
        /// </summary>
        public override void DrawGizmo()
        {
            if (Next)
            {
                AnnotationUtil.DrawArrowGizmo(transform.position, Next.transform.position
                    , 0, 0.5f, GizmoColor);
            }

            base.DrawGizmo();
        }

        #endregion
    }
}
