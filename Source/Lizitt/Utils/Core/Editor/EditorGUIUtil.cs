using UnityEditor;
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
using UnityEngine;

namespace com.lizitt.u3d.editor
{
    /// <summary>
    /// Provides general purpose GUI utility functions for the Unity Editor.
    /// </summary>
    public static class EditorGUIUtil
    {

        private static GUIStyle m_RedLabel;
        private static int IndentAmount = 2;

        public static void BeginSection(string headingLabel)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(headingLabel, EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            EditorGUI.indentLevel += IndentAmount;
        }

        public static void EndSection()
        {
            EditorGUI.indentLevel -= IndentAmount;
        }

        /// <summary>
        /// True if the delete button is pressed.
        /// </summary>
        /// <param name="property"></param>
        /// <returns>True if the delete button was pressed.</returns>
        public static bool DeletablePropertyField(SerializedProperty property, GUIContent label)
        {
            if (property == null)
                throw new System.ArgumentNullException();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(property, label);
            bool result = GUILayout.Button("X", GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only supportx array properties containing a single type.  [int, int] not [int, float] 
        /// </para>
        /// </remarks>
        /// <param name="property"></param>
        /// <param name="index"></param>
        public static void DeleteArrayElementAndResize(SerializedProperty property, int index)
        {
            if (!property.isArray)
                throw new System.ArgumentException("Property is not an array.");

            var size = property.arraySize;
            var typ = property.GetArrayElementAtIndex(index).propertyType;

            // Do this eary.  Don't want to delete current element if can't finish.
            if (typ == SerializedPropertyType.Character || typ == SerializedPropertyType.Generic
                || typ == SerializedPropertyType.Gradient)
            {
                throw new System.ArgumentException("Unsupported array type: " + typ);
            }

            property.DeleteArrayElementAtIndex(index);

            if (property.arraySize != size)
                // I've seen it happen with integers.
                return;

            for (int i = index; i < size - 1; i++)
            {
                var to = property.GetArrayElementAtIndex(i);
                var from = property.GetArrayElementAtIndex(i + 1);

                switch (typ)
                {
                    case SerializedPropertyType.AnimationCurve:

                        to.animationCurveValue = from.animationCurveValue;
                        break;

                    case SerializedPropertyType.Boolean:

                        to.boolValue = from.boolValue;
                        break;

                    case SerializedPropertyType.Bounds:

                        to.boundsValue = from.boundsValue;
                        break;

                    case SerializedPropertyType.Color:

                        to.colorValue = from.colorValue;
                        break;

                    case SerializedPropertyType.Enum:

                        to.enumValueIndex = from.enumValueIndex;
                        break;

                    case SerializedPropertyType.Float:

                        to.floatValue = from.floatValue;
                        break;

                    case SerializedPropertyType.Integer:

                        to.intValue = from.intValue;
                        break;

                    case SerializedPropertyType.LayerMask:

                        // This is a guess.
                        to.intValue = from.intValue;
                        break;

                    case SerializedPropertyType.ObjectReference:

                        to.objectReferenceValue = from.objectReferenceValue;
                        break;

                    case SerializedPropertyType.Rect:

                        to.rectValue = from.rectValue;
                        break;

                    case SerializedPropertyType.String:

                        to.stringValue = from.stringValue;
                        break;

                    case SerializedPropertyType.Vector2:

                        to.vector2Value = from.vector2Value;
                        break;

                    case SerializedPropertyType.Vector3:

                        to.vector3Value = from.vector3Value;
                        break;

                    default:

                        // Just in case an unknown unsupported by gets in the mix.

                        throw new System.ArgumentException(
                            "Property type not supported: " + property.propertyType);
                }
            }

            property.arraySize--;
        }

        public static GUIStyle RedLabel
        {
            get
            {
                if (m_RedLabel == null)
                {
                    m_RedLabel = new GUIStyle(GUI.skin.label);
                    m_RedLabel.fontStyle = FontStyle.Bold;
                    m_RedLabel.normal.textColor = Color.red;
                }

                return m_RedLabel;
            }
        }

        public static int DrawEnumFlagsField(
            Rect position, int value, System.Type enumTyp, GUIContent label, bool sort)
        {
            var itemNames = System.Enum.GetNames(enumTyp);
            var itemValues = System.Enum.GetValues(enumTyp) as int[];

            if (sort)
            {
                System.Array.Sort(itemNames, itemValues);
                System.Array.Sort(itemNames);
            }

            int val = value;
            int maskVal = 0;
            for (int i = 0; i < itemValues.Length; i++)
            {
                if (itemValues[i] != 0)
                {
                    if ((val & itemValues[i]) == itemValues[i])
                        maskVal |= 1 << i;
                }
                else if (val == 0)
                    maskVal |= 1 << i;
            }

            int newMaskVal = EditorGUI.MaskField(position, label, maskVal, itemNames);
            int changes = maskVal ^ newMaskVal;

            for (int i = 0; i < itemValues.Length; i++)
            {
                if ((changes & (1 << i)) != 0)            // Has this list item changed?
                {
                    if ((newMaskVal & (1 << i)) != 0)     // Has it been set?
                    {
                        if (itemValues[i] == 0)           // Special case: if "0" is set, just set the val to 0
                        {
                            val = 0;
                            break;
                        }
                        else
                            val |= itemValues[i];
                    }
                    else                                  // It has been reset
                        val &= ~itemValues[i];
                }
            }
            return val;
        }
    }
}
