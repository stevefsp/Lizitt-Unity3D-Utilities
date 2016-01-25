/*
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
    /// rather than a slider.  So it is more suitable for large range properties.
    /// </para>
    /// </remarks>
    public class ClampAttribute
        : PropertyAttribute
    {
        private readonly float floatMinimum;
        private readonly float floatMaximum;

        private readonly int intMinimum;
        private readonly int intMaximum;

        /// <summary>
        /// Float constructor.
        /// </summary>
        /// <param name="minimum">The minimum value. (Inclusive.)</param>
        /// <param name="maximum">The maximum value. (Inclusive.)</param>
        public ClampAttribute(float minimum, float maximum)
        {
            this.floatMinimum = minimum;
            this.intMinimum = (int)minimum;

            this.floatMaximum = maximum;
            this.intMaximum = (int)maximum;
        }

        /// <summary>
        /// Integer constructor.
        /// </summary>
        /// <param name="minimum">The minimum value. (Inclusive.)</param>
        /// <param name="maximum">The maximum value. (Inclusive.)</param>
        public ClampAttribute(int minimum, int maximum)
        {
            this.floatMinimum = (float)minimum;
            this.intMinimum = minimum;

            this.floatMaximum = (float)maximum;
            this.intMaximum = maximum;
        }

        /// <summary>
        /// Clamps the float value to the minimum and maximum. (Inclusive.)
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public float Clamp(float value)
        {
            return Mathf.Clamp(value, floatMinimum, floatMaximum);
        }

        /// <summary>
        /// Clamps the integer value to the minimum and maximum. (Inclusive.)
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public int Clamp(int value)
        {
            return Mathf.Clamp(value, intMinimum, intMaximum);
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
        public readonly float floatMinimum;

        /// <summary>
        /// The minimum allowed integer value. (Inclusive.)
        /// </summary>
        public readonly int intMinimum;

        /// <summary>
        /// Float constructor.
        /// </summary>
        /// <param name="minimum">The minimum allowed value. (Inclusive.)</param>
        public ClampMinimumAttribute(float minimum)
        {
            this.floatMinimum = minimum;
            this.intMinimum = (int)minimum;
        }

        /// <summary>
        /// Integer constructor.
        /// </summary>
        /// <param name="minimum">The minimum allowed value. (Inclusive.)</param>
        public ClampMinimumAttribute(int minimum)
        {
            this.floatMinimum = (float)minimum;
            this.intMinimum = minimum;
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
        public readonly System.Type enumType;

        /// <summary>
        /// True if the flag names should be sorted.  Otherwise use default sorting.
        /// </summary>
        public readonly bool sort;

        /// <summary>
        /// True if the flag value should be displayed after the property name.
        /// </summary>
        public readonly bool displayValue;


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
            this.enumType = enumType;
            this.sort = sort;
            this.displayValue = displaValue;
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

    public class RequiredValueAttribute
        : PropertyAttribute
    {
        public System.Type ReferenceType { get; set; }
        public bool AllowSceneObjects { get; set; }

        public RequiredValueAttribute(
            System.Type referenceType = null, bool allowSceneObjects = false)
        {
            ReferenceType = referenceType;
            AllowSceneObjects = allowSceneObjects;
        }
    }

    /// <summary>
    /// Display a dropdown containing a list of components attached to the current GameObject or
    /// any of its children.  (Used to limit assignment to only local components.)
    /// </summary>
    public class LocalComponentPopupAttribute
        : PropertyAttribute
    {
        /// <summary>
        /// The property's component type.  (Used for the local component search.)
        /// </summary>
        public System.Type ComponentType { get; set; }

        /// <summary>
        /// If true, only a non-null property value is valid.
        /// </summary>
        public bool Required { get; set; }

        public string SearchPropertyPath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentType">
        /// The property's component type.  (Used for the local component search. E.g. Animator)
        /// </param>
        /// <param name="required">
        /// If true, only a non-null property value is valid.  Otherwise null is permitted.
        /// </param>
        public LocalComponentPopupAttribute(
            System.Type componentType, bool required = false, string searchPropertyPath = null)
        {
            ComponentType = componentType;
            Required = required;
            SearchPropertyPath = searchPropertyPath;
        }
    }
}