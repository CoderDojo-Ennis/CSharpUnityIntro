using UnityEngine.UI;

namespace GeekyMonkey
{
    public static class GmImageExtensions
    {
        public static void SetAlpha(this UnityEngine.UI.Image img, float alpha)
        {
            img.color = img.color.WithAlpha(alpha);
        }
   }
}