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
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace com.lizitt.editor
{
    /// <summary>
    /// A GUI element representing a reorderable list of complex objects
    /// for an arbitrary array.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This GUI element manages high level drawing of the list, but allows custom 
    /// element drawing and element add behavior.
    /// </para>
    /// </remarks>
    /// <seealso cref="ReferenceListControl"/>
    /// <see cref="ObjectListDrawer"/>
    public class ReorderableListControl
    {
        private static readonly float HeaderHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static readonly float FooterHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        /// <summary>
        /// An upate action for a list element.
        /// </summary>
        /// <param name="elementProperty">The element to update.</param>
        public delegate void UpdateListElement(SerializedProperty elementProperty);

        /// <summary>
        /// A draw action for a list element.
        /// </summary>
        /// <param name="position">The draw area.</param>
        /// <param name="elementProperty">The property to draw.</param>
        /// <param name="isActive">True if the property is active.</param>
        /// <param name="isFocused">True if the property is focused.</param>
        public delegate void DrawElement(Rect position, SerializedProperty elementProperty,
            bool isActive, bool isFocused);

        private readonly ReorderableList m_List;
        private readonly float m_ElementHeight;
        private readonly UpdateListElement m_OnAddElement;
        private readonly DrawElement m_OnDrawElement;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listProperty">The array property to be drawn.</param>
        /// <param name="headerTitle">The title to display in the list header.</param>
        /// <param name="singleClickDelete">
        /// If true, the list will support single click deleting of elements.  Otherwise the
        /// default Unity behavior will be used.
        /// </param>
        /// <param name="elementHeight">The draw height of each element.</param>
        /// <param name="onDrawElement">The method to use for drawing an element. (Required)</param>
        /// <param name="onAddElement">
        /// The method to use for adding new elements, or null for default Unity behavior.
        /// </param>
        public ReorderableListControl(SerializedProperty listProperty, 
            string headerTitle, bool singleClickDelete, float elementHeight,
            DrawElement onDrawElement, UpdateListElement onAddElement = null)
        {
            if (onDrawElement == null)
                throw new System.ArgumentNullException("onDrawElement");

            m_ElementHeight = elementHeight;
            m_OnAddElement = onAddElement;
            m_OnDrawElement = onDrawElement;

            m_List = CreateList(listProperty, headerTitle, singleClickDelete);
        }

        /// <summary>
        /// The draw height for the list.
        /// </summary>
        /// <param name="listProperty">
        /// The array property. (The same field used in the constructor.)
        /// </param>
        /// <returns>The draw height fo the list.</returns>
        public float GetPropertyHeight(SerializedProperty listProperty)
        {
            return HeaderHeight + FooterHeight + EditorGUIUtility.singleLineHeight
                + (m_ElementHeight * Mathf.Max(1, listProperty.arraySize));
        }

        /// <summary>
        /// Draw the GUI element.
        /// </summary>
        /// <param name="position">The position of the GUI element.</param>
        /// <param name="listProperty">
        /// The array property. (The same field used in the constructor.)
        /// </param>
        /// <param name="label">The property label.</param>
        public void OnGUI(Rect position, SerializedProperty listProperty, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, listProperty);

            m_List.serializedProperty = listProperty;
            m_List.DoList(position);

            EditorGUI.EndProperty();
        }

        private ReorderableList CreateList(
            SerializedProperty listProperty, string headerTitle, bool singleClickDelete)
        {
            var list = new ReorderableList(listProperty.serializedObject
                , listProperty, true, true, true, true);

            list.headerHeight = HeaderHeight;
            list.footerHeight = FooterHeight;

            list.drawHeaderCallback = 
                rect => { EditorGUI.LabelField(rect, headerTitle); };

            list.elementHeight = m_ElementHeight;

            list.drawElementCallback = (position, index, isActive, isFocused) =>
                {
                    m_OnDrawElement(position, list.serializedProperty.GetArrayElementAtIndex(index), 
                        isActive, isFocused);
                };

            if (m_OnAddElement != null)
            {
                list.onAddCallback =
                    roList => { roList.index = AddPrototype(roList.serializedProperty); };
            }

            if (singleClickDelete)
                list.onRemoveCallback = EditorGUIUtil.DoSingleClickRemove;

            return list;
        }

        private int AddPrototype(SerializedProperty listProperty)
        {
            int nidx = listProperty.arraySize;

            listProperty.arraySize++;

            var element = listProperty.GetArrayElementAtIndex(nidx);

            m_OnAddElement(element);

            listProperty.serializedObject.ApplyModifiedProperties();

            return nidx;
        }
    }
}
