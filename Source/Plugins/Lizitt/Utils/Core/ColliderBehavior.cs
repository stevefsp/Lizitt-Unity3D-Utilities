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
    /// Collider statuc categories.
    /// </summary>
    public enum ColliderBehaviorCategory
    {
        /*
         * Design notes:
         * 
         * Various custom editors must be updated to handle changes made to this enumeration.
         * 
         */

        /// <summary>
        /// Only behaviors for colliders that don't have a rigidbody.
        /// </summary>
        Static = 0,

        /// <summary>
        /// Only behaviors for colliders that have a rigidbody.
        /// </summary>
        Rigidbody,
    }

    /// <summary>
    /// Collider behavior types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the 
    /// <a href="http://docs.unity3d.com/Manual/CollidersOverview.html">Collider Overview</a>
    /// page in the Unity Manual for details on collider behaviour under various configuations.
    /// </para>
    /// <para>
    /// Use the <c>Rigidbody.SetBehavior()</c> extension change the behavior of a collider.
    /// </para>
    /// </remarks>
    public enum ColliderBehavior
    {
        /*
         * Custom editor note:  There is a custom editor for this enumeration.  The editor will need to be updated
         * if any members are added or removed, or if the values are changed in a way that alters their 
         * rigidbody/static category. 
         */

        /// <summary>
        /// Collision detection is disabled.  (No behavior.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Collider does not exchange any collision events.</li>
        /// <li>Collider does not exchange any trigger events.</li>
        /// <li>Collider is not detectable by raycasts.</li>
        /// </ul>
        /// </para>
        /// <para>
        /// All collision detection is disabled if any of the following conditions exist:
        /// </para>
        /// <para>
        /// <ul>
        /// <li>The collider's rigidbody <c>Rigidbody.detectCollisions</c> is false. (This is a non-serializable 
        /// setting.)</li>
        /// <li>The collider is disabled.</li>
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
        /// <li>Collider exchanges collision events with colliders of all types.</li>
        /// <li>Collider exchanges trigger events with triggers of all types.</li>
        /// <li>Collider is detectable by raycasts.</li>
        /// </ul>
        /// </para>
        /// </remarks>
        RigidbodyCollider,

        /// <summary>
        /// Kinematic Rigidbody Collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Collider exchanges collision events only with rigidbody colliders. (Not with static or other
        /// kinematic colliders.)</li>
        /// <li>Collider exchanges trigger events with all trigger types.</li>
        /// <li>Collider is detectable by raycasts.</li>
        /// </ul>
        /// </remarks>
        KinematicCollider,

        /// <summary>
        /// Non-Kinematic rigidbody trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Collider exchanges no collision events.</li>
        /// <li>Collider exchanges trigger events with all collider and trigger types.</li>
        /// <li>Collider is detectable by raycasts.</li>
        /// </ul>
        /// </remarks>
        RigidbodyTrigger,

        /// <summary>
        /// Non-Kinematic rigidbody trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Collider exchanges no collision events.</li>
        /// <li>Collider exchanges trigger events with all collider and trigger types.</li>
        /// <li>Collider is detectable by raycasts.</li>
        /// </ul>
        /// </remarks>
        KinematicTrigger,

        /// <summary>
        /// Static collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Exchanges collision events only with Rigidbody colliders.  (Not with static or kinematic colliders.)</li>
        /// <li>Collider exchanges trigger events with rigidbody and dynamic triggers.  (Not with static triggers.)</li>
        /// <li>Colliders are detectable by raycasts.</li>
        /// </ul>
        /// </remarks>
        StaticCollider,

        /// <summary>
        /// Static collider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Behaviours
        /// </para>
        /// <para>
        /// <ul>
        /// <li>Colliders do not exchange collision events.</li>
        /// <li>Colliders exchange trigger events with all collider and trigger types.</li>
        /// <li>Colliders are detectable by raycasts.</li>
        /// </ul>
        /// </para>
        /// </remarks>
        StaticTrigger,
    }
}
