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
    /// Provides annotation features for use with Gizmos.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Most of the methods use the Gizmos class to draw annotations, restricting their use
    /// to the same Unity callback methods as Gizmos.
    /// </para>
    /// </remarks>
    public static class AnnotationUtil
    {
        private const float Epsilon = 0.001f;

        /// <summary>
        /// If true, annotations should always be drawn, otherwise annotations should follow
        /// their standard beahvior.
        /// </summary>
        public static bool drawAlways = false;

        #region Circle Gizmo

        private const int CircleSegments = 20;

        /// <summary>
        /// Access via property only.
        /// </summary>
        private static float[] mDir;

        private static float[] Dir
        {
            get
            {
                if (mDir == null)
                {
                    mDir = new float[CircleSegments * 2];
                    for (int i = 0; i < CircleSegments; i++)
                    {
                        float a =
                            (float)i / (float)CircleSegments * Mathf.PI * 2;
                        mDir[i * 2] = Mathf.Cos(a);
                        mDir[i * 2 + 1] = Mathf.Sin(a);
                    }
                }
                return mDir;
            }
        }

        /// <summary>
        /// Draws a circle gizmo aligned with the xz-plane.
        /// </summary>
        /// <param name="transform">The transform to use for center, rotation, and scale.</param>
        /// <param name="localOffset">The local offset of the center of the circle.</param>
        /// <param name="radius">The radius of the circle. [Limit: >= 0]</param>
        /// <param name="color">The color of the circle.</param>
        public static void DrawCircleGizmo(Transform transform, Vector3 localOffset, float radius, Color color)
        {
            Gizmos.color = color;
            Gizmos.matrix = transform.localToWorldMatrix;

            float[] dir = Dir;

            var center = localOffset;

            for (int i = 0, j = CircleSegments - 1; i < CircleSegments; j = i++)
            {
                Vector3 a = new Vector3(center.x + dir[j * 2 + 0] * radius
                    , center.y
                    , center.z + dir[j * 2 + 1] * radius);

                Vector3 b = new Vector3(center.x + dir[i * 2 + 0] * radius
                    , center.y
                    , center.z + dir[i * 2 + 1] * radius);

                Gizmos.DrawLine(a, b);
            }

            Gizmos.matrix = Matrix4x4.identity;
        }

        #endregion

        #region Arrow Gizmo

        /// <summary>
        /// Draw an arrow gizmo.
        /// </summary>
        /// <param name="pointA">The start position of the arrow.</param>
        /// <param name="pointB">The end position of the arrow.</param>
        /// <param name="headScaleA">
        /// The size of the arrow head at the start position. [Limit: >= 0, 0 = No head]
        /// </param>
        /// <param name="headScaleB">
        /// The size of the arrow head at the end position. [Limit: >= 0, 0 = No head]
        /// </param>
        /// <param name="color">The color of the arrow.</param>
        public static void DrawArrowGizmo(Vector3 pointA, Vector3 pointB
            , float headScaleA, float headScaleB
            , Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(pointA, pointB);

            if (headScaleA > Epsilon)
                AppendArrowHead(pointA, pointB, headScaleA);
            if (headScaleB > Epsilon)
                AppendArrowHead(pointB, pointA, headScaleB);
        }

        private static void AppendArrowHead(Vector3 start, Vector3 end, float headScale)
        {
            if (Vector3.SqrMagnitude(end - start) < Epsilon * Epsilon)
                return;

            Vector3 az = (end - start).normalized;

            Vector3 vbase = start + az * headScale;
            Vector3 offset = Vector3.Cross(Vector3.up, az) * headScale * 0.333f;

            Gizmos.DrawLine(start, vbase + offset);
            Gizmos.DrawLine(start, vbase - offset);
        }

        #endregion

        #region Standard Marker

        /// <summary>
        /// The standard marker size used by <see cref="DrawMarkerGizmo(Transform, bool, Color)"/>. 
        /// </summary>
        public static Vector3 markerSize = new Vector3(0.15f, 0.01f, 0.15f);

        /// <summary>
        /// Draws a flattened box and x-maker gizmo suitable for selection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This gizmo is often combined with other annotations to make selection easier.
        /// Line-only based annotations can be hard to select.
        /// </para>
        /// </remarks>
        /// <param name="transform">
        /// The transform that represents the position, rotation, and scale of the marker.
        /// </param>
        /// <param name="localOffset">
        /// The local position offset of the marker.
        /// </param>
        /// <param name="color">
        /// The color of the marker.
        /// </param>
        /// <param name="includeRotation">
        /// If true the local rotation of <paramref name="transform"/> will be used.  Otherwise the
        /// marker will be aligned with the world axes.
        /// </param>
        public static void DrawMarkerGizmo(Transform transform, Vector3 localOffset, Color color, 
            bool includeRotation = true, float scale = 0.2f)
        {
            Gizmos.color = color;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawLine(localOffset, localOffset + Vector3.up * scale);
            Gizmos.DrawLine(localOffset + Vector3.right * scale, localOffset - Vector3.right * scale);
            Gizmos.DrawLine(
                localOffset, localOffset + Vector3.forward * (includeRotation ? 2 : 1) * scale);

            if (!includeRotation)
                Gizmos.DrawLine(localOffset, localOffset + Vector3.back * scale);

            Gizmos.DrawCube(localOffset, markerSize);

            Gizmos.matrix = Matrix4x4.identity;
        }

        #endregion

        #region Miscellaneous Gizmos

        /// <summary>
        /// Draws an x-marker gizmo.
        /// </summary>
        /// <param name="transform">The position, rotation, and scale of the x-marker.</param>
        /// <param name="localOffset">The local position offset of the x-marker.</param>
        /// <param name="color">The color of the x-marker.</param>
        public static void DrawXGizmo(Transform transform, Vector3 localOffset, Color color)
        {
            Gizmos.color = color;
            const float scale = 0.2f;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawLine(localOffset, localOffset + Vector3.up * scale);
            Gizmos.DrawLine(localOffset + Vector3.right * scale, localOffset - Vector3.right * scale);
            Gizmos.DrawLine(
                localOffset + Vector3.forward * scale, localOffset - Vector3.forward * scale);

            Gizmos.matrix = Matrix4x4.identity;
        }
        #endregion
    }
}
