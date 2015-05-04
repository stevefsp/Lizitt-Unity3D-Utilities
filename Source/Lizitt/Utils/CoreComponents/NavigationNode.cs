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
using System.Collections.Generic;
using UnityEngine;

namespace com.lizitt.u3d
{
    /// <summary>
    /// Represents a navigation marker that is part of a node network.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is similar to a waypoint graph, but each node can have an arbitrary number of
    /// exit nodes instead of just one.
    /// </para>
    /// </remarks>
    public class NavigationNode
        : NavigationMarker, IEnumerable<NavigationNode>
    {
        [SerializeField]
        [Tooltip("The exit nodes for the node.")]
        private NavigationNode[] m_Links = new NavigationNode[0];

        /// <summary>
        /// Gets the specified exit node. [Limits: 0 &lt;= value &lt; index]
        /// </summary>
        /// <param name="index">The index of the link to retrieve.</param>
        /// <returns></returns>
        public NavigationNode this[int index]
        {
            get { return m_Links[index]; }
        }

        /// <summary>
        /// The number of exit links for the node.
        /// </summary>
        public int LinkCount
        {
            get {return m_Links.Length; }
        }

        /// <summary>
        /// An enumerator of the exit links for the node.
        /// </summary>
        /// <returns>An enumerator of the exit links for the node.</returns>
        public IEnumerator<NavigationNode> GetEnumerator()
        {
            foreach (var item in m_Links)
            {
                if (item)
                    yield return item;
            }
        }

        /// <summary>
        /// An enumerator of the exit links for the node.
        /// </summary>
        /// <returns>An enumerator of the exit links for the node.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// The color to use for Gizmos.
        /// </summary>
        public override Color GizmoColor
        {
            get { return ColorUtil.Cyan; }
        }

        /// <summary>
        /// Draw the gizmo.  (Only call from Gizmo-legal methods.)
        /// </summary>
        public override void DrawGizmo()
        {
            foreach (var item in this)
            {
                if (item)
                {
                    AnnotationUtil.DrawArrowGizmo(
                        transform.position, item.transform.position, 0, 0.5f, GizmoColor);
                }
            }

            base.DrawGizmo();
        }
    }
}
