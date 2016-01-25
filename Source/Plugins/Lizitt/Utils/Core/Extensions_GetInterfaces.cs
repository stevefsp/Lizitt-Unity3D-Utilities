/*
 * Copyright (c) 2015-2016 Stephen A. Pratt
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
    public static partial class Extensions
    {
        /*
         * Design notes:
         * 
         * Encapulates a variety of extension methods.  They are kept in one place to reduce
         * namespace clutter.  E.g. Want to avoid GameObjectExt, TranformExt, etc.  
         * 
         * Only when a particular type deserves its own utility class, due to complexity or to
         * colocate with non-extension methods, are a type's extensions considered for
         * movement.  (See AnimatorUtil, ColorUtil, etc.)
         */

        #region Get Interface (Single)

        /// <summary>
        /// Returns the component of the specified type if the object has one attached, optionally 
        /// disgregarding disabled Behaviours.
        /// </summary>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// For performance reasons, this method will only process the first component found,  
        /// so it can't be used reliably in situations where there is more than one component
        /// that implements the interface and only one is enabled.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="component">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <returns>The component of the specified type, or null if none found.</returns>
        public static T GetInterface<T>(this Component component, bool enabledOnly) where T : class
        {
            var item = component.GetComponent<T>();

            if (enabledOnly)
            {
                var behaviour = item as Behaviour;
                return !behaviour || behaviour.enabled ? item : null;
            }

            return item;
        }

        /// <summary>
        /// Returns the component of the specified type if the object has one attached, optionally
        /// disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// For performance reasons, this method will only process the first component found,  
        /// so it can't be used reliably in situations where there is more than one component
        /// that implements the interface and only one is enabled.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="gameObject">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <returns>The component of the specified type, or null if none found.</returns>
        public static T GetInterface<T>(this GameObject gameObject, bool enabledOnly)
            where T : class
        {
            var item = gameObject.GetComponent<T>();

            if (enabledOnly)
            {
                var behaviour = item as Behaviour;
                return !behaviour || behaviour.enabled ? item : null;
            }

            return item;
        }

        #endregion

        #region Get Interfaces (Multi)

        /// <summary>
        /// Returns all components of the specified type if the object has any
        /// attached, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// The array will be zero length if no components are found.  Whether or not a
        /// non-zero length array contains components depends on the value of 
        /// <see cref="compressResult"/>.
        /// </para>
        /// <para>
        /// If <paramref name="compressResult"/> is true and disabled components are found, then
        /// this method will generate extra garbage collection as the search array is 'compressed' 
        /// to remove null values.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="component">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <param name="compressResult">
        /// If true the returned array will only contain non-null entries.  Otherwise the 
        /// returned array will contain null values at indices where a component was found to be
        /// disabled. (Assuming <paramref name="enabledOnly"/> is true.)
        /// </param>
        /// <returns>An array containing any components found.</returns>
        public static T[] GetInterfaces<T>(
            this Component component, bool enabledOnly, bool compressResult) where T : class
        {
            var items = component.GetComponents<T>();

            return enabledOnly ? PurgeDisabledBehaviours(items, compressResult) : items;
        }

        /// <summary>
        /// Returns all components of the specified type if the object has any
        /// attached, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as the the <c>GetComponent()</c>.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, <c>GetComponent()</c> method ignores disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears <c>GetComponent()</c> doesn't check if it is returning a 
        /// <c>Behaviour</c>, so it doesn't perform an enabled check.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// The array will be zero length if no components are found.  Whether or not a
        /// non-zero length array contains components depends on the value of 
        /// <see cref="compressResult"/>.
        /// </para>
        /// <para>
        /// If <paramref name="compressResult"/> is true and disabled components are found, then
        /// this method will generate extra garbage collection as the search array is 'compressed' 
        /// to remove null values.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="gameObject">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <param name="compressResult">
        /// If true the returned array will only contain non-null entries.  Otherwise the 
        /// returned array will contain null values at indices where a component was found to be
        /// disabled. (Assuming <paramref name="enabledOnly"/> is true.)
        /// </param>
        /// <returns>An array containing any components found.</returns>
        public static T[] GetInterfaces<T>(
            this GameObject gameObject, bool enabledOnly, bool compressResult) where T : class
        {
            var items = gameObject.GetComponents<T>();

            return enabledOnly ? PurgeDisabledBehaviours(items, compressResult) : items;
        }

        #endregion

        #region Get Interface in Children (Single)

        /// <summary>
        /// Returns the component of the specified type if the object has one attached
        /// directly or to one of its children, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// For performance reasons, this method will only process the first component found,  
        /// so it can't be used reliably in situations where there is more than one component
        /// that implements the interface and only one is enabled.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="component">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <returns>The component of the specified type, or null if none found.</returns>
        public static T GetInterfaceInChildren<T>(this Component component, bool enabledOnly)
            where T : class
        {
            var item = component.GetComponentInChildren<T>();

            if (enabledOnly)
            {
                var behaviour = item as Behaviour;
                return !behaviour || behaviour.enabled ? item : null;
            }

            return item;
        }

        /// <summary>
        /// Returns the component of the specified type if the object has one attached
        /// directly or to one of its children, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// For performance reasons, this method will only process the first component found,  
        /// so it can't be used reliably in situations where there is more than one component
        /// that implements the interface and only one is enabled.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="gameObject">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <returns>The component of the specified type, or null if none found.</returns>
        public static T GetInterfaceInChildren<T>(this GameObject gameObject, bool enabledOnly)
            where T : class
        {
            var item = gameObject.GetComponentInChildren<T>();

            if (enabledOnly)
            {
                var behaviour = item as Behaviour;
                return !behaviour || behaviour.enabled ? item : null;
            }

            return item;
        }

        #endregion

        #region Get Components in Children (Multi)

        /// <summary>
        /// Returns all components of the specified type if the object has any attached directly
        /// or to one of its children, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// The array will be zero length if no components are found.  Whether or not a
        /// non-zero length array contains components depends on the value of 
        /// <see cref="compressResult"/>.
        /// </para>
        /// <para>
        /// If <paramref name="compressResult"/> is true and disabled components are found, then
        /// this method will generate extra garbage collection as the search array is 'compressed' 
        /// to remove null values.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="component">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <param name="compressResult">
        /// If true the returned array will only contain non-null entries.  Otherwise the 
        /// returned array will contain null values at indices where a component was found to be
        /// disabled. (Assuming <paramref name="enabledOnly"/> is true.)
        /// </param>
        /// <returns>An array containing any components found.</returns>
        public static T[] GetInterfacesInChildren<T>(
            this Component component, bool enabledOnly, bool compressResult) where T : class
        {
            var items = component.GetComponentsInChildren<T>();

            return enabledOnly ? PurgeDisabledBehaviours(items, compressResult) : items;
        }

        /// <summary>
        /// Returns all components of the specified type if the object has any attached directly
        /// or to one of its children, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// The array will be zero length if no components are found.  Whether or not a
        /// non-zero length array contains components depends on the value of 
        /// <see cref="compressResult"/>.
        /// </para>
        /// <para>
        /// If <paramref name="compressResult"/> is true and disabled components are found, then
        /// this method will generate extra garbage collection as the search array is 'compressed' 
        /// to remove null values.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="gameObject">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <param name="compressResult">
        /// If true the returned array will only contain non-null entries.  Otherwise the 
        /// returned array will contain null values at indices where a component was found to be
        /// disabled. (Assuming <paramref name="enabledOnly"/> is true.)
        /// </param>
        /// <returns>An array containing any components found.</returns>
        public static T[] GetInterfacesInChildren<T>(
            this GameObject gameObject, bool enabledOnly, bool compressResult) where T : class
        {
            var items = gameObject.GetComponentsInChildren<T>();

            return enabledOnly ? PurgeDisabledBehaviours(items, compressResult) : items;
        }

        #endregion

        #region Get Component In Parent (Single)

        /// <summary>
        /// Returns the component of the specified type if the object has one attached
        /// directly or to one of its parents, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// For performance reasons, this method will only process the first component found,  
        /// so it can't be used reliably in situations where there is more than one component
        /// that implements the interface and only one is enabled.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="component">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <returns>The component of the specified type, or null if none found.</returns>
        public static T GetInterfaceInParent<T>(this Component component, bool enabledOnly)
            where T : class
        {
            var item = component.GetComponentInParent<T>();

            if (enabledOnly)
            {
                var behaviour = item as Behaviour;
                return !behaviour || behaviour.enabled ? item : null;
            }

            return item;
        }

        /// <summary>
        /// Returns the component of the specified type if the object has one attached
        /// directly or to one of its parents, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// For performance reasons, this method will only process the first component found,  
        /// so it can't be used reliably in situations where there is more than one component
        /// that implements the interface and only one is enabled.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="gameObject">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <returns>The component of the specified type, or null if none found.</returns>
        public static T GetInterfaceInParent<T>(this GameObject gameObject, bool enabledOnly)
            where T : class
        {
            var item = gameObject.GetComponentInParent<T>();

            if (enabledOnly)
            {
                var behaviour = item as Behaviour;
                return !behaviour || behaviour.enabled ? item : null;
            }

            return item;
        }

        #endregion

        #region Get Interaces in Parent (Multi)

        /// <summary>
        /// Returns all components of the specified type if the object has any attached directly
        /// or to one of its parents, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// The array will be zero length if no components are found.  Whether or not a
        /// non-zero length array contains components depends on the value of 
        /// <see cref="compressResult"/>.
        /// </para>
        /// <para>
        /// If <paramref name="compressResult"/> is true and disabled components are found, then
        /// this method will generate extra garbage collection as the search array is 'compressed' 
        /// to remove null values.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="component">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <param name="compressResult">
        /// If true the returned array will only contain non-null entries.  Otherwise the 
        /// returned array will contain null values at indices where a component was found to be
        /// disabled. (Assuming <paramref name="enabledOnly"/> is true.)
        /// </param>
        /// <returns>An array containing any components found.</returns>
        public static T[] GetInterfacesInParent<T>(
            this Component component, bool enabledOnly, bool compressResult) where T : class
        {
            var items = component.GetComponentsInParent<T>();

            return enabledOnly ? PurgeDisabledBehaviours(items, compressResult) : items;
        }

        /// <summary>
        /// Returns all components of the specified type if the object has any attached directly
        /// or to one of its parents, optionally disgregarding disabled Behaviours.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only useful when T is an interface type.  Otherwise it will return
        /// the same result as its equivalent GetComponent method.
        /// </para>
        /// <para>
        /// As of Unity 5.2.2, GetComponent methods ignore disabled <c>Behaviour</c>'s.  
        /// But when the type is an interface implemented by a <c>Behaviour</c> then it 
        /// will return every the component, whether it is enabled or disabled.  
        /// (It appears the GetComponent methods don't check if they are returning a 
        /// <c>Behaviour</c>, so they don't perform enabled checks.) When 
        /// <paramref name="enabledOnly"/> is true, this method will perform an extra check 
        /// to see if the search result is a <c>Behaviour</c> and whether or not it is enabled.
        /// </para>
        /// <para>
        /// The array will be zero length if no components are found.  Whether or not a
        /// non-zero length array contains components depends on the value of 
        /// <see cref="compressResult"/>.
        /// </para>
        /// <para>
        /// If <paramref name="compressResult"/> is true and disabled components are found, then
        /// this method will generate extra garbage collection as the search array is 'compressed' 
        /// to remove null values.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">An interface type.</typeparam>
        /// <param name="gameObject">The object to search.</param>
        /// <param name="enabledOnly">True if only enabled objects should be returned.</param>
        /// <param name="compressResult">
        /// If true the returned array will only contain non-null entries.  Otherwise the 
        /// returned array will contain null values at indices where a component was found to be
        /// disabled. (Assuming <paramref name="enabledOnly"/> is true.)
        /// </param>
        /// <returns>An array containing any components found.</returns>
        public static T[] GetInterfacesInParent<T>(
            this GameObject gameObject, bool enabledOnly, bool compressResult) where T : class
        {
            var items = gameObject.GetComponentsInParent<T>();

            return enabledOnly ? PurgeDisabledBehaviours(items, compressResult) : items;
        }

        #endregion

        #region Get Component Utilities

        /// <summary>
        /// Nulls or removes disabled components from the provided array.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="items">yThe items to check.</param>
        /// <param name="compressResult">
        /// If true and <paramref name="items"/> contains disabled compoents, compress the items 
        /// into a new array with only the enabled components. Otherwise just null out the
        /// array entries that have disabled components.
        /// </param>
        /// <returns>The reference to the processed array.</returns>
        private static T[] PurgeDisabledBehaviours<T>(T[] items, bool compressResult)
            where T : class
        {
            int enabledCount = 0;
            for (int i = 0; i < items.Length; i++)
            {
                var behaviour = items[i] as Behaviour;
                if (!behaviour || behaviour.enabled)  // non-Behaviours are always enabled.
                    enabledCount++;
                else
                    items[i] = null;
            }

            if (compressResult && enabledCount < items.Length)
                items = items.Compress();

            return items;
        }

        #endregion
    }
}