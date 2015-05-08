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
using UnityEngine.Serialization;

namespace com.lizitt.u3d
{
    /// <summary>
    /// A Transform-based translate/rotate/scale interpolator.
    /// </summary>
    public abstract class TrsInterpolator
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If true, the component will start playing at component start.  Otherwise"
            + " the Play method must be used to start interpolation.")]
        [FormerlySerializedAs("m_AutoStart")]
        private bool m_AutoPlay = false;

        [SerializeField]
        [Tooltip("If true the component will be processed during LateUpdate instead of Update.")]
        private bool m_UseLateUpdate = false;

        private InterpolationStatus m_Status = InterpolationStatus.Inactive;

        /// <summary>
        /// The common interpolator settings.
        /// </summary>
        protected abstract BaseTrsInterpolationParams BaseSettings { get; }

        /// <summary>
        /// The interpolation helper.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is undefined until <see cref="CreateHelper"/> is called.
        /// </para>
        /// </remarks>
        protected abstract ITrsInterpolationHelper Helper { get; }

        /// <summary>
        /// Create the interpolation helper.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will be called once, the first time the helper is needed. 
        /// <see cref=" Helper"/> must be available after this method completes.
        /// </para>
        /// </remarks>
        protected abstract void CreateHelper();

        /// <summary>
        /// Called whenever the interpolator's status changes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This callback allows concrete classes to respond to changes in the status.
        /// </para>
        /// </remarks>
        protected virtual void LocalOnStatusChange()
        {
        }

        /// <summary>
        /// If true, the helper will continue to be updated until the <see cref="Status"/> changes 
        /// to complete.
        /// </summary>
        protected virtual bool ContinueOnComplete
        {
            get { return false; }
        }

        /// <summary>
        /// True if the behavior is complete and <see cref="Status"/> should transition to complete.
        /// </summary>
        protected virtual bool IsBehaviourComplete
        {
            get { return false; }
        }

        /// <summary>
        /// The current status of the interpolator.
        /// </summary>
        public InterpolationStatus Status
        {
            get { return m_Status; }
        }

        /// <summary>
        /// If true, the component will start playing at component start.  Otherwise
        /// the Play method must be used to start interpolation.
        /// </summary>
        public bool AutoPlay
        {
            get { return m_AutoPlay; }
            set { m_AutoPlay = value; }
        }

        /// <summary>
        /// If true the component will be processed during LateUpdate instead of Update.
        /// </summary>
        public bool UseLateUpdate
        {
            get { return m_UseLateUpdate; }
            set { m_UseLateUpdate = value; }
        }

        // Don't make it public.  There are periods where this may not match status.
        private bool IsComplete
        {
            get { return IsBehaviourComplete || (Helper != null && Helper.IsComplete); }
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
            get
            {
                return BaseSettings.ItemToTransform ? BaseSettings.ItemToTransform : transform;
            }
            set
            {
                if (Helper == null)
                    BaseSettings.ItemToTransform = value;
                else
                    Helper.Reset(value, Helper.Space);
            }
        }

        /// <summary>
        /// The item being interpolated to.  (If null, only the <see cref="MatchOffset"/> is used.)
        /// </summary>
        public Transform MatchItem
        {
            get { return BaseSettings.MatchItem; }
            set
            {
                if (Helper == null)
                    BaseSettings.MatchItem = value;
                else
                    Helper.MatchItem = value;
            }
        }

        /// <summary>
        /// The offset from <see cref="MatchItem"/> if it is set, otherwise the fixed offset.
        /// </summary>
        public Vector3 MatchOffset
        {
            get { return BaseSettings.MatchOffset; }
            set
            {
                if (Helper == null)
                    BaseSettings.MatchOffset = value;
                else
                    Helper.MatchOffset = value;
            }
        }

        /// <summary>
        /// The space the interpolation is performed in.
        /// </summary>
        public Space Space
        {
            get { return BaseSettings.Space; }
            set
            {
                if (Helper == null)
                    BaseSettings.Space = value;
                else
                    Helper.Reset(Helper.ItemToTransform, value);
            }
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
            get { return BaseSettings.AutoApply; }
            set
            {
                if (Helper == null)
                    BaseSettings.AutoApply = value;
                else
                    Helper.AutoUpdate = value;
            }
        }

        /// <summary>
        /// The current interpolated value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value is undefined until after the first interpolator update.
        /// </para>
        /// </remarks>
        public Vector3 InterpolatedValue
        {
            get { return Helper.InterpolatedValue; }
        }

        // Unity reflection behavior requires that this be protected.
        protected void Start()
        {
            if (m_AutoPlay)
                Play();
        }

        // Unity reflection behavior requires that this be protected.
        protected void Update()
        {
            if (!m_UseLateUpdate)
                Process();
        }

        // Unity reflection behavior requires that this be protected.
        protected void LateUpdate()
        {
            if (m_UseLateUpdate)
                Process();
        }

        private void Process()
        {
            if (m_Status == InterpolationStatus.Inactive || m_Status == InterpolationStatus.Paused)
                return;

            if (m_Status == InterpolationStatus.Playing)
            {
                if (IsBehaviourComplete)  // Check only the behavior.
                {
                    m_Status = InterpolationStatus.Complete;
                    LocalOnStatusChange();
                }

                Helper.Update();

                if (IsComplete)  // Checks both behavior and helper.
                {
                    m_Status = InterpolationStatus.Complete;
                    LocalOnStatusChange();
                }
            }
            else if (!IsBehaviourComplete && ContinueOnComplete)
                Helper.Update();
            else
                return;  // Don't want to do the post update.

            LocalProcess();
        }

        /// <summary>
        /// Called whenever the interpolator is processed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only called while the interpolator is playing, though the status may show as complete
        /// if a transition has occurred during the current update.
        /// </para>
        /// </remarks>
        protected virtual void LocalProcess()
        {
        }

        /// <summary>
        /// Play the iterpolator.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The interpolator will be reset and re-started if this method is called when
        /// <see cref="Status"/> is complete.
        /// </para>
        /// </remarks>
        public void Play()
        {
            switch (m_Status)
            {
                case InterpolationStatus.Playing:

                    return;

                case InterpolationStatus.Paused:

                    m_Status = InterpolationStatus.Playing;
                    LocalOnStatusChange();
                    return;

                case InterpolationStatus.Complete:

                    Reset();
                    break;
            }

            if (Helper == null)
            {
                if (!BaseSettings.ItemToTransform)
                    BaseSettings.ItemToTransform = transform;
                CreateHelper();
            }
            else
                Helper.Reset();

            m_Status = InterpolationStatus.Playing;
            LocalOnStatusChange();
        }

        /// <summary>
        /// Pauses the interpolator.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is only valid to call this method while <see cref="Status"/> is playing or paused.
        /// </para>
        /// </remarks>
        public void Pause()
        {
            switch (m_Status)
            {
                case InterpolationStatus.Playing:

                    m_Status = InterpolationStatus.Paused;
                    LocalOnStatusChange();
                    return;

                case InterpolationStatus.Paused:

                    return;
            }

            Debug.LogError("Can't pause from status: " + m_Status);
        }

        /// <summary>
        /// Resets the interpolator for re-use.
        /// </summary>
        public void Reset()
        {
            // Design note: Children can capture the reset by overriding OnStatusChange().
            // So don't make this virtual.

            if (Helper != null)
                // Always reset helper.  Helper may need internal refresh.
                Helper.Reset();

            if (m_Status != InterpolationStatus.Inactive)
            {
                m_Status = InterpolationStatus.Inactive;
                LocalOnStatusChange();
            }

            if (m_AutoPlay)
                Play();
        }
    }
}
