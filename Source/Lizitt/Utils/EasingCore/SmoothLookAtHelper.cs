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
    /// look at behavior.
    /// </summary>
    [System.Serializable]
    public class SmoothLookAtParams
        : BaseTrsInterpolationParams
    {
        [Header("Look At Settings")]

        [SerializeField]
        [Tooltip("The speed of the look at behavior. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_Speed = 1;

        [SerializeField]
        [Tooltip("The rotation's up direction. (Vector3.zero is interpreted as Vector3.up)")]
        private Vector3 m_UpVector = Vector3.up;

        [SerializeField]
        [Tooltip(
            "If true, the y-axis delta between the transform item and look position be ignored.")]
        private bool m_KeepVertical = false;

        /// <summary>
        /// The speed of the look at behavior. [Limit: >= 0]
        /// </summary>
        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = Mathf.Max(0, value); }
        }

        /// <summary>
        /// The rotation's up direction. (Vector3.zero is interpreted as Vector3.up)
        /// </summary>
        public Vector3 UpVector
        {
            get { return m_UpVector; }
            set { m_UpVector = value; }
        }

        /// <summary>
        /// If true, the y-axis delta between <see cref="ItemToTransform"/> and the look position
        /// is ignored.
        /// </summary>
        public bool KeepVertical
        {
            get { return m_KeepVertical; }
            set { m_KeepVertical = value; }
        }
    }

    /// <summary>
    /// An interpolator that continuously and smoothly rotates a transform to look at another 
    /// transform.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interpolator never completes and is optimized for a dynamic look target.
    /// </para>
    /// </remarks>
    public class SmoothLookAtHelper
        : BaseTrsInterpolationHelper<SmoothLookAtParams>
    {
        private Quaternion m_LastRotation;
        private Quaternion m_DesiredRotation;
        private bool m_IsInitialized;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SmoothLookAtHelper()
        {
            Settings = new SmoothLookAtParams();
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
        public SmoothLookAtHelper(SmoothLookAtParams settings)
            : base(settings)
        {
        }

        /// <summary>
        /// The speed of the look at behavior. [Limit: >= 0]
        /// </summary>
        public float Speed
        {
            get { return Settings.Speed; }
            set { Settings.Speed = value; }
        }

        /// <summary>
        /// The rotation's up direction. (Vector3.zero is interpreted as Vector3.up)
        /// </summary>
        public Vector3 UpVector 
        {
            get { return Settings.UpVector; }
            set { Settings.UpVector = value; }
        }

        /// <summary>
        /// If true, the y-axis delta between <see cref="ItemToTransform"/> and the look position
        /// is ignored.
        /// </summary>
        public bool KeepVertical
        {
            get { return Settings.KeepVertical; }
            set { Settings.KeepVertical = value; }
        }

        /// <summary>
        /// Always returns false.  This interpolator never completes.
        /// </summary>
        public override bool IsComplete
        {
            get { return false; }
        }

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
            m_IsInitialized = false;
        }

        /// <summary>
        /// Updates the interpolator based on Time.deltaTime.
        /// </summary>
        /// <returns>True.  (Never completes.)</returns>
        public override bool Update()
        {
            if (!m_IsInitialized)
            {
                if (!Initialize())
                    return false;
            }

            Vector3 lookAtPos;
            
            if (MatchItem)
            {
                lookAtPos = MatchOffset == Vector3.zero
                    ? MatchItem.position
                    : MatchItem.TransformPoint(MatchOffset);
            }
            else
                lookAtPos = MatchOffset;

            if (KeepVertical)
                lookAtPos.y = ItemToTransform.position.y;

            var diff = lookAtPos - ItemToTransform.position;
            if (diff != Vector3.zero && diff.sqrMagnitude > 0)
            {
                m_DesiredRotation =
                    Quaternion.LookRotation(diff, UpVector == Vector3.zero ? Vector3.up : UpVector);
            }

            m_LastRotation =
                Quaternion.Slerp(m_LastRotation, m_DesiredRotation, Speed * Time.deltaTime);

            InterpolatedValue = m_LastRotation.eulerAngles;

            if (AutoUpdate)
            {
                if (RigidBody && !RigidBody.isKinematic)
                    RigidBody.MoveRotation(m_LastRotation);
                else
                    ItemToTransform.rotation = m_LastRotation;
            }

            return true;
        }

        private bool Initialize()
        {
            if (!ItemToTransform)
            {
                Debug.LogError("Item to transform is null.  Can't initialize helper.");
                return false;
            }

            Reset();

            m_LastRotation = ItemToTransform.rotation;
            m_DesiredRotation = ItemToTransform.rotation;

            m_IsInitialized = true;

            return true;
        }
    }
}

