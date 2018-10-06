using System;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GeekyMonkeySpriteRendererExtensions
    {
        public static GmMonoBehaviourEventPromise FadeAlpha(this SpriteRenderer sr, MonoBehaviour mb, float fromAlpha, float toAlpha, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                sr.SetAlpha(toAlpha);
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            sr.SetAlpha(fromAlpha);
            return mb.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            sr.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
            }, realtime);
        }
    }
}