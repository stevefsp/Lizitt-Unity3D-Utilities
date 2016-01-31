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
using UnityEditorInternal;
using UnityEngine;
using System.Linq;

namespace com.lizitt.editor
{
    /// <summary>
    /// Draws a user friendly GUI element for a list of <see cref="MaterialOverride"/> objects,
    /// including optional restriction of selections to local renderers.
    /// </summary>
    public class MaterialOverrideListControl
    {
        private struct Props
        {
            public SerializedProperty renderer;
            public SerializedProperty index;
            public SerializedProperty material;

            public Props(SerializedProperty property)
            {
                material = property.FindPropertyRelative("m_Material");

                var ptrProp = property.FindPropertyRelative("m_Target");

                renderer = ptrProp.FindPropertyRelative("m_Renderer");
                index = ptrProp.FindPropertyRelative("m_MaterialIndex");
            }
        }

        private class LocalInfo
        {
            public GameObject GameObject { get; set; }
            public List<Renderer> Items { get; set; }
            public GUIContent[] Labels { get; set; }

            public Material[][] Materials { get; set; }
            public GUIContent[][] MaterialLabels { get; set; }

            public void Load(GameObject gameObject)
            {
                this.GameObject = gameObject;

                if (Items == null)
                    Items = new List<Renderer>(5);
                else
                    Items.Clear();

                Labels = null;
                Materials = null;
                MaterialLabels = null;

                if (this.GameObject == null)
                {
                    Items.Add(null);

                    Labels = new GUIContent[1];
                    Labels[0] = 
                        new GUIContent("Local reference not assigned.", "No local information available.");

                    return;
                }

                LoadRenderers(gameObject.transform);

                if (Items.Count == 0)
                {
                    Items.Add(null);
                    Labels = new GUIContent[1];
                    Labels[0] = new GUIContent("No renderers.", "No renderers found.");
                    return;
                }

                Items.Add(null);

                Labels = new GUIContent[Items.Count];
                Materials = new Material[Items.Count][];
                MaterialLabels = new GUIContent[Items.Count][];

                var iter = Items.OrderBy(o => (o ? o.name : ""));

                int i = 0;
                foreach (var item in iter)
                {
                    //Debug.Log("Item: " + item);

                    Items[i] = item;

                    if (!item)
                    {
                        Labels[i] = new GUIContent("Select Renderer...");
                        Materials[i] = new Material[0];
                        MaterialLabels[i] = new GUIContent[0];
                    }
                    else
                    {
                        var mats = Items[i].sharedMaterials;
                        var labels = new GUIContent[mats.Length];

                        for (int j = 0; j < mats.Length; j++)
                            labels[j] = new GUIContent(mats[j] ? mats[j].name : "Material " + j);

                        Labels[i] = new GUIContent(item.name);
                        Materials[i] = mats;
                        MaterialLabels[i] = labels;
                    }

                    i++;
                }
            }

            private void LoadRenderers(Transform transform)
            {
                var renderer = transform.GetComponent<Renderer>();
                if (renderer)
                    Items.Add(renderer);

                for (int i = 0; i < transform.childCount; i++)
                    LoadRenderers(transform.GetChild(i));
            }
        }

        private static readonly float HeaderHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static readonly float FooterHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static readonly float ElementHeight = 
            EditorGUIUtility.singleLineHeight + (EditorGUIUtility.singleLineHeight * 1.1f * 2)
            + EditorGUIUtility.standardVerticalSpacing * 4
            + 10;  // Improves separation of elements.  Looks better.

        private static readonly GUIContent RendererLabel = new GUIContent(
            "Renderer", "The renderer that contains the material to replace. (Required)");

        private static readonly GUIContent TargetLabel = new GUIContent(
            "Target", "The material that will be replaced.");

        private static readonly GUIContent MaterialLabel = new GUIContent(
            "Override", "Material that will override the target material.");

        private ReorderableList m_List;
        private LocalInfo m_LocalInfo = null;
        private string m_SearchPropertyPath = null;
        private bool m_LocalOnly = false;

        /// <summary>
        /// Constructor. (Select any renderer.)
        /// </summary>
        /// <param name="property">
        /// A property representing an array <see cref="MaterialOverride"/> references.
        /// </param>
        public MaterialOverrideListControl(SerializedProperty property)
        {
            m_LocalOnly = false;

            CreateList(property);
        }

        /// <summary>
        /// Constructor. (Only renderers local to reference object.)
        /// </summary>
        /// <param name="property">
        /// A property representing an array <see cref="MaterialOverride"/> references.
        /// </param>
        /// <param name="searchPropertyPath">
        /// The path (from the serialized object root) of the property that will supply the
        /// reference object, or null if the referene object is the property's target object.
        /// </param>
        public MaterialOverrideListControl(SerializedProperty property, string searchPropertyPath)
        {
            m_SearchPropertyPath = searchPropertyPath;
            m_LocalOnly = true;

            CreateList(property);
        }

