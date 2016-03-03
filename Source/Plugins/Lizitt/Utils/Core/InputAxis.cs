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

namespace com.lizitt
{
    /// <summary>
    /// The string names for the standard <c>UnityEngine.Input</c> axes.
    /// </summary>
    public static class InputAxis
    {
        private static string m_Horizontal = "Horizontal";

        /// <summary>
        /// The hoizontal action button. (Usually right/left.)
        /// </summary>
        public static string Horizontal
        {
            get { return InputAxis.m_Horizontal; }
            set { InputAxis.m_Horizontal = value; }
        }

        private static string m_Vertical = "Vertical";

        /// <summary>
        /// The vertical action button.  (Usually up/down or forward/back depending on context.)
        /// </summary>
        public static string Vertical
        {
            get { return InputAxis.m_Vertical; }
            set { InputAxis.m_Vertical = value; }
        }

        private static string m_Fire1 = "Fire1";

        /// <summary>
        /// The primary fire/select button.
        /// </summary>
        public static string Fire1
        {
            get { return InputAxis.m_Fire1; }
            set { InputAxis.m_Fire1 = value; }
        }

        private static string m_Fire2 = "Fire2";

        /// <summary>
        /// The secondary fire/select button.
        /// </summary>
        public static string Fire2
        {
            get { return InputAxis.m_Fire2; }
            set { InputAxis.m_Fire2 = value; }
        }

        private static string m_Fire3 = "Fire3";

        /// <summary>
        /// The tertiary fire/select button.
        /// </summary>
        public static string Fire3
        {
            get { return InputAxis.m_Fire3; }
            set { InputAxis.m_Fire3 = value; }
        }

        private static string m_Jump = "Jump";

        /// <summary>
        /// The jump button.
        /// </summary>
        public static string Jump
        {
            get { return InputAxis.m_Jump; }
            set { InputAxis.m_Jump = value; }
        }

        private static string m_MouseX = "Mouse X";

        /// <summary>
        /// The mouseX movement delta  (Up/Down)
        /// </summary>
        public static string MouseX
        {
            get { return InputAxis.m_MouseX; }
            set { InputAxis.m_MouseX = value; }
        }

        private static string m_MouseY = "Mouse Y";

        /// <summary>
        /// The mouseY movement delta. (Right/Left)
        /// </summary>
        public static string MouseY
        {
            get { return InputAxis.m_MouseY; }
            set { InputAxis.m_MouseY = value; }
        }

        private static string m_MouseScrollWheel = "Mouse ScrollWheel";

        /// <summary>
        /// The mouse scrollwheel movement delta.
        /// </summary>
        public static string MouseScrollWheel
        {
            get { return InputAxis.m_MouseScrollWheel; }
            set { InputAxis.m_MouseScrollWheel = value; }
        }

        private static string m_Submit = "Submit";

        /// <summary>
        /// The submit button.
        /// </summary>
        public static string Submit
        {
            get { return InputAxis.m_Submit; }
            set { InputAxis.m_Submit = value; }
        }

        private static string m_Cancel = "Cancel";

        /// <summary>
        /// The cancel button.
        /// </summary>
        public static string Cancel
        {
            get { return InputAxis.m_Cancel; }
            set { InputAxis.m_Cancel = value; }
        }
    }
}