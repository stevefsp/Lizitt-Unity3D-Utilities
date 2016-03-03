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
    /// Provides a variety of extension methods.
    /// </summary>
    public static partial class Extensions
    {
        /*
         * Collider extensions
         * 
         * Design notes:
         * 
         * The Extensions class encapulates a large variety of extension methods
         * in one class so they don't clutter the namespaces.  E.g. Want to avoid GameObjectExt, 
         * TranformExt, etc.  
         * 
         * Only when a particular type deserves its own utility class will extensions be
         * considered for movement.  (See: MecanimUtil, ColorUtil, etc.)
         */

        /// <summary>
        /// Returns the Rigidbody currently attached to the collider or the one that will be
        /// attached.  Otherwise null.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If Collider.attachedRigidbody is not assigned, then this method will return the
        /// result of a parent search.  This is helpful when it is not know if the collider has
        /// been fully initialized.  (E.g. When trying to locate a collider's the rigidbody 
        /// outside of play mode.
        /// </para>
        /// </remarks>
        /// <param name="collider">The collider to check.</param>
        /// <returns>
        /// Returns the Rigidbody currently attached to the collider or the one that will be
        /// attached.  Otherwise null.
        /// </returns>
        public static Rigidbody GetAssociatedRigidBody(this Collider collider)
        {
            return collider.attachedRigidbody
                ? collider.attachedRigidbody
                : collider.GetComponentInParent<Rigidbody>();
        }

        /// <summary>
        /// Gets the behavior type of the collider based on its current configuration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See the 
        /// <a href="http://docs.unity3d.com/Manual/CollidersOverview.html">Collider Overview</a>
        /// page in the Unity Manual for details.
        /// </para>
        /// </remarks>
        /// <param name="collider">The collider.</param>
        /// <returns>The behavior type of collider based on its current configuration.</returns>
        public static ColliderStatus GetStatus(this Collider collider)
        {
            if (!collider)
                return ColliderStatus.Disabled;

            var rigidbody = collider.GetAssociatedRigidBody();

            if (rigidbody)
            {
                if (collider.enabled && rigidbody.detectCollisions)
                {
                    if (collider.isTrigger)
                    {
                        return rigidbody.isKinematic
                            ? ColliderStatus.KinematicTrigger
                            : ColliderStatus.RigidbodyTrigger;
                    }
                    else
                    {
                        return rigidbody.isKinematic
                            ? ColliderStatus.KinematicCollider
                            : ColliderStatus.RigidbodyCollider;
                    }
                }
                else if (rigidbody.isKinematic || !rigidbody.useGravity)
                    return ColliderStatus.Disabled;
                else
                    return ColliderStatus.GravityBody;
            }
            else if (collider.enabled)  // No rigidbody.
            {
                return collider.isTrigger
                    ? ColliderStatus.StaticTrigger
                    : ColliderStatus.StaticCollider;
            }
            else
                return ColliderStatus.Disabled;
        }

        private const string BadRigidBodyTransition =
            "Invalid transition: Collider does not have a Rigidbody component: {0} -> {1}";

        private const string BadStaticTransition =
            "Invalid transition: Collider has a Rigidbody component: {0} -> {1}";

        /// <summary>
        /// Configures the collider for the specified behavior.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See the 
        /// <a href="http://docs.unity3d.com/Manual/CollidersOverview.html">Collider Overview</a>
        /// page in the Unity Manual for details.
        /// </para>
        /// <para>
        /// This method is not appropriate for use with compound rigidbody collider configurations
        /// since it only updates the specified collider and its ridigbody.  It does not try to
        /// locate other colliders that might be associated with the rigidbody.
        /// </para>
        /// <para>
        /// Transitions between rigidbody and static behaviors is not supported since doing so
        /// would required adding and destroying rigidbodys.
        /// </para>
        /// </remarks>
        /// <param name="collider">The collider to update.</param>
        /// <param name="status">The desired behavior type.</param>
        public static void SetStatus(this Collider collider, ColliderStatus status)
        {
            if (status == ColliderStatus.Disabled && !collider)
                return;

            var rigidbody = collider.GetAssociatedRigidBody();

            switch (status)
            {
                case ColliderStatus.KinematicCollider:

                    if (rigidbody)
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = true;
                        collider.isTrigger = false;
                        collider.enabled = true;
                    }
                    else
                    {
                        Debug.LogErrorFormat(
                            collider, BadRigidBodyTransition, collider.GetStatus(), status);
                    }

                    break;

                case ColliderStatus.KinematicTrigger:

                    if (rigidbody)
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = true;
                        collider.isTrigger = true;
                        collider.enabled = true;
                    }
                    else
                    {
                        Debug.LogErrorFormat(
                            collider, BadRigidBodyTransition, collider.GetStatus(), status);
                    }

                    break;

                case ColliderStatus.Disabled:

                    if (collider)
                        collider.enabled = false;
                    if (rigidbody)
                        rigidbody.isKinematic = true;   // Yes, this is needed.

                    break;

                case ColliderStatus.RigidbodyCollider:

                    if (rigidbody)
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = false;
                        collider.isTrigger = false;
                        collider.enabled = true;
                    }
                    else
                    {
                        Debug.LogErrorFormat(
                            collider, BadRigidBodyTransition, collider.GetStatus(), status);
                    }

                    break;

                case ColliderStatus.RigidbodyTrigger:

                    if (rigidbody)
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = false;
                        collider.isTrigger = true;
                        collider.enabled = true;
                    }
                    else
                    {
                        Debug.LogErrorFormat(
                            collider, BadRigidBodyTransition, collider.GetStatus(), status);
                    }

                    break;

                case ColliderStatus.StaticCollider:

                    if (rigidbody)
                    {
                        Debug.LogErrorFormat(
                            collider, BadStaticTransition, collider.GetStatus(), status);
                    }
                    else
                    {
                        collider.isTrigger = false;
                        collider.enabled = true;
                    }

                    break;

                case ColliderStatus.StaticTrigger:

                    if (rigidbody)
                    {
                        Debug.LogErrorFormat(
                            collider, BadStaticTransition, collider.GetStatus(), status);
                    }
                    else
                    {
                        collider.isTrigger = true;
                        collider.enabled = true;
                    }

                    break;

                case ColliderStatus.GravityBody:

                    if (rigidbody)
                    {
                        rigidbody.isKinematic = false;
                        rigidbody.useGravity = true;
                        collider.enabled = false;
                    }
                    else
                    {
                        Debug.LogErrorFormat(collider, BadRigidBodyTransition, collider.GetStatus(), status);
                    }

                    break;
            }
        }

        public static void SetStatus(this Collider collider, ColliderStatus status, bool useGravity)
        {
            var rigidbody = collider.GetAssociatedRigidBody();

            if (useGravity && !rigidbody)
            {
                Debug.LogError("Can't set a collider without a rigidbody to use gravity.", collider);
                return;
            }

            if (status == ColliderStatus.GravityBody && !useGravity)
            {
                Debug.LogErrorFormat(collider, "Invalid opetation: A status of {0} is not compatible with"
                    + " a 'useGravity' value of false.", status);
                return;
            }

            rigidbody.useGravity = useGravity;
            SetStatus(collider, status); 
        }
    }
}