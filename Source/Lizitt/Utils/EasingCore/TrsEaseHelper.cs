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
    /// Settings related to a <see cref="ITrsInterpolationHelper"/> that uses the standard easing
    /// functions.
    /// </summary>
    [System.Serializable]
    public class TrsEaseParams
        : TrsInterpolationParams
    {
        [SerializeField]
        [Tooltip("The type of ease function to use.")]
        private EaseType m_EaseMode = EaseType.Linear;

        [SerializeField]
        [Tooltip("True if the match item should be treated as dynamic rather than static."
            + " (Dynamic is more expensive and may result in less accurate interpolation.)")]
        private bool m_IsMatchDynamic = false;

        /// <summary>
        /// The current ease mode.
        /// </summary>
        public EaseType EaseMode
        {
            get { return m_EaseMode; }
            internal set { m_EaseMode = value; }
        }

        /// <summary>
        /// True if the match item should be treated as dynamic rather than static.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A dynamic match target is a target that may change after interpolation
        /// begins.  Dynamic  match targets are generally more expensive to deal with and
        /// may result in less accurate interpolation.
        /// </para>
        /// </remarks>
        public virtual bool MatchIsDynamic
        {
            get { return m_IsMatchDynamic; }
            set { m_IsMatchDynamic = value; }
        }
    }

    /// <summary>
    /// Provides <see cref="ITrsInterpolator"/> features that uses the standard easing
    /// functions.
    /// </summary>
    public abstract class TrsEaseHelper
        : TrsInterpolationHelper<TrsEaseParams>
    {
        private EaseFunction m_Function;

#if UNITY_EDITOR
        private EaseType m_LastMode;
#endif

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="easeMode">The ease mode.</param>
        public TrsEaseHelper(EaseType easeMode)
        {
            Settings = new TrsEaseParams();
            Settings.EaseMode = easeMode;
            m_Function = Easing.GetFunction(Settings.EaseMode);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">The settings to use.  (Uses the reference, not a copy.)</param>
        public TrsEaseHelper(TrsEaseParams settings)
            : base(settings)
        {
            m_Function = Easing.GetFunction(settings.EaseMode);
        }

        /// <summary>
        /// The ease mode.
        /// </summary>
        public EaseType EaseMode
        {
            get { return Settings.EaseMode; }
            set
            {
                if (Settings.EaseMode == value)
                    return;

                Settings.EaseMode = value;
                m_Function = Easing.GetFunction(Settings.EaseMode);
            }
        }

        protected sealed override Vector3 Interpolate(Vector3 from, Vector3 to, float normalizedTime)
        {
#if UNITY_EDITOR
            if (m_LastMode != Settings.EaseMode)
            {
               // Special case: User changed the mode in inspector.
                m_Function = Easing.GetFunction(Settings.EaseMode);
                m_LastMode = Settings.EaseMode;
            }
#endif
            return new Vector3(
                m_Function(from.x, to.x, normalizedTime),
                m_Function(from.y, to.y, normalizedTime),
                m_Function(from.z, to.z, normalizedTime)
            );
        }

        protected sealed override Vector3 RefreshToValue(Vector3 fromValue)
        {
            return Settings.MatchIsDynamic ? GetToValue(fromValue) : To;
        }

        protected sealed override bool StopOnComplete
        {
            get { return !Settings.MatchIsDynamic;}
        }
    }
}

