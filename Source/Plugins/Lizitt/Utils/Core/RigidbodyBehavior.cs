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
    /// Rigidbody collision behavior types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="http://docs.unity3d.com/Manual/CollidersOverview.html">Collider Overview</a> page in the 
    /// Unity Manual for details on collider behaviour under various configuations. The names in this enumeration 
    /// generally match with the definitions discussed in the above link.
    /// </para>
    /// </remarks>
    public enum RigidbodyBehavior
    {
        /// <summary>
        /// Collision detection and gravity are disabled.  (No rigidbody behavior.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders exchange no collision events.</li>
        /// <li>Colliders exchange no trigger events.</li>
        /// <li>Not detectable by raycasts.</li>
        /// <li>Not effected by gravity.</li>
        /// </ul>
        /// </para>
        /// <para>
        /// All collision detection is disabled if any of the following conditions exist:
        /// </para>
        /// <para>
        /// <ul>
        /// <li><c>Rigidbody.detectCollisions</c> is false. (This is a non-serializable setting.)</li>
        /// <li>The rigidbody has no colliders.</li>
        /// <li>All of the rigidbody's colliders are disabled.</li>
        /// </ul>
        /// </para>
        /// </remarks>
        Disabled = 0,

        /// <summary>
        /// Non-kinematic rigidbody collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders exchange collision events with colliders of all types.</li>
        /// <li>Colliders exchange trigger events with triggers of all types.</li>
        /// <li>Colliders are detectable by raycasts.</li>
        /// <li>May or may not be effected by gravity.</li>
        /// </ul>
        /// </para>
        /// </remarks>
        RigidbodyCollider = (int)ColliderBehavior.RigidbodyCollider,

        /// <summary>
        /// Kinematic Rigidbody Collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders exchange collision events with rigidbody colliders, and trigger events with all trigger types.</li>
        /// <li>Colliders exchange no events of any type with static colliders or other kinematic rigidbody objects.</li>
        /// <li>Colliders are detectable by raycasts.</li>
        /// <li>Not effected by gravity.</li>
        /// </ul>
        /// <para>
        /// The value of <c>Rigidbody.useGravity</c> is irrelevant for this type.
        /// </para>
        /// </remarks>
        KinematicCollider = (int)ColliderBehavior.KinematicCollider,

        /// <summary>
        /// Non-Kinematic rigidbody trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders exchange no collision events.</li>
        /// <li>Colliders exchange trigger events with all collider and trigger types.</li>
        /// <li>Colliders are detectable by raycasts.</li>
        /// <li>May or may not be effected by gravity.</li>
        /// </ul>
        /// <para>
        /// A rigidbody of this type should usually have <c>Rigidbody.useGravity</c> set to false. Otherwise it will 
        /// fall downward forever.
        /// </para>
        /// </remarks>
        RigidbodyTrigger = (int)ColliderBehavior.RigidbodyTrigger,

        /// <summary>
        /// Kinematic Rigidbody Trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders exchange no collision events.</li>
        /// <li>Colliders exchange trigger events with all collider and trigger types.</li>
        /// <li>Colliders are detectable by raycasts.</li>
        /// <li>Not effected by gravity.</li>
        /// </ul>
        /// </para>
        /// <para>
        /// The value of <c>Rigidbody.useGravity</c> is irrelevant for this type.
        /// </para>
        /// </remarks>
        KinematicTrigger = (int)ColliderBehavior.KinematicTrigger,

        /// <summary>
        /// Gravity enabled while all collision detection is disabled.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders exchange no collision or trigger events with any other objects.</li>
        /// <li>Not detectable by raycast.</li>
        /// <li>Effected by gravity.</li>
        /// </ul>
        /// </para>
        /// <para>
        /// All collision detection is disabled if any of the following conditions exist:
        /// </para>
        /// <para>
        /// <ul>
        /// <li><c>Rigidbody.detectCollisions</c> is false. (This is a non-serializable setting.)</li>
        /// <li>The rigidbody has no colliders.</li>
        /// <li>All of the rigidbody's colliders are disabled.</li>
        /// </ul>
        /// </para>
        /// </remarks>
        GravityBody = int.MaxValue - 1,

        Mixed = int.MaxValue,
    }
}
