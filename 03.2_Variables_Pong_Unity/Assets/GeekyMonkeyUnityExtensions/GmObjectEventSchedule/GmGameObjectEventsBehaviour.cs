using System;
using UnityEngine;

namespace GeekyMonkey
{
    /// <summary>
    /// MonoBehaviour that gets attached go game objects to process the schedule
    /// </summary>
    public class GmGameObjectEventsBehaviour : MonoBehaviour
    {
        private GmObjectEventActionList actionList;
        private GmObjectEventActionList actionListUnscaled;
        private GmObjectEventActionList actionListFixed;
        private float elapsedTime = 0;
        private float elapsedTimeUnscaled = 0;
        private float elapsedFixedTime = 0;
        private bool firstUpdate = true;
        private bool firstFixedUpdate = true;

        /// <summary>
        /// List of pending actions for scaled time
        /// </summary>
        public GmObjectEventActionList ActionList
        {
            get
            {
                if (actionList == null)
                {
                    actionList = new GmObjectEventActionList();
                }
                return actionList;
            }
        }

        /// <summary>
        /// List of pending actions for fixed time
        /// </summary>
        public GmObjectEventActionList ActionListFixed
        {
            get
            {
                if (actionListFixed == null)
                {
                    actionListFixed = new GmObjectEventActionList();
                }
                return actionListFixed;
            }
        }

        /// <summary>
        /// List of pending actions for unscaled time
        /// </summary>
        public GmObjectEventActionList ActionListUnscaled
        {
            get
            {
                if (actionListUnscaled == null)
                {
                    actionListUnscaled = new GmObjectEventActionList();
                }
                return actionListUnscaled;
            }
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            // The first update gets a large delta time. Throw this out and wait for the next one
            if (firstUpdate)
            {
                elapsedTime = 0;
                elapsedTimeUnscaled = 0;
                firstUpdate = false;
            }
            else
            {
                // Advance time
                elapsedTime += Time.deltaTime;

                // Pop out actions and execute them
                ActionList.ExecuteEvents(elapsedTime);

                // Advance Unscaled time
                elapsedTimeUnscaled += Time.unscaledDeltaTime;

                // Pop out actions and execute them
                ActionListUnscaled.ExecuteEvents(elapsedTimeUnscaled);
            }
        }

        /// <summary>
        /// Update is called at fixed intervals
        /// </summary>
        void FixedUpdate()
        {
            // The first update gets a large delta time. Throw this out and wait for the next one
            if (firstFixedUpdate)
            {
                elapsedFixedTime = 0;
                firstFixedUpdate = false;
            }
            else
            {
                // Advance Fixed time
                elapsedFixedTime += Time.fixedTime;

                // Pop out actions and execute them
                ActionListFixed.ExecuteEvents(elapsedFixedTime);
            }
        }

        /// <summary>
        /// Schedule an action using scaled time
        /// </summary>
        /// <param name="seconds">Seconds from now</param>
        /// <param name="callback">The callback action</param>
        /// <returns>Scheduled Action (Promise)</returns>
        internal GmObjectEventPromise ScheduleAction(float seconds, Action callback, GmObjectEventPromise parentPromise = null)
        {
            //Debug.Log("Schedule for " + Mathf.Round((elapsedTime + seconds) * 10) / 10 + " scaled");
            return ActionList.Add(elapsedTime + seconds, callback, parentPromise);
        }

        /// <summary>
        /// Schedule an action using unscaled time
        /// </summary>
        /// <param name="seconds">Seconds from now</param>
        /// <param name="callback">The callback action</param>
        /// <returns>Scheduled Action (Promise)</returns>
        internal GmObjectEventPromise ScheduleActionUnscaled(float seconds, Action callback, GmObjectEventPromise parentPromise = null)
        {
            //Debug.Log("Schedule for " + Mathf.Round((elapsedTimeUnscaled + seconds) * 10) / 10 + " unscaled");
            return ActionListUnscaled.Add(elapsedTimeUnscaled + seconds, callback, parentPromise);
        }
    }

}
