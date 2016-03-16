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
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

namespace com.lizitt.editor
{
    /// <summary>
    /// Provides GUI utility functions for the Unity Editor.
    /// </summary>
    public static class LizEditorGUIUtil
    {
        #region GUI Styles

        private static GUIStyle m_RedLabel;

        /// <summary>
        /// A bold red label style.
        /// </summary>
        public static GUIStyle RedLabel
        {
            get
            {
                if (m_RedLabel == null)
                {
                    m_RedLabel = new GUIStyle(EditorStyles.label);
                    m_RedLabel.fontStyle = FontStyle.Bold;
                    m_RedLabel.normal.textColor = Color.red;
                    m_RedLabel.name = "RedBoldLabel";
                }

                return m_RedLabel;
            }
        }

        private static GUIStyle m_RedPopup;

        /// <summary>
        /// A bold red popup style.
        /// </summary>
        public static GUIStyle RedPopup
        {
            get
            {
                if (m_RedPopup == null)
                {
                    m_RedPopup = new GUIStyle(EditorStyles.popup);
                    m_RedPopup.normal.textColor = Color.red;
                    m_RedPopup.focused.textColor = Color.red;
                    m_RedPopup.name = "RedPopup";
                }

                return m_RedPopup;
            }
        }

        private static GUIStyle m_RedButton;

        /// <summary>
        /// A red button style.
        /// </summary>
        public static GUIStyle RedButton
        {
            get
            {
                if (m_RedButton == null)
                {
                    m_RedButton = new GUIStyle(GUI.skin.button);
                    m_RedButton.normal.textColor = Color.red;
                    m_RedButton.focused.textColor = Color.red;
                    m_RedButton.name = "RedButton";
                }

                return m_RedButton;
            }
        }

        private static GUIStyle m_YellowLabel;

        /// <summary>
        /// A yellow label style.
        /// </summary>
        public static GUIStyle YellowLabel
        {
            get
            {
                if (m_YellowLabel == null)
                {
                    m_YellowLabel = new GUIStyle(EditorStyles.label);
                    m_YellowLabel.fontStyle = FontStyle.Bold;
                    m_YellowLabel.normal.textColor = Color.yellow;
                    m_YellowLabel.name = "YellowBoldLabel";
                }

                return m_YellowLabel;
            }
        }

        private static GUIStyle m_YellowButton;

        /// <summary>
        /// A yellow button style.
        /// </summary>
        public static GUIStyle YellowButton
        {
            get
            {
                if (m_YellowButton == null)
                {
                    m_YellowButton = new GUIStyle(GUI.skin.button);
                    m_YellowButton.normal.textColor = Color.yellow;
                    m_YellowButton.focused.textColor = Color.yellow;
                    m_YellowButton.name = "YellowButton";
                }

                return m_YellowButton;
            }
        }

        #endregion

        #region Formatting

        private static Stack<float> m_WidthStack = null;

        /// <summary>
        /// Sets the label width, preserving the original label width for restoration using <see cref="EndLabelWidth"/>
        /// </summary>
        /// <param name="width">The desired label width.</param>
        public static void BeginLabelWidth(float width)
        {
            if (m_WidthStack == null)
                m_WidthStack = new Stack<float>();

            m_WidthStack.Push(EditorGUIUtility.labelWidth);
            EditorGUIUtility.labelWidth = width;
        }

        /// <summary>
        /// Restores the label width to the value it was before the last <see cref="BeginLabelWidth"/> call.
        /// </summary>
        public static void EndLabelWidth()
        {
            if (m_WidthStack.Count > 0)
                EditorGUIUtility.labelWidth = m_WidthStack.Pop();
            else
                Debug.LogError("EndLabelWidth() called without BeginLabelWidth()");
        }

        #endregion

        #region SerializedObject

