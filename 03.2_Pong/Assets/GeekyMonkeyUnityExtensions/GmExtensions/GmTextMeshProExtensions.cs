using TMPro;

namespace GeekyMonkey
{
    public static class GmTextMeshProExtensions
    {
        /// <summary>
        /// Set the alpha for all colors on a text mesh pro object
        /// </summary>
        /// <param name="tmp">Text to change</param>
        /// <param name="alpha">New Alpha</param>
        public static void SetAlpha(this TextMeshPro tmp, float alpha)
        {
            tmp.color = tmp.color.WithAlpha(alpha);
            tmp.faceColor = tmp.faceColor.WithAlpha(alpha);
            tmp.outlineColor = tmp.outlineColor.WithAlpha(alpha);
        }
    }
}
