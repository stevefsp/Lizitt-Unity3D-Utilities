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
using UnityEngine;
using com.lizitt.editor;

namespace com.lizitt.editor
{
    /// <summary>
    /// A GUI element representing a reorderable list of single-line object references.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The object refrence elements are drawn on a single line without labels. Custom
    /// multi-line list elements are not supported.
    /// </para>
    /// <para>
    /// This drawer supports two scenarios: The default behavior is that the list property is 
    /// the same one provided to <see cref="OnGUI"/>.  But it can also be an arbitrary sub-property.
    /// Concrete classes can override <see cref="GetListProperty"/> to override the defualt
    /// behavior.
    /// </para>
    /// <para>
    /// This object list is expecially useful for lists the need to be restricted but can't
    /// be restricted using Unity's standard object picker.  For example, you may want to 
    /// only accepted object's that implement a specific interface. Override <see cref="Validate"/>
    /// to implement custom validations.
    /// </para>
    /// </remarks>
    /// <seealso cref="ReferenceListComponent"/>
    public abstract class ObjectListDrawer
        : PropertyDrawer
    {
        ReorderableListComponent m_GuiElement;

        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        /// <returns>See Unity documentation.</returns>
        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var listProp = GetListProperty(property);

            CheckInitialized(listProp);

            return m_GuiElement.GetPropertyHeight(listProp);
        }

        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var listProp = GetListProperty(property);

            CheckInitialized(listProp);

            m_GuiElement.OnGUI(position, listProp, label);
        }

        private void CheckInitialized(SerializedProperty listProperty)
        {
            if (m_GuiElement == null)
            {
                m_GuiElement = new ReorderableListComponent(listProperty, HeaderTitle, true,
                    EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 3, 
                    DrawElement, HandleAddElement);
            }
        }

        private void HandleAddElement(SerializedProperty elementProperty)
        {
            elementProperty.objectReferenceValue = null;
        }

        private void DrawElement(Rect position, SerializedProperty elementProperty,
            bool isActive, bool isFocused)
        {
            position = EditorGUIUtil.SingleLinePosition(
                position, EditorGUIUtility.standardVerticalSpacing);

            EditorGUI.PropertyField(position, elementProperty, GUIContent.none);

            var comp = elementProperty.objectReferenceValue;
            if (comp && !ValidateComponent(comp))
                elementProperty.objectReferenceValue = null;
        }

        /// <summary>
        /// The title to be displayed in the list's header.
        /// </summary>
        protected abstract string HeaderTitle { get; }

        /// <summary>
        /// The list property being drawn.
        /// </summary>
        /// <param name="property">
        /// The property from <see cref="OnGUI"/>  and <see cref="GetPropertyHeight"/>.
        /// </param>
        /// <returns>The list property being drawn.</returns>
        protected virtual SerializedProperty GetListProperty(SerializedProperty property)
        {
            return property;
        }

        /// <summary>
        /// Test the reference object for validity, reporting any errors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called during <see cref="OnGUI"/> to validate assignments to each
        /// list element. If an assignment is invalid, the element will be set to null.
        /// </para>
        /// </remarks>
        /// <param name="obj">The object being validated.</param>
        /// <returns>True if the object is valid, otherwise false.</returns>
        protected virtual bool ValidateComponent(Object obj)
        {
            return true;
        }
    }
}

