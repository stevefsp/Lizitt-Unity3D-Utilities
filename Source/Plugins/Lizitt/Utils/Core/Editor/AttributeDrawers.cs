/*
 * Copyright (c) 2013-2015 Stephen A. Pratt
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
    [CustomPropertyDrawer(typeof(ClampAttribute))]
    [CanEditMultipleObjects]
    public sealed class ClampDrawer
        : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.isArray)
                // Silently fail for now.  (This can happen in Behavior Designer.)
                return;

            var attr = attribute as ClampAttribute;

            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                var val = EditorGUI.IntField(position, label, property.intValue);

                if (EditorGUI.EndChangeCheck())
                    property.intValue = attr.Clamp(val);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                var val = EditorGUI.FloatField(position, label, property.floatValue);

                if (EditorGUI.EndChangeCheck())
                    property.floatValue = attr.Clamp(val);
            }
            else if (!property.isArray)
                throw new System.InvalidOperationException("Property is not an integer or float.");

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(ClampMinimumAttribute))]
    [CanEditMultipleObjects]
    public sealed class ClampMinimumDrawer
        : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.isArray)
                // Silently fail for now.  (This can happen in Behavior Designer.)
                return;

            var attr = attribute as ClampMinimumAttribute;

            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                var val = EditorGUI.IntField(position, label, property.intValue);

                if (EditorGUI.EndChangeCheck())
                    property.intValue = Mathf.Max(attr.intMinimum, val);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                var val = EditorGUI.FloatField(position, label, property.floatValue);

                if (EditorGUI.EndChangeCheck())
                    property.floatValue = Mathf.Max(attr.floatMinimum, val);
            }
            else if (!property.isArray)
                throw new System.InvalidOperationException("Property is not an integer or float.");

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer 
        : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as EnumFlagsAttribute;

            label = EditorGUI.BeginProperty(position, label, property);
            label.text = label.text;
            if (attr.displayValue)
                label.text += " (" + property.intValue + ")";

            EditorGUI.BeginChangeCheck();

            int val =
                EditorGUIUtil.DrawEnumFlagsField(position, property.intValue, attr.enumType, label, attr.sort);

            if (EditorGUI.EndChangeCheck())
                property.intValue = val;
        }
    }

    [CustomPropertyDrawer(typeof(UnityLayerAttribute))]
    public class UnityLayerAttributeDrawer 
        : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            label.text = label.text + " (" + property.intValue + ")";

            EditorGUI.BeginChangeCheck();

            // This won't allow flags.  That is OK since LayerMask should be used in that
            // situation.
            int val = EditorGUI.LayerField(position, label, property.intValue);

            if (EditorGUI.EndChangeCheck())
                property.intValue = val;
        }
    }
}
