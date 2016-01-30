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
    /// Common collider status values.
    /// </summary>
    public enum ColliderStatus
    {
        /// <summary>
        /// Not a collider, or collision detection disabled.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges no collision or trigger events with any other objects.
        /// </para>
        /// <para>
        /// Not detectable by raycast.
        /// </para>
        /// </remarks>
        Disabled = 0,

        /// <summary>
        /// Non-Kinematic Rigidbody Collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges collision events with all collider types and trigger events with all 
        /// trigger types.
        /// </para>
        /// <para>
        /// Detectable by raycasts.
        /// </para>
        /// </remarks>
        RigidbodyCollider,

        /// <summary>
        /// Kinematic Rigidbody Collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges collision events with rigidbody colliders, and trigger events with all 
        /// trigger types.
        /// </para>
        /// <para>
        /// Exchanges no events of any type with static colliders or other kinematic rigidbody 
        /// objects.
        /// </para>
        /// <para>
        /// Detectable by raycasts.
        /// </para>
        /// </remarks>
        KinematicCollider,

        /// <summary>
        /// Non-Kinematic Rigidbody Trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges trigger events with all collider and trigger types.
        /// </para>
        /// <para>
        /// Exchanges no collision events with any collider or trigger type.
        /// </para>
        /// <para>
        /// Detectable by raycasts.
        /// </para>
        /// <para>
        /// Triggers of this type may need to have their Rigidbody.useGravity set to false.
        /// Otherwise the trigger may simply fall downward forever.
        /// </para>
        /// </remarks>
        RigidbodyTrigger,

        /// <summary>
        /// Kinematic Rigidbody Trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges trigger events with all collider and trigger types.
        /// </para>
        /// <para>
        /// Exchanges no collision events with any collider or trigger type.
        /// </para>
        /// <para>
        /// Detectable by raycasts.
        /// </para>
        /// </remarks>
        KinematicTrigger,

        /// <summary>
        /// Static Collider
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges collision events with Rigidbody colliders, and trigger events with
        /// Kinematic and Rigidbody triggers.
        /// </para>
        /// <para>
        /// Does not exchange any events with Kinematic colliders, other static colliders, or
        /// static triggers.
        /// </para>
        /// <para>
        /// Not detectable by raycast.
        /// </para>
        /// </remarks>
        StaticCollider,

        /// <summary>
        /// Static Trigger
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exchanges trigger events with all types except static colliders and other static
        /// triggers.
        /// </para>
        /// <para>
        /// Does not exchange any collider events.
        /// </para>
        /// <para>
        /// Not detectable by raycast.
        /// </para>
        /// </remarks>
        StaticTrigger,
    }
}
