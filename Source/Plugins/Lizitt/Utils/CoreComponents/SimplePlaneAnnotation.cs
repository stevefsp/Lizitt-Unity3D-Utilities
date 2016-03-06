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
#if UNITY_EDITOR

using UnityEngine;

namespace com.lizitt
{
    /// <summary>
    /// A colored plane annotation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The annotation's GameObject should usually be marked as EditorOnly since the plane
    /// is a gizmo.
    /// </para>
    /// </remarks>
    [AddComponentMenu(LizittUtil.LizittMenu + "Plane Annotation (Editor Only)", LizittUtil.EditorComponentMenuOrder)]
    public class SimplePlaneAnnotation
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The color of the plane.")]
        private Color m_Color = new Color(0.0f, 0.78f, 0.78f, 0.6f);  // Cyan.
        
        [SerializeField]
        [Tooltip("The plane's local position offset.")]
        private Vector3 m_Offset = Vector3.zero;

        /// <summary>
        /// The color of the plane.
        /// </summary>
        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        /// <summary>
        /// The plane's local position offset.
        /// </summary>
        public Vector3 PositionOffset
        {
            get { return m_Offset; }
            set { m_Offset = value; }
        }

        #region Gizmo

        void OnDrawGizmos()
        {
            Gizmos.color = Color;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(PositionOffset, new Vector3(1, 0.01f, 1));
        }

        #endregion
    }
}
#endif
