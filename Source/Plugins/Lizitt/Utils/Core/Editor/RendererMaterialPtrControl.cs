
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
using System.Linq;  // Used for sorting dropdown values.
using UnityEditor;
using UnityEngine;

namespace com.lizitt.editor
{
    public class RendererMaterialPtrControl
    {
        #region Label Related Members

        private static readonly GUIContent RendererLabel = new GUIContent(
            "Renderer", "The target renderer.");

        private static readonly GUIContent TargetLabel = new GUIContent(
            "Material", "The target material.");

        private static readonly GUIContent[] NoneAvailableLabels = 
            new GUIContent[1] { new GUIContent("<None Available>") };

        public static readonly GUIContent NoneMaterial =
            new GUIContent("None", "Renderer not assigned.");

        private static GUIContent[] GetMaterialLabels(Renderer renderer)
        {
            var mats = renderer.sharedMaterials;

            if (mats.Length == 0)
                return NoneAvailableLabels;

            var labels = new GUIContent[mats.Length];

            for (int i = 0; i < mats.Length; i++)
                labels[i] = new GUIContent(mats[i] ? mats[i].name : "Material " + i);

            return labels;
        }

        #endregion

        #region Support Classes/Structs

        private struct Props
        {
            public SerializedProperty renderer;
            public SerializedProperty index;

            public Props(SerializedProperty property)
            {
                renderer = property.FindPropertyRelative("m_Renderer");
                index = property.FindPropertyRelative("m_MaterialIndex");
            }
        }

        private class SearchInfo
        {
            public GameObject gameObject = null;

            public readonly List<Renderer> renderers = new List<Renderer>(5);
            public GUIContent[] rendererLabels;

            public readonly List<GUIContent[]> materialLabels = new List<GUIContent[]>(5);

            public void Update(GameObject gameObject)
            {
                if (gameObject && this.gameObject == gameObject)
                    return;

                this.gameObject = gameObject;

                renderers.Clear();
                rendererLabels = null;
                materialLabels.Clear();

                LoadComponents(gameObject);

                if (renderers.Count == 0)
                {
                    renderers.Add(null);
                    rendererLabels = NoneAvailableLabels;
                    return;
                }

                renderers.Add(null);
                var iter = renderers.OrderBy(o => (o ? o.name : ""));

                rendererLabels = new GUIContent[renderers.Count];

                int j = 0;
                foreach (var item in iter)
                {
                    renderers[j] = item;
                    rendererLabels[j] =
                        new GUIContent(item ? item.name : "Select renderer...");

                    if (item)
                        materialLabels.Add(GetMaterialLabels(item));
                    else
                        materialLabels.Add(null);  // Will happen once.

                    j++;
                }
            }

            private void LoadComponents(GameObject gameObject)
            {
                // This process is desinged to support both scene objects and prefab assets.
                // (Before Unity 5.3, GetComponentInChildren() and related methods don't work 
                // with assets.)

                // Null is expected.  It means the search root is currently unassigned.
                if (gameObject)
                {
                    foreach (var item in gameObject.GetComponents<Renderer>())
                        renderers.Add(item);

                    for (int i = 0; i < gameObject.transform.childCount; i++)
                        LoadComponents(gameObject.transform.GetChild(i).gameObject);
                }
            }
        }

        private class RendererInfo
        {
            public Renderer renderer;
            public GUIContent[] materialLabels = null;

            public void Update(Renderer renderer)
            {
                if (this.renderer == renderer)
                    return;

                this.renderer = renderer;

                materialLabels = GetMaterialLabels(renderer);
            }
        }

        #endregion

        #region Fields and Constructors

        private bool m_RequireLocal;
        private string m_SearchPropertyPath;

        private SearchInfo m_SearchInfo;
        private RendererInfo m_RendererInfo;


        /// <summary>
        /// Create a control that limits selection to available local renderers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <paramref name="searchPropertyPath"/> is supplied, then it must be the full path
        /// from the root of the the serailized object and it must be an object reference
        /// property of type component, game object, or transform.  A null value will result
        /// in the serialized object being directly searched.
        /// </para>
        /// </remarks>
        /// <param name="searchPropertyPath">
        /// The property path of the serialized object's reference field that will be 
        /// searched for renderer's, or null if the serialized object should be searched.
        /// </param>
        public RendererMaterialPtrControl(string searchPropertyPath)
        {
            m_SearchPropertyPath = searchPropertyPath;
            m_RequireLocal = true;
        }

