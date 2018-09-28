using System;

namespace GeekyMonkey
{
    /// <summary>
    /// A scheduled action
    /// </summary>
    public class GmObjectEventAction : GmObjectEventPromise
    {
        public GmObjectEventActionKey Key;
        public Action Action;

        /// <summary>
        /// Construct a acheduled action
        /// </summary>
        /// <param name="scheduledTime">The scheduled time</param>
        /// <param name="action">The action callback</param>
        public GmObjectEventAction(float scheduledTime, Action action, GmObjectEventPromise parentPromise = null)
        {
            Key = new GmObjectEventActionKey(scheduledTime);
            Action = action;
            if (parentPromise != null)
            {
                parentPromise.ChildPromise = this;
            }
        }
    }
}
