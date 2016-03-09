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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.lizitt.editor
{
    public struct SerializedFieldInfo
    {
        public string name;
        public bool isHidden;
        public System.Type typ;

        public override string ToString()
        {
            return string.Format("Name: {0}, Type: {1}, IsHidden: {2}", name, typ, isHidden);
        }
    }

    /// <summary>
    /// Provides general purpose utility functions for the Unity Editor.
    /// </summary>
    public static class LizittEditorUtil
    {
        #region Menu Related Members

        /// <summary>
        /// The tools menu.
        /// </summary>
        public const string BaseToolsMenu = "Tools/";

        /// <summary>
        /// The root Lizitt menu name.
        /// </summary>
        public const string LizittToolsMenu = BaseToolsMenu + LizittUtil.Menu;

        public const string LizittGameObjectMenu = "GameObject/" + LizittUtil.Menu;

        #endregion

        #region Global Assets

        /// <summary>
        /// The storage location for global assets.
        /// </summary>
        public const string GlobalAssetPath = "Assets/GlobalAssets/";

        /// <summary>
        /// Gets the specified global asset.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Global assets are singleton assets that exist in a well known path within the project.
        /// </para>
        /// <para>
        /// If the global asset does not exist a new one will be created, so this method will
        /// return null only on error.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of global asset.</typeparam>
        /// <returns>The global asset.</returns>
        public static T GetGlobalAsset<T>()
            where T : ScriptableObject
        {
            return GetGlobalAsset<T>(GlobalAssetPath);
        }

        public static T GetGlobalAsset<T>(string path)
            where T : ScriptableObject
        {
            string rootPath = (path == null || path.Length == 0) 
                ? GlobalAssetPath 
                : path;

            if (!Directory.Exists(rootPath))
            {
                Debug.LogError("Global assets path does not exist: " + rootPath);
                return null;
            }

            string name = typeof(T).Name;
            string assetPath = rootPath + name + ".asset";

            T result = (T)AssetDatabase.LoadMainAssetAtPath(assetPath);

            if (!result)
            {
                result = ScriptableObject.CreateInstance<T>();
                result.name = name;

                AssetDatabase.CreateAsset(result, assetPath);
                AssetDatabase.ImportAsset(assetPath);
            }

            return result;
        }

        #endregion

        # region Scene Camera and Raycasting

        /// <summary>
        /// Attempts to get a scene position based on the last scene view camera.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful for creating game objects in the scene via script.
        /// </para>
        /// <para>
        /// The returned position will be one of the following. (In order of priority.)
        /// </para>
        /// <ol>
        /// <li>The hit position of a ray cast from the scene view camera.</li>
        /// <li>A position <paramref name="failDistance"/> forward of the scene view camera.</li>
        /// <li>The zero vector. (If no scene view camera was found.)</li>
        /// </ol>
        /// </remarks>
        /// <param name="maxDistance">The maximum raycast distance.</param>
        /// <param name="failDistance">The position distance to return if the raycast failes.</param>
        /// <returns>The recommended position in the scene.</returns>
        public static Vector3 GetCreatePosition(float maxDistance = 500, float failDistance = 15)
        {
            RaycastHit hit;
            if (RaycastFromCamera(maxDistance, out hit))
                return hit.point;

            return GetSceneCameraForwardPosition(failDistance);
        }

        /// <summary>
        /// Positions and aligns the transform on the surface of the current scene camera's target.
        /// </summary>
        /// <param name="transform">The transform. (Required)</param>
        /// <param name="maxDistance">The maximum racast distance.</param>
        /// <param name="failDistance">The distance to use if the raycast fails to hit.</param>
        public static void AlignAtSceneViewHit(Transform transform, float maxDistance = 500, float failDistance = 15)
        {
            RaycastHit hit;
            if (RaycastFromCamera(maxDistance, out hit))
            {
                transform.position = hit.point;
                transform.up = hit.normal;
            }
            else
            {
                transform.position = GetSceneCameraForwardPosition(failDistance);
                transform.rotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// Performs a default raycast from the last scene view camera.
        /// </summary>
        /// <param name="maxDistance">The maximum raycast distance.</param>
        /// <param name="hit">The hit information.</param>
        /// <returns>True if a hit was detected, otherwise false.</returns>
        public static bool RaycastFromCamera(float maxDistance, out RaycastHit hit)
        {
            hit = new RaycastHit();

            Camera cam = SceneView.lastActiveSceneView.camera;
            if (!cam)
                return false;

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            return Physics.Raycast(ray, out hit, maxDistance);
        }

        /// <summary>
        /// Gets a position forward of the last active scene camera by the specified distance, or <c>Vector3.zero</c> 
        /// on failure.
        /// </summary>
        /// <param name="distance">The distance forward.</param>
        /// <returns>
        /// A position forward of the last active scene camera by the specified distance, or <c>Vector3.zero</c>
        /// on failure.
        /// </returns>
        public static Vector3 GetSceneCameraForwardPosition(float distance = 1)
        {
            Camera cam = SceneView.lastActiveSceneView.camera;
            if (!cam)
                return Vector3.zero;

            return cam.transform.position + cam.transform.forward * distance;
        }

        #endregion

        #region Serialized Object Members

        /// <summary>
        /// Get the serialized field information for a Monobehaviour script.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This information can be usedful when creating custom editors.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of Monobehaviour script.</typeparam>
        /// <param name="list">The list to add the infomration to.  (List will not be cleared.) (Optional)</param>
        /// <returns>
        /// A reference to <paramref name="list"/> if if was provided, or a new list if <paramref name="list"/> 
        /// was null.
        /// </returns>
        public static List<SerializedFieldInfo> AddSerializedFields<T>(List<SerializedFieldInfo> list = null)
            where T : MonoBehaviour
        {
            if (list == null)
                list = new List<SerializedFieldInfo>();

            var typ = typeof(T);

            while (typ != typeof(MonoBehaviour))
            {
                foreach (var info in typ.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (info.IsPublic || info.GetCustomAttributes(typeof(SerializeField), true).Length != 0)
                    {
                        var data = new SerializedFieldInfo();
                        data.name = info.Name;
                        data.typ = info.FieldType;
                        data.isHidden = info.GetCustomAttributes(typeof(HideInInspector), true).Length != 0;
                        list.Add(data);
                    }
                }

                typ = typ.BaseType;
            }

            return list;
        }

        #endregion
    }
}
