/*
 * Copyright (c) 2012-2015 Stephen A. Pratt
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
using UnityEditor;
using UnityEngine;

namespace com.lizitt.editor
{
    /// <summary>
    /// Marker utilities.
    /// </summary>
    public static class MarkerEditorUtil
    {
        #region GameObject Menus

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "Marker", false, 7)]
        private static void CreateMarker(MenuCommand command)
        {
            CreateMarker<Marker>(command, false);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "NavMarker", false, 8)]
        private static void CreateNavigationMarker(MenuCommand command)
        {
            CreateMarker<NavigationMarker>(command, false);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "NavNode", false, 9)]
        private static void CreateNavigationNode(MenuCommand command)
        {
            CreateMarker<NavigationNode>(command, false);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "NavWaypoint", false, 10)]
        private static void CreateNavigationWaypoint(MenuCommand command)
        {
            CreateMarker<NavigationWaypoint>(command, false);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "Aligned Marker", false, 11)]
        private static void CreateAlignedMarker(MenuCommand command)
        {
            CreateMarker<Marker>(command, true);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "Aligned NavMarker", false, 12)]
        private static void CreateAlignedNavigationMarker(MenuCommand command)
        {
            CreateMarker<NavigationMarker>(command, true);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "Aligned NavNode", false, 13)]
        private static void CreateAlignedNavigationNode(MenuCommand command)
        {
            CreateMarker<NavigationNode>(command, true);
        }

        [MenuItem("GameObject/" + LizittUtil.LizittMenu + "Aligned NavWaypoint", false, 14)]
        private static void CreateAlignedNavigationWaypointMarker(MenuCommand command)
        {
            CreateMarker<NavigationWaypoint>(command, true);
        }

        private static void CreateMarker<T>(MenuCommand command, bool aligned)
            where T : Marker
        {
            GameObject go;

            if (command.context && command.context is GameObject)
                go = CreateAlignedMarker<T>((command.context as GameObject).transform).gameObject;
            else if (aligned)
                go = CreateAlignedMarker<T>().gameObject;
            else
                go = CreatePositionedMarker<T>().gameObject;

            Selection.activeObject = go;
        }

        #endregion

        #region Create Markers

        /// <summary>
        /// Create a marker that is positioned on the surface of the current scene camera's target. (Includes undo.)
        /// </summary>
        /// <typeparam name="T">The type of marker.</typeparam>
        /// <param name="name">The name, or null to use the marker type's name.</param>
        /// <param name="maxDistance">The maximum raycast distance.</param>
        /// <param name="failDistance">
        /// The distance to position the the marker at if the raycast fails to hit anything.
        /// </param>
        /// <returns>The newly created marker.</returns>
        public static T CreatePositionedMarker<T>(string name = null, float maxDistance = 500, float failDistance = 15)
            where T : Marker
        {
            return CreateMarker<T>(EditorUtil.GetCreatePosition());
        }

        /// <summary>
        /// Create a marker that is positioned and aligned with the surface of the current scene camera's target. 
        /// (Includes undo.)
        /// </summary>
        /// <typeparam name="T">The type of marker.</typeparam>
        /// <param name="name">The name, or null to use the marker type's name.</param>
        /// <param name="maxDistance">The maximum raycast distance.</param>
        /// <param name="failDistance">
        /// The distance to position the the marker at if the raycast fails to hit anything.
        /// </param>
        /// <returns>The newly created marker.</returns>
        public static T CreateAlignedMarker<T>(string name = null, float maxDistance = 500, float failDistance = 15)
            where T : Marker
        {
            var go = CreateMarker<T>(name);
            EditorUtil.AlignAtSceneViewHit(go.transform, maxDistance, failDistance);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            return go.GetComponent<T>();
        }

        /// <summary>
        /// Create a marker with the specified position and rotation. (Includes undo.)
        /// </summary>
        /// <typeparam name="T">The type of marker.</typeparam>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="name">The name, or null to use the marker type's name.</param>
        /// <returns>The newly created marker.</returns>
        public static T CreateMarker<T>(Vector3 position, Quaternion rotation, string name = null)
            where T : Marker
        {
            var go = CreateMarker<T>(name);
            go.transform.position = position;
            go.transform.rotation = rotation;

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            return go.GetComponent<T>();
        }

        /// <summary>
        /// Create a marker with the specified position. (Includes undo.)
        /// </summary>
        /// <typeparam name="T">The type of marker.</typeparam>
        /// <param name="position">The position.</param>
        /// <param name="name">The name, or null to use the marker type's name.</param>
        /// <returns>The newly created marker.</returns>
        public static T CreateMarker<T>(Vector3 position, string name = null)
            where T : Marker
        {
            return CreateMarker<T>(position, Quaternion.identity, name);
        }

        /// <summary>
        /// Create a marker that is parented and aligned to the specified transform. (Includes undo.)
        /// </summary>
        /// <typeparam name="T">The type of marker.</typeparam>
        /// <param name="parent">The transform to parent and align to.</param>
        /// <param name="name">The name, or null to use the marker type's name.</param>
        /// <returns>The newly created marker.</returns>
        public static T CreateAlignedMarker<T>(Transform parent, string name = null)
            where T : Marker
        {
            var go = CreateMarker<T>(name);
            GameObjectUtility.SetParentAndAlign(go, parent.gameObject);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            return go.GetComponent<T>();
        }

        private static GameObject CreateMarker<T>(string name = null)
        {
            return new GameObject(string.IsNullOrEmpty(name) ? typeof(T).Name : name, typeof(T));
        }

        #endregion
    }
}
