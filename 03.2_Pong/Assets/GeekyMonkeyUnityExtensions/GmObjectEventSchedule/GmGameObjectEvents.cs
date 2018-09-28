using System;
using UnityEngine;

namespace GeekyMonkey
{
    /// <summary>
    /// Extension methods applied to GameObject
    /// </summary>
    public static class GmGameObjectEvents
    {
        public static GmGameObjectEventsBehaviour GetEventBehaviour(this GameObject gameObject)
        {
            var eventsBehaviour = gameObject.GetComponent<GmGameObjectEventsBehaviour>();
            if (eventsBehaviour == null)
            {
                eventsBehaviour = gameObject.AddComponent<GmGameObjectEventsBehaviour>();
            }

            return eventsBehaviour;
        }

        /// <summary>
        /// Schedule a callback using scaled time
        /// </summary>
        /// <param name="gameObject">This game object</param>
        /// <param name="seconds">Seconds from now (scaled)</param>
        /// <param name="callback">Callback action</param>
        /// <returns>Promise</returns>
        public static GmObjectEventPromise Delay(this GameObject gameObject, float seconds, Action callback = null)
        {
            return gameObject.GetEventBehaviour().ScheduleAction(seconds, callback);
        }

        /// <summary>
        /// Schedule a callback using unscaled time
        /// </summary>
        /// <param name="gameObject">This game object</param>
        /// <param name="seconds">Seconds from now (unscaled)</param>
        /// <param name="callback">Callback action</param>
        /// <returns>Promise</returns>
        public static GmObjectEventPromise DelayUnscaled(this GameObject gameObject, float seconds, Action callback = null)
        {
            return gameObject.GetEventBehaviour().ScheduleActionUnscaled(seconds, callback);
        }

        /// <summary>
        /// Repeat an action forever (or until aborted)
        /// </summary>
        /// <param name="gameObject">This game object</param>
        /// <param name="seconds">Seconds before runs</param>
        /// <param name="callback">Callback action</param>
        /// <param name="parentPromise">Parent promise used for aborting (generally NULL)</param>
        /// <returns></returns>
        public static GmObjectEventPromise Forever(this GameObject gameObject, float seconds, Action callback, GmObjectEventPromise parentPromise = null)
        {
            if (parentPromise == null)
            {
                parentPromise = new GmObjectEventPromise();
            }

            // Schedule the next one
            gameObject.GetEventBehaviour().ScheduleAction(seconds, () =>
            {
                if (!parentPromise.IsAborted && !parentPromise.IsDone)
                {
                    callback();
                // Schedule another one until aborted
                gameObject.Forever(seconds, callback, parentPromise);
                }
            }, parentPromise);

            return parentPromise;
        }

        /// <summary>
        /// Repeat an action forever (or until aborted)
        /// </summary>
        /// <param name="gameObject">This game object</param>
        /// <param name="seconds">Seconds before runs</param>
        /// <param name="callback">Callback action</param>
        /// <param name="parentPromise">Parent promise used for aborting (generally NULL)</param>
        /// <returns></returns>
        public static GmObjectEventPromise ForeverUnscaled(this GameObject gameObject, float seconds, Action callback, GmObjectEventPromise parentPromise = null)
        {
            if (parentPromise == null)
            {
                parentPromise = new GmObjectEventPromise();
            }

            // Schedule the next one
            gameObject.GetEventBehaviour().ScheduleActionUnscaled(seconds, () =>
            {
                if (!parentPromise.IsAborted && !parentPromise.IsDone)
                {
                    callback();
                // Schedule another one until aborted
                gameObject.ForeverUnscaled(seconds, callback, parentPromise);
                }
            }, parentPromise);

            return parentPromise;
        }

        /// <summary>
        /// Repeat an action a number of times (or until aborted)
        /// </summary>
        /// <param name="gameObject">This game object</param>
        /// <param name="seconds">Seconds before runs (sclaed)</param>
        /// <param name="callback">Callback action</param>
        /// <param name="parentPromise">Parent promise used for aborting (generally NULL)</param>
        /// <returns></returns>
        public static GmObjectEventPromise Repeat(this GameObject gameObject, float seconds, int count, Action callback, GmObjectEventPromise parentPromise = null)
        {
            if (parentPromise == null)
            {
                parentPromise = new GmObjectEventPromise();
            }

            // All done
            if (count <= 0)
            {
                parentPromise.Done();
            }
            else
            {
                // Schedule the next one
                gameObject.GetEventBehaviour().ScheduleAction(seconds, () =>
                {
                    if (!parentPromise.IsAborted && !parentPromise.IsDone)
                    {
                        callback();
                    // Schedule another one until aborted
                    gameObject.Repeat(seconds, count - 1, callback, parentPromise);
                    }
                }, parentPromise);
            }

            return parentPromise;
        }

        /// <summary>
        /// Repeat an action a number of times (or until aborted)
        /// </summary>
        /// <param name="gameObject">This game object</param>
        /// <param name="seconds">Seconds before runs (unscaled)</param>
        /// <param name="callback">Callback action</param>
        /// <param name="parentPromise">Parent promise used for aborting (generally NULL)</param>
        /// <returns></returns>
        public static GmObjectEventPromise RepeatUnscaled(this GameObject gameObject, float seconds, int count, Action callback, GmObjectEventPromise parentPromise = null)
        {
            if (parentPromise == null)
            {
                parentPromise = new GmObjectEventPromise();
            }

            // All done
            if (count <= 0)
            {
                parentPromise.Done();
            }
            else
            {
                // Schedule the next one
                gameObject.GetEventBehaviour().ScheduleActionUnscaled(seconds, () =>
                {
                    if (!parentPromise.IsAborted && !parentPromise.IsDone)
                    {
                        callback();
                    // Schedule another one until aborted
                    gameObject.RepeatUnscaled(seconds, count - 1, callback, parentPromise);
                    }
                }, parentPromise);
            }

            return parentPromise;
        }
    }
}