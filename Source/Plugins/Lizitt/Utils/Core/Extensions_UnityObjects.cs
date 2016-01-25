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
    /// <summary>
    /// Provides a variety of extension methods.
    /// </summary>
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

        #region SafeDestory

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

        /// <summary>
        /// Destroys the object in the correct way depending on whether or not the application
        /// is playing.
        /// </summary>
        /// <param name="obj">The object to destory.</param>
        public static void SafeDestroy(this Component component)
        {
            if (Application.isPlaying)
                GameObject.Destroy(component);
            else
                GameObject.DestroyImmediate(component);
        }

        #endregion

        #region GameObject

        /// <summary>
        /// Bakes the skinned mesh into a statuc mesh the delete the skinned mesh components.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Using <paramref name="source"/> is useful for baking skinned meshes in a 
        /// 'non-destructive' way that preserves animation state. The baking is always
        /// destructive to <paramref name="target"/>.  But when the source is provided, the 
        /// matching skinned meshes are found and the <paramref name="source"/> skinned meshes
        /// are baked (preserving animation state) and applied to <paramref name="target"/>. 
        /// An example process is to duplicate <paramref name="source"/>, then perform the bake
        /// on the duplicate, preserving the original.
        /// </para>
        /// </remarks>
        /// <param name="target">The game object containing the skinned mesh to bake.</param>
        /// <param name="source">
        /// The original/source of the target to bake the skinned mesh from.  (Optional)
        /// </param>
        public static void BakeSkinnedMeshes(this GameObject target, GameObject source = null)
        {
            if (source)
            {
                var outfitRenderObjects =
                    new System.Collections.Generic.Dictionary<string, GameObject>();

                // A newly instantiated mesh starts in its bind pose.  So have to bake the
                // skinned meshes from the source outfit to the target outfit.

                var origTargetName = target.name;
                target.name = source.name;  // Important since the root may have a skinned mesh.

                foreach (var renderer in target.GetComponentsInChildren<Renderer>())
                {
                    outfitRenderObjects.Add(renderer.name, renderer.gameObject);

                    if (renderer is SkinnedMeshRenderer)
                        renderer.SafeDestroy();
                }

                foreach (var renderer in source.GetComponentsInChildren<Renderer>())
                {
                    var outfitRenderObject = outfitRenderObjects[renderer.name];

                    var smr = renderer as SkinnedMeshRenderer;

                    if (smr)
                    {
                        // Bake from source.
                        var mesh = new Mesh();
                        mesh.name = smr.sharedMesh.name + LizittUtil.BakeSuffix;
                        smr.BakeMesh(mesh);

                        // Transfer to outfit.

                        outfitRenderObject.AddComponent<MeshFilter>().mesh = mesh;

                        var outfitRenderer = outfitRenderObject.AddComponent<MeshRenderer>();
                        outfitRenderer.sharedMaterials = renderer.sharedMaterials;
                        outfitRenderer.enabled = smr.enabled;
                    }
                    else
                    {
                        // There have been some cases where this is known to have occurred.
                        var outfitRenderer = outfitRenderObject.GetComponent<Renderer>();
                        outfitRenderer.enabled = renderer.enabled;
                    }
                }

                target.name = origTargetName;
            }
            else
            {
                foreach (var renderer in target.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    var mesh = new Mesh();
                    mesh.name = renderer.sharedMesh.name + LizittUtil.BakeSuffix;
                    renderer.BakeMesh(mesh);

                    var rgo = renderer.gameObject;
                    rgo.AddComponent<MeshFilter>().mesh = mesh;
                    rgo.AddComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;

                    renderer.SafeDestroy();
                }
            }
        }

        /// <summary>
        /// Safely destorys all colliders and rigidbody's attached to the object and its children.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Includes inactive components.  Can be used in both play and non-play modes.
        /// </para>
        /// </remarks>
        /// <param name="gameObject">The object to operate against.</param>
        public static void DestroyCollision(this GameObject gameObject)
        {
            foreach (var item in gameObject.GetComponentsInChildren<Rigidbody>(true))
                item.SafeDestroy();

            foreach (var item in gameObject.GetComponentsInChildren<Collider>(true))
                item.SafeDestroy();
        }

        #endregion

        #region Component

        /// <summary>
        /// Instantiates a new instance of the component's GameObject and returns the instance's 
        /// new component.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is a shortcut for the following complex command:
        /// <code>((GameObject)GameObject.Instantiate(comp.gameObject)).GetComponent[T]();</code>
        /// </para>
        /// </remarks>
        /// <param name="component">The componet that needs to be duplicated.</param>
        /// <returns>A new instance of the new component.</returns>
        public static T Instantiate<T>(this T component) where T : Component
        {
            return ((GameObject)GameObject.Instantiate(component.gameObject)).GetComponent<T>();
        }

        #endregion

        #region Transform & RigidBody

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
            // while the vertical angle is derived via projection onto the yz-plane.  So the
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

                    // HACK: Use transform to convert from local to world space.
                    trans.localPosition = position;
                    position = trans.position;

                    trans.position = original;
                }

                rigidBody.MovePosition(position);
            }
            else
                trans.MoveTo(position, space);
        }

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

        #endregion

        #region Unity.Object

        /// <summary>
        /// The clone suffix automatically appended by Unity to instantiated instances of prefabs.
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