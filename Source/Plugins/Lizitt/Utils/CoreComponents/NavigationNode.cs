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
    /// Represents a navigation marker that is part of a node network.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is similar to a waypoint graph, but each node can have an arbitrary number of exit nodes instead 
    /// of just one.
    /// </para>
    /// </remarks>
    [AddComponentMenu(LizMenu.Menu + "Navigation Node", LizMenu.MarkerComponentMenuOrder + 2)]
    public class NavigationNode
        : NavigationMarker
    {
        [SerializeField]
        [Tooltip("The exit nodes for the node.")]
        private BaseNavigationMarker[] m_Links = new BaseNavigationMarker[0];

        #region Iteration Members

        /// <summary>
        /// Gets the specified link's node.
        /// </summary>
        /// <param name="index">
        /// The index of the link to retrieve. [Limits: 0 &lt;= value &lt; <see cref="LinkCount"/>]
        /// </param>
        /// <returns>The specified link's node.</returns>
        public BaseNavigationMarker this[int index]
        {
            get { return m_Links[index]; }
        }

        /// <summary>
        /// Get the link at the specified index.
        /// </summary>
        /// <param name="index">The index. [0 &lt;= value &lt; <see cref="LinkCount"/></param>
        /// <returns>The link, or null if there is none.</returns>
        public override BaseNavigationMarker GetLink(int index)
        {
            return m_Links[index];
        }

        /// <summary>
        /// The number of outgoing links.
        /// </summary>
        public override int LinkCount
        {
            get {return m_Links.Length; }
        }

        /// <summary>
        /// An enumerator of the exit links for the node.
        /// </summary>
        /// <returns>An enumerator of the links for the node.</returns>
        public override IEnumerator<BaseNavigationMarker> GetEnumerator()
        {
            foreach (var item in m_Links)
            {
                if (item)
                    yield return item;
            }
        }

        #endregion

        #region Gizmo Members

        /// <summary>
        /// The color to use for Gizmos.
        /// </summary>
        protected override Color GizmoColor
        {
            get { return ColorUtil.Cyan; }
        }

        protected override void OnDrawGizmos()
        {
            foreach (var item in this)
            {
                if (item)
                    AnnotationUtil.DrawArrowGizmo(transform.position, item.transform.position, 0, 0.5f, GizmoColor);
            }

            base.OnDrawGizmos();
        }

        #endregion
    }
}
