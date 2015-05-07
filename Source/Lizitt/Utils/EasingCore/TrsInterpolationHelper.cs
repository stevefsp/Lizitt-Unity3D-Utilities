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
    /// Common time-based <see cref="ITrsInterpolationHelper"/> settings.
    /// </summary>
    [System.Serializable]
    public class TrsInterpolationParams
        : BaseTrsInterpolationParams
    {
        [Header("Interpolation Settings")]

        [SerializeField]
        [Tooltip("The length of the interpolation in seconds. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_Duration = 1;

        /// <summary>
        /// The length of the interpolation in seconds. [Limit: >= 0]
        /// </summary>
        public float Duration
        {
            get { return m_Duration; }
            set { m_Duration = Mathf.Max(0, value); }
        }
    }

    /// <summary>
    /// Provides common time-based <see cref="ITrsInterpolationHelper"/> features.
    /// </summary>
    /// <typeparam name="T">
    /// The type of interpolation settings used by the interpolator.
    /// </typeparam>
    public abstract class TrsInterpolationHelper<T>
        : BaseTrsInterpolationHelper<T>
        where T : TrsInterpolationParams
    {
        private bool m_IsInitialized;
        private bool m_IsComplete;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TrsInterpolationHelper()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">The settings to use.  (Uses the reference, not a copy.)</param>
        public TrsInterpolationHelper(T settings)
            : base(settings)
        {
        }

        /// <summary>
        /// The number of seconds the operation will take to complete.
        /// </summary>
        public float Duration
        {
            get { return Settings.Duration; }
            set { Settings.Duration = value; }
        }

        /// <summary>
        /// The value being interpolated from.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value is undefined until after the first update.
        /// </para>
        /// </remarks>
        private Vector3 From { get; set; }

        /// <summary>
        /// The value being interpolated toward.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value is undefined until after the first update.
        /// </para>
        /// </remarks>
        public Vector3 To { get; private set; }

        /// <summary>
        /// The number of seconds the interpolation has been running.
        /// </summary>
        public float Time { get; private set; }

        /// <summary>
        /// The normalized completion time for the current run. (0 to 1)
        /// </summary>
        public float NormalizedTime { get; private set; }

        /// <summary>
        /// If true the current run is complete. (<see cref="NormalizedTime"/> >= 0)
        /// </summary>
        public override bool IsComplete
        {
            get { return m_IsComplete; }
        }

        /// <summary>
        /// Resets the interpolator so it can be re-used.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            From = Vector3.zero;
            To = Vector3.zero;
            Time = 0;
            NormalizedTime = 0;
            m_IsComplete = false;

            m_IsInitialized = false;
        }

        /// <summary>
        /// Updates the instance until the interpolation is complete.
        /// </summary>
        /// <returns>
        /// True while the interpolation is running.  False after the interpolation is complete.
        /// </returns>
        public override bool Update()
        {
            if (m_IsComplete && StopOnComplete)
                return false;

            if (!m_IsInitialized)
            {
                Initialize();

                if (!m_IsInitialized)
                {
                    Debug.LogError("Helper failed to initialize.");
                    return false;
                }
            }

            Time += UnityEngine.Time.deltaTime;
            To = RefreshToValue(From);

            if (Duration == 0)
            {
                ApplyUpdate(To);
                m_IsComplete = true;
                return false;
            }

            NormalizedTime = Time / Duration;

            if (NormalizedTime >= 1)
            {
                ApplyUpdate(To);
                m_IsComplete = true;
                return false;
            }

            var nval = Interpolate(From, To, NormalizedTime);
            ApplyUpdate(nval);
            
            return true;
        }

        /// <summary>
        /// Implements the interpolation algorithm.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Called during <see cref="Update"/> when the interpolator is active and normalized time
        /// is greater than 0.0 and less than 1.0.  An interpolator is active if it is not complete
        /// or is complete, but <see cref="StopOnComplete"/> is false.
        /// </para>
        /// </remarks>
        /// <param name="from">The value to interpolate from. (The value at time 0.0.)</param>
        /// <param name="to">The value to interpolate to. (The value at time 1.0.)</param>
        /// <param name="normalizedTime">The normalized time. (0 to 1)</param>
        /// <returns>The interpolated value.</returns>
        protected abstract Vector3 Interpolate(Vector3 from, Vector3 to, float normalizedTime);

        /// <summary>
        /// Applies the interpolated value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only called when the interpolator is active. An interpolator is active if it is not complete
        /// or is complete, but <see cref="StopOnComplete"/> is false.
        /// </para>
        /// </remarks>
        /// <param name="value">The interpolated value for the current time.</param>
        protected abstract void ApplyUpdate(Vector3 value);

        /// <summary>
        /// Gets the value being interpolated from.  (The value at time 0.0.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is called during the first update after a reset.  (Once per run.)
        /// </para>
        /// <para>
        /// This interpolator does not support a dynamic <see cref="From"/> value.
        /// </para>
        /// </remarks>
        /// <returns>The value being interpolated from.  (The value at time 0.0.)</returns>
        protected abstract Vector3 GetFromValue();

        /// <summary>
        /// Gets the value being interpolated to.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is called during the first update after a reset.  (Once per run.)
        /// It is called after <see cref="GetFromValue"/>, so <see cref="From"/> is available.
        /// </para>
        /// </remarks>
        /// <returns>The value being interpolated to.</returns>
        protected abstract Vector3 GetToValue(Vector3 fromValue);

        /// <summary>
        /// Refreshes the <see cref="To"/> value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Called during <see cref="Update"/> when the interpolator is active.  
        /// An interpolator is active if it is not complete or is complete, but 
        /// <see cref="StopOnComplete"/> is false.  It is always called before 
        /// <see cref="Interpolate"/>.
        /// </para>
        /// <para>
        /// Behavior is dependant on the concrete class.  Some classes implement a fixed 
        /// <see cref="To"/> value, others have dynamic <see cref="To"/> values.
        /// </para>
        /// </remarks>
        /// <param name="fromValue">
        /// The current <see cref="From"/> value that should be used.
        /// </param>
        /// <returns></returns>
        protected abstract Vector3 RefreshToValue(Vector3 fromValue);

        /// <summary>
        /// If true the <see cref="Update"/> should become idle as soon as <see cref="IsComplete"/>
        /// is true.  Otherwise the updates should continue event if complete.
        /// </summary>
        protected abstract bool StopOnComplete { get; }

        private void Initialize()
        {
            Reset();

            if (!ItemToTransform)
            {
                Debug.LogError("Transform item is not assigned.");
                return;
            }

            From = GetFromValue();
            // Must be called after GetFromValue since it may depend on that value.
            To = GetToValue(From);

            m_IsInitialized = true;
        }
    }
}

