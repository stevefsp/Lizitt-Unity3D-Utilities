/*
 * Copyright (c) 2015 Stephen A. Pratt
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
using System.Linq;

namespace com.lizitt.u3d
{
    /// <summary>
    /// Provides a variety of extension methods.
    /// </summary>
    public static class Extensions
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

        #region GameObject

        /// <summary>
        /// Destroys the object in the correct way depending on whether or not the application
        /// is playing.
        /// </summary>
        /// <param name="obj">The object to destory.</param>
        public static void SafeDestroy(this GameObject obj)
        {
            if (Application.isPlaying)
                GameObject.Destroy(obj);
            else
                GameObject.DestroyImmediate(obj);
        }

        #endregion

        #region Component

        /// <summary>
        /// Destroys the object in the correct way depending on whether or not the application
        /// is playing.
        /// </summary>
        /// <param name="obj">The object to destory.</param>
        public static void SafeDestroy(this Component comp)
        {
            if (Application.isPlaying)
                GameObject.Destroy(comp);
            else
                GameObject.DestroyImmediate(comp);
        }

        /// <summary>
        /// Instantiates a new version of the component's GameObject and returns its new
        /// component.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is a shortcut for the following complex command:
        /// <code>((GameObject)GameObject.Instantiate(comp.gameObject)).GetComponent();</code>
        /// </para>
        /// </remarks>
        /// <param name="comp">The componet that needs to be duplicated.</param>
        /// <returns>A new instance of the component.</returns>
        public static T Instantiate<T>(this T comp) where T : Component
        {
            return ((GameObject)GameObject.Instantiate(comp.gameObject)).GetComponent<T>();
        }

        #endregion

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
        /// Randomizes the contents of the array.
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
        /// Randomizes the contents of the list.
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

        # region Math and Geometry (Including Vectors)

        /// <summary>
        /// Gets the XZ-plane distance between the specified points.
        /// </summary>
        /// <param name="u">Vector u.</param>
        /// <param name="v">Vector v.</param>
        /// <returns>The distance between the specified points on the xz-plane.</returns>
        public static float DistanceXZ(this Vector3 u, Vector3 v)
        {
            float dx = v.x - u.x;
            float dz = v.z - u.z;
            return (float)System.Math.Sqrt(dx * dx + dz * dz);
        }

        /// <summary>
        /// Determines whether or not the two points are within range of each other based on a 
        /// xz-plane radius and a y-axis height.
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
        /// tolerance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Equality is based on a range check.  Is u within tolerance distance of v?
        /// This type of check is valid for either position or direction vectors.
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
        /// Determines whether or not the specified vectors are equal within the specified tolerance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Equality is based on a range check.  Is u within tolerance distance of v?
        /// This type of check is valid for either position or direction vectors.
        /// </para>
        /// </remarks>
        /// <param name="u">Vector u.</param>
        /// <param name="v">Vector v</param>
        /// <param name="tolerance">The allowed tolerance. [Limit: >= 0]</param>
        /// <returns>
        /// True if the specified vectors are similar enough to be considered equal.
        /// </returns>
        public static bool SloppyEquals2D(this Vector2 u, Vector2 v, float tolerance = MathUtil.Tolerance)
        {
            // The name is odd because Unity considers 'SloppyEquals' and the Vector3 version
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
        /// them, relative to the reference direction, if determined.
        /// </para>
        /// <para>
        /// WARNING: The result of this algorithm is undefined if either of the direction vectors 
        /// has no significant xz-plane projection.  (I.e. The Vector3.up or Vector3.down.)
        /// </para>
        /// </remarks>
        /// <param name="fromDirection">The reference direction.</param>
        /// <param name="toDirection">The target direction.</param>
        /// <returns>The signed y-axis angle. [Range: -180 to 180]</returns>
        public static float SignedAngleY(this Vector3 fromDirection, Vector3 toDirection)
        {
            // Get a numeric angle for each vector, on the X-Z plane, relative to world forward.
            float angleA = Mathf.Atan2(fromDirection.x, fromDirection.z) * Mathf.Rad2Deg;
            float angleB = Mathf.Atan2(toDirection.x, toDirection.z) * Mathf.Rad2Deg;

            return Mathf.DeltaAngle(angleA, angleB);

            // TODO: Check performance against this other algorithm. (Not unit tested.)

            //if (toDirection == Vector3.zero)
            //    return 0;

            //float angle = Vector3.Angle(fromDirection, toDirection);
            //Vector3 normal = Vector3.Cross(fromDirection, toDirection);
            //angle = Mathf.Sign(Vector3.Dot(normal, Vector3.up));

            //return angle;
        }

        /// <summary>
        /// Get the signed angle around the y-axis between the two quaternions where up 
        /// is world up.  [Range: -180 to 180]
        /// </summary>
        /// <remarks>
        /// <para>
        /// WARNING: The result of this algorithm is undefined if either of the rotations has 
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

        #region Unused (Restore and test when needed.)

        // Can restore these when needed.  Had unit tests as some point, but not in the current
        // test suite.  (Check old CAINav.)

        ///// <summary>
        ///// Creates an array of vectors from a flattend array of vectors.
        ///// </summary>
        ///// <param name="flatVectors">
        ///// An array of vectors. [(x, y, z) * vectorCount]
        ///// </param>
        ///// <returns>An array of vectors.</returns>
        //public static Vector3[] GetVectors(this float[] flatVectors)
        //{
        //    int count = flatVectors.Length / 3;
        //    Vector3[] result = new Vector3[count];

        //    for (int i = 0; i < count; i++)
        //    {
        //        int p = i * 3;
        //        result[i] = new Vector3(flatVectors[p + 0]
        //            , flatVectors[p + 1]
        //            , flatVectors[p + 2]);
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Flattens the vector array into a float array in the form (x, y, z) * count.
        ///// </summary>
        ///// <param name="vectors">
        ///// An array of vectors.
        ///// </param>
        ///// <param name="count">
        ///// The number of vectors. [Limit: vectors.Length >= count]
        ///// </param>
        ///// <returns>An array of flattened vectors.</returns>
        //public static float[] Flatten(this Vector3[] vectors, int count)
        //{
        //    float[] result = new float[count * 3];

        //    for (int i = 0; i < count; i++)
        //    {
        //        result[i * 3] = vectors[i].x;
        //        result[i * 3 + 1] = vectors[i].y;
        //        result[i * 3 + 2] = vectors[i].z;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Copies a range of vectors from a flattend array to a vector array.
        ///// </summary>
        ///// <remarks>
        ///// <para>
        ///// If a target buffer is provided is must be large enough to contain the results.
        ///// </para>
        ///// <para>
        ///// If the target buffer is null a new array will be created with a length of
        ///// <paramref name="count"/> + <paramref name="targetIndex"/>
        ///// </para>
        ///// </remarks>
        ///// <param name="source">
        ///// An array of vectors in the form (x, y, z) * vectorCount.
        ///// </param>
        ///// <param name="sourceIndex">
        ///// The index of the start vector in the source array.
        ///// </param>
        ///// <param name="target">
        ///// The target vector buffer. (Or null.)
        ///// </param>
        ///// <param name="targetIndex">
        ///// The index within the target buffer to start the copy.
        ///// </param>
        ///// <param name="count">
        ///// The number of vectors to copy.
        ///// </param>
        ///// <returns>
        ///// The reference to the <paramref name="target"/> buffer, or a new array if no buffer 
        ///// was provided.
        ///// </returns>
        //public static Vector3[] Flatten(this float[] source , int sourceIndex
        //    , Vector3[] target, int targetIndex, int count)
        //{
        //    if (target == null)
        //        target = new Vector3[count + targetIndex];

        //    for (int i = 0; i < count; i++)
        //    {
        //        int p = (sourceIndex + i) * 3;
        //        target[targetIndex + i] = new Vector3(source[p + 0], source[p + 1], source[p + 2]);
        //    }

        //    return target;
        //}

        ///// <summary>
        ///// Gets the minimum and maximum bounds of the AABB that contains the array of points.
        ///// </summary>
        ///// <param name="vectors">
        ///// An array of points.
        ///// </param>
        ///// <param name="count">
        ///// The number of points in the array. [Limit: vectors.Length >= count]
        ///// </param>
        ///// <param name="boundsMin">
        ///// The mimimum bounds of the AABB.
        ///// </param>
        ///// <param name="boundsMax">
        ///// The maximum bounds of the AABB.
        ///// </param>
        //public static void GetBounds(this Vector3[] vectors
        //    , int count
        //    , out Vector3 boundsMin
        //    , out Vector3 boundsMax)
        //{
        //    boundsMin = vectors[0];
        //    boundsMax = vectors[0];

        //    for (int i = 1; i < count; i++)
        //    {
        //        boundsMin.x = System.Math.Min(boundsMin.x, vectors[i].x);
        //        boundsMin.y = System.Math.Min(boundsMin.y, vectors[i].y);
        //        boundsMin.z = System.Math.Min(boundsMin.z, vectors[i].z);
        //        boundsMax.x = System.Math.Max(boundsMax.x, vectors[i].x);
        //        boundsMax.y = System.Math.Max(boundsMax.y, vectors[i].y);
        //        boundsMax.z = System.Math.Max(boundsMax.z, vectors[i].z);
        //    }
        //}

        ///// <summary>
        ///// Translates point A toward point B by the specified factor of the distance between them.
        ///// </summary>
        ///// <remarks>
        ///// <para>
        ///// Examples:
        ///// </para>
        ///// <para>
        ///// If the factor is 0.0, then the result will equal A.<br/>
        ///// If the factor is 0.5, then the result will be the midpoint between A and B.<br/>
        ///// If the factor is 1.0, then the result will equal B.<br/>
        ///// </para>
        ///// </remarks>
        ///// <param name="a">Point A.</param>
        ///// <param name="b">Point B.</param>
        ///// <param name="factor">
        ///// The factor that governs the distance the point is translated from A toward B.
        ///// </param>
        ///// <returns>The point translated toward point B from point A.</returns>
        //public static Vector3 TranslateToward(Vector3 a, Vector3 b, float factor)
        //{
        //    return new Vector3(a.x + (b.x - a.x) * factor
        //        , a.y + (b.y - a.y) * factor
        //        , a.z + (b.z - a.z) * factor);
        //}

        #endregion

        #endregion

        #region Transform & RigidBody

        /// <summary>
        /// Gets the unsigned angle between the transform's forward and a target.
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
        /// Applies the position apropriately based on the state of the rigid body.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Takes into account whether the rigidbody is kinematic or not.
        /// </para>
        /// </remarks>
        /// <param name="rigidBody">The rigidbody to move.</param>
        /// <param name="position">The position to move to.</param>
        /// <param name="space">The space to move in.</param>
        public static void MoveTo(
            this Rigidbody rigidBody, Vector3 position, Space space = Space.World)
        {
            var trans = rigidBody.transform;

            if (!rigidBody.isKinematic)
            {
                if (space == Space.Self)
                {
                    var original = trans.position;

                    // Hack time!  Using transform to convert from local 
                    // to world space.
                    trans.localPosition = position;
                    position = trans.position;

                    trans.position = original;
                }

                rigidBody.MovePosition(position);
            }
            else
                trans.MoveTo(position, space);
        }

        #endregion

        #region Unity.Object

        /// <summary>
        /// The clone suffix automatically appended to instantiated instances of prefabs.
        /// </summary>
        public const string CloneSuffix = "(Clone)";

        /// <summary>
        /// Strips all instances of "(Clone)" from the object name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: "FooThing(Clone)(Clone)" will become "FooThing".
        /// </para>
        /// </remarks>
        /// <param name="obj">
        /// The object to strip.
        /// </param>
        public static void StripCloneName(this Object obj)
        {
            obj.name = obj.name.Replace(CloneSuffix, "");
        }

        /// <summary>
        /// Replaces all instances of "(Clone)" in the object name with the value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example: If value is "_Baked", then "FooThing(Clone)(Clone)" will become 
        /// "FooThing_Baked_Baked".
        /// </para>
        /// </remarks>
        /// <param name="obj">
        /// The object to perform the replace operation on.
        /// </param>
        /// <param name="value">
        /// The value to replace the clone suffix with.
        /// </param>
        public static void ReplaceCloneName(this Object obj, string value)
        {
            obj.name = obj.name.Replace(CloneSuffix, value);
        }

        #endregion
    }
}