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
    /// Provides various common utility features.
    /// </summary>
    public static class LizittUtil
    {
        #region Constants

        /// <summary>
        /// A name suffix used to label baked items.  (E.g. The baked version of a skinned mesh.)
        /// </summary>
        public const string BakeSuffix = "_Baked";

        #endregion

        #region Extensions

        /// <summary>
        /// Returns true if the object is null or is a destroyed UnityEngine.Object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A common way of null checking a UnityEngine.Object is using a boolean check.
        /// (E.g. <c>myUnityObject == true</c>)  Such a check is helpful since it takes into
        /// account the possible destruction of the object.  This method
        /// will check a non-null reference to see if it is a UnityEngine.Object and perform the
        /// destruction check.
        /// </para>
        /// </remarks>
        /// <param name="obj">The reference to check.</param>
        /// <returns>
        /// True if the object is null or is a destroyed UnityEngine.Object.
        /// </returns>
        public static bool IsUnityDestroyed<T>(T obj) where T : class
        {
            // Note: This method is here instead of in Extensions because it is useful to call it directly as
            // partr of a null check.

            if (obj != null)
            {
                if (obj is Object)
                    return !(obj as Object);

                return false;
            }

            return true;
        }

        #endregion

        #region Collider Status

        // Note: There isn't really any better place to put this right now, at least not without cluttering up
        // the namespace.  It isn't useful enough to implement as an extension, so it should'nt go there.

        /// <summary>
        /// Attempts to derive a collider from a UnityEngine.Object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method detects the type of <paramref name="obj"/> then uses the most appropriate method.
        /// (I.e. Simple casting, a component search, etc.)
        /// </para>
        /// <para>
        /// Can be called lazily.  A null object will return a null collider.
        /// </para>
        /// </remarks>
        /// <param name="obj">The object to attepmts to get a collider from. (Can be null)</param>
        /// <param name="searchChildren">
        /// If true and a component search is required, then include children in the search, otherwise only search 
        /// the Component or GameObject.</param>
        /// <returns></returns>
        public static Collider DeriveCollider(Object obj, bool searchChildren = true)
        {
            if (!obj)
                return null;
            if (obj is Collider)
                return obj as Collider;
            if (obj is GameObject)
                return (obj as GameObject).GetComponentInChildren<Collider>();
            if (obj is Component)
                return (obj as Component).GetComponentInChildren<Collider>();

            // This may be ok.  Reference may not yet be set.
            return null;
        }

        #endregion

        #region Unity Editor

        /*
         * This region includes utility members for use with the Unity editor.  
         * E.g. The CreateAssetMenu attribute.
         */

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

        #endregion
    }
}
