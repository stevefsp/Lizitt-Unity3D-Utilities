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

namespace com.lizitt.u3d
{
    /// <summary>
    /// The string values for the default Unity tags.
    /// </summary>
    public static class DefaultTag
    {
        /// <summary>
        /// The default tag name representing player objects.
        /// </summary>
        public const string Player = "Player";

        /// <summary>
        /// The tag name for the main camera.
        /// </summary>
        public const string MainCamera = "MainCamera";

        /// <summary>
        /// The default tag name for untagged objected.
        /// </summary>
        public const string Untagged = "Untagged";

        /// <summary>
        /// The default tag name for respawn objects.
        /// </summary>
        public const string Respawn = "Respawn";

        /// <summary>
        /// The default tag name for finish objects.
        /// </summary>
        public const string Finish = "Finish";

        /// <summary>
        /// The tag for objects that are not to be included in non-editor builds.
        /// </summary>
        public const string EditorOnly = "EditorOnly";

        /// <summary>
        /// The default tag name for game controller objects.
        /// </summary>
        public const string GameController = "GameController";
    }
}