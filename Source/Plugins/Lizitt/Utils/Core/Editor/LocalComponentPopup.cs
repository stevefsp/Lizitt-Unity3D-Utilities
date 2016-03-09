/*
 * Copyright (c) 2016 Stephen A. Pratt
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
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace com.lizitt.editor
{
    /// <summary>
    /// Draws a popup GUI element containing a list of components that are attached to the
    /// property's game object or one of its children.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is sometimes useful to restrict selection of a reference field to only objects
    /// that are 'local' to the current object.  E.g. Only colliders that are attached to the
    /// current <c>GameObject</c> or its children.  The class will draw popup for 
    /// reference fields that only allows selection of local objects.
    /// </para>
    /// </remarks>
    public class LocalComponentPopup
    {
        private class Info
        {
            public GameObject GameObject { get; set; }
            public readonly List<Component> Items = new List<Component>(5);
            public GUIContent[] ItemLabels { get; set; } // << Must be an array for editor purposes.

            public void Refresh(GameObject gameObject, System.Type typ, bool required)
            {
                // This process is desinged to support both scene objects and prefab assets.
                // (GetComponentInChildren() and related methods don't work with assets.)

                this.GameObject = gameObject;

                Items.Clear();
                ItemLabels = null;

                LoadComponents(gameObject, typ);

                if (Items.Count == 0)
                {
                    Items.Add(null);
                    ItemLabels = new GUIContent[1];
                    ItemLabels[0] = new GUIContent("<None Available>");
                    return;
                }

                Items.Add(null);
                var iter = Items.OrderBy(o => (o ? o.name : ""));

                ItemLabels = new GUIContent[Items.Count];

                var noneLabel = required ? "Select " + typ.Name + "..." : "<None>";

                int j = 0;
                foreach (var item in iter)
                {
                    Items[j] = item;

                    ItemLabels[j] = new GUIContent(item ? item.name : noneLabel);

                    j++;
                }
            }

            // TODO: Unscheduled: Add a simpler 5.3+ version.  (GetComponentsInChildren now works on prefabs.)
            private void LoadComponents(GameObject gameObject, System.Type typ)
            {
                // Game object can be null when the 'search object' is unassigned,
                // so a null is not an error.
                if (gameObject)
                {
                    foreach (var item in gameObject.GetComponents(typ))
                        Items.Add(item);

                    for (int i = 0; i < gameObject.transform.childCount; i++)
                        LoadComponents(gameObject.transform.GetChild(i).gameObject, typ);
                }
            }
        }

        private readonly System.Type m_ComponentType;

        /// <summary>
        /// The reference property's component type.  (Used for the local component search.)
        /// </summary>
        public System.Type ComponentType
        {
            get { return m_ComponentType; }
        } 

        /// <summary>
        /// If true, only a non-null reference is valid.  Otherwise nulls are considered a valid
        /// selection.
        /// </summary>
        public bool Required { get; set; }

        private Info m_ListInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentType">
        /// The reference property's component type.  (Used for the local component search.)
        /// </param>
        /// <param name="required">
        /// If true, only a non-null reference is valid.  Otherwise nulls are considered a valid
        /// selection.
        /// </param>
        public LocalComponentPopup(System.Type componentType, bool required = false)
        {
            m_ComponentType = componentType;
            Required = required;
        }

        /// <summary>
        /// Draws the GUI element.
        /// </summary>
        /// <param name="position">The draw area of the element.</param>
        /// <param name="property">
        /// The property of type <c>SerializedPropertyType.ObjectReference</c>
        /// </param>
        /// <param name="label">The property label.</param>
        /// <param name="gameObject">The game object to search for local components, or null
        /// if the property's target object should be used.</param>
        public void OnGUI(Rect position, SerializedProperty property, GUIContent label, 
            GameObject gameObject = null)
        {
            if (label != null)
                label = EditorGUI.BeginProperty(position, label, property);

            if (!gameObject)
                gameObject = LizittEditorGUIUtil.GetReferenceObject(property);

            if (m_ListInfo == null || m_ListInfo.GameObject != gameObject)
            {
                m_ListInfo = m_ListInfo == null ? new Info() : m_ListInfo;
                m_ListInfo.Refresh(gameObject, m_ComponentType, Required);
            }

            int iOrig = 0;
            GUIStyle style = EditorStyles.popup;

            if (property.objectReferenceValue)
            {
                iOrig = m_ListInfo.Items.IndexOf(property.objectReferenceValue as Component);
                iOrig = iOrig == -1 ? 0 : iOrig;
            }
            else if (Required)
                style = LizittEditorGUIUtil.RedPopup;

            int iSel = (label == null)
                ? EditorGUI.Popup(position, iOrig, m_ListInfo.ItemLabels, style)
                : EditorGUI.Popup(position, label, iOrig, m_ListInfo.ItemLabels, style);

            if (iSel != iOrig)
                property.objectReferenceValue = m_ListInfo.Items[iSel];

            if (label != null)
                EditorGUI.EndProperty();
        }

        public Component OnGUI(Rect position, Component currentValue, GUIContent label, GameObject gameObject)
        {
            if (m_ListInfo == null || m_ListInfo.GameObject != gameObject)
            {
                m_ListInfo = m_ListInfo == null ? new Info() : m_ListInfo;
                m_ListInfo.Refresh(gameObject, m_ComponentType, Required);
            }

            int iOrig = 0;
            GUIStyle style = EditorStyles.popup;

            if (currentValue)
            {
                iOrig = m_ListInfo.Items.IndexOf(currentValue);
                iOrig = iOrig == -1 ? 0 : iOrig;
            }
            else if (Required)
                style = LizittEditorGUIUtil.RedPopup;

            int iSel = (label == null)
                ? EditorGUI.Popup(position, iOrig, m_ListInfo.ItemLabels, style)
                : EditorGUI.Popup(position, label, iOrig, m_ListInfo.ItemLabels, style);

            return m_ListInfo.Items[iSel];
        }
    }
}
