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
         * Vector extensions.
         */

        /// <summary>
        /// Gets the distance between the specified points when projected onto the xz-place.
        /// </summary>
        /// <param name="u">Vector u.</param>
        /// <param name="v">Vector v.</param>
        /// <returns>
        /// The distance between the specified points when projected onto the xz-place.
        /// </returns>
        public static float DistanceXZ(this Vector3 u, Vector3 v)
        {
            float dx = v.x - u.x;
            float dz = v.z - u.z;
            return Mathf.Sqrt(dx * dx + dz * dz);
        }

        /// <summary>
        /// Determines whether or not the two points are within range of each other based on  xz-plane radius and 
        /// a y-axis height.  (A cylindrical range check.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Essentially, one point defines the centroid of a cylinder and the other is tested for inclusion.  
        /// The height test is <c>(Math.Abs(deltaY) &lt; height)</c>
        /// </para>
        /// <para>
        /// See <see cref="SloppyEquals(Vector3, Vector3, float)"/> for a more traditional
        /// spherical test.
        /// </para>
        /// </remarks>
        /// <param name="u">Point A.</param>
        /// <param name="v">Point B.</param>
        /// <param name="radius">The allowed radius on the xz-plane.</param>
        /// <param name="height">The allowed y-axis delta.</param>
        /// <returns>
        /// True if the two vectors are within the xz-radius and y-height of each other.
        /// </returns>
        public static bool IsInRange(this Vector3 u, Vector3 v, float radius, float height)
        {
            Vector3 d = v - u;
            return (d.x * d.x + d.z * d.z) < radius * radius
                && Mathf.Abs(d.y) <= height;
        }

        /// <summary>
        /// Determines whether or not the specified vectors are equal within the specified tolerance. 
        /// (A sphere range check.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Equality is based on a range check.  Is u within tolerance distance of v? This type of check is valid
        /// for both position and direction vectors.
        /// </para>
        /// </remarks>
        /// <param name="u">Vector u.</param>
        /// <param name="v">Vector v.</param>
        /// <param name="tolerance">The allowed tolerance. [Limit: >= 0]</param>
        /// <returns>
        /// True if the specified vectors are close enough to be considered equal.
        /// </returns>
        public static bool SloppyEquals(this Vector3 u, Vector3 v, float tolerance = MathUtil.Tolerance)
        {
            Vector3 d = u - v;
            return (d.x * d.x + d.y * d.y + d.z * d.z) <= tolerance * tolerance;
        }

        /// <summary>
        /// Determines whether or not the specified vectors that represent euler angles are equal within the specified
        /// tolerance.  (Each angle within tolerance.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Comparing the equality of euler angles is complicated by the fact that angles are cyclic. (0 == 360 == 720)
        /// This method takes this into account. Each angle is compared against the tolerance.  If any angle check 
        /// fails, the check fails.
        /// </para>
        /// </remarks>
        /// <param name="eulerAnglesA">Euler angles u. (Degrees)</param>
        /// <param name="eulerAnglesB">Euler angles v. (Degrees)</param>
        /// <param name="tolerance">The allowed tolerance. [Limit: >= 0] (Degrees)</param>
        /// <returns>True if the euler angles are close enough to be considered equal.</returns>
        public static bool SloppyEqualsAngles(
            this Vector3 eulerAnglesA, Vector3 eulerAnglesB, float tolerance = MathUtil.Tolerance)
        {
            eulerAnglesA = Quaternion.Euler(eulerAnglesA).eulerAngles;
            eulerAnglesB = Quaternion.Euler(eulerAnglesB).eulerAngles;

            return !((Mathf.Abs(eulerAnglesA.x - eulerAnglesB.x) > tolerance)
                || (Mathf.Abs(eulerAnglesA.y - eulerAnglesB.y) > tolerance)
                || (Mathf.Abs(eulerAnglesA.z - eulerAnglesB.z) > tolerance));
        }

        /// <summary>
        /// Updates the euler angles so they are all within the standard quaternion range. (0 &lt;= value &lt; 360)
        /// </summary>
        /// <remarks>
        /// <para>
        /// In this context, 'standard' means the angle range returned by Quaterion.eulerAngles.
        /// </para>
        /// </remarks>
        /// <param name="eulerAngles">The euler angles. (Degrees)</param>
        /// <returns>The euler angles in the standard quaternion range. (0 &lt;= value &lt; 360)</returns>
        public static Vector3 StandardizeAngles(this Vector3 eulerAngles)
        {
            return Quaternion.Euler(eulerAngles).eulerAngles;
        }

        /// <summary>
        /// Determines whether or not the specified vectors are equal within the specified tolerance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Equality is based on a range check.  Is u within tolerance distance of v? This type of check is valid
        /// for both position and direction vectors.
        /// </para>
        /// </remarks>
        /// <param name="u">Vector u.</param>
        /// <param name="v">Vector v</param>
        /// <param name="tolerance">The allowed tolerance. [Limit: >= 0]</param>
        /// <returns>
        /// True if the specified vectors are similar enough to be considered equal.
        /// </returns>
        public static bool SloppyEquals2D(
            this Vector2 u, Vector2 v, float tolerance = MathUtil.Tolerance)
        {
            // The name is odd because Unity considers 'SloppyEquals' for the Vector3 version
            // to be ambiguous.  Possibly a Mono thing since Visual Studio doesn't complain.

            float dx = u.x - v.x;
            float dy = u.y - v.y;
            return (dx * dx + dy * dy) <= tolerance * tolerance;
        }

        /// <summary>
        /// Gets the signed angle around the y-axis between the two direction vectors where up is world up.  
        /// [Range: -180 to 180]
        /// </summary>
        /// <remarks>
        /// <para>
        /// Basically, the direction vectors are projected onto the xz-plane and the angle between them, relative 
        /// to the reference direction, is determined.
        /// </para>
        /// <para>
        /// Because this algorithm has no concept of a local up-axis it is not suitable for some use cases. 
        /// See <see cref="AimAngles"/> if a local-space angle is required.
        /// </para>
        /// <para>
        /// Warning: The result of this algorithm is undefined if either of the direction vectors has no significant
        /// xz-plane projection.  (I.e. The Vector3.up or Vector3.down.)
        /// </para>
        /// </remarks>
        /// <param name="fromDirection">The reference direction.</param>
        /// <param name="targetDirection">The target direction.</param>
        /// <returns>The signed y-axis angle. [Range: -180 to 180]</returns>
        public static float SignedAngleY(this Vector3 fromDirection, Vector3 toDirection)
        {
            // Get a numeric angle for each vector, on the X-Z plane, relative to world forward.
            float angleA = Mathf.Atan2(fromDirection.x, fromDirection.z) * Mathf.Rad2Deg;
            float angleB = Mathf.Atan2(toDirection.x, toDirection.z) * Mathf.Rad2Deg;

            return Mathf.DeltaAngle(angleA, angleB);
        }
    }
}