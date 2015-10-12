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
    /// An marker representing a position with a radius.
    /// </summary>
    public class Marker
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The radius of the marker.")]
        [ClampMinimum(0)]
        private float m_Radius = 0.1f;

        /// <summary>
        /// The radius of the marker.
        /// </summary>
        public float Range
        {
            get { return m_Radius; }
            set { m_Radius = Mathf.Max(0, value); }
        }

        /// <summary>
        /// True if the provided position is within the marker's radius.
        /// </summary>
        /// <param name="position">The position to test.</param>
        /// <returns></returns>
        public bool IsInRange(Vector3 position)
        {
            return (transform.position - position).sqrMagnitude < m_Radius * m_Radius;
        }

        /// <summary>
        /// Applies the marker to the transform only if the transform is outside of the
        /// markers tolerances.  Otherwise leaves the transform unchanged.
        /// </summary>
        /// <param name="trans">The transform to apply the marker to.</param>
        public virtual void ApplyTo(Transform trans)
        {
            if (!IsInRange(trans.position))
                trans.MoveToSafe(transform.position, Space.World);
        }

        /// <summary>
        /// The color to use for gizmos.
        /// </summary>
        public virtual Color GizmoColor
        {
            get { return ColorUtil.Blue; }
        }

        /// <summary>
        /// Draw the gizmo.  (Only call from Gizmo-legal methods.)
        /// </summary>
        public virtual void DrawGizmo()
        {
            DrawStandardGizmo(transform, Range, GizmoColor, false);
        }

        /// <summary>
        /// Draw the standard marker gizmo.
        /// </summary>
        /// <param name="transform">The location of the gizmo.</param>
        /// <param name="range">The radius of the gizmo.</param>
        /// <param name="color">The gizmo color.</param>
        /// <param name="includeRotation">
        /// If true the marker will be modified to include a forward direction indicator.
        /// </param>
        protected static void DrawStandardGizmo(
            Transform transform, float range, Color color, bool includeRotation)
        {
            AnnotationUtil.DrawCircleGizmo(transform, Vector3.zero, range, color);
            AnnotationUtil.DrawMarkerGizmo(transform, Vector3.zero, color, includeRotation);
        }
    }
}
