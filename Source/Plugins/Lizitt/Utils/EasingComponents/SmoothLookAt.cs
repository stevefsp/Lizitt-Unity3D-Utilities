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

namespace com.lizitt.easing
{
    /// <summary>
    /// An interpolator that continuously and smoothly rotates a transform to look at another 
    /// transform.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interpolator never completes and is optimized for a dynamic look target.
    /// </para>
    /// </remarks>
    [AddComponentMenu(LizittMenu.EasingMenu + "Smooth LootAt", LizittMenu.EasingComponentMenuOrder + 2)]
    public class SmoothLookAt
        : TrsInterpolator
    {
        [Space(5)]

        [SerializeField]
        private SmoothLookAtParams m_MainSettings = new SmoothLookAtParams();

        private SmoothLookAtHelper m_Helper;

        /// <summary>
        /// The speed of the look at behavior. [Limit: >= 0]
        /// </summary>
        public float Speed
        {
            get { return m_MainSettings.Speed; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.Speed = value;
                else
                    m_Helper.Speed = value;
            }
        }

        /// <summary>
        /// The rotation's up direction. (Vector3.zero is interpreted as Vector3.up)
        /// </summary>
        public Vector3 UpVector
        {
            get { return m_MainSettings.UpVector; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.UpVector = value;
                else
                    m_Helper.UpVector = value;
            }
        }

        /// <summary>
        /// If true, the y-axis delta between <see cref="ItemToTransform"/> and the look position
        /// is ignored.
        /// </summary>
        public bool KeepVertical
        {
            get { return m_MainSettings.KeepVertical; }
            set
            {
                if (m_Helper == null)
                    m_MainSettings.KeepVertical = value;
                else
                    m_Helper.KeepVertical = value;
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
            m_Helper = new SmoothLookAtHelper(m_MainSettings);
        }
    }
}