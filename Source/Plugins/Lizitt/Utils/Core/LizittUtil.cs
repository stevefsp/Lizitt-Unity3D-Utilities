/*
 * Copyright (c) 2016 Stephen A. Pratt
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
    /// Provides various common utility features.
    /// </summary>
    public static class LizittUtil
    {
        /// <summary>
        /// A name suffix used to label baked items.  (E.g. The baked version of a skinned mesh.)
        /// </summary>
        public const string BakeSuffix = "_Baked";

        #region Unity Editor

        /*
         * This region includes utility members for use with the Unity editor.  
         * E.g. The CreateAssetMenu attribute.
         */

        /// <summary>
        /// The base order for menu items in the Lizitt asset creation menu.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Used for the <c>order</c> parameter of the <c>CreateAssetMenu</c> attribute.
        /// E.g <c>order = LizittUtil.BaseMenuOrder + 10</c></para>
        /// </remarks>
        public const int BaseMenuOrder = 1000;

        /// <summary>
        /// The Lizitt menu name in path form.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Used for the <c>menuName</c> parameter of the <c>CreateAssetMenu</c> attribute.
        /// E.g <c>menuName = LizittUtil.LizittMenu + "My Component Name"</c></para>
        /// </remarks>
        public const string LizittMenu = "Lizitt/";

        #endregion
    }
}