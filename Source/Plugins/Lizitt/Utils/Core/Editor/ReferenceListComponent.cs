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
    /// A GUI element representing a reorderable list of single-line object references
    /// for an arbitrary list property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The object refrence elements are drawn on a single line without labels. Custom
    /// multi-line list elements are not supported.
    /// </para>
    /// <para>
    /// If you are implementing a simple attribute drawer for an array field, then 
    /// <see cref="ObjectListDrawer"/> may be more appropriate to use.  This class can be used
    /// in more situations, but is more complex to use.
    /// </para>
    /// </remarks>
    /// <seealso cref="ObjectListDrawer"/>
    public class ReferenceListControl
    {
        // TODO: Synchroized the behavior of this class and the ObjectListDrawer.

        private static readonly float HeaderHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static readonly float FooterHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static readonly float ElementHeight =
            EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2 * 1.4f)
            + 3;  // Improves the layout, especially for the final element just above the footer.

        private ReorderableList m_List;

        public ReferenceListControl(
            SerializedProperty listProperty, string headerTitle, bool singleClickRemove)
        {
            m_List = CreateList(listProperty, headerTitle, singleClickRemove);
        }

        public float GetPropertyHeight(SerializedProperty listProperty)
        {
            return HeaderHeight + FooterHeight + EditorGUIUtility.singleLineHeight
                + (ElementHeight * Mathf.Max(1, listProperty.arraySize));
        }

        public void OnGUI(Rect position, SerializedProperty listProperty, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, listProperty);

            m_List.serializedProperty = listProperty;
            m_List.DoList(position);

            EditorGUI.EndProperty();
        }

        private ReorderableList CreateList(SerializedProperty listProperty, string headerTitle, bool singleClickRemove)
        {
            var list = new ReorderableList(listProperty.serializedObject
                , listProperty, true, true, true, true);

            list.headerHeight = HeaderHeight;
            list.footerHeight = FooterHeight;

            list.drawHeaderCallback = 
                rect => { EditorGUI.LabelField(rect, headerTitle); };

            list.elementHeight = ElementHeight;

            list.drawElementCallback = (position, index, isActive, isFocused) =>
                {
                    position = EditorGUIUtil.SingleLinePosition(
                        position,  EditorGUIUtility.standardVerticalSpacing * 1.5f);

                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(position, element, GUIContent.none);
                };

            list.onAddCallback = 
                roList => { roList.index = AddElement(roList.serializedProperty); };

            if (singleClickRemove)
                list.onRemoveCallback = EditorGUIUtil.DoSingleClickRemove;

            return list;
        }

        private int AddElement(SerializedProperty listProperty)
        {
            int nidx = listProperty.arraySize;

            listProperty.arraySize++;

            var element = listProperty.GetArrayElementAtIndex(nidx);
            // Override default behavior.  Rarely want to duplicate accessories.
            element.objectReferenceValue = null;

            listProperty.serializedObject.ApplyModifiedProperties();

            return nidx;
        }
    }
}
