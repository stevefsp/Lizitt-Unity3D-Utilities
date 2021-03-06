﻿/*
 * Copyright (c) 2015 Stephen A. Pratt
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

namespace com.lizitt
{
    /// <summary>
    /// Clamp the value of the property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Similar to the built in Range attribute, except it only uses a normal edit box, 
    /// rather than a slider, so it is more suitable for large range properties.
    /// </para>
    /// </remarks>
    public class ClampAttribute
        : PropertyAttribute
    {
        private readonly float FloatMinimum;
        private readonly float FloatMaximum;

        private int IntegerMinimum
        {
            get { return (int)FloatMinimum; }
        }

        private int IntegerMaximum
        {
            get { return (int)FloatMaximum; }
        }

        /// <summary>
        /// Float constructor.
        /// </summary>
        /// <param name="minimum">The minimum value. (Inclusive.)</param>
        /// <param name="maximum">The maximum value. (Inclusive.)</param>
        public ClampAttribute(float minimum, float maximum)
        {
            this.FloatMinimum = minimum;
            this.FloatMaximum = maximum;
        }

        /// <summary>
        /// Integer constructor.
        /// </summary>
        /// <param name="minimum">The minimum value. (Inclusive.)</param>
        /// <param name="maximum">The maximum value. (Inclusive.)</param>
        public ClampAttribute(int minimum, int maximum)
        {
            this.FloatMinimum = (float)minimum;
            this.FloatMaximum = (float)maximum;
        }

        /// <summary>
        /// Clamps the float value to the minimum and maximum. (Inclusive.)
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public float Clamp(float value)
        {
            return Mathf.Clamp(value, FloatMinimum, FloatMaximum);
        }

        /// <summary>
        /// Clamps the integer value to the minimum and maximum. (Inclusive.)
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public int Clamp(int value)
        {
            return Mathf.Clamp(value, IntegerMinimum, IntegerMaximum);
        }
    }

    /// <summary>
    /// Clamp the minimum value of the property.
    /// </summary>
    public class ClampMinimumAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The minimum allowed float value. (Inclusive.)
        /// </summary>
        public readonly float FloatMinimum;

        /// <summary>
        /// The minimum allowed integer value. (Inclusive.)
        /// </summary>
        public int IntegerMinimum
        {
            get { return (int)FloatMinimum; }
        }

        /// <summary>
        /// Float constructor.
        /// </summary>
        /// <param name="minimum">The minimum allowed value. (Inclusive.)</param>
        public ClampMinimumAttribute(float minimum)
        {
            this.FloatMinimum = minimum;
        }

        /// <summary>
        /// Integer constructor.
        /// </summary>
        /// <param name="minimum">The minimum allowed value. (Inclusive.)</param>
        public ClampMinimumAttribute(int minimum)
        {
            this.FloatMinimum = (float)minimum;
        }
    }

    /// <summary>
    /// Display a custom flags editor for an enumerated property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Unlike the default Unity flags editor, selecting "Everything" will only set
    /// all existing flags.  The default Unity behavior is to set all current and 
    /// future flags (-1).
    /// </para>
    /// </remarks>
    public class EnumFlagsAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The flag type.
        /// </summary>
        public readonly System.Type EnumType;

        /// <summary>
        /// True if the flag names should be sorted.  Otherwise use default sorting.
        /// </summary>
        public readonly bool Sort;

        /// <summary>
        /// True if the flag value should be displayed after the property name.
        /// </summary>
        public readonly bool DisplayValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumType">The flag type.</param>
        /// <param name="sort">
        /// True if the flag names should be sorted.  Otherwise use default sorting.
        /// </param>
        /// <param name="displaValue">
        /// True if the flag value should be displayed after the property name.
        /// </param>
        public EnumFlagsAttribute(System.Type enumType, bool sort = false, bool displaValue = false)
        {
            this.EnumType = enumType;
            this.Sort = sort;
            this.DisplayValue = displaValue;
        }
    }    
    
    /// <summary>
    /// Displays an field popup with the enumeration names sorted.
    /// </summary>
    public class SortedEnumPopupAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The enum type.
        /// </summary>
        public readonly System.Type EnumType;

        /// <summary>
        /// If true, include the label, otherwise only diplay the enum selector.
        /// </summary>
        public readonly bool IncludeLabel;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        public SortedEnumPopupAttribute(System.Type enumType, bool includeLabel = true)
        {
            this.EnumType = enumType;
            this.IncludeLabel = includeLabel;
        }
    }

    /// <summary>
    /// Display a dropdown selection to select a single Unity layer.  [Limit: Integer only.]
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is useful when a layer field needs to be a single layer integer value rather than a 
    /// UnityEngine.LayerMask.
    /// </para>
    /// </remarks>
    public class UnityLayerAttribute
        : PropertyAttribute
    {
    }

    /// <summary>
    /// Displays a GUI element that will display an visual error if the assigned value is null, zero, or an empty 
    /// string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Supports float, int, object reference, and strings.  Any other values will display their default
    /// GUI element.  For strings, a string with the contains only white space is considered an empty string.
    /// </para>
    /// </remarks>
    public class RequiredValueAttribute
        : PropertyAttribute
    {
        public readonly System.Type ReferenceType;
        public readonly bool AllowSceneObjects;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="referenceType">The object type for object reference fields, or null if not applicable.</param>
        /// <param name="allowSceneObjects">
        /// If true, allow selection of scene objects.  (Only applicable to object reference fields.
        /// </param>
        public RequiredValueAttribute(System.Type referenceType = null, bool allowSceneObjects = false)
        {
            ReferenceType = referenceType;
            AllowSceneObjects = allowSceneObjects;
        }
    }

    /// <summary>
    /// Display a dropdown containing a list of components attached to the search oject or or any of its children.  
    /// (Used to limit assignment to only local components.)
    /// </summary>
    public class LocalComponentPopupAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The property's component type.  (Used for the local component search.)
        /// </summary>
        public readonly System.Type ComponentType;

        /// <summary>
        /// If true, only a non-null property value is valid.
        /// </summary>
        public readonly bool Required;

        /// <summary>
        /// The property path of the object to search for local components.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See <see cref="EditorGUIUtil.GetReferenceObject"/> for details on interpretation of this field.
        /// </para>
        /// </remarks>
        public readonly string SearchPropertyPath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentType">
        /// The property's component type.  (Used for the local component search. E.g. Animator)
        /// </param>
        /// <param name="required">
        /// If true, only a non-null property value is valid.  Otherwise null is permitted.
        /// </param>
        /// <param name="searchPropertyPath">
        /// The property path of the object to search for local components.  
        /// (<see cref="EditorGUIUtil.GetReferenceObject"/> for details.)
        /// </param>
        public LocalComponentPopupAttribute(
            System.Type componentType, bool required = false, string searchPropertyPath = null)
        {
            ComponentType = componentType;
            Required = required;
            SearchPropertyPath = searchPropertyPath;
        }
    }

    /// <summary>
    /// Require only objects of the specified type to be assigned.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Only useful for object reference fields.
    /// </para>
    /// <para>
    /// This attribute is useful when an assigned object needs to implement a non-UnityEngine.Object API, such as
    /// an interface.  (E.g. A UnityEngine.Object must implement the IAgent interface.)
    /// </para>
    /// </remarks>
    public class RequireObjectTypeAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The required object type.
        /// </summary>
        public readonly System.Type RequiredType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="requiredType">The required object type.</param>
        public RequireObjectTypeAttribute(System.Type requiredType)
        {
            RequiredType = requiredType;
        }
    }

    /// <summary>
    /// Draw a popup that only contains the collider status types for the specified category.
    /// </summary>
    public class FilteredColliderStatusAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The category of status types to include.
        /// </summary>
        public readonly ColliderBehaviorCategory FilterType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filerType">The category of status types to include.</param>
        public FilteredColliderStatusAttribute(ColliderBehaviorCategory filerType)
        {
            FilterType = filerType;
        }
    }

    /// <summary>
    /// Draw a popup that only contains collider status options that are relavent to a reference collider.
    /// </summary>
    public class DynamicColliderStatusAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The serialized object path to the object that is, or contains, the reference collider. Or null to use
        /// <c>SerializedObject.targetObject</c>.
        /// </summary>
        public readonly string ReferencePath;

        /// <summary>
        /// If true, <see cref="ReferencePath"/> is relative to the current property, otherwise is relative to
        /// <c>SerializedProperty.serializedObject</c>.
        /// </summary>
        public readonly bool PathIsRelative;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="referencePath">
        /// The serialized object path to the object that is, or contains, the reference collider. Or null to use
        /// <c>SerializedObject.targetObject</c>.
        /// </param>
        /// <param name="referenceIsAbsolute">
        /// If true, <paramref name="referencePath"/> is relative to the current property, otherwise is relative to
        /// <c>SerializedProperty.serializedObject</c>.
        /// </param>
        public DynamicColliderStatusAttribute(string referencePath = null, bool referenceIsAbsolute = false)
        {
            ReferencePath = referencePath;
            PathIsRelative = referenceIsAbsolute;
        }
    }
}