        /// <summary>
        /// Derives a desired reference object using a standard algorithm.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In this context, a 'reference object' is an object that is needed for further
        /// processing.  For example, maybe a GUI element needs to find all colliders on an 
        /// GameObject in order to display the information in a popup.  This method provides 
        /// a standard algorithm for deriving the needed object.
        /// </para>
        /// <para>
        /// This method supports two methods for finding the reference object.  If 
        /// <paramref name="properyPath"/> is null, the search root will be the property's target
        /// object. (I.e. <c>SerializedProperty.serializedObject.targetObject</c>)  If the object 
        /// needs to come from a <c>SerializedPropertyType.ObjectRefernce</c> property, 
        /// then <paramref name="propertyPath"/> needs to to specify the target property.  
        /// E.g.: 'm_Prototype' is a serialized field that references a prefab to be searched.
        /// </para>
        /// <para>
        /// <paramref name="propertyPath"/> must be for a property of type 
        /// <c>SerializedPropertyType.ObjectRefernce</c>.  The refernce type must be of type
        /// <c>GameObject</c> or <c>Component</c>.  (Remember: A <c>Transform</c> is a 
        /// <c>Component</c>.)
        /// </para>
        /// </remarks>
        /// <param name="property">The property that contains the object to be searched.</param>
        /// <param name="propertyPath">
        /// The property path of a <c>SerializedPropertyType.ObjectRefernce</c> property.
        /// </param>
        /// <param name="pathIsRelative">
        /// If true, the <paramref name="propertyPath"/> is relative to <paramref name="property"/>,
        /// otherwise it relative to <c>SerializedProperty.serializedObject.</c>
        /// </param>
        /// <returns>The desired reference object, of null if one could not be found.</returns>
        public static GameObject GetReferenceObject(
            SerializedProperty property, string propertyPath = null, bool pathIsRelative = true)
        {
            Object targ = property.serializedObject.targetObject;

            if (propertyPath != null)
            {
                var prop = pathIsRelative
                    ? property.FindPropertyRelative(propertyPath)
                    : property.serializedObject.FindProperty(propertyPath);

                if (prop == null)
                {
                    Debug.LogError("Invalid propertyPath: " + propertyPath, targ);
                    return null;
                }

                if (prop.propertyType != SerializedPropertyType.ObjectReference)
                {
                    Debug.LogErrorFormat(targ, "Invalid property type at path: {0}, (Path: {1})",
                        prop.propertyType, propertyPath);
                    return null;
                }

                targ = prop.objectReferenceValue;

                if (!targ)
                    return null;  // Not an error.
            }

            GameObject go = null;

            if (targ is GameObject)
                go = (GameObject)targ;
            else if (targ is Component)
                go = ((Component)targ).gameObject;
            else
            {
                Debug.LogError(
                    "Invalid target object type.  Expect GameObject or Component:" + targ, targ);
            }

            return go;
        }

        #endregion

        #region GUI Element Draw Position

        /// <summary>
        /// Derives a rectangle for a single line height rectangle at the specified position.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Converts 'full draw area' position into a single line position.  This is useful when
        /// starting a group of GUI elements with a single line element.
        /// </para>
        /// </remarks>
        /// <param name="position">A position representing the avaiable draw area.</param>
        /// <param name="heightOffset">The extra height offset to add above the single line.</param>
        /// <param name="heightFactor">
        /// The value to multiply the single line height by to make the line larger or smaller
        /// than the standard. (I.e. <c>singleLineHeight * heightFactor</c>)
        /// </param>
        /// <returns></returns>
        public static Rect SingleLinePosition(Rect position, float heightOffset = 0, float heightFactor = 1)
        {
            return new Rect(position.x, position.y + heightOffset,
                position.width, EditorGUIUtility.singleLineHeight * heightFactor);
        }

        /// <summary>
        /// Gets the position where the next GUI element should be drawn based on the last
        /// element's position.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful when stacking GUI elements vertially.  It increments the
        /// element position using standard spacing.
        /// </para>
        /// </remarks>
        /// <param name="position">The position where the last GUI element was drawn.</param>
        /// <param name="extraVerticalSpacing">
        /// Additional vertical spacing to add to the standard vertical spacing.
        /// </param>
        /// <param name="preserveHeight">
        /// If true the height of <paramref name="position"/> will be preserved, otherwise a
        /// standard single line rectangle will be returned.
        /// </param>
        /// <returns>The position where the next GUI element should be drawn.</returns>
        public static Rect NextGuiElementPosition(
            Rect position, float extraVerticalSpacing = 0, bool preserveHeight = true)
        {
            return new Rect(position.xMin, 
                position.yMax + EditorGUIUtility.standardVerticalSpacing + extraVerticalSpacing,
                position.width, preserveHeight ? position.height : EditorGUIUtility.singleLineHeight);
        }

        /// <summary>
        /// Get the label position for use in a GUI element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used in conjunction with <see cref="LabelAfter"/> to create GUI elements
        /// with a same-line custom label.
        /// </para>
        /// </remarks>
        /// <param name="linePosition">The draw area for the entire GUI element.</param>
        /// <param name="labelWidth">The width to reserve for the label.</param>
        /// <returns>A label position for use in a GUI element.</returns>
        public static Rect LabelPosition(Rect linePosition, float labelWidth)
        {
            return new Rect(linePosition.x, linePosition.y, labelWidth, linePosition.height);
        }

        /// <summary>
        /// Get the content postion for use in a GUI element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used in conjunction with <see cref="LabelPosition"/> to create GUI 
        /// elements with a same-line custom label.
        /// </para>
        /// </remarks>
        /// <param name="linePosition">The draw area for the entire GUI element.</param>
        /// <param name="labelWidth">The width to reserve for the label.</param>
        /// <param name="horizontalSpace">
        /// The desired horizontal space between the label position and the content position.
        /// </param>
        /// <returns>The content postion for use in a GUI element.</returns>
        public static Rect LabelAfter(Rect linePosition, float labelWidth, float horizontalSpace = 5)
        {
            float w = labelWidth + horizontalSpace;

            return new Rect(
                linePosition.x + w, linePosition.y, linePosition.width - w, linePosition.height);
        }

        #endregion

        #region Old Style Array Handling

        /// <summary>
        /// Delete the currently selected list element in a single call.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For reference arrays: Somewhere around Unity 5.3 the default behavior for removing a 
        /// reference element from a ReorderableList changed to clear the element on the first 
        /// click, then delete it on the second click.  When attached to 
        /// Reorderable.onRemoveCallback, this method will restore the single-click remove behavior.
        /// </para>
        /// </remarks>
        /// <param name="list">The list to operate on.</param>
        public static void DoSingleClickRemove(ReorderableList list)
        {
            var i = list.index;

            DeleteArrayElementAtIndex(list.serializedProperty, i);

            if (i != 0)
                list.index = i - 1;
        }

        /// <summary>
        /// Deletes the property array element in a single call.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For reference arrays: Somewhere around Unity 5.3 the default behavior for removing a 
        /// reference element from a SerializedProperty array changed to clear the element 
        /// on the first call, then delete it on the second click.  This method deletes the element in a 
        /// single call.
        /// </para>
        /// </remarks>
        /// <param name="arrayProperty">The array property to operate on.</param>
        /// <param name="index">The index of the element to delete.</param>
        public static void DeleteArrayElementAtIndex(SerializedProperty arrayProperty, int index)
        {
            var count = arrayProperty.arraySize;

            arrayProperty.DeleteArrayElementAtIndex(index);

            // The above call may just null the content of the element, so need to really delete 
            // it.
            if (count == arrayProperty.arraySize)
                arrayProperty.DeleteArrayElementAtIndex(index);

            arrayProperty.serializedObject.ApplyModifiedProperties();
        }

        #endregion

        /// <summary>
        /// Sets the rigidbody behavior with undo.
        /// </summary>
        /// <param name="rigidbody">The rigidbody. (Required)</param>
        /// <param name="behavior">The desired behavior.</param>
        /// <param name="singleUndo">
        /// If true, the operation will be combined into a single undo transaction.  Otherwise it is the 
        /// responsibility of the caller to make sure it is properly combined.
        /// </param>
        /// <param name="undoLabel">The undo label for the operation.</param>
        public static void SetRigidbodyBehavior(Rigidbody rigidbody, RigidbodyBehavior behavior,
            bool singleUndo = true, string undoLabel = "Set Rigidbody Behavior")
        {
            if (!rigidbody)
            {
                Debug.Log("Can't set status of a null rigidbody.");
                return;
            }
            
            if (rigidbody.GetBehavior() == behavior)
                return;

            if (singleUndo)
                Undo.IncrementCurrentGroup();

            Undo.RecordObject(rigidbody, undoLabel);
            Undo.RecordObjects(rigidbody.GetColliders(true), undoLabel);

            rigidbody.SetBehavior(behavior, true);

            if (singleUndo)
                Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}
