/*
 * Copyright (c) 2013-2016 Stephen A. Pratt
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
    /*
     * Keep simple attribute drawers here.  If a drawer gets to complex, then move it to its 
     * own source file.
     */

    /// <summary>
    /// Draws fields marked with the <see cref="ClampAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(ClampAttribute))]
    [CanEditMultipleObjects]
    public sealed class ClampDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
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

    /// <summary>
    /// Draws fields marked with the <see cref="ClampMinimumAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(ClampMinimumAttribute))]
    [CanEditMultipleObjects]
    public sealed class ClampMinimumDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
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
                    property.intValue = Mathf.Max(attr.IntegerMinimum, val);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                var val = EditorGUI.FloatField(position, label, property.floatValue);

                if (EditorGUI.EndChangeCheck())
                    property.floatValue = Mathf.Max(attr.FloatMinimum, val);
            }
            else if (!property.isArray)
                throw new System.InvalidOperationException("Property is not an integer or float.");

            EditorGUI.EndProperty();
        }
    }

    /// <summary>
    /// Draws fields marked with <see cref="EnumFlagsAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer 
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as EnumFlagsAttribute;

            label = EditorGUI.BeginProperty(position, label, property);
            if (attr.DisplayValue)
                label.text += " (" + property.intValue + ")";

            EditorGUI.BeginChangeCheck();

            int val =
                LizittEditorGUI.EnumFlagsField(position, property.intValue, attr.EnumType, label, attr.Sort);

            if (EditorGUI.EndChangeCheck())
                property.intValue = val;

            EditorGUI.EndProperty();
        }
    }

    /// <summary>
    /// Draws fields marked with <see cref="SortedEnumPopupAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(SortedEnumPopupAttribute))]
    public class SortedEnumPopupAttributeDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as SortedEnumPopupAttribute;

            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            int val = attr.IncludeLabel
                ? LizittEditorGUI.SortedEnumPopup(position, label, property.intValue, attr.EnumType)
                : LizittEditorGUI.SortedEnumPopup(position, property.intValue, attr.EnumType);

            if (EditorGUI.EndChangeCheck())
                property.intValue = val;

            EditorGUI.EndProperty();
        }
    }

    /// <summary>
    /// Draws fields marked with the <see cref="UnityLayerAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(UnityLayerAttribute))]
    public class UnityLayerAttributeDrawer 
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            //label.text = label.text + " (" + property.intValue + ")";  // Just doesn't look good.

            EditorGUI.BeginChangeCheck();

            // This won't allow flags.  That is OK since LayerMask should be used in that
            // situation.
            int val = EditorGUI.LayerField(position, label, property.intValue);

            if (EditorGUI.EndChangeCheck())
                property.intValue = val;

            EditorGUI.EndProperty();
        }
    }


    /// <summary>
    /// Draws fields marked with the <see cref="RequiredValueAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(RequiredValueAttribute))]
    public class RequiredValueAttributeDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            label.tooltip = label.tooltip + " (Required)";

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:

                    OnGUIFloat(position, property, label);
                    break;

                case SerializedPropertyType.Integer:

                    OnGUIInteger(position, property, label);
                    break;

                case SerializedPropertyType.ObjectReference:

                    OnGUIObject(position, property, label);
                    break;

                case SerializedPropertyType.String:

                    OnGuiString(position, property, label);
                    break;

                default:

                    EditorGUI.PropertyField(position, property, label, true);
                    break;
            }

            EditorGUI.EndProperty();
        }

        private void OnGuiString(Rect position, SerializedProperty property, GUIContent label)
        {
            // Can't do this.  It can result in the GUI field loosing focus unexpectedly.
            //if (property.stringValue.Trim().Length > 0)
            //{
            //    EditorGUI.PropertyField(position, property, label);
            //    return;
            //}

            var style = property.stringValue.Trim().Length == 0
                ? LizittEditorGUIUtil.RedLabel
                : EditorStyles.label;

            var rect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);

            EditorGUI.LabelField(rect, label, style);

            rect =
                new Rect(rect.xMax + 5, rect.y, position.width - rect.width - 5, position.height);

            EditorGUI.BeginChangeCheck();

            var val = EditorGUI.TextField(rect, GUIContent.none, property.stringValue);

            if (EditorGUI.EndChangeCheck())
                property.stringValue = val;
        }

        private void OnGUIInteger(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            var style = property.intValue == 0 ? LizittEditorGUIUtil.RedLabel : EditorStyles.numberField;
            var val = EditorGUI.IntField(position, label, property.intValue, style);

            if (EditorGUI.EndChangeCheck())
                property.intValue = val;
        }

        private void OnGUIFloat(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            var style = property.floatValue == 0 ? LizittEditorGUIUtil.RedLabel : EditorStyles.numberField;

            var val = EditorGUI.FloatField(position, label, property.floatValue, style);

            if (EditorGUI.EndChangeCheck())
                property.floatValue = val;
        }

        private void OnGUIObject(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as RequiredValueAttribute;

            if (attr.ReferenceType == null || property.objectReferenceValue)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var rect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);

            EditorGUI.LabelField(rect, label, LizittEditorGUIUtil.RedLabel);

            rect =
                new Rect(rect.xMax + 5, rect.y, position.width - rect.width - 5, position.height);

            EditorGUI.BeginChangeCheck();

            var val = EditorGUI.ObjectField(rect, GUIContent.none, 
                property.objectReferenceValue, attr.ReferenceType, attr.AllowSceneObjects);

            if (EditorGUI.EndChangeCheck())
                property.objectReferenceValue = val;
        }
    }

    /// <summary>
    /// Draws fields marked with the <see cref="LocalComponentPopupAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(LocalComponentPopupAttribute))]
    public class LocalComponentPopupDrawer
        : PropertyDrawer
    {
        LocalComponentPopup m_GuiElement;

        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (LocalComponentPopupAttribute)attribute;

            if (m_GuiElement == null)
                m_GuiElement = new LocalComponentPopup(attr.ComponentType, attr.Required);

            m_GuiElement.OnGUI(position, property, label, 
                LizittEditorGUIUtil.GetReferenceObject(property, attr.SearchPropertyPath, false));
        }
    }

    /// <summary>
    /// Draws fields marked with the <see cref="RequiredValueAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(RequireObjectTypeAttribute))]
    public class RequireObjectTypeAttributeDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var orig = property.objectReferenceValue;

                EditorGUI.PropertyField(position, property);

                var value = property.objectReferenceValue;
                var typ = (attribute as RequireObjectTypeAttribute).RequiredType;
                if (value && !typ.IsInstanceOfType(value))
                {
                    Debug.LogErrorFormat(null, 
                        "Invalid observer: {0} does not implement {1}.", value.GetType().Name, typ.Name);

                    property.objectReferenceValue = orig;
                }
            }
            else
            {
                Debug.LogWarningFormat(
                    "Property is not an opject reference: {0} ({1}", property.propertyPath, property.propertyType);
                EditorGUI.PropertyField(position, property);
            }

            EditorGUI.EndProperty();
        }
    }

    /// <summary>
    /// Draws fields marked with the <see cref="FilteredColliderStatusAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(FilteredColliderStatusAttribute))]
    public class FilteredColliderStatusAttributeDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as FilteredColliderStatusAttribute;

            LizittEditorGUI.ColliderBehaviorPopup(position, label, property, attr.FilterType);
        }
    }

    /// <summary>
    /// Draws fields marked with the <see cref="DynamicColliderStatusAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(DynamicColliderStatusAttribute))]
    public class DynamicColliderStatusAttributeDrawer
        : PropertyDrawer
    {
        /// <summary>
        /// See Unity documentation.
        /// </summary>
        /// <param name="position">See Unity documentation.</param>
        /// <param name="property">See Unity documentation.</param>
        /// <param name="label">See Unity documentation.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as DynamicColliderStatusAttribute;
            LizittEditorGUI.ColliderBehaviorPopup(position, property, label, attr.ReferencePath, attr.PathIsRelative);
        }
    }
}
