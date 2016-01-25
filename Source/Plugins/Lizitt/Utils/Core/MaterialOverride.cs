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
    /// Represents a material override for a particular renderer's material index.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is useful when defining material overide configurations.  For example, an
    /// agent prefab may have a default material, but 'TeamA' gets one material override while
    /// 'TeamB' gets a different override.
    /// </para>
    /// </remarks>
    [System.Serializable]
    public class MaterialOverride
    {
        /*
         * Design notes:
         * 
         * An editor is important for usability, but not sure how to generalize it yet.  Use of
         * this class can vary greatly.  See the outfitter project for an example of an editor
         * along with some notes.
         * 
         */

        [SerializeField]
        [Tooltip("The target of the override.")]
        private RendererMaterialPtr m_Target = new RendererMaterialPtr();

        [SerializeField]
        [Tooltip("The material that will replace the target material.")]
        private Material m_Material = null;

        /// <summary>
        /// Default constructor.  (State will be invalid until all values are manually set.)
        /// </summary>
        public MaterialOverride() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Invalid (temporary) values are accepted.  The values must be set to valid values
        /// before use.
        /// </para>
        /// </remarks>
        /// <param name="material">The override material.</param>
        /// <param name="renderer">The target renderer.</param>
        /// <param name="materialIndex">
        /// The index of the material on the target renderer.
        /// </param>
        public MaterialOverride(Material material, Renderer renderer = null, int materialIndex = -1)
        {
            m_Material = material;
            m_Target = new RendererMaterialPtr(renderer, materialIndex);
        }

        /// <summary>
        /// The target of the override.
        /// </summary>
        public RendererMaterialPtr Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        
        /// <summary>
        /// The material override.
        /// </summary>
        public Material Material
        {
            get { return m_Material; }
            set { m_Material = value; }
        }

        /// <summary>
        /// Apply the material to the renderer.
        /// </summary>
        public void Apply()
        {
            m_Target.ApplySharedMaterial(m_Material);
        }
    }
}