        /// <summary>
        /// Create a control that allows the selection of any renderer and provides a list of
        /// available materials when appropriate.
        /// </summary>
        public RendererMaterialPtrControl()
        {
            m_RequireLocal = false;
        }

        #endregion

        #region Main Public Members

        public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineCount = label == GUIContent.none ? 2 : 3;

            return EditorGUIUtility.singleLineHeight * lineCount
                + EditorGUIUtility.standardVerticalSpacing * (lineCount + 1);
        }

        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_RequireLocal)
            {
                var go = EditorGUIUtil.GetReferenceObject(property, m_SearchPropertyPath, false);

                if (m_SearchInfo == null)
                    m_SearchInfo = new SearchInfo();

                 m_SearchInfo.Update(go);
            }
            else
                m_SearchInfo = null;

            bool hasLabel = label != GUIContent.none;  // Must be before 'begin property'.

            label = EditorGUI.BeginProperty(position, label, property);

            Rect rect = EditorGUIUtil.SingleLinePosition(position);

            if (hasLabel)
                EditorGUI.LabelField(rect, label);

            var origLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;

            if (m_SearchInfo != null)
                DrawLocal(rect, property, hasLabel);
            else
                DrawBasic(rect, property, hasLabel);

            EditorGUIUtility.labelWidth = origLabelWidth;

            EditorGUI.EndProperty();
        }

        #endregion

        #region Private Draw Members

        /// <summary>
        /// For when there is a list of known renderers. (ProtoInfo is available.)
        /// </summary>
        private void DrawLocal(Rect position, SerializedProperty property, bool hasLabel)
        {
            var props = new Props(property);

            var rect = hasLabel ? EditorGUIUtil.NextGuiElementPosition(position) : position;

            int iOrig = 0;

            if (props.renderer.objectReferenceValue)
            {
                iOrig = m_SearchInfo.renderers.IndexOf(props.renderer.objectReferenceValue as Renderer);
                iOrig = iOrig == -1 ? 0 : iOrig;
            }

            int iSel = EditorGUI.Popup(rect, RendererLabel, iOrig, m_SearchInfo.rendererLabels);

            if (iSel != iOrig)
                props.renderer.objectReferenceValue = m_SearchInfo.renderers[iSel];

            int iRen = iSel;

            rect = EditorGUIUtil.NextGuiElementPosition(rect);

            // iRen == 0 means 'No renderer selected.'
            DrawMaterials(rect, props.index, iRen == 0 ? null : m_SearchInfo.materialLabels[iRen]);
        }

        /// <summary>
        /// Draw the GUI elements for when there are no known renderers. (SearchInfo is not avaiable.)
        /// </summary>
        private void DrawBasic(Rect position, SerializedProperty property, bool hasLabel)
        {
            var props = new Props(property);

            var rect = EditorGUIUtil.NextGuiElementPosition(position, 0, false);
            EditorGUI.PropertyField(rect, props.renderer);

            GUIContent[] labels;

            if (props.renderer.objectReferenceValue)
            {
                if (m_RendererInfo == null)
                    m_RendererInfo = new RendererInfo();

                var renderer = props.renderer.objectReferenceValue as Renderer;

                if (m_RendererInfo.renderer != renderer)
                    m_RendererInfo.Update(renderer);

                labels = m_RendererInfo.materialLabels;
            }
            else
                labels = null;

            rect = EditorGUIUtil.NextGuiElementPosition(rect);

            DrawMaterials(rect, props.index, labels);
        }

        /// <summary>
        /// Draw the standard materials GUI element.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="indexProperty">The property that represents the index field.</param>
        /// <param name="labels">The material name labels, or null if there is no renderer.</param>
        private void DrawMaterials(Rect position, SerializedProperty indexProperty, GUIContent[] labels)
        {
            if (labels == null)
                EditorGUI.LabelField(position, TargetLabel, NoneMaterial);
            else
            {
                var iOrig = Mathf.Clamp(indexProperty.intValue, 0, labels.Length);

                var iSel = EditorGUI.Popup(position, TargetLabel, iOrig, labels);
                if (iSel != iOrig)
                    indexProperty.intValue = iSel;
            }
        }

        #endregion
    }
}