        /// <summary>
        /// The required draw height of the list.
        /// </summary>
        /// <param name="property">
        /// The property representing an array <see cref="MaterialOverride"/> references.
        /// (For the same field as the property used in the constructor.)
        /// </param>
        public float GetPropertyHeight(SerializedProperty property)
        {
            int itemCount = m_List.serializedProperty.arraySize;

            float result = HeaderHeight + FooterHeight + EditorGUIUtility.singleLineHeight;

            if (itemCount == 0)
            {
                m_List.elementHeight = EditorGUIUtility.singleLineHeight;
                result += EditorGUIUtility.singleLineHeight * 1.1f;
            }
            else
            {
                m_List.elementHeight = ElementHeight;
                result += ElementHeight * itemCount;
            }

            return result;
        }

        /// <summary>
        /// Draws the GUI element.
        /// </summary>
        /// <param name="position">The draw area.</param>
        /// <param name="property">
        /// The property representing an array <see cref="MaterialOverride"/> references.
        /// (For the same field as the property used in the constructor.)
        /// </param>
        /// <param name="label">The property label.</param>
        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_LocalOnly)
            {
                var gameObject = EditorGUIUtil.GetReferenceObject(property, m_SearchPropertyPath, false);

                if (m_LocalInfo == null || m_LocalInfo.GameObject != gameObject)
                {
                    m_LocalInfo = new LocalInfo();
                    m_LocalInfo.Load(gameObject);
                }
            }

            label = EditorGUI.BeginProperty(position, label, property);

            m_List.serializedProperty = property;
            m_List.DoList(position);

            EditorGUI.EndProperty();
        }

        private void CreateList(SerializedProperty property)
        {
            var list = new ReorderableList(property.serializedObject, property
                , true, true, true, true);

            list.headerHeight = HeaderHeight;
            list.footerHeight = FooterHeight;

            list.drawHeaderCallback = delegate(Rect rect)
            {
                EditorGUI.LabelField(rect, "Material Overrides");
            };

            list.drawElementCallback =
                delegate(Rect position, int index, bool isActive, bool isFocused)
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);

                    if (m_LocalInfo == null)
                        StandardGUI(position, element);
                    else
                        LocalGUI(position, element);
                };

            list.onAddCallback = delegate(ReorderableList roList)
            {
                roList.index = AddItem(roList.serializedProperty);
            };

            m_List = list;
        }

        private int AddItem(SerializedProperty listProp)
        {
            int nidx = listProp.arraySize;

            listProp.arraySize++;

            var element = listProp.GetArrayElementAtIndex(nidx);
            // Override default behavior.  Rarely want to duplicate.

            var props = new Props(element);

            props.index.intValue = 0;
            props.renderer.objectReferenceValue = null;
            props.material.objectReferenceValue = null;

            listProp.serializedObject.ApplyModifiedProperties();

            return nidx;
        }

        private void LocalGUI(Rect position, SerializedProperty property)
        {
            var origLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;

            var props = new Props(property);
            var info = m_LocalInfo;

            // Renderer

            int iOrig = 0;

            GUIStyle style;
            if (props.renderer.objectReferenceValue)
            {
                iOrig = info.Items.IndexOf(props.renderer.objectReferenceValue as Renderer);
                iOrig = iOrig == -1 ? 0 : iOrig;
                style = EditorStyles.popup;
            }
            else
                style = EditorGUIUtil.RedPopup;

            var rect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing + 2
                , position.width, EditorGUIUtility.singleLineHeight * 1.15f);
            int iSel = EditorGUI.Popup(rect, RendererLabel, iOrig, info.Labels, style);

            if (iSel != iOrig)
                props.renderer.objectReferenceValue = info.Items[iSel];

            rect = EditorGUIUtil.NextGuiElementPosition(rect);

            if (iSel == 0)
                EditorGUI.LabelField(rect, TargetLabel, new GUIContent("None"));
            else
            {
                GUIContent[] labels = info.MaterialLabels[iSel];

                iOrig = Mathf.Clamp(props.index.intValue, 0, labels.Length);

                iSel = EditorGUI.Popup(rect, TargetLabel, iOrig, labels);
                if (iSel != iOrig)
                    props.index.intValue = iSel;
            }

            // Override material.

            rect = EditorGUIUtil.NextGuiElementPosition(rect, 0, false);
            EditorGUI.PropertyField(rect, props.material, MaterialLabel);

            EditorGUIUtility.labelWidth = origLabelWidth;
        }

        private void StandardGUI(Rect position, SerializedProperty property)
        {
            var props = new Props(property);


            var rect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing
                , position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rect, props.renderer);

            rect = EditorGUIUtil.NextGuiElementPosition(rect);
            EditorGUI.PropertyField(rect, props.index);

            rect = EditorGUIUtil.NextGuiElementPosition(rect);
            EditorGUI.PropertyField(rect, props.material);
        }
    }
}
