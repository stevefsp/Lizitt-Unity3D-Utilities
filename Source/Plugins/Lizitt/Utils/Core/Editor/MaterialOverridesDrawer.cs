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

namespace com.lizitt.editor
{
    /// <summary>
    /// The GUI element for fields marked with <see cref="MaterialOverridesAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(MaterialOverridesAttribute))]
    public class MaterialOverridesDrawer
        : PropertyDrawer
    {
        private const string ItemPropName = "m_Items";

        private MaterialOverrideListControl m_List;

        /// <summary>
        /// See Unity documentation.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckInitialized(property);
            return m_List.GetPropertyHeight(property.FindPropertyRelative(ItemPropName));
        }

        /// <summary>
        /// See Unity documentation.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckInitialized(property);
            m_List.OnGUI(position, property.FindPropertyRelative(ItemPropName), label);
        }

        private void CheckInitialized(SerializedProperty property)
        {
            if (m_List == null)
            {
                var attr = attribute as MaterialOverridesAttribute;

                property = property.FindPropertyRelative(ItemPropName);
                if (attr.LocalOnly)
                    m_List = new MaterialOverrideListControl(property, attr.SearchPropertyPath);
                else
                    m_List = new MaterialOverrideListControl(property);
            }
        }
    }
}
