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
namespace com.lizitt.easing
{
    /// <summary>
    /// Interpolates a transform's euler angles using a standard easing function.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All rotation axes are interpolated using the same ease mode.
    /// </para>
    /// <para>
    /// The <see cref="TrsInterpolator.InterpolatedValue"/> is guarenteed to match the
    /// final value at completion.  After completion, if the match item
    /// is dynamic, the <see cref="TrsInterpolator.InterpolatedValue"/> will stay 
    /// locked to the final value. (No further interpolation, lock step behavior.)
    /// </para>
    /// <para>
    /// Because of the guarentee that the interpolation will complete within the specified
    /// duration, this class is generally designed for static use.  It works best when the 
    /// settings and the <see cref="TrsInterpolator.MatchItem"/> remain static for during of the interpolation.  
    /// Any changes during the the interpolation will warp the easing behavior.
    /// </para>
    /// </remarks>
    [UnityEngine.AddComponentMenu(LizittUtil.EasingMenu + "Ease RotateTo", LizittUtil.EasingMenuOrder + 40)]
    public class EaseRotateTo
        : TrsEase
    {
        /// <summary>
        /// <see cref="TrsEase.LocalCreateHelper"/>
        /// </summary>
        protected override TrsEaseHelper LocalCreateHelper(TrsEaseParams settings)
        {
            return new EaseRotateToHelper(settings);
        }
    }
}
