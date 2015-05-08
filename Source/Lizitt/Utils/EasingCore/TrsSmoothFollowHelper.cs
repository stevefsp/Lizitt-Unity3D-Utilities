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

namespace com.lizitt.u3d
{
    /// <summary>
    /// Settings related to a <see cref="ITrsInterpolator"/> that implements a continuous smooth 
    /// follow behavior.
    /// </summary>
    [System.Serializable]
    public class TrsSmoothFollowParams
        : BaseTrsInterpolationParams
    {
        [Header("Smooth Follow Settings")]

        [SerializeField]
        [Tooltip("The approximate completion time.  (Will be limited by maximum speed.) [Limit: > 0]")]
        [ClampMinimum(MathUtil.Epsilon)]
        private float m_SmoothTime = 1;

        [SerializeField]
        [Tooltip("The maximum allowed transition rate. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_MaximumSpeed = 999;

        /// <summary>
        /// The approximate completion time.  (Will be limited by maximum speed.) [Limit: > 0]
        /// </summary>
        public float SmoothTime
        {
            get { return m_SmoothTime; }
            set { m_SmoothTime = Mathf.Max(MathUtil.Epsilon, value); }
        }

        /// <summary>
        /// The maximum allowed transition rate. [Limit: >= 0]
        /// </summary>
        public float MaximumSpeed
        {
            get { return m_MaximumSpeed; }
            set { m_MaximumSpeed = Mathf.Max(0, value); }
        }
    }

    /// <summary>
    /// An interpolator that continuously and smoothly interpolates a transform to match another
    /// item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interpolator never completes and is optimized for a target.
    /// </para>
    /// </remarks>
    public abstract class TrsSmoothFollowHelper
        : BaseTrsInterpolationHelper<TrsSmoothFollowParams>
    {
        private Vector3 m_Velocity;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TrsSmoothFollowHelper()
        {
            Settings = new TrsSmoothFollowParams();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="settings"/> is stored as a reference, not a copy.  So changes to
        /// the settings object will be reflected in the interpolator.
        /// </para>
        /// </remarks>
        /// <param name="settings">The settings to use.</param>
        public TrsSmoothFollowHelper(TrsSmoothFollowParams settings)
            : base(settings)
        {
        }

        /// <summary>
        /// The approximate completion time.  (Will be limited by maximum speed.) [Limit: > 0]
        /// </summary>
        public float SmoothTime
        {
            get { return Settings.SmoothTime; }
            set { Settings.SmoothTime = value; }
        }

        /// <summary>
        /// The maximum allowed transition rate. [Limit: >= 0]
        /// </summary>
        public float MaximumSpeed
        {
            get { return Settings.MaximumSpeed; }
            set { Settings.MaximumSpeed = value; }
        }

        /// <summary>
        /// The current interpolation velocity.
        /// </summary>
        public Vector3 Velocity
        {
            get { return m_Velocity; }
        }

        /// <summary>
        /// False.  Never completes.
        /// </summary>
        public override bool IsComplete
        {
            get { return false; }
        }

        /// <summary>
        /// Updates the interpolation using Time.deltaTime.
        /// </summary>
        /// <returns>
        /// True.  (Never completes.)
        /// </returns>
        public override bool Update()
        {
            var current = GetCurrentValue();
            var target = GetTargetValue(current);

            var nval = Interpolate(current, target, ref m_Velocity, SmoothTime, MaximumSpeed);

            ApplyUpdate(nval);

            return true;
        }

        /// <summary>
        /// Applys the interpolated value to <see cref="ItemToTransform."/>
        /// </summary>
        /// <param name="value">The value to apply.</param>
        protected abstract void ApplyUpdate(Vector3 value);

        /// <summary>
        /// Gets the value to be interpolated toward the target.
        /// </summary>
        /// <returns>The value to be interpolated toward the target.</returns>
        protected abstract Vector3 GetCurrentValue();

        /// <summary>
        /// Gets the value to be interpolated toward.
        /// </summary>
        /// <param name="fromValue">The value being interpolated from.</param>
        /// <returns>The value to be interpolated toward.</returns>
        protected abstract Vector3 GetTargetValue(Vector3 fromValue);

        /// <summary>
        /// Interpolates the two values based on the interpolator behavior.
        /// </summary>
        /// <param name="current">The value being interpolted from.</param>
        /// <param name="target">The value being interpolated to.</param>
        /// <param name="velocity">The current interpolation velocity.</param>
        /// <param name="smoothTime">The interpolation duration. [Limit: > 0]</param>
        /// <param name="maximumSpeed">The maximum interpolation speed. [Limit: >= 0]</param>
        /// <returns></returns>
        protected abstract Vector3 Interpolate(Vector3 current, Vector3 target, 
            ref Vector3 velocity, float smoothTime, float maximumSpeed);


        /// <summary>
        /// Resets the interpolator to its start state.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method must be called if a Rigidbody is added to or removed from 
        /// <see cref="ItemToTransform"/> in order to refresh the interpolator's internal state.
        /// </para>
        /// </remarks>
        public override void Reset()
        {
            base.Reset();
            m_Velocity = Vector3.zero;
        }
    }
}

