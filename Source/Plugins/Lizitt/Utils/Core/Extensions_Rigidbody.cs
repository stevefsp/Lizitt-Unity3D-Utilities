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
using System.Collections.Generic;

namespace com.lizitt
{
    public static partial class Extensions
    {       
        /*
         * Rigidbody extensions.
         */

        /// <summary>
        /// Applies the position apropriately, taking into account whether or not the rigidbody is kinematic.
        /// </summary>
        /// <param name="rigidbody">The rigidbody to move.</param>
        /// <param name="position">The position to move to.</param>
        /// <param name="space">The space to move in.</param>
        public static void MoveTo(this Rigidbody rigidbody, Vector3 position, Space space = Space.World)
        {
            // TODO: v0.3: Add unit test.

            var trans = rigidbody.transform;

            if (!rigidbody.isKinematic)
            {
                if (space == Space.Self)
                {
                    var original = trans.position;

                    // HACK: Using the transform to convert from local to world space.
                    trans.localPosition = position;
                    position = trans.position;

                    trans.position = original;
                }

                rigidbody.MovePosition(position);
            }
            else
                trans.MoveTo(position, space);
        }

        #region Rigidbody Status

        // Sized for a complex humanoid, which is usually the most complex rigidbody structure used in Unity. 
        // (15 body parts + a capsule collider)
        private const int DefaultColliderBufferSize = 16;

        private static List<Collider> m_ColliderBuffer = null;

        private static List<Collider> PrepareColliderBuffer()
        {
            if (m_ColliderBuffer == null)
                m_ColliderBuffer = new List<Collider>(DefaultColliderBufferSize);

            m_ColliderBuffer.Clear();
            return m_ColliderBuffer;
        }

        /// <summary>
        /// Gets the behavior for the rigidbody.
        /// </summary>
        /// <param name="rigidbody">The rigidbody.</param>
        /// <returns>The behavior for the rigidbody.</returns>
        /// <seealso cref="RigidbodyBehavior"/>
        public static RigidbodyBehavior GetBehavior(this Rigidbody rigidbody)
        {
            if (!rigidbody)
                return RigidbodyBehavior.Disabled;

            GetColliders(rigidbody, PrepareColliderBuffer(), false);

            if (m_ColliderBuffer.Count == 0)
            {
                return (!rigidbody.isKinematic && rigidbody.useGravity)
                    ? RigidbodyBehavior.GravityBody
                    : RigidbodyBehavior.Disabled;
            }

            var cstatus = GetBehavior(m_ColliderBuffer[0], rigidbody);

            for (int i = 1; i < m_ColliderBuffer.Count; i++)
            {
                var nstatus = GetBehavior(m_ColliderBuffer[i], rigidbody);
                if (nstatus != cstatus)
                    return RigidbodyBehavior.Mixed;
            }

            m_ColliderBuffer.Clear();
            return (cstatus == ColliderBehavior.Disabled && !rigidbody.isKinematic && rigidbody.useGravity)
                ? RigidbodyBehavior.GravityBody
                : (RigidbodyBehavior)cstatus;
        }

        private const string InvalidRigidbodyBehavior =
            "{0}: Rigidbody has no colliders.  Invalid transition: {1} -> {2}";

        /// <summary>
        /// Configures the rigidbody and its colliders for the specified behavior.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See the 
        /// <a href="http://docs.unity3d.com/Manual/CollidersOverview.html">Collider Overview</a>
        /// page in the Unity Manual for details.
        /// </para>
        /// <para>
        /// This method will fail if the rigidbody has no colliders and <paramref name="status"/> is a 'rigidbody' 
        /// or 'kinematic' behavior type.  (Can't activate collider behavior on a rigidbody without colliders.)
        /// </para>
        /// </remarks>
        /// <param name="rigidbody">The rigidbody. (Required)</param>
        /// <param name="status">The desired behavior.</param>
        /// <param name="includeInactive">If true, include inactive game objects in the collider search.</param>
        /// <returns>True if the status change was successful.  False on an error.</returns>
        public static bool SetBehavior(
            this Rigidbody rigidbody, RigidbodyBehavior behavior, bool includeInactive = false)
        {
            if (behavior == RigidbodyBehavior.Mixed)
            {
                Debug.LogError("Can't set rigidbody status to: " + behavior, rigidbody);
                return false;
            }

            GetColliders(rigidbody, PrepareColliderBuffer(), includeInactive);

            bool success = true;

            /*
             * Design note:
             * 
             * Don't simplify the code by moving the collider loop to the end of the method.  Some behaviors only
             * require changing a single collider setting.  Don't make uncessary changes.  E.g. If  disabling a 
             * collider, don't mess with its trigger state.
             */

            switch (behavior)
            {
                case RigidbodyBehavior.KinematicCollider:

                    if (m_ColliderBuffer.Count == 0)
                    {
                        Debug.LogErrorFormat(
                            rigidbody, InvalidRigidbodyBehavior, rigidbody.name, rigidbody.GetBehavior(), behavior);
                        success = false;
                    }
                    else
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = true;

                        for (int i = 0; i < m_ColliderBuffer.Count; i++)
                        {
                            m_ColliderBuffer[i].isTrigger = false;
                            m_ColliderBuffer[i].enabled = true;
                        }

                    }
                    break;

                case RigidbodyBehavior.KinematicTrigger:

                    if (m_ColliderBuffer.Count == 0)
                    {
                        Debug.LogErrorFormat(
                            rigidbody, InvalidRigidbodyBehavior, rigidbody.name, rigidbody.GetBehavior(), behavior);
                        success = false;
                    }
                    else
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = true;

                        for (int i = 0; i < m_ColliderBuffer.Count; i++)
                        {
                            m_ColliderBuffer[i].isTrigger = true;
                            m_ColliderBuffer[i].enabled = true;
                        }
                    }

