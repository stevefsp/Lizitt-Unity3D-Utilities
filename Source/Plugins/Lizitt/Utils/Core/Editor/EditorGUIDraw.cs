﻿/*
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
using UnityEditor;
using UnityEngine;

namespace com.lizitt.editor
{
    /// <summary>
    /// Provides methods for displaying custom controls.  
    /// </summary>
    /// <remarks>
    /// <para>
    /// The purpose of this class is similar to <c>EditorGUI</c> and <c>EditorGUILayout</c>.  It agregates
    /// custom controls in a single location for easier discovery.
    /// </para>
    /// </remarks>
    public static class EditorGUIDraw
    {
        /*
         * Design notes:
         * 
         * Draw methods are growing in number and cluttering up EdtiorGUIUtil.  Also, some of these methods
         * were traditially implemented as static methods in their primary editor class, which make them hard
         * to find.
         * 
         * In general, if a GUI control's draw method is used by more than one class, it should go here.
         * 
         */

        #region Collider Status Members

        #region Static Data

        // These arrays are private for now, but It is OK to make them either internal or public if it becomes
        // woth it.

        /// <summary>
        /// The labels for all valid rigidbody collider options.  (Including disabled. Shares index values with 
        /// <see cref="RigidBodyColliderStatusValues"/>)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Warning: For performance reasons this is a direct reference to a global array.  Don't change its
        /// elements.
        /// </para>
        /// </remarks>
        private static readonly GUIContent[] RigidBodyColliderStatusLabels = new GUIContent[]
        {
            new GUIContent(ColliderStatus.Disabled.ToString()),
            new GUIContent(ColliderStatus.KinematicCollider.ToString()),
            new GUIContent(ColliderStatus.KinematicTrigger.ToString()),
            new GUIContent(ColliderStatus.RigidbodyCollider.ToString()),
            new GUIContent(ColliderStatus.RigidbodyTrigger.ToString()),
            new GUIContent(ColliderStatus.GravityBody.ToString()),
        };

        /// <summary>
        /// The values for all valid rigidbody collider options.  (Including disabled. Shares index values with 
        /// <see cref="RigidBodyColliderStatusLabels"/>)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Warning: For performance reasons this is a direct reference to a global array.  Don't change its
        /// elements.
        /// </para>
        /// </remarks>
        private static readonly int[] RigidBodyColliderStatusValues = new int[]
        {
            (int)ColliderStatus.Disabled,
            (int)ColliderStatus.KinematicCollider,
            (int)ColliderStatus.KinematicTrigger,
            (int)ColliderStatus.RigidbodyCollider,
            (int)ColliderStatus.RigidbodyTrigger,
            (int)ColliderStatus.GravityBody,
        };

        /// <summary>
        /// The labels for all valid static collider options.  (Including disabled. Shares index values with 
        /// <see cref="StaticColliderStatusValues"/>)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Warning: For performance reasons this is a direct reference to a global array.  Don't change its
        /// elements.
        /// </para>
        /// </remarks>
        public static readonly GUIContent[] StaticColliderStatusLabels = new GUIContent[]
        {
            new GUIContent(ColliderStatus.Disabled.ToString()),
            new GUIContent(ColliderStatus.StaticCollider.ToString()),
            new GUIContent(ColliderStatus.StaticTrigger.ToString()),
        };

        /// <summary>
        /// The values for all valid static collider options.  (Including disabled. Shares index values with 
        /// <see cref="StaticColliderStatusLabels"/>)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Warning: For performance reasons this is a direct reference to a global array.  Don't change its
        /// elements.
        /// </para>
        /// </remarks>
        public static readonly int[] StaticColliderStatusValues = new int[]
        {
            (int)ColliderStatus.Disabled,
            (int)ColliderStatus.StaticCollider,
            (int)ColliderStatus.StaticTrigger,
        };

        #endregion

        /// <summary>
        /// Draw a popup that only contains collider status options that are relavent to a reference collider.
        /// </summary>
        /// <param name="position">The draw position.</param>
        /// <param name="property">A property representing a <see cref="ColliderStatus"/> field.</param>
        /// <param name="label">The label.</param>
        /// <param name="referencePath">
        /// The serialized object path to the object that is, or contains, the reference collider. Or null to use
        /// <c>SerializedObject.targetObject</c>.</param>
        /// <param name="pathIsRelative">
        /// If true, <see cref="ReferencePath"/> is relative to the current property, otherwise is relative to
        /// <c>SerializedProperty.serializedObject</c>.</param>
        public static void FilteredColliderStatusPopup(Rect position, SerializedProperty property, GUIContent label,
            string referencePath = null, bool pathIsRelative = true)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            Collider collider = null;
            if (string.IsNullOrEmpty(referencePath))
                collider = LizittUtil.DeriveCollider(property.serializedObject.targetObject);
            else
            {
                SerializedProperty refProp = pathIsRelative
                    ? property.FindPropertyRelative(referencePath)
                    : property.serializedObject.FindProperty(referencePath);

                if (refProp == null)
                {
                    Debug.LogError("Invalid property reference path: " + referencePath,
                        property.serializedObject.targetObject);
                }
                else if (refProp.propertyType == SerializedPropertyType.ObjectReference)
                    collider = LizittUtil.DeriveCollider(refProp.objectReferenceValue);
                else
                {
                    Debug.LogErrorFormat(property.serializedObject.targetObject,
                        "Invalid property type.  Expected '{0}', but was '{1}'.  Path: '{2}'",
                        SerializedPropertyType.ObjectReference, property.propertyType, referencePath);
                }
            }

            EditorGUIDraw.FilteredColliderStatusPopup(position, label, property, collider);

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Draw a popup that only contains status values relavant to the collider.
        /// </summary>
        /// <param name="position">The draw position.</param>
        /// <param name="label">The label.</param>
        /// <param name="property">The property representing a <see cref="ColliderStatus"/> field.</param>
        /// <param name="collider">The collider, or null to display all options.</param>
        public static void FilteredColliderStatusPopup(
            Rect position, GUIContent label, SerializedProperty property, Collider collider)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                Debug.LogErrorFormat(property.serializedObject.targetObject,
                    "The property is not an enueration type: '{0}'. Falling back to default editor control.",
                    property.propertyPath);
                EditorGUI.PropertyField(position, property);
            }

            var choice = 
                (int)FilteredColliderStatusPopup(position, label, (ColliderStatus)property.intValue, collider);

            if (choice != property.intValue)
                property.intValue = choice;
        }

        /// <summary>
        /// Draw a popup that only contains status values relavant to category.
        /// <param name="position">The draw position.</param>
        /// <param name="label">The label.</param>
        /// <param name="property">The property representing a <see cref="ColliderStatus"/> field.</param>
        /// <param name="category">The category of status values to include.</param>
        public static void FilteredColliderStatusPopup(
            Rect position, GUIContent label, SerializedProperty property, ColliderStatusCategory category)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                Debug.LogErrorFormat(property.serializedObject.targetObject,
                    "The property is not an enueration type: '{0}'. Falling back to default editor control.",
                    property.propertyPath);
                EditorGUI.PropertyField(position, property);
            }

            var choice =
                (int)FilteredColliderStatusPopup(position, label, (ColliderStatus)property.intValue, category);

            if (choice != property.intValue)
                property.intValue = choice;
        }

        /// <summary>
        /// Draw a popup that only contains status values relavant to the collider.
        /// </summary>
        /// <param name="position">The draw position.</param>
        /// <param name="label">The label.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="collider">The collider, or null to display all options.</param>
        /// <returns>The selected value.</returns>
        public static ColliderStatus FilteredColliderStatusPopup(
            Rect position, GUIContent label, ColliderStatus currentValue, Collider collider)
        {
            if (!collider)
                return (ColliderStatus)EditorGUI.EnumPopup(position, label, currentValue);

            var category = collider.GetAssociatedRigidBody()
                ? ColliderStatusCategory.RigidBody
                : ColliderStatusCategory.Static;

            return FilteredColliderStatusPopup(position, label, currentValue, category);
        }

        /// <summary>
        /// Draw a popup that only contains status values relavant to category
        /// </summary>
        /// <param name="position">The draw position.</param>
        /// <param name="label">The label.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="category">The category of status values to include.</param>
        /// <returns>The selected value.</returns>
        public static ColliderStatus FilteredColliderStatusPopup(
            Rect position, GUIContent label, ColliderStatus currentValue, ColliderStatusCategory category)
        {
            GUIContent[] displayOptions = null;
            int[] optionValues = null;

            switch (category)
            {
                case ColliderStatusCategory.RigidBody:

                    displayOptions = RigidBodyColliderStatusLabels;
                    optionValues = RigidBodyColliderStatusValues;
                    break;

                case ColliderStatusCategory.Static:

                    displayOptions = StaticColliderStatusLabels;
                    optionValues = StaticColliderStatusValues;

                    break;

                default:

                    Debug.LogWarning(
                        "Internal error: Unhandled collider category. Defaulted to EnumPopup: " + category);

                    return (ColliderStatus)EditorGUI.EnumPopup(position, label, currentValue);
            }

            return (ColliderStatus)EditorGUI.IntPopup(position, label, (int)currentValue, displayOptions, optionValues);
        }

        #endregion

        #region General Enumeration Members

        /// <summary>
        /// Draws a mask field for the specified enum type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only call this method when it is valid to call EditorGUI methods.
        /// </para>
        /// </remarks>
        /// <param name="position">The draw position.</param>
        /// <param name="value">The current value of the mask field.</param>
        /// <param name="enumTyp">The type of enum to display.</param>
        /// <param name="label">The field label.</param>
        /// <param name="sort">
        /// True if the enum names should be sorted, otherwise use the default order provided
        /// by the system.
        /// </param>
        /// <returns>The result of the mask field.</returns>
        public static int EnumFlagsField(Rect position, int value, System.Type enumTyp, GUIContent label, bool sort)
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

        /// <summary>
        /// Draws a popup wit the enumeration names sorted.
        /// </summary>
        /// <param name="position">The draw position.</param>
        /// <param name="selectedValue">The current value of the mask field.</param>
        /// <param name="label">The field label.</param>
        /// <param name="enumTyp">The type of enum to display.</param>
        /// <returns>The selected enumeration value.</returns>
        public static int SortedEnumPopup(Rect position, GUIContent label, int selectedValue, System.Type enumTyp)
        {
            EditorGUI.LabelField(EditorGUIUtil.LabelPosition(position, EditorGUIUtility.labelWidth), label);

            return SortedEnumPopup(EditorGUIUtil.LabelAfter(
                position, EditorGUIUtility.labelWidth, 0), selectedValue, enumTyp);
        }

        /// <summary>
        /// Draws a popup wit the enumeration names sorted.  (Without a label.)
        /// </summary>
        /// <param name="position">The draw position.</param>
        /// <param name="selectedValue">The current value of the mask field.</param>
        /// <param name="enumTyp">The type of enum to display.</param>
        /// <returns>The selected enumeration value.</returns>
        public static int SortedEnumPopup(Rect position, int selectedValue, System.Type enumTyp)
        {
            var itemNames = System.Enum.GetNames(enumTyp);
            var itemValues = System.Enum.GetValues(enumTyp) as int[];
            System.Array.Sort(itemNames, itemValues);
            System.Array.Sort(itemNames);

            return EditorGUI.IntPopup(position, selectedValue, itemNames, itemValues);
        }

        #endregion
    }
}
