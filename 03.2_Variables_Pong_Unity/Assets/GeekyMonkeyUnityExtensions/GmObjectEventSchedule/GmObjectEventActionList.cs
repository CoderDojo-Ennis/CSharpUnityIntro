using System;
using System.Collections.Generic;

namespace GeekyMonkey
{
    /// <summary>
    /// List of scheduled future actions
    /// ToDo: use a pool (array?) to avoid garbage collection
    /// </summary>
    public class GmObjectEventActionList : SortedList<GmObjectEventActionKey, GmObjectEventAction>
    {
        /// <summary>
        /// Add an event at the time scale/offset for this list
        /// </summary>
        /// <param name="scheduledTime">the scheduled time in the scale/offset for this list</param>
        /// <param name="action">The callback action</param>
        /// <returns>Scheduled Action (Promise)</returns>
        public GmObjectEventAction Add(float scheduledTime, Action action, GmObjectEventPromise parentPromise = null)
        {
            var newScheduledAction = new GmObjectEventAction(scheduledTime, action, parentPromise);
            Add(newScheduledAction.Key, newScheduledAction);
            return newScheduledAction;
        }

        /// <summary>
        /// Execute all of the events scheduled in the past
        /// </summary>
        /// <param name="elapsedTime">Elapsed time in the scale and offset of this list</param>
        internal void ExecuteEvents(float elapsedTime)
        {
            while (Count > 0 && Keys[0].ScheduledTime <= elapsedTime)
            {
                //Debug.Log("Fire " + Keys[0].ScheduledTime + " < " + elapsedTime);
                GmObjectEventAction scheduledAction = Values[0];
                if (!scheduledAction.IsAborted && !scheduledAction.IsDone)
                {
                    scheduledAction.Action();
                    scheduledAction.Done();
                }
                RemoveAt(0);
            }
        }
    }
}
