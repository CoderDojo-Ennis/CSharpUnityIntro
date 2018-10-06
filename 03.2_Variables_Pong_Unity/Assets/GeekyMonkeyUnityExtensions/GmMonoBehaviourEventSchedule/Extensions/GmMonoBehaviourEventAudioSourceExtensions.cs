using System;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventAudioSourceExtensions
    {
        /// <summary>
        /// Fade the audio source volume over time
        /// </summary>
        /// <param name="m">Material</param>
        /// <param name="mb">MonoBehaviour used for events</param>
        /// <param name="fromVolume">From Volume (0-1)</param>
        /// <param name="toVolume">To Volume (0-1)</param>
        /// <param name="seconds">Seconds</param>
        public static GmMonoBehaviourEventPromise Fade(this AudioSource source, MonoBehaviour mb, float fromVolume, float toVolume, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                source.volume = toVolume;
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            source.volume = fromVolume;
            return mb.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
                //Debug.Log("Fade % = " + timePercent);
                source.volume = Mathf.Lerp(fromVolume, toVolume, timePercent);
            }, realtime);
        }

        /// <summary>
        /// Fade the audio source volume over time
        /// </summary>
        /// <param name="m">Material</param>
        /// <param name="mb">MonoBehaviour used for events</param>
        /// <param name="fromVolume">From Volume (0-1)</param>
        /// <param name="toVolume">To Volume (0-1)</param>
        /// <param name="seconds">Seconds</param>
        public static GmMonoBehaviourEventPromise FadePitch(this AudioSource source, MonoBehaviour mb, float fromPitch, float toPitch, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                source.pitch = toPitch;
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            source.pitch = fromPitch;
            return mb.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
                //Debug.Log("Fade % = " + timePercent);
                source.pitch = Mathf.Lerp(fromPitch, toPitch, timePercent);
            }, realtime);
        }
    }
}
