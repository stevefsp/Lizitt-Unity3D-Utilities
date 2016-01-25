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
    /// Represents a pointer to a specific renderer material index.  
    /// (A renderer/material index pair.)
    /// </summary>
    [System.Serializable]
    public class RendererMaterialPtr
    {
        [SerializeField]
        [Tooltip("The renderer the material index applies to, or null for 'unspecified'.")]
        private Renderer m_Renderer = null;

        [SerializeField]
        [Tooltip("The index of the material in the render's material array, or -1 for"
            + " 'unspecified'.")]
        private int m_MaterialIndex = -1;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The constructor will accept a null renderer to allow for flexibility.  If
        /// <paramref name="renderer "/>is null, <paramref name="materialIndex"/> will 
        /// be forced to -1.
        /// </para>
        /// </remarks>
        /// <param name="renderer">The target renderer.</param>
        /// <param name="materialIndex">
        /// The index of the material in the renderer's material array, or -1 for 'undefined'.
        /// </param>
        public RendererMaterialPtr(Renderer renderer = null, int materialIndex = -1)
        {
            Set(renderer, materialIndex);
        }

        /// <summary>
        /// True if the pointer is fully defined. (Points to a material.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Because the number of materials in a render is not expected to change, and the cost
        /// of getting a material count, the material index is only validated when it is set.
        /// </para>
        /// </remarks>
        public bool IsDefined
        {
            get { return m_Renderer && m_MaterialIndex >= 0; }
        }

        /// <summary>
        /// The renderer <see cref="MaterialIndex"/> applies to.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A value of null means 'undefined.'
        /// </para>
        /// </remarks>
        public Renderer Renderer
        {
            get { return m_Renderer; }
        }

        /// <summary>
        /// The index of the material in the render's material array, or -1 for 'undefined'.
        /// </summary>
        /// <para>
        /// <para>
        /// Because the number of materials in a render is not expected to change, and the cost
        /// of getting a material count, the material index is only validated when it is set.
        /// </para>
        /// </para>
        public int MaterialIndex
        {
            get { return m_MaterialIndex; }
        }

        /// <summary>
        /// The shared material assigned to the pointer's index, or null if the pointer is 
        /// 'undefined'.
        /// </summary>
        public Material SharedMaterial
        {
            get
            {
                if (IsDefined)
                    return m_Renderer.sharedMaterials[m_MaterialIndex];

                return null;
            }
        }

        /// <summary>
        /// Sets the renderer and material index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <paramref name="renderer"/> value of null represents an 'undefined' state and will
        /// force <paramref name="materialIndex"/> to -1.
        /// </para>
        /// </remarks>
        /// <param name="renderer">
        /// The renderer the material index applies to, or null for 'undefined'.
        /// </param>
        /// <param name="materialIndex">
        /// The index of the material in the render's material array, or -1 for 'undefined'.
        /// </param>
        public void Set(Renderer renderer, int materialIndex)
        {
            if (!renderer)
            {
                m_Renderer = null;
                m_MaterialIndex = -1;
                return;
            }

            if (materialIndex != -1)
            {
                if (materialIndex < 0 || materialIndex >= renderer.sharedMaterials.Length)
                {
                    Debug.LogError("Material index is out of range.");
                    materialIndex = -1;
                }
            }

            m_Renderer = renderer;
            m_MaterialIndex = materialIndex;
        }

        /// <summary>
        /// Applies the material as a shared material to the defined renderer and material index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A null material will be ignored.  So it is not possible to use the method to apply 
        /// a null material.
        /// </para>
        /// <para>
        /// If <see cref="IsDefined"/> is false, no action will be taken.  (This is not 
        /// considered an error.)
        /// </para>
        /// </remarks>
        /// <param name="material">The material to apply.</param>
        /// <returns>
        /// True if the material was applied, or false if no change was made.
        /// </returns>
        public bool ApplySharedMaterial(Material material)
        {
            return ApplySharedMaterial(material, m_Renderer, m_MaterialIndex);
        }

        /// <summary>
        /// Applies the material as a shared material to the renderer at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The only invalid parameter value is a material index that is out of range and
        /// not -1.  A null material, null renderer, or -1 index means that no
        /// change will occur.
        /// </para>
        /// <para>
        /// It is not possible to use the method to apply a null material.
        /// </para>
        /// </remarks>
        /// <param name="material">The material to apply. (Required)</param>
        /// <param name="renderer">The renderer to apply to the material to.</param>
        /// <param name="materialIndex">The material index to assign the material to.</param>
        /// <returns>
        /// True if the material was applied, or false if no change was made.
        /// </returns>
        public static bool ApplySharedMaterial(Material material, Renderer renderer, int materialIndex)
        {
            if (!material || !renderer || materialIndex == -1)
                // This is OK.  It just means that there is no override to apply.
                return false;

            var mats = renderer.sharedMaterials;
            mats[materialIndex] = material;
            renderer.sharedMaterials = mats;

            return true;
        }
    }
}