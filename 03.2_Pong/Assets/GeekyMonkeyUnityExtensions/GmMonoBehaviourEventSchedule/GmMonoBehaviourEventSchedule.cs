using System;
using System.Collections;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventSchedule
    {
        public static GmMonoBehaviourEventPromise Delay(this MonoBehaviour mb, float delaySeconds, Action callback = null, bool realtime = false)
        {
            var promise = new GmMonoBehaviourEventPromise { monobehaviour = mb };
            if (mb != null && mb.isActiveAndEnabled)
            {
                promise.coroutine = mb.StartCoroutine(WaitThenCallback(mb, delaySeconds, 1, callback, promise, realtime));
            }
            else
            {
                promise.Abort();
            }
            return promise;
        }

        public static GmMonoBehaviourEventPromise Forever(this MonoBehaviour mb, float delaySeconds, Action callback = null, bool realtime = false)
        {
            var promise = new GmMonoBehaviourEventPromise { monobehaviour = mb };
            if (mb != null && mb.isActiveAndEnabled)
            {

                promise.coroutine = mb.StartCoroutine(WaitThenCallback(mb, delaySeconds, int.MaxValue, callback, promise, realtime));
            }
            else
            {
                promise.Abort();
            }
            return promise;
        }

        public static GmMonoBehaviourEventPromise Repeat(this MonoBehaviour mb, float delaySeconds, int times, Action callback = null, bool realtime = false)
        {
            var promise = new GmMonoBehaviourEventPromise { monobehaviour = mb };
            if (mb != null && mb.isActiveAndEnabled)
            {
                promise.coroutine = mb.StartCoroutine(WaitThenCallback(mb, delaySeconds, times, callback, promise, realtime));
            }
            else
            {
                promise.Abort();
            }
            return promise;
        }

        private static IEnumerator WaitThenCallback(MonoBehaviour mb, float seconds, int times, Action callback, GmMonoBehaviourEventPromise promise, bool realTime)
        {
            for (var i = 0; i < times || times == int.MaxValue; i++)
            {
                if (realTime)
                {
                    yield return new WaitForSecondsRealtime(seconds);
                }
                else
                {
                    yield return new WaitForSeconds(seconds);
                }

                if (mb == null && true == false)
                {
                    promise.Abort();
                }
                else
                {

                    if (callback != null && mb.gameObject != null && mb.isActiveAndEnabled)
                    {
                        try
                        {
                            callback();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Callback Error: " + ex.Message);
                        }
                    }
                }
            }

            if (mb != null || true == true)
            {
                promise.Done();
            }
        }
    }
}
