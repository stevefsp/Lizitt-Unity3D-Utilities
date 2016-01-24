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
 * 
 */
namespace com.lizitt
{
    /// <summary>
    /// Represents various easing types/agorithms.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Most of these easing types refer to common easing functions used by CSS3, Javascript,
    /// jQuery, etc. Examples of most of the algorithms can be found at 
    /// <a href="http://easings.net/">easings.net</a>.  Hover over the example graphs 
    /// to see a demonstration the algorithm.  More details and links can be found at
    /// <a href="http://www.robertpenner.com/easing/">Robert Penner's Easing Site</a>.  
    /// </para>
    /// <para>
    /// The general naming convention uses 'in' to mean 'special behavior at the start' and 
    /// 'out' to mean 'special behavior at the end'.  For example, an algorithm may
    /// implement slower change at the beginning, special occilations at the end, etc.
    /// </para>
    /// </remarks>
    public enum EaseType
    {
        /// <summary>
        /// Linear interpolation.
        /// </summary>
        Linear = 0,

        /// <summary>
        /// Interpolate similarly to <see cref="Linear"/>, but with a gradual speed up at the
        /// beginning, and slow down at the end, of the interpolation range.
        /// </summary>
        /// <remarks>
        /// <para>This is similar to <see cref="EaseInOutQuad"/>, but uses Unity's built-in
        /// algorithm.
        /// </para>
        /// </remarks>
        SmoothStep,

        /// <summary>
        /// Quadratic easing - Slower at start.
        /// </summary>
        EaseInQuad,

        /// <summary>
        /// Quadratic easing - Slower at end.
        /// </summary>
        EaseOutQuad,

        /// <summary>
        /// Quadratic easing - Slower at start and end.
        /// </summary>
        EaseInOutQuad,

        /// <summary>
        /// Cubic easing - Slower at start.
        /// </summary>
        EaseInCubic,

        /// <summary>
        /// Cubic easing - Slower at end.
        /// </summary>
        EaseOutCubic,

        /// <summary>
        /// Cubic easing - Slower at start and end.
        /// </summary>
        EaseInOutCubic,

        /// <summary>
        /// Quartic easing - Slower at start.
        /// </summary>
        EaseInQuart,

        /// <summary>
        /// Quartic easing - Slower at end.
        /// </summary>
        EaseOutQuart,

        /// <summary>
        /// Quartic easing - Slower at start and end.
        /// </summary>
        EaseInOutQuart,

        /// <summary>
        /// Quintic easing - Slower at start.
        /// </summary>
        EaseInQuint,

        /// <summary>
        /// Quintic easing - Slower at end.
        /// </summary>
        EaseOutQuint,

        /// <summary>
        /// Quintic easing - Slower at start and end.
        /// </summary>
        EaseInOutQuint,

        /// <summary>
        /// Sinusoidal (Sine) easing - Slower at start.
        /// </summary>
        EaseInSine,

        /// <summary>
        /// Sinusoidal (Sine) easing - Slower at end.
        /// </summary>
        EaseOutSine,

        /// <summary>
        /// Sinusoidal (Sine) easing - Slower at start and end.
        /// </summary>
        EaseInOutSine,

        /// <summary>
        /// Exponential easing - Slower at start.
        /// </summary>
        EaseInExpo,

        /// <summary>
        /// Exponential easing - Slower at end.
        /// </summary>
        EaseOutExpo,

        /// <summary>
        /// Exponential easing - Slower at start and end.
        /// </summary>
        EaseInOutExpo,
        
        /// <summary>
        /// Circular easing - Slower at start.
        /// </summary>
        EaseInCirc,

        /// <summary>
        /// Exponential easing - Slower at end.
        /// </summary>
        EaseOutCirc,

        /// <summary>
        /// Exponential easing - Slower at start and end.
        /// </summary>
        EaseInOutCirc,

        /// <summary>
        /// Spring easing.
        /// </summary>
        Spring,

        /// <summary>
        /// Bounce easing - Small bounces nearer to start.
        /// </summary>
        EaseInBounce,

        /// <summary>
        /// Bounce easing - Small bounces nearer to end, and inverted.
        /// </summary>
        EaseOutBounce,

        /// <summary>
        /// Bounce easing - <see cref="EaseInBounce"/> appended to <see cref="EaseOutBounce"/>.
        /// </summary>
        EaseInOutBounce,

        /// <summary>
        /// Ease back - Overshoot just after the start.
        /// </summary>
        EaseInBack,

        /// <summary>
        /// Ease back -  - Overshoot just before the end.
        /// </summary>
        EaseOutBack,

        /// <summary>
        /// Ease back - Overshoot near the start and end.
        /// </summary>
        EaseInOutBack,

        /// <summary>
        /// Elastic ease - Bouncing with overshoot near the start.
        /// </summary>
        EaseInElastic,

        /// <summary>
        /// Elastic ease - Bouncing with overshoot near the end.
        /// </summary>
        EaseOutElastic,

        /// <summary>
        /// Elastic ease - Bouncing with overshoot near the start and the end.
        /// </summary>
        EaseInOutElastic,

        /// <summary>
        /// Circular lerp - Liner interpolation with wrapping at the 0 and 360 boundaries.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Useful for interpolating angles such as Transform.eulerAngles so easing will
        /// occur in the correct (shortest) direction.
        /// </para>
        /// </remarks>
        CircularLerp,
    }
}

