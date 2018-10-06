using UnityEngine;

namespace GeekyMonkey
{
    public static class GmSpriteRendererExtensions
    {
        /// <summary>
        /// Set the Alpha value on a sprite renderer
        /// </summary>
        /// <param name="sr">Sprite Renderrer</param>
        /// <param name="alpha">New Alpha value</param>
        public static void SetAlpha(this SpriteRenderer sr, float alpha)
        {
            sr.color = sr.color.WithAlpha(alpha);
        }
    }
}
