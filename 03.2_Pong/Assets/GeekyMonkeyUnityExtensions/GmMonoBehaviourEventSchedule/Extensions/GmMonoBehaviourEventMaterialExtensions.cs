using System;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventSpriteRendererExtensions
    {
        /// <summary>
        /// Fade the main color from one color to another over time
        /// </summary>
        /// <param name="m">Material</param>
        /// <param name="mb">MonoBehaviour used for events</param>
        /// <param name="fromColor">From Color</param>
        /// <param name="toColor">To Color</param>
        /// <param name="seconds">Seconds</param>
        public static GmMonoBehaviourEventPromise Fade(this Material mat, MonoBehaviour mb, Color fromColor, Color toColor, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                mat.SetColor("_Color", toColor);
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            mat.SetColor("_Color", fromColor);
            return mb.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
                //Debug.Log("Fade % = " + timePercent);
                mat.SetColor("_Color", Color.Lerp(fromColor, toColor, timePercent));
            }, realtime);
        }
    }
}
