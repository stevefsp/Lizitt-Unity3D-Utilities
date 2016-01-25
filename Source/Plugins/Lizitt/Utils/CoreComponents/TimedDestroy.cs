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
using System.Collections;
using UnityEngine;

namespace com.lizitt
{
    /// <summary>
    /// Automatically destroys the target after the specified number of seconds.
    /// </summary>
    public class TimedDestroy
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The object to destroy, or self if null.")]
        private GameObject m_Target;

        [SerializeField]
        [Tooltip("The number of seconds to wait before destroying the target. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_Delay = 1f;

        /// <summary>
        /// The object to destroy, or self if null.
        /// </summary>
        public GameObject Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        /// <summary>
        /// The number of seconds to wait before destroying the target. [Limit: >= 0]
        /// </summary>
        public float Delay
        {
            get { return m_Delay; }
            set { m_Delay = Mathf.Max(value); }
        }

        IEnumerator Start()
        {
            if (!m_Target)
                m_Target = gameObject;

            if (m_Delay > 0)
                yield return new WaitForSeconds(m_Delay);

            m_Target.SafeDestroy();           
        }
    }
}
