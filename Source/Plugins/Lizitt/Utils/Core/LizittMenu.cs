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
using UnityEngine;

namespace com.lizitt
{
    /// <summary>
    /// Provides various constants for use with Unity Editor menus attributes.
    /// </summary>
    public static class LizittMenu
    {
        /// <summary>
        /// The general Lizitt menu path.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Used with <c>AddComponentMenu</c> and <c>MenuItem</c>
        /// </para>
        /// </remarks>
        public const string Menu = "Lizitt/";

        /// <summary>
        /// The menu path for the easing menu.
        /// </summary>
        public const string EasingMenu = Menu + "Easing/";

        /// <summary>
        /// The maximum number of defined orders within a menu group.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following condition must be met in order for menu groups to remain separate:
        /// UpperGroupLowestOrder >= LowerGroupHighestOrder + 10.  Lizitt uses this number of keep its menu
        /// groups properly organized.  If groups start to merge, then increasing the value of this constant
        /// will fix the problem.
        /// </para>
        /// <para>
        /// This value can also be used by other libraries to standardize maximum group size across a project.
        /// </para>
        /// </remarks>
        public const int MenuGroupAllocation = 10;

        /// <summary>
        /// The menu order at which a menu item will be inserted into a group after the last Unity default menu item 
        /// in the Assets -> Create menu.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A menu order of (thisValue - 1) will group a menu item with the last Unity default menu item.
        /// </para>
        /// </remarks>
        public const int CreateAssetMenuOrderEnd = 712;

        /// <summary>
        /// The base order of the Lizitt asset creation menu.  (After the last default Unity menu item in 
        /// Asset -> Create.)
        /// </summary>
        public const int AssetMenuOrderStart = CreateAssetMenuOrderEnd + 100;

        // This value has no effect.  It can be 10000000 or -1000000 and there won't be a difference.  The menu will
        // be just above or below the 'Scripts' menu item. Unity decides.  The value is defined here just in case 
        // the behaivor changes in a later Unity release.
        /// <summary>
        /// The base menu order for component menu items.
        /// </summary>
        public const int ComponentMenuOrderStart = 1000;

        /// <summary>
        /// The base order for easing components.  (Has its own sub-menu.  See <see cref="EasingMenu"/>
        /// </summary>
        public const int EasingComponentMenuOrder = ComponentMenuOrderStart;

        /// <summary>
        /// The base menu order for marker components.
        /// </summary>
        public const int MarkerComponentMenuOrder = EasingComponentMenuOrder + MenuGroupAllocation + 10;

        /// <summary>
        /// The base menu order for utility components.
        /// </summary>
        public const int UtilityComponentMenuOrder = MarkerComponentMenuOrder + MenuGroupAllocation + 10;

        /// <summary>
        /// The base menu order for editor-only components.
        /// </summary>
        public const int EditorComponentMenuOrder = UtilityComponentMenuOrder + MenuGroupAllocation + 10;
    }
}
