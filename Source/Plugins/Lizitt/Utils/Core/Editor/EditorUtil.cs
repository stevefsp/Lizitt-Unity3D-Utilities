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
using UnityEditor;
using UnityEngine;

namespace com.lizitt.u3d.editor
{
    /// <summary>
    /// Provides general purpose utility functions for the Unity Editor.
    /// </summary>
    public static class EditorUtil
    {
        /// <summary>
        /// The base priority for the Lizitt asset creation menu.
        /// </summary>
        public const int AssetGroup = 1000;

        /// <summary>
        /// The base priority for the Lizitt game object creation menu.
        /// </summary>
        public const int GameObjectGroup = 2000;

        /// <summary>
        /// The base priority for the Lizitt view menu.
        /// </summary>
        public const int ViewGroup = 3000;

        public const int UtilityGroup = 4000;

        /// <summary>
        /// The base priority for the Lizitt manager menu group.
        /// </summary>
        public const int ManagerGroup = 9000;

        /// <summary>
        /// The base priority for the global menu group.
        /// </summary>
        public const int GlobalGroup = 10000;

        /// <summary>
        /// The root Lizitt menu name.
        /// </summary>
        public const string MainMenu = "Tools/Lizitt/";

        /// <summary>
        /// The Lizitt view menu name.
        /// </summary>
        public const string ViewMenu = MainMenu + "View/";

        public const string AssetCreateMenu = MainMenu + "Create Asset/";

        public const string UnityAssetCreateMenu = "Assets/Create/Lizitt/";

        public const string UtilityMenu = MainMenu + "Utilities/";

        public const string GlobalAssetPath = "Assets/GlobalAssets/";

        // TODO: Create a generic global asset method in the main EditorUtil class, then have
        // this method call that one with a standard path.
        /// <summary>
        /// Gets the specified global asset.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Global assets are singleton assets that exist in a well known path within the project.
        /// </para>
        /// <para>
        /// If the global asset does not exist a new one will be created.  So this method will
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

