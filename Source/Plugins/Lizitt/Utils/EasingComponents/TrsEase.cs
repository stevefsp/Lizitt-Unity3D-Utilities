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
    /// A Monobehaviour that implements <see cref="ITrsInterpolationHelper"/> features that use the 
    /// standard easing functions.
    /// </summary>
    public abstract class TrsEase
        : TrsInterpolator
    {
        [Space(5)]

        [SerializeField]
        private TrsEaseParams m_MainSettings = new TrsEaseParams();

        [Space(5)]

        [SerializeField]
        [Tooltip("The action to take when the ease operation is complete.")]
        private EaseCompletionType m_CompletionAction = EaseCompletionType.StopUpdating;

        private TrsEaseHelper m_Helper;
        private bool m_IsBehaviorComplete;

        /// <summary>
        /// The ease type to use.
        /// </summary>
        public EaseType Mode
        {
            get { return m_MainSettings.EaseMode; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.EaseMode = value;
                else
                    m_Helper.EaseMode = value;
            }
        }

        /// <summary>
        /// The length of the ease in seconds. [Limit: >= 0]
        /// </summary>
        public float Duration
        {
            get { return m_MainSettings.Duration; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.Duration = value;
                else
                    m_Helper.Duration = value;
            }
        }

        /// <summary>
        /// The action to take when the ease operation is complete.
        /// </summary>
        public EaseCompletionType CompletionAction
        {
            get { return m_CompletionAction; }
            set { m_CompletionAction = value; }
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
        public bool MatchIsDynamic
        {
            get { return m_MainSettings.MatchIsDynamic; }
            set { m_MainSettings.MatchIsDynamic = value; }
        }

        /// <summary>
        /// Creates the interpolation helper.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called to initialize <see cref="Helper"/>.
        /// </para>
        /// </remarks>
        /// <param name="settings">The settings to use.</param>
        /// <returns>An interpolation helper.</returns>
        protected abstract TrsEaseHelper LocalCreateHelper(TrsEaseParams settings);

        protected override bool IsBehaviourComplete
        {
            get { return m_IsBehaviorComplete; }
        }

        protected override bool ContinueOnComplete
        {
            get { return m_CompletionAction == EaseCompletionType.ContinueUpdating; }
        }

        protected override void LocalOnStatusChange()
        {
            if (Status == InterpolationStatus.Inactive)
            {
                m_IsBehaviorComplete = false;
                return;
            }

            if (Status != InterpolationStatus.Complete)
                return;

            switch (m_CompletionAction)
            {
                case EaseCompletionType.DestroyScript:

                    m_IsBehaviorComplete = true;
                    Destroy(this);
                    break;
            }
        }

        protected override BaseTrsInterpolationParams BaseSettings
        {
            get { return m_MainSettings; }
        }

        protected override ITrsInterpolationHelper Helper
        {
            get { return m_Helper; }
        }

        protected override void CreateHelper()
        {
            m_Helper = LocalCreateHelper(m_MainSettings);
        }
    }
}