                    break;

                case RigidbodyBehavior.Disabled:

                    rigidbody.isKinematic = true;   // Yes, this is needed.

                    for (int i = 0; i < m_ColliderBuffer.Count; i++)
                        m_ColliderBuffer[i].enabled = false;

                    break;

                case RigidbodyBehavior.RigidbodyCollider:

                    if (m_ColliderBuffer.Count == 0)
                    {
                        Debug.LogErrorFormat(
                            rigidbody, InvalidRigidbodyBehavior, rigidbody.name, rigidbody.GetBehavior(), behavior);
                        success = false;
                    }
                    else
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = false;

                        for (int i = 0; i < m_ColliderBuffer.Count; i++)
                        {
                            m_ColliderBuffer[i].isTrigger = false;
                            m_ColliderBuffer[i].enabled = true;
                        }
                    }

                    break;

                case RigidbodyBehavior.RigidbodyTrigger:

                    if (m_ColliderBuffer.Count == 0)
                    {
                        Debug.LogErrorFormat(
                            rigidbody, InvalidRigidbodyBehavior, rigidbody.name, rigidbody.GetBehavior(), behavior);
                        success = false;
                    }
                    else
                    {
                        rigidbody.detectCollisions = true;
                        rigidbody.isKinematic = false;

                        for (int i = 0; i < m_ColliderBuffer.Count; i++)
                        {
                            m_ColliderBuffer[i].isTrigger = true;
                            m_ColliderBuffer[i].enabled = true;
                        }
                    }

                    break;

                case RigidbodyBehavior.GravityBody:

                    if (rigidbody)
                    {
                        rigidbody.isKinematic = false;
                        rigidbody.useGravity = true;

                        for (int i = 0; i < m_ColliderBuffer.Count; i++)
                            m_ColliderBuffer[i].enabled = false;
                    }

                    break;
            }

            m_ColliderBuffer.Clear();
            return success;
        }

        /// <summary>
        /// Get the colliders associated with the rigidbody.
        /// </summary>
        /// <param name="rigidbody">The rigidbody. (Required)</param>
        /// <param name="results">The list to append the results into, or null to create a new list.</param>
        /// <param name="includeInactive">If true, include inactive game objects in the collider search.</param>
        /// <returns>A refernce <paramref name="results"/> if it was non-null, or a reference to a new list.</returns>
        public static List<Collider> GetColliders(
            this Rigidbody rigidbody, List<Collider> results, bool includeInactive = false)
        {
            if (results == null)
                results = new List<Collider>(DefaultColliderBufferSize);

            var startIndex = results.Count;

            rigidbody.GetComponentsInChildren<Collider>(includeInactive, results);

            for (int i = results.Count - 1; i >= startIndex; i--)
            {
                if (results[i].GetAssociatedRigidBody() != rigidbody)
                    results.RemoveAt(i);
            }

            return results;
        }

        /// <summary>
        /// Get the colliders associated with the rigidbody.
        /// </summary>
        /// <param name="rigidbody">The rigidbody. (Required)</param>
        /// <param name="includeInactive">If true, include inactive game objects in the collider search.</param>
        /// <returns>An array of colliders associated with the rigidbody.</returns>
        public static Collider[] GetColliders(this Rigidbody rigidbody, bool includeInactive = false)
        {
            var result = GetColliders(rigidbody, PrepareColliderBuffer(), includeInactive).ToArray();
            m_ColliderBuffer.Clear();
            return result;
        }

        #endregion
    }
}