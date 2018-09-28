using UnityEngine;

namespace GeekyMonkey
{
    public static class GmColorExtensions
    {
        /// <summary>
        /// Convert a color to another color with a different alpha
        /// </summary>
        /// <param name="color">The original color</param>
        /// <param name="alpha">New alpha value</param>
        /// <returns>new color with the specified alpha</returns>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Convert a color to another color with a different alpha
        /// </summary>
        /// <param name="color">The original color</param>
        /// <param name="alpha">New alpha value</param>
        /// <returns>new color with the specified alpha</returns>
        public static Color32 WithAlpha(this Color32 color, float alpha)
        {
            return new Color32(color.r, color.g, color.b, (byte)(alpha * 255f));
        }
    }
}
