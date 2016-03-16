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
         * GameObject extensions.
         */

        /// <summary>
        /// Destroys the object in the correct way depending on whether or not the application
        /// is playing.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        public static void SafeDestroy(this GameObject obj)
        {
            if (Application.isPlaying)
                GameObject.Destroy(obj);
            else
                GameObject.DestroyImmediate(obj);
        }

        /// <summary>
        /// Bakes all skinned meshes into a static meshes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Using <paramref name="source"/> is useful for baking skinned meshes in a 
        /// 'non-destructive' way that preserves animation state. The baking is always
        /// destructive to <paramref name="target"/>.  But when the source is provided, the 
        /// matching skinned meshes are found and the <paramref name="source"/> skinned meshes
        /// are baked (preserving animation state) and applied to <paramref name="target"/>. 
        /// An example used case is to duplicate <paramref name="source"/>, then perform 
        /// the bake on the duplicate, preserving the original.
        /// </para>
        /// </remarks>
        /// <param name="target">
        /// The game object containing the skinned meshes to bake. (Required)
        /// </param>
        /// <param name="source">
        /// The original/source to bake the skinned meshes from.  (Optional)
        /// </param>
        public static void BakeSkinnedMeshes(this GameObject target, GameObject source = null)
        {
            if (source)
            {
                var outfitRenderObjects =
                    new System.Collections.Generic.Dictionary<string, GameObject>();

                // A newly instantiated mesh starts in its bind pose, so have to bake the
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
                        mesh.name = smr.sharedMesh.name + LizUtil.BakeSuffix;
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
                    mesh.name = renderer.sharedMesh.name + LizUtil.BakeSuffix;
                    renderer.BakeMesh(mesh);

                    var rgo = renderer.gameObject;
                    rgo.AddComponent<MeshFilter>().mesh = mesh;
                    rgo.AddComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;

                    renderer.SafeDestroy();
                }
            }
        }

        /// <summary>
        /// Safely destroys all colliders and rigidbody's attached to the object and its children.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Includes inactive components.  Can be used in both play and non-play modes.
        /// </para>
        /// </remarks>
        /// <param name="gameObject">The object to operate against.</param>
        public static void DestroyPhysicsComponents(this GameObject gameObject, bool includeInactive)
        {
            foreach (var item in gameObject.GetComponentsInChildren<Rigidbody>(includeInactive))
                item.SafeDestroy();

            foreach (var item in gameObject.GetComponentsInChildren<Collider>(includeInactive))
                item.SafeDestroy();
        }
    }
}