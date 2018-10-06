using System;
using TMPro;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventTextMeshProExtensions
    {
        /// <summary>
        /// Fade the alpha over time
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="fromAlpha"></param>
        /// <param name="toAlpha"></param>
        /// <param name="seconds"></param>
        /// <param name="realtime"></param>
        /// <returns></returns>
        public static GmMonoBehaviourEventPromise FadeAlpha(this TextMeshPro tmp, float fromAlpha, float toAlpha, float seconds, bool realtime)
        {
            if (seconds == 0)
            {
                tmp.SetAlpha(toAlpha);
                var done = new GmMonoBehaviourEventPromise();
                done.Done();
                return done;
            }

            float intervalSeconds = 0.1f;
            float step = 0;
            int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
            //Debug.Log("Fade Steps = " + fadeSteps);

            tmp.SetAlpha(fromAlpha);
            return tmp.Repeat(intervalSeconds, fadeSteps, () =>
            {
                step++;
                float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            tmp.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
            }, true);
        }

        /// <summary>
        /// Type out the characters one at a time
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="mb"></param>
        /// <param name="characterSeconds"></param>
        /// <param name="characterShown"></param>
        /// <returns></returns>
        public static GmMonoBehaviourEventPromise Type(this TextMeshPro tmp, MonoBehaviour mb, float characterSeconds, bool realtime = false, Action characterShown = null)
        {
            if (tmp == null || mb == null)
            {
                return null;
            }

            GmMonoBehaviourEventPromise finishPromise;

            TMP_TextInfo textInfo = tmp.textInfo;

            // Get # of Visible Character in text object
            tmp.maxVisibleCharacters = 999999;
            int charCount = tmp.text.Length; //  textInfo.characterCount; 
                                             //Debug.Log("Typing " + charCount + " characters:" + tmp.text);

            tmp.maxVisibleCharacters = 0;

            finishPromise = mb.Repeat(characterSeconds, charCount, () =>
            {
                char lastChar = tmp.text[tmp.maxVisibleCharacters];

            //Debug.Log("Typing character");
            tmp.maxVisibleCharacters += 1;

                if (lastChar != ' ' && lastChar != '\r' && lastChar != '\n')
                {
                    if (characterShown != null)
                    {
                        characterShown();
                    }
                }
            }, realtime);

            return finishPromise;
        }

        /// <summary>
        /// Type out the characters one at a time
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="mb"></param>
        /// <param name="characterSeconds"></param>
        /// <param name="characterShown"></param>
        /// <returns></returns>
        public static GmMonoBehaviourEventPromise Type(this TextMeshProUGUI tmp, MonoBehaviour mb, float characterSeconds, bool realtime = false, Action characterShown = null)
        {
            if (tmp == null || mb == null)
            {
                return null;
            }

            GmMonoBehaviourEventPromise finishPromise;

            TMP_TextInfo textInfo = tmp.textInfo;

            // Get # of Visible Character in text object
            tmp.maxVisibleCharacters = 999999;
            int charCount = tmp.text.Length; //  textInfo.characterCount; 
                                             //Debug.Log("Typing " + charCount + " characters:" + tmp.text);

            tmp.maxVisibleCharacters = 0;

            finishPromise = mb.Repeat(characterSeconds, charCount, () =>
            {
            //Debug.Log("Typing character");
            tmp.maxVisibleCharacters += 1;
                if (characterShown != null)
                {
                    characterShown();
                }
            }, realtime);

            return finishPromise;
        }
    }
}