        /// <summary>
        /// Provides an editor GUI for adding/removing objects from a list based on object type.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="label">The GUI label. (Type description.)</param>
        /// <param name="items">The list of items to manage.</param>
        /// <param name="allowScene">Allow scene objects in the list.</param>
        /// <returns>True if the GUI changed within the method.</returns>
        public static bool OnGUIManageObjectList<T>(string label, List<T> items, bool allowScene) 
            where T : UnityEngine.Object
        {
            if (items == null)
                return false;

            bool origChanged = GUI.changed;
            GUI.changed = false;

            // Never allow nulls.  So get rid of them first.
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] == null)
                {
                    items.RemoveAt(i);
                    GUI.changed = true;
                }
            }

            GUILayout.Label(label);

            if (items.Count > 0)
            {
                int delChoice = -1;

                for (int i = 0; i < items.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    T item = (T)EditorGUILayout.ObjectField(items[i], typeof(T), allowScene);

                    if (item == items[i] || !items.Contains(item))
                        items[i] = item;

                    if (GUILayout.Button("X", GUILayout.Width(30)))
                        delChoice = i;

                    EditorGUILayout.EndHorizontal();
                }

                if (delChoice >= 0)
                {
                    GUI.changed = true;
                    items.RemoveAt(delChoice);
                }
            }

            EditorGUILayout.Separator();

            T nitem = (T)EditorGUILayout.ObjectField("Add", null, typeof(T), allowScene);

            if (nitem != null)
            {
                if (!items.Contains(nitem))
                {
                    items.Add(nitem);
                    GUI.changed = true;
                }
            }

            bool result = GUI.changed;
            GUI.changed = GUI.changed || origChanged;

            return result;
        }

        /// <summary>
        /// Provides an editor GUI for adding/removing strings from a list.
        /// </summary>
        /// <param name="items">The list of strings to manage.</param>
        /// <param name="isTags">True if the strings represent tags.</param>
        /// <returns>True if the GUI changed within the method.</returns>
        public static bool OnGUIManageStringList(List<string> items, bool isTags)
        {
            if (items == null)
                return false;

            bool origChanged = GUI.changed;
            GUI.changed = false;

            if (items.Count > 0)
            {
                GUILayout.Label((isTags ? "Tags" : "Items"));

                int delChoice = -1;

                for (int i = 0; i < items.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    string item;

                    if (isTags)
                        item = EditorGUILayout.TagField(items[i]);
                    else
                        item = EditorGUILayout.TextField(items[i]);

                    if (item == items[i] || !items.Contains(item))
                        items[i] = item;

                    if (GUILayout.Button("X", GUILayout.Width(30)))
                        delChoice = i;

                    EditorGUILayout.EndHorizontal();
                }

                if (delChoice >= 0)
                {
                    GUI.changed = true;
                    items.RemoveAt(delChoice);
                }
            }

            EditorGUILayout.Separator();

            string ntag = EditorGUILayout.TagField("Add", "");

            if (ntag.Length > 0)
            {
                if (!items.Contains(ntag))
                {
                    GUI.changed = true;
                    items.Add(ntag);
                }
            }

            bool result = GUI.changed;
            GUI.changed = GUI.changed || origChanged;

            return result;
        }

        /// <summary>
        /// Creates a new asset in the same directory as the specified asset.
        /// </summary>
        /// <typeparam name="T">The type of asse to create.</typeparam>
        /// <param name="atAsset">The asset where the new asset should be colocated.</param>
        /// <param name="name">The name of the asset or object's type name if null.</param>
        /// <param name="label">The asset label to attach. (If applicable.)</param>
        /// <returns>The new asset.</returns>
        public static T CreateAsset<T>(ScriptableObject atAsset, string name,  string label) 
            where T : ScriptableObject
        {
            if (name == null || name.Length == 0)
                name = typeof(T).ToString();

            string path = GenerateStandardPath(name);

            T result = ScriptableObject.CreateInstance<T>();
            result.name = name;

            AssetDatabase.CreateAsset(result, path);

            if (label.Length > 0)
                AssetDatabase.SetLabels(result, new string[1] { label });

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path);

            return result;
        }

        /// <summary>
        /// Creates a new asset at the standard path.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method tries to detect the directory of the currently selected asset.  If it 
        /// can't, it will place the new asset in the root asset directory.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of asse to create.</typeparam>
        /// <param name="name">The name of the asset or object's type name if null.</param>
        /// <param name="label">The asset label.</param>
        /// <returns>The new asset.</returns>
        public static T CreateAsset<T>(string name, string label) 
            where T : ScriptableObject
        {
            if (name == null || name.Length == 0)
                name = typeof(T).ToString();

            string path = GenerateStandardPath(name);

            T result = ScriptableObject.CreateInstance<T>();
            result.name = name;

            AssetDatabase.CreateAsset(result, path);

            if (label.Length > 0)
                AssetDatabase.SetLabels(result, new string[1] { label });

            AssetDatabase.SaveAssets(); 
            AssetDatabase.ImportAsset(path);

            return result;
        }

        /// <summary>
        /// Attempts to get a scene position based on the current SceneView camera.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful for creating game objects in the scene via script.
        /// </para>
        /// <para>
        /// The returned position will be one of the following. (In order of priority.)
        /// </para>
        /// <ol>
        /// <li>The hit position of a ray cast from the SceneView camera.</li>
        /// <li>A position aproximately 15 units forward from the SceneView camera.</li>
        /// <li>The zero vector.</li>
        /// </ol>
        /// </remarks>
        /// <returns>The recommended position in the scene.</returns>
        public static Vector3 GetCreatePosition()
        {
            Camera cam = SceneView.lastActiveSceneView.camera;

            if (!cam)
                return Vector3.zero;

            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hit, 500))
                return hit.point;
                  
            return cam.transform.position + cam.transform.forward * 15;
        }

        private static string GenerateStandardPath(string name)
        {
            string dir = "";

            foreach (var item in Selection.objects)
            {
                dir = AssetDatabase.GetAssetPath(item);

                if (dir.Length > 0)
                {
                    if (!Directory.Exists(dir))
                        // Selection must be a file asset.
                        dir = Path.GetDirectoryName(dir);

                    break;
                }
            }

            if (dir.Length == 0)
                dir = "Assets";

            return AssetDatabase.GenerateUniqueAssetPath(dir + "/" + name + ".asset");
        }
    }
}
