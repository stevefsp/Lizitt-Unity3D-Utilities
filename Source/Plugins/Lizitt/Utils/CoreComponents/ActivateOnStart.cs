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

namespace com.lizitt
{
    /// <summary>
    /// Sets the target GameObject's active state on start, then Destroys itself.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Useful for activating objects on scene load that you don't normally want active
    /// in the editor.  (E.g. GUI overlay components.)
    /// </para>
    /// <para>
    /// Hint: Set this component's layer tag to EditorOnly if it should not be in the final
    /// project build.
    /// </para>
    /// <para>
    /// Remember: Must be attached to an active object to function.
    /// </para>
    /// </remarks>
    [AddComponentMenu(LizMenu.Menu + "Activate On Start", LizMenu.UtilityComponentMenuOrder + 0)]
    public class ActivateOnStart
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The object to activate or deativate.")]
        private GameObject m_Target = null;

        [SerializeField]
        [Tooltip("True if the target should be activated, false if it should be deactivated.")]
        private bool m_Activate = true;

        void Start()
        {
            if (m_Target)
                m_Target.SetActive(m_Activate);

            GameObject.Destroy(this);
        }
    }
}
