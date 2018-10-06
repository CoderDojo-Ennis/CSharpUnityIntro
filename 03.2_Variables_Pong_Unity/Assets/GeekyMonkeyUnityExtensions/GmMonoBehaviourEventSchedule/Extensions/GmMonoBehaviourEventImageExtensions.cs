using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventImageExtensions
    {
        /// <summary>
        /// Fade the alpha of the imgae from one value to another
        /// </summary>
        /// <param name="img"></param>
        /// <param name="mb"></param>
        /// <param name="fromAlpha"></param>
        /// <param name="toAlpha"></param>
        /// <param name="seconds"></param>
        /// <param name="realtime"></param>
        /// <returns></returns>
        public static GmMonoBehaviourEventPromise FadeAlpha(this Image img, MonoBehaviour mb, float fromAlpha, float toAlpha, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                img.SetAlpha(toAlpha);
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            img.color = img.color.WithAlpha(fromAlpha);
            return mb.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
                //Debug.Log("Fade % = " + timePercent);
                img.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
            });
        }
    }
}
