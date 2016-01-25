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
using System.Collections.Generic;
using System.Linq;
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

        #region Randomize

        /// <summary>
        /// Creates an array containing the randomized content of the enumerable object.
        /// </summary>
        /// <param name="items">
        /// The enumerable object that provides the items to be randomized.
        /// </param>
        /// <returns>
        /// An array containing the randomized content of the enumerable object.
        /// </returns>
        public static T[] CreateRandomized<T>(this IEnumerable<T> items)
        {
            var result = items.ToArray<T>();

            int n = result.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = result[k];
                result[k] = result[n];
                result[n] = value;
            }

            return result;
        }

        /// <summary>
        /// Randomizes the contents of the array.   (In-place randomiziaton.)
        /// </summary>
        /// <param name="items">The array to randomize.</param>
        public static void Randomize<T>(this T[] items)
        {
            int n = items.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = items[k];
                items[k] = items[n];
                items[n] = value;
            }
        }

        /// <summary>
        /// Randomizes the contents of the list.  (In-place randomiziaton.)
        /// </summary>
        /// <param name="items">The list to randomize.</param>
        public static void Randomize<T>(this List<T> items)
        {
            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = items[k];
                items[k] = items[n];
                items[n] = value;
            }
        }

        #endregion

        # region Math and Geometry (Including Vectors & Quaternions)

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
            return (float)System.Math.Sqrt(dx * dx + dz * dz);
        }

        /// <summary>
        /// Determines whether or not the two points are within range of each other based on 
        /// xz-plane radius and a y-axis height.  (A cylindrical range check.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Essentially, one point defines the centroid of a cylinder and the other is 
        /// tested for inclusion.  The height test is <c>(Math.Abs(deltaY) &lt; height)</c>
        /// </para>
        /// <para>
        /// See <see cref="SloppyEquals(Vector3, Vector3, float)"/> for a more traditional
        /// spherical test.
        /// </para>
        /// </remarks>
        /// <param name="a">Point A.</param>
        /// <param name="b">Point B.</param>
        /// <param name="radius">The allowed radius on the xz-plane.</param>
        /// <param name="height">The allowed y-axis delta.</param>
        /// <returns>
        /// True if the two vectors are within the xz-radius and y-height of each other.
        /// </returns>
        public static bool IsInRange(this Vector3 a, Vector3 b, float radius, float height)
        {
            Vector3 d = b - a;
            return (d.x * d.x + d.z * d.z) < radius * radius
                && System.Math.Abs(d.y) < height;
        }

        /// <summary>
        /// Determines whether or not the specified vectors are equal within the specified 
        /// tolerance. (A sphere range check.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Equality is based on a range check.  Is u within tolerance distance of v?
        /// This type of check is valid for both position and direction vectors.
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
        /// Determines whether or not the specified vectors are equal within the specified 
        /// tolerance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Equality is based on a range check.  Is u within tolerance distance of v?
        /// This type of check is valid for both position and direction vectors.
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
        /// Gets the signed angle around the y-axis between the two direction vectors where up 
        /// is world up.  [Range: -180 to 180]
        /// </summary>
        /// <remarks>
        /// <para>
        /// Basically, the direction vectors are projected onto the xz-plane and the angle between
        /// them, relative to the reference direction, is determined.
        /// </para>
        /// <para>
        ///  Because this algorithm has no concept of a local up-axis it is not suitable 
        /// for some use cases. See <see cref="AimAngles"/> if a local-space angle is required.
        /// </para>
        /// <para>
        /// Warning: The result of this algorithm is undefined if either of the direction vectors 
        /// has no significant xz-plane projection.  (I.e. The Vector3.up or Vector3.down.)
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

        /// <summary>
        /// Get the signed angle around the y-axis between the two quaternions where up 
        /// is world up.  [Range: -180 to 180]
        /// </summary>
        /// <remarks>
        /// <para>
        /// Because this algorithm ignores the quaterion up-axis it is not suitable for some
        /// use cases.  If a local-space angle is required, then see <see cref="AimAngles"/>.
        /// </para>
        /// <para>
        /// Warning: The result of this algorithm is undefined if either of the rotations has 
        /// no xz-plane component.  (I.e. The look direction is Vector3.up or Vector3.down.)
        /// </para>
        /// </remarks>
        /// <param name="from">The reference rotation.</param>
        /// <param name="to">The target rotation.</param>
        /// <returns>The signed y-axis angle. [Range: -180 to 180]</returns>
        public static float SignedAngleY(this Quaternion from, Quaternion to)
        {
            // Get the forward vector for each rotation
            Vector3 forwardA = from * Vector3.forward;
            Vector3 forwardB = to * Vector3.forward;

            // Get a numeric angle for each vector, on the X-Z plane, relative to world forward.
            float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
            float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;
            
            return Mathf.DeltaAngle(angleA, angleB);
        }

        #endregion

        #region Array / List

        /// <summary>
        /// Compresses an array by removing all null values.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only valid for use with arrays of reference types.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="items">The array to compress.</param>
        /// <returns>
        /// A new array if compression was needed, or the original array if no compression
        /// was needed.
        /// </returns>
        public static T[] Compress<T>(this T[] items)
            where T : class
        {
            if (items == null)
                return null;

            if (items.Length == 0)
                return items;

            int count = 0;

            foreach (T item in items)
                count += (item == null) ? 0 : 1;

            if (count == items.Length)
                return items;

            var result = new T[count];

            if (count == 0)
                return result;

            count = 0;
            foreach (T item in items)
            {
                if (item != null)
                    result[count++] = item;
            }

            return result;
        }

        /// <summary>
        /// Removes all null or destoryed Unity objects from the list.
        /// </summary>
        /// <param name="items">The list of objects to operate on.</param>
        public static void PurgeNulls<T>(this List<T> items)
            where T : UnityEngine.Object
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (!items[i])
                    items.RemoveAt(i);
            }
        }

        #endregion
    }
}