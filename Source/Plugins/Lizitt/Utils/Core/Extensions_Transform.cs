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
         * Transform extensions.
         */

        /// <summary>
        /// Gets the unsigned angle between the transform's forward direction and a target.
        /// [Range: 0 to 180]
        /// </summary>
        /// <remarks>
        /// <para>
        /// WARNING: The result is undefined if the transforms are colocated or the derived
        /// target direction is effectively zero.  (E.g. If <paramref name="ignoreHeight"/> is
        /// true and colocated on the xz-plane.)
        /// </para>
        /// </remarks>
        /// <param name="transform">The reference transform.</param>
        /// <param name="target">The target transform.</param>
        /// <param name="ignoreHeight">If true, ignore the y-axis difference between the
        /// two transforms.</param>
        /// <returns>
        /// The unsigned angle between the transform's forward and the target.
        /// [Range: 0 to 180]</returns>
        public static float AngleToTarget(
            this Transform transform, Transform target, bool ignoreHeight = false)
        {
            var targetPos = target.position;

            if (ignoreHeight)
                targetPos.y = transform.position.y;

            return Vector3.Angle(targetPos - transform.position, transform.forward);
        }

        /// <summary>
        /// Gets the signed local-space horizontal (x) and vertical (y) angles from the reference 
        /// to the target direction. (Degrees) [Limits: -180 to 180, both axes]
        /// </summary>
        /// <para>
        /// The angles represent the right/left and up/down angles necessary to aim 
        /// <paramref name="reference"/> toward <see cref="targetDirection"/>.  This is similar to
        /// yaw and pitch.  The angles are useful in situations where local-space 2D angles are
        /// needed. E.g. For animation aim blending.
        /// </para>
        /// <para>
        /// The horzontal angle (x) represents the angle around the local y-axis from 
        /// <paramref name="reference"/>'s forward direction to <see cref="targetDirection"/>.  
        /// Negative values represent a right 'turn' while positive values prepresent left.
        /// </para>
        /// <para>
        /// The vertical angle (y) represents the angle around the local x-axis from
        /// <paramref name="reference"/>'s forward direction to <see cref="targetDirection"/>.
        /// Negative values represent a pitch upward while positive values prepresent downward.
        /// (Note: This value often needs to be negated for use in animation blend
        /// graphs since they often define positive angles as up.)
        /// </para>
        /// <para>
        /// Warning: The result is undefined if the normalized direction vector from
        /// <paramref name="reference"/> to <paramref name="targetDirection"/> 
        /// is either Vector3.up or Vector3.down.  This is a limitation of the math.
        /// (There is no such thing as a horizontal angle when aiming straight up/down.)
        /// </para>
        /// <para>
        /// The angles can be converted into a local-space direction as follows.
        /// (Useful for debug display purposes):
        /// </para>
        /// <para>
        /// <code>
        /// var rot = Quaternion.Euler(new Vector3(angle.y, angle.x, 0));
        /// dir = reference.TransformDirection(rot * Vector3.forward);
        /// </code>
        /// </para>
        /// </remarks>
        /// <param name="reference">The tranform that represents the reference forward direction.
        /// </param>
        /// <param name="targetDirection">The world-space aim direction. </param>
        /// <returns>
        /// The signed local-space horizontal (x) and vertical (y) angles from the reference 
        /// forward to the target direction. (Degrees) [Limits: -180 to 180, both axes]
        /// </returns>
        public static Vector2 AimAngles(this Transform reference, Vector3 targetDirection)
        {
            var fwd = Vector3.forward;
            var dir = reference.InverseTransformDirection(targetDirection);
            dir.y = 0;

            var horizAngle = Vector3.Angle(fwd, dir) * (dir.x > 0 ? 1 : -1);

            // These next steps remove the influence of the horizontal angle from the shared
            // z-value. (The horizontal angle is derived via projection onto the xz-plane 
            // while the vertical angle is derived via projection onto the yz-plane, so the
            // z-value contains shared information.) 

            // This matix operation is equivalent to InvertTransformDirection.

            var matrix = Matrix4x4.TRS(
                Vector3.zero,
                reference.rotation * Quaternion.Euler(new Vector3(0, horizAngle, 0)),
                Vector3.one).inverse;

            dir = matrix.MultiplyVector(targetDirection);
            dir.x = 0;

            var vertAngle = Vector3.Angle(fwd, dir) * (dir.y > 0 ? -1 : 1);

            return new Vector2(horizAngle, vertAngle);
        }

        /// <summary>
        /// Applies the position to the transform based on the provided settings.
        /// </summary>
        /// <param name="transform">The transform to move.</param>
        /// <param name="position">The position to move to.</param>
        /// <param name="space">The space to move in.</param>
        public static void MoveTo(
            this Transform transform, Vector3 position, Space space = Space.World)
        {
            if (space == Space.Self)
                transform.localPosition = position;
            else
                transform.position = position;
        }

        /// <summary>
        /// Applies the position to the transform, choosing the most appropriate method.
        /// (Rigidbody aware.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Checks for the existance of a rigidbody and decides on the most appropriate method 
        /// of movement.
        /// </para>
        /// </remarks>
        /// <param name="transform">The transform to move.</param>
        /// <param name="position">The position to move to.</param>
        /// <param name="space">The space to move in.</param>
        public static void MoveToSafe(
            this Transform transform, Vector3 position, Space space = Space.World)
        {
            var rb = transform.GetComponent<Rigidbody>();

            if (rb)
                rb.MoveTo(position, space);
            else
                transform.MoveTo(position, space);
        }

        /// <summary>
        /// Gets the nearest shared parent, or null if there is none.  (Lowest common ancestor.)
        /// </summary>
        /// <param name="transform">Transform A (Required.)</param>
        /// <param name="other">Transform B (Required.)</param>
        /// <returns>Gets the nearest shared parent, or null if there is none.</returns>
        public static Transform GetSharedParent(this Transform transform, Transform other)
        {
            if (!(transform && other))
            {
                Debug.LogError("One or both transforms are null.  A null transform can't have a parent.");
                return null;
            }

            if (transform == other)
            {
                Debug.LogWarning("The two transforms are the same object.  Always same shared parent.", transform);
                return transform.parent;
            }

            if (transform.IsChildOf(other) || other.IsChildOf(transform))
            {
                // Technically, the question is answered, so only a warning.
                Debug.LogWarning("One of the transforms is a parent of the other.  Can't share a parent.", transform);
                return null;
            }

            transform = transform.parent;
            while (transform)
            {
                if (other.IsChildOf(transform))
                    return transform;

                transform = transform.parent;
            }

            return transform;
        }
    }
}