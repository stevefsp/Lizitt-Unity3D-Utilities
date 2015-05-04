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

namespace com.lizitt.u3d
{
    /// <summary>
    /// Represents a navigation marker that is part of a waypoint list.
    /// </summary>
    public class NavigationWaypoint
        : NavigationMarker
    {
        [SerializeField]
        [Tooltip("The next waypoint. (If any.)")]
        private NavigationWaypoint m_Next = null;

        /// <summary>
        /// The next waypoint, or null if the waypoint is a terminus.
        /// </summary>
        public NavigationWaypoint Next
        {
            get { return m_Next; }
            set { m_Next = value; }
        }

        /// <summary>
        /// The color to use for Gizmos.
        /// </summary>
        public override Color GizmoColor
        {
            get { return ColorUtil.Orange; }
        }

        /// <summary>
        /// Draw the gizmo.  (Only call from Gizmo-legal methods.)
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
    }
}
