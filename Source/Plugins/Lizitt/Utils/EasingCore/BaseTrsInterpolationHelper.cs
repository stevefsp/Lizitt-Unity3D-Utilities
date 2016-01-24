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
    /*
     * Design notes:
     * 
     * Easing/Interpolation is implemented using a dual object design in order to better support 
     * Unity components.  The two classes are settings & interpolator.
     * 
     * Having separate settings means that components that want to implement easing won't
     * have to implement their own serialization.  Also, it allows components to more easily 
     * support live tweaking in editor playmode.  Without a shared settings object, playmode
     * tweaking would require a lot of extra wiring to transfer editor changes between the component
     * and helper.
     * 
     * A weakness in this design is that a single settings reference may be shared by two
     * separate objects with no clear or enforcable ownership.
     * 
     */

    /// <summary>
    /// Common <see cref="ITrsInterpolationHelper"/> settings.
    /// </summary>
    [System.Serializable]
    public class BaseTrsInterpolationParams
    {
        [Header("Item to Interpolate")]

        [SerializeField]
        [Tooltip("The transform to operate on.")]
        private Transform m_Item = null;

        [SerializeField]
        [Tooltip("The interpolation space.")]
        private Space m_Space = Space.World;

        [SerializeField]
        [Tooltip("If true, the interpolator will automatically update the item,"
            + " otherwise only the interpolation value will be updated.")]
        private bool m_AutoApply = true;

        [Header("Item to Match")]

        [SerializeField]
        [Tooltip("The item to interpolate toward.  (If null, only the match offset is used.)")]
        private Transform m_MatchItem = null;

        [SerializeField]
        [Tooltip(
            "The offset from the match item if it is set, otherwise the fixed offset.")]
        private Vector3 m_MatchOffset = Vector3.zero;

        /// <summary>
        /// The Rigidbody object attached to <see cref="ItemToTransform"/>, or null if there is
        /// none.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is only guarenteed to be up-to-date after <see cref="Refresh"/> is called.
        /// </para>
        /// </remarks>
        public Rigidbody RigidBody { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseTrsInterpolationParams()
        {
        }

        /// <summary>
        /// The item the interpolation is being applied to.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Setting this property to a new value will reset <see cref="RigidBody"/> to null.
        /// </para>
        /// </remarks>
        public Transform ItemToTransform
        {
            get { return m_Item; }
            set 
            {
                if (m_Item == value)
                    return;

                m_Item = value;
                this.RigidBody = null;
            }
        }

        /// <summary>
        /// The space the interpolation is performed in.
        /// </summary>
        public Space Space
        {
            get { return m_Space; }
            set { m_Space = value; }
        }

        /// <summary>
        /// The item being interpolated to.  (If null, only the <see cref="MatchOffset"/> is used.)
        /// </summary>
        public Transform MatchItem
        {
            get { return m_MatchItem; }
            set { m_MatchItem = value; }
        }

        /// <summary>
        /// The offset from <see cref="MatchItem"/> if it is set, otherwise the fixed offset.
        /// </summary>
        public Vector3 MatchOffset
        {
            get { return m_MatchOffset; }
            set { m_MatchOffset = value; }
        }        
        
        /// <summary>
        /// True if the interpolator should apply its interpolation to <see cref="ItemToTransform"/>
        /// during its update.  Otherwise, only update interpolation state.
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are use cases where interpolation results need to be applied by a different
        /// component or at a different time in the engine loop that the interpolator is being
        /// updated.  Setting this field to false allows for such scenarios.
        /// </para>
        /// </remarks>
        public bool AutoApply
        {
            get { return m_AutoApply; }
            set { m_AutoApply = value; }
        }

        /// <summary>
        /// Refreshes internal cached values related to the item to transform. (Such as 
        /// <see cref="RigidBody"/>.
        /// </summary>
        public virtual void Refresh()
        {
            this.RigidBody = m_Item ? m_Item.GetComponent<Rigidbody>() : null;
        }
    }

    /// <summary>
    /// Provides common <see cref="ITrsInterpolationHelper"/> features.
    /// </summary>
    /// <typeparam name="T">
    /// The type of interpolation settings used by the interpolator.
    /// </typeparam>
    public abstract class BaseTrsInterpolationHelper<T> 
        : ITrsInterpolationHelper 
        where T : BaseTrsInterpolationParams
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseTrsInterpolationHelper()
        {
            // A default constructor is needed to allow concrete classes
            // to construct their own settings.
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">The settings to use.  (Uses the reference, not a copy.)</param>
        public BaseTrsInterpolationHelper(T settings)
        {
            if (settings == null)
                throw new System.ArgumentNullException("settings");

            Settings = settings;
        }

        // Note: The settings setter is not private in order to allow concretes 
        // to use the default constructor.

        /// <summary>
        /// The interpolator's settings.  (Must be set before use.)
        /// </summary>
        protected T Settings { get; set; }

        /// <summary>
        /// The interpolated value.  (The meaning of the value is context sensitive.)
        /// </summary>
        public Vector3 InterpolatedValue { get; protected set; }

        /// <summary>
        /// The item the interpolation is being applied to.
        /// </summary>
        public Transform ItemToTransform 
        {
            get { return Settings.ItemToTransform; }
        }

        /// <summary>
        /// The Rigidbody object attached to <see cref="ItemToTransform"/>, or null if there is
        /// none.
        /// </summary>
        public Rigidbody RigidBody 
        {
            get { return Settings.RigidBody; }
        }

        /// <summary>
        /// The item being interpolated to.  (If null, only <see cref="MatchOffset"/> is used.)
        /// </summary>
        public Transform MatchItem 
        {
            get { return Settings.MatchItem;  }
            set { Settings.MatchItem = value; }
        }

        /// <summary>
        /// The offset from <see cref="MatchItem"/> if it is set, otherwise the fixed offset.
        /// </summary>
        public Vector3 MatchOffset
        {
            get { return Settings.MatchOffset; }
            set { Settings.MatchOffset = value; }
        }

        /// <summary>
        /// The space the interpolation is performed in.
        /// </summary>
        public Space Space
        {
            get { return Settings.Space; }
        }        
        
        /// <summary>
        /// True if the interpolator should apply its interpolation to <see cref="ItemToTransform"/>
        /// during its update.  Otherwise, only update interpolation state.
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are use cases where interpolation results need to be applied by a different
        /// component or at a different time in the engine loop that the interpolator is being
        /// updated.  Setting this field to false allows for such scenarios.
        /// </para>
        /// </remarks>
        public bool AutoUpdate
        {
            get { return Settings.AutoApply; }
            set { Settings.AutoApply = value; }
        }

        /// <summary>
        /// Resets the interpolator for re-use.
        /// </summary>
        public virtual void Reset()
        {
            Settings.Refresh();
        }

        /// <summary>
        /// Resets the interpolator for re-use, setting the parameters in the process.
        /// </summary>
        /// <param name="itemToTransform">The item the interpolation is being applied to.</param>
        /// <param name="space">The space the interpolation is performed in.</param>
        public void Reset(Transform itemToTransform, Space space)
        {
            Settings.Space = space;
            Settings.ItemToTransform = itemToTransform;

            Reset();
        }

        /// <summary>
        /// Resets the interpolator for re-use, setting the parameters in the process.
        /// </summary>
        /// <param name="itemToTransform">The item the interpolation is being applied to.</param>
        public void Reset(Transform itemToTransform)
        {
            Settings.ItemToTransform = itemToTransform;

            Reset();
        }

        /// <summary>
        /// Resets the interpolator for re-use, setting the parameters in the process.
        /// </summary>
        /// <param name="space">The space the interpolation is performed in.</param>
        public void Reset(Space space)
        {
            Settings.Space = space;

            Reset();
        }

        /// <summary>
        /// Updates the interpolator based on its settings.
        /// </summary>
        /// <returns></returns>
        public abstract bool Update();

        /// <summary>
        /// True if interpolation is complete and no further changes to 
        /// <see cref="ItemToTransform"/> or <see cref="InterpolatedValue"/> will be made. 
        /// (Even if further calls to update are made.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Whether an interpolator ever completes is dependant on the concrete implementation.
        /// Some interpolators are designed to never complete.  (E.g. Followers.)
        /// </para>
        /// </remarks>
        public abstract bool IsComplete { get; }

        #region Position Helpers

        /// <summary>
        /// Gets the standard <see cref="ItemToTransform"/> position based on the value of
        /// <see cref="Space"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behavior is undefined if this method is used before the settings have been
        /// applied and <see cref="ItemToTransform"/> has been set to a non-null value.
        /// </para>
        /// </remarks>
        /// <returns>The proper <see cref="ItemToTransform"/> position base on the value of
        /// <see cref="Space"/>.</returns>
        protected Vector3 GetStandardFromPosition()
        {
            return (Space == UnityEngine.Space.Self)
                ? ItemToTransform.localPosition
                : ItemToTransform.position;
        }

        /// <summary>
        /// Gets the standard match position based on the interpolator settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behavior is undefined if this method is used before the settings have been
        /// applied and <see cref="ItemToTransform"/> has been set to a non-null value.
        /// </para>
        /// </remarks>
        /// <returns>The standard match position based on the interpolator settings.</returns>
        protected Vector3 GetStandardToPosition()
        {
            var result = MatchOffset;

            if (Settings.MatchItem)
            {
                result = Space == Space.World
                    ? Settings.MatchItem.position + result
                    : Settings.MatchItem.localPosition + result;
            }

            return result;
        }

        /// <summary>
        /// Applies the position using the standard method. (Updates both 
        /// <see cref="InterpolatedValue"/> and <see cref="ItemToTransform"/> as appropriate.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Takes into account the value of <see cref="AutoUpdate"/>, the existance of a 
        /// RigidBody, the <see cref="Space"/>, etc.
        /// </para>
        /// <para>
        /// Behavior is undefined if this method is used before the settings have been
        /// applied and <see cref="ItemToTransform"/> has been set to a non-null value.
        /// </para>
        /// </remarks>
        /// <param name="position">The position to apply.</param>
        protected void ApplyStandardPositionUpdate(Vector3 position)
        {
            InterpolatedValue = position;

            if (AutoUpdate)
            {
                if (this.RigidBody)
                    this.RigidBody.MoveTo(InterpolatedValue, this.Space);
                else
                    ItemToTransform.MoveTo(InterpolatedValue, this.Space);
            }
        }

        #endregion

        #region Rotation Helpers

        /// <summary>
        /// Gets the standard <see cref="ItemToTransform"/> euler angles based on the value of
        /// <see cref="Space"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behavior is undefined if this method is used before the settings have been
        /// applied and <see cref="ItemToTransform"/> set to a non-null value.
        /// </para>
        /// </remarks>
        /// <returns>The proper <see cref="ItemToTransform"/> euler angles base on the value of
        /// <see cref="Space"/>.</returns>
        protected Vector3 GetStandardFromRotation()
        {
            return (Space == UnityEngine.Space.Self)
                            ? ItemToTransform.localEulerAngles
                            : ItemToTransform.eulerAngles;
        }

        /// <summary>
        /// Gets the standard match euler angles based on the interpolator settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behavior is undefined if this method is used before the settings have been
        /// applied and <see cref="ItemToTransform"/> set to a non-null value.
        /// </para>
        /// </remarks>
        /// <returns>The standard match position based on the interpolator settings.</returns>
        protected Vector3 GetStandardToRotation(Vector3 fromValue)
        {
            var result = MatchOffset;
            if (Settings.MatchItem)
            {
                result = Space == Space.World
                    ? Settings.MatchItem.eulerAngles + result
                    : Settings.MatchItem.localEulerAngles + result;
            }

            // This makes sure the shortest path is followed.
            result.x = Easing.Clerp(fromValue.x, result.x, 1);
            result.y = Easing.Clerp(fromValue.y, result.y, 1);
            result.z = Easing.Clerp(fromValue.z, result.z, 1);

            return result;
        }

        /// <summary>
        /// Applies the eulerAngles using the standard method. (Updates both 
        /// <see cref="InterpolatedValue"/> and <see cref="ItemToTransform"/> as appropriate.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Takes into account the value of <see cref="AutoUpdate"/>, the existance of a 
        /// RigidBody, the <see cref="Space"/>, etc.
        /// </para>
        /// <para>
        /// Behavior is undefined if this method is used before the settings have been
        /// applied and <see cref="ItemToTransform"/> set to a non-null value.
        /// </para>
        /// </remarks>
        /// <param name="eulerAngles">The angles, in degrees, to apply.</param>
        protected void ApplyStandardRotationUpdate(Vector3 eulerAngles)
        {
            InterpolatedValue = eulerAngles;

            var rotation = Quaternion.Euler(eulerAngles);

            if (RigidBody && !RigidBody.isKinematic)
            {
                if (Space == UnityEngine.Space.Self)
                {
                    var original = ItemToTransform.rotation;

                    // Hack time!  Using transform to convert from local 
                    // to world space.
                    ItemToTransform.localRotation = rotation;
                    InterpolatedValue = ItemToTransform.eulerAngles;
                    rotation = Quaternion.Euler(InterpolatedValue);

                    // Restore to original
                    ItemToTransform.rotation = original;
                }

                if (AutoUpdate)
                    RigidBody.MoveRotation(rotation);
            }
            else if (AutoUpdate)
            { 
                if (Space == UnityEngine.Space.Self)
                    ItemToTransform.localRotation = rotation;
                else
                    ItemToTransform.rotation = rotation;
            }
        }

        #endregion
    }
}

