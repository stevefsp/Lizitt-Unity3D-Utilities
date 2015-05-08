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
    /// A Monobehaviour that implements <see cref="ITrsInterpolator"/> features that use 
    /// Mathf.SmoothStep for interpolation.
    /// </summary>
    public abstract class TrsSmoothStep
        : TrsInterpolator
    {
        [Space(5)]

        [SerializeField]
        private TrsInterpolationParams m_MainSettings = new TrsInterpolationParams();

        [Space(5)]

        [SerializeField]
        [Tooltip("The action to take when the interpolation operation is complete.")]
        private EaseCompletionType m_CompletionAction = EaseCompletionType.StopUpdating;

        private TrsSmoothStepHelper m_Helper;
        private bool m_IsBehaviourComplete = false;

        /// <summary>
        /// The length of the interpolation in seconds. [Limit: >= 0]
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

        protected override bool ContinueOnComplete
        {
            get { return m_CompletionAction == EaseCompletionType.ContinueUpdating; }
        }

        protected override BaseTrsInterpolationParams BaseSettings
        {
            get { return m_MainSettings; }
        }

        protected override ITrsInterpolationHelper Helper
        {
            get { return m_Helper; }
        }

        protected override bool IsBehaviourComplete
        {
            get { return m_IsBehaviourComplete; }
        }

        protected override void CreateHelper()
        {
            m_Helper = CreateHelper(m_MainSettings);
        }

        protected abstract TrsSmoothStepHelper CreateHelper(TrsInterpolationParams settings);

        protected override void LocalOnStatusChange()
        {
            if (Status == InterpolationStatus.Inactive)
            {
                m_IsBehaviourComplete = false;
                return;
            }

            if (Status != InterpolationStatus.Complete)
                return;

            switch (m_CompletionAction)
            {
                case EaseCompletionType.DestroyScript:

                    Destroy(this);
                    m_IsBehaviourComplete = true;
                    break;
            }
        }
    }
}
