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

namespace com.lizitt
{
    /// <summary>
    /// Provides constants related to the default Unity layers.
    /// </summary>
    /// <remarks>
    /// Must use UnityEngine.LayerMask for masking since layers are NOT flag values.  
    /// The values are zero indexed based.  (See the Tag and Layers inspector panel in the 
    /// Unity editor.)
    /// </remarks>
    public static class UnityLayer
    {
        /// <summary>
        /// The default layer. (See Unity documentation.)
        /// </summary>
        public const int Default = 0;

        /// <summary>
        /// The transparent FX layer. (See Unity documentation.)
        /// </summary>
        public const int TransparentFX = 1;

        /// <summary>
        /// The ignore raycast layer. (Same as Physics.IgnoreRaycastLayer)
        /// </summary>
        public const int IgnoreRaycast = Physics.IgnoreRaycastLayer;

        /// <summary>
        /// The water layer. (See Unity documentation.)
        /// </summary>
        public const int Water = 4;

        /// <summary>
        /// The UI layer. (See Unity documentation.)
        /// </summary>
        public const int UI = 5;

        /// <summary>
        /// The default raycast layers.  (Same as Physics.DefaultRaycastLayers)
        /// </summary>
        public const int DefaultRaycast = Physics.DefaultRaycastLayers;

        /// <summary>
        /// The value representing all layers.  (Same as Physics.AllLayers)
        /// </summary>
        public const int All = Physics.AllLayers;
    }
}