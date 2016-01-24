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
    /// An interpolator that continuously and smoothly interpolates a transform to match another
    /// item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interpolator never completes and is optimized for a target.
    /// </para>
    /// </remarks>
    public abstract class TrsSmoothFollow
        : TrsInterpolator
    {
        [Space(5)]

        [SerializeField]
        private TrsSmoothFollowParams m_MainSettings = new TrsSmoothFollowParams();

        private TrsSmoothFollowHelper m_Helper;

        protected override BaseTrsInterpolationParams BaseSettings
        {
            get { return m_MainSettings; }
        }

        protected override ITrsInterpolationHelper Helper
        {
            get { return m_Helper; }
        }

        /// <summary>
        /// The approximate completion time.  (Will be limited by maximum speed.) [Limit: > 0]
        /// </summary>
        public float SmoothTime
        {
            get { return m_MainSettings.SmoothTime; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.SmoothTime = value;
                else
                    m_Helper.SmoothTime = value;
            }
        }

        /// <summary>
        /// The maximum allowed transition rate. [Limit: >= 0]
        /// </summary>
        public float MaximumSpeed
        {
            get { return m_MainSettings.MaximumSpeed; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.MaximumSpeed = value;
                else
                    m_Helper.MaximumSpeed = value;
            }
        }

        protected override void CreateHelper()
        {
            m_Helper = CreateHelper(m_MainSettings);
        }

        /// <summary>
        /// Create a helper the uses the provided settings.
        /// </summary>
        /// <param name="settings">The helper settings.</param>
        /// <returns>A helper the uses the provided settings.</returns>
        protected abstract TrsSmoothFollowHelper CreateHelper(TrsSmoothFollowParams settings);
    }
}
