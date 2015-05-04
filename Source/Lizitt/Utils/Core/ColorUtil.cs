/*
 * Copyright (c) 2011-2015 Stephen A. Pratt
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

namespace com.lizitt.u3d
{
    /// <summary>
    /// Provides color related utility methods and predefined colors.
    /// </summary>
    public static class ColorUtil
    {
        /*
         * Acknowledgments:
         * 
         * Color values and naming convention: http://www.rapidtables.com/web/color/RGB_Color.htm
         * 
         */

        public static readonly Color Maroon = RgbColor(128, 0, 0);
        public static readonly Color DarkRed = RgbColor(139, 0, 0);
        public static readonly Color Brown = RgbColor(165, 42, 42);
        public static readonly Color Firebrick = RgbColor(178, 34, 34);
        public static readonly Color Crimson = RgbColor(220, 20, 60);
        public static readonly Color Red = RgbColor(255, 0, 0);
        public static readonly Color Tomato = RgbColor(255, 99, 71);
        public static readonly Color Coral = RgbColor(255, 127, 80);
        public static readonly Color IndianRed = RgbColor(205, 92, 92);
        public static readonly Color LightCoral = RgbColor(240, 128, 128);
        public static readonly Color DarkSalmon = RgbColor(233, 150, 122);
        public static readonly Color Salmon = RgbColor(250, 128, 114);
        public static readonly Color LightSalmon = RgbColor(255, 160, 122);
        public static readonly Color OrangeRed = RgbColor(255, 69, 0);
        public static readonly Color DarkOrange = RgbColor(255, 140, 0);
        public static readonly Color Orange = RgbColor(255, 165, 0);
        public static readonly Color Gold = RgbColor(255, 215, 0);
        public static readonly Color DarkGoldenrod = RgbColor(184, 134, 11);
        public static readonly Color Goldenrod = RgbColor(218, 165, 32);
        public static readonly Color PaleGoldenrod = RgbColor(238, 232, 170);
        public static readonly Color DarkKhaki = RgbColor(189, 183, 107);
        public static readonly Color Khaki = RgbColor(240, 230, 140);
        public static readonly Color Olive = RgbColor(128, 128, 0);
        public static readonly Color Yellow = RgbColor(255, 255, 0);
        public static readonly Color YellowGreen = RgbColor(154, 205, 50);
        public static readonly Color DarkOliveGreen = RgbColor(85, 107, 47);
        public static readonly Color OliveDrab = RgbColor(107, 142, 35);
        public static readonly Color LawnGreen = RgbColor(124, 252, 0);
        public static readonly Color Chartreuse = RgbColor(127, 255, 0);
        public static readonly Color GreenYellow = RgbColor(173, 255, 47);
        public static readonly Color DarkGreen = RgbColor(0, 100, 0);
        public static readonly Color Green = RgbColor(0, 128, 0);
        public static readonly Color ForestGreen = RgbColor(34, 139, 34);
        public static readonly Color Lime = RgbColor(0, 255, 0);
        public static readonly Color LimeGreen = RgbColor(50, 205, 50);
        public static readonly Color LightGreen = RgbColor(144, 238, 144);
        public static readonly Color PaleGreen = RgbColor(152, 251, 152);
        public static readonly Color DarkSeaGreen = RgbColor(143, 188, 143);
        public static readonly Color MediumSpringGreen = RgbColor(0, 250, 154);
        public static readonly Color SpringGreen = RgbColor(0, 255, 127);
        public static readonly Color SeaGreen = RgbColor(46, 139, 87);
        public static readonly Color MediumAquamarine = RgbColor(102, 205, 170);
        public static readonly Color MediumSeaGreen = RgbColor(60, 179, 113);
        public static readonly Color LightSeaGreen = RgbColor(32, 178, 170);
        public static readonly Color DarkSlateGray = RgbColor(47, 79, 79);
        public static readonly Color Teal = RgbColor(0, 128, 128);
        public static readonly Color DarkCyan = RgbColor(0, 139, 139);
        public static readonly Color Aqua = RgbColor(0, 255, 255);
        public static readonly Color Cyan = RgbColor(0, 255, 255);
        public static readonly Color LightCyan = RgbColor(224, 255, 255);
        public static readonly Color DarkTurquoise = RgbColor(0, 206, 209);
        public static readonly Color Turquoise = RgbColor(64, 224, 208);
        public static readonly Color MediumTurquoise = RgbColor(72, 209, 204);
        public static readonly Color PaleTurquoise = RgbColor(175, 238, 238);
        public static readonly Color Aquamarine = RgbColor(127, 255, 212);
        public static readonly Color PowderBlue = RgbColor(176, 224, 230);
        public static readonly Color CadetBlue = RgbColor(95, 158, 160);
        public static readonly Color SteelBlue = RgbColor(70, 130, 180);
        public static readonly Color CornflowerBlue = RgbColor(100, 149, 237);
        public static readonly Color DeepSkyBlue = RgbColor(0, 191, 255);
        public static readonly Color DodgerBlue = RgbColor(30, 144, 255);
        public static readonly Color LightBlue = RgbColor(173, 216, 230);
        public static readonly Color SkyBlue = RgbColor(135, 206, 235);
        public static readonly Color LightSkyBlue = RgbColor(135, 206, 250);
        public static readonly Color MidnightBlue = RgbColor(25, 25, 112);
        public static readonly Color Navy = RgbColor(0, 0, 128);
        public static readonly Color DarkBlue = RgbColor(0, 0, 139);
        public static readonly Color MediumBlue = RgbColor(0, 0, 205);
        public static readonly Color Blue = RgbColor(0, 0, 255);
        public static readonly Color RoyalBlue = RgbColor(65, 105, 225);
        public static readonly Color BlueViolet = RgbColor(138, 43, 226);
        public static readonly Color Indigo = RgbColor(75, 0, 130);
        public static readonly Color DarkSlateBlue = RgbColor(72, 61, 139);
        public static readonly Color SlateBlue = RgbColor(106, 90, 205);
        public static readonly Color MediumSlateBlue = RgbColor(123, 104, 238);
        public static readonly Color MediumPurple = RgbColor(147, 112, 219);
        public static readonly Color DarkMagenta = RgbColor(139, 0, 139);
        public static readonly Color DarkViolet = RgbColor(148, 0, 211);
        public static readonly Color DarkOrchid = RgbColor(153, 50, 204);
        public static readonly Color MediumOrchid = RgbColor(186, 85, 211);
        public static readonly Color Purple = RgbColor(128, 0, 128);
        public static readonly Color Thistle = RgbColor(216, 191, 216);
        public static readonly Color Plum = RgbColor(221, 160, 221);
        public static readonly Color Violet = RgbColor(238, 130, 238);
        public static readonly Color Magenta = RgbColor(255, 0, 255);
        public static readonly Color Orchid = RgbColor(218, 112, 214);
        public static readonly Color MediumVioletRed = RgbColor(199, 21, 133);
        public static readonly Color PaleVioletRed = RgbColor(219, 112, 147);
        public static readonly Color DeepPink = RgbColor(255, 20, 147);
        public static readonly Color HotPink = RgbColor(255, 105, 180);
        public static readonly Color LightPink = RgbColor(255, 182, 193);
        public static readonly Color Pink = RgbColor(255, 192, 203);
        public static readonly Color AntiqueWhite = RgbColor(250, 235, 215);
        public static readonly Color Beige = RgbColor(245, 245, 220);
        public static readonly Color Bisque = RgbColor(255, 228, 196);
        public static readonly Color BlanchedAlmond = RgbColor(255, 235, 205);
        public static readonly Color Wheat = RgbColor(245, 222, 179);
        public static readonly Color CornSilk = RgbColor(255, 248, 220);
        public static readonly Color LemonChiffon = RgbColor(255, 250, 205);
        public static readonly Color LightGoldenrodYellow = RgbColor(250, 250, 210);
        public static readonly Color LightYellow = RgbColor(255, 255, 224);
        public static readonly Color SaddleBrown = RgbColor(139, 69, 19);
        public static readonly Color Sienna = RgbColor(160, 82, 45);
        public static readonly Color Chocolate = RgbColor(210, 105, 30);
        public static readonly Color Peru = RgbColor(205, 133, 63);
        public static readonly Color SandyBrown = RgbColor(244, 164, 96);
        public static readonly Color BurlyWood = RgbColor(222, 184, 135);
        public static readonly Color Tan = RgbColor(210, 180, 140);
        public static readonly Color RosyBrown = RgbColor(188, 143, 143);
        public static readonly Color Moccasin = RgbColor(255, 228, 181);
        public static readonly Color NavajoWhite = RgbColor(255, 222, 173);
        public static readonly Color PeachPuff = RgbColor(255, 218, 185);
        public static readonly Color MistyRose = RgbColor(255, 228, 225);
        public static readonly Color LavenderBlush = RgbColor(255, 240, 245);
        public static readonly Color Linen = RgbColor(250, 240, 230);
        public static readonly Color OldLace = RgbColor(253, 245, 230);
        public static readonly Color PapayaWhip = RgbColor(255, 239, 213);
        public static readonly Color SeaShell = RgbColor(255, 245, 238);
        public static readonly Color MintCream = RgbColor(245, 255, 250);
        public static readonly Color SlateGray = RgbColor(112, 128, 144);
        public static readonly Color LightSlateGray = RgbColor(119, 136, 153);
        public static readonly Color LightSteelBlue = RgbColor(176, 196, 222);
        public static readonly Color Lavender = RgbColor(230, 230, 250);
        public static readonly Color FloralWhite = RgbColor(255, 250, 240);
        public static readonly Color AliceBlue = RgbColor(240, 248, 255);
        public static readonly Color GhostWhite = RgbColor(248, 248, 255);
        public static readonly Color Honeydew = RgbColor(240, 255, 240);
        public static readonly Color Ivory = RgbColor(255, 255, 240);
        public static readonly Color Azure = RgbColor(240, 255, 255);
        public static readonly Color Snow = RgbColor(255, 250, 250);
        public static readonly Color Black = RgbColor(0, 0, 0);
        public static readonly Color DimGray = RgbColor(105, 105, 105);
        public static readonly Color Gray = RgbColor(128, 128, 128);
        public static readonly Color DarkGray = RgbColor(169, 169, 169);
        public static readonly Color Silver = RgbColor(192, 192, 192);
        public static readonly Color LightGray = RgbColor(211, 211, 211);
        public static readonly Color Gainsboro = RgbColor(220, 220, 220);
        public static readonly Color WhiteSmoke = RgbColor(245, 245, 245);
        public static readonly Color White = RgbColor(255, 255, 255);

        /// <summary>
        /// Creates a color from an RGB integer color.
        /// </summary>
        /// <param name="r">The red component. [Limit: 0 - 255]</param>
        /// <param name="g">The green component. [Limit: 0 - 255]</param>
        /// <param name="b">The blue component. [Limit: 0 - 255]</param>
        /// <param name="alpha">The alpha component. [Limit: 0 - 255]</param>
        /// <returns>The color equivalent to the RGB color.</returns>
        public static Color RgbColor(int r, int g, int b, float alpha = 1)
        {
            return new Color((float)r/255, (float)g/255, (float)b/255, alpha);
        }

        /// <summary>
        /// Creates a color from a hex format color. (E.g. 0xFFCCAA)
        /// </summary>
        /// <param name="hex">The hex value of the color.</param>
        /// <param name="alpha">The color's alpha.</param>
        /// <returns>The color equivalent to the hex color.</returns>
        public static Color HexColor(int hex, float alpha)
        {
            float factor = 1f / 255;
            float r = ((hex >> 16) & 0xff) * factor;
            float g = ((hex >> 8) & 0xff) * factor;
            float b = (hex & 0xff) * factor;
            Color c = new Color(r, g, b, alpha);
            return c;
        }

        /// <summary>
        /// Derives a color from an integer value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is useful for generating a variety of colors that are visually disimilar.
        /// </para>
        /// </remarks>
        /// <param name="i">An integer value to create the color from.</param>
        /// <param name="alpha">The color's alpha.</param>
        /// <returns>A color based on the integer value.</returns>
        public static Color IntColor(int i, float alpha)
        {
            // r, g, and b are constrained to between 1 and 4 inclusive.
            const float factor = 63f / 255f;  // Approximately 0.25.
	        float r = Bit(i, 1) + Bit(i, 3) * 2 + 1;
	        float g = Bit(i, 2) + Bit(i, 4) * 2 + 1;
	        float b = Bit(i, 0) + Bit(i, 5) * 2 + 1;
            return new Color(r * factor, g * factor, b * factor, alpha);
        }

        // Returns 1 if the bit at position b in value a is 1. Otherwise returns 0.
        private static int Bit(int a, int b)
        {
            return (a & (1 << b)) >> b;
        }

        /// <summary>
        /// Applies an HSV transform to an RGB color.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Hue shifts the base color where any mutiple of 360 will result in no color change.
        /// Saturation shifts the color along the grayscale/color axis.
        /// Value shifts the color along the black/color axis.
        /// </para>
        /// <para>
        /// Algorithm adapted from <a href="http://beesbuzz.biz/code/hsv_color_transforms.php">
        /// http://beesbuzz.biz/code/hsv_color_transforms.php</a>
        /// </para>
        /// </remarks>
        /// <param name="color">
        /// The color to transform.
        /// </param>
        /// <param name="hueShift">
        /// Hue shift (Degrees)
        /// </param>
        /// <param name="saturation">
        /// Saturation multiplier. (Greyscale/Color) [Limit: value >= 0]
        /// </param>
        /// <param name="value">
        /// Value multiplier. (Black/Color) [Limit: value >= 0]
        /// </param>
        /// <returns>The color shifted by the HSV values.</returns>
        public static Color TransformHSV(
            this Color color, float hueShift, float saturation, float value)
        {
            saturation = Mathf.Max(0, saturation);
            value = Mathf.Max(0, value);

            float vsu = value * saturation * Mathf.Cos(hueShift * Mathf.PI / 180);
            float vsw = value * saturation * Mathf.Sin(hueShift * Mathf.PI / 180);

            Color result = color;  // All but alpha will be overwritten.

            result.r = 
                  (0.299f * value + 0.701f * vsu + 0.168f * vsw) * color.r
                + (0.587f * value - 0.587f * vsu + 0.330f * vsw) * color.g
                + (0.114f * value - 0.114f * vsu - 0.497f * vsw) * color.b;
            result.g = 
                  (0.299f * value - 0.299f * vsu - 0.328f * vsw) * color.r
                + (0.587f * value + 0.413f * vsu + 0.035f * vsw) * color.g
                + (0.114f * value - 0.114f * vsu + 0.292f * vsw) * color.b;
            result.b = 
                  (0.299f * value - 0.300f * vsu + 1.250f * vsw) * color.r
                + (0.587f * value - 0.588f * vsu - 1.050f * vsw) * color.g
                + (0.114f * value + 0.886f * vsu - 0.203f * vsw) * color.b;

            return result;
        }
    }
}
