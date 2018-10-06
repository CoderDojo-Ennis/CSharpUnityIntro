using UnityEngine;

namespace GeekyMonkey
{
    public static class GmEventsMonoBehaviourExtensions
    {
        /// <summary>
        /// Remove all listeners on this behaviour
        /// </summary>
        /// <param name="target">Object that event subscriptions were added to</param>
        public static void RemoveEventListeners(this MonoBehaviour mb)
        {
            Events.RemoveListeners(mb);
        }
    }
}
