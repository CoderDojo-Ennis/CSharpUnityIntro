using System;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventLightExtensions
    {
        /// <summary>
        /// Fade the light intensity over time
        /// </summary>
        /// <param name="m">Material</param>
        /// <param name="mb">MonoBehaviour used for events</param>
        /// <param name="fromVolume">From Volume (0-1)</param>
        /// <param name="toVolume">To Volume (0-1)</param>
        /// <param name="seconds">Seconds</param>
        public static GmMonoBehaviourEventPromise FadeIntensity(this Light light, MonoBehaviour mb, float fromIntensity, float toIntensity, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                light.intensity = toIntensity;
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            light.intensity = fromIntensity;
            return mb.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
                //Debug.Log("Fade % = " + timePercent);
                light.intensity = Mathf.Lerp(fromIntensity, toIntensity, timePercent);
            });
        }
    }
}
