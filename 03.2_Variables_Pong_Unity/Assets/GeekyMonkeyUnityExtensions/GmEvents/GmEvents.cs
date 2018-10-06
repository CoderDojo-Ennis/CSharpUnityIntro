/**
 * Borrowed from https://gist.github.com/wmiller/3903205 and extended for:
 *  * Static instead of Singleton
 *  * Remove all listeners for a given target
 */

using System;
using System.Collections.Generic;

namespace GeekyMonkey
{
    /// <summary>
    /// Base class for a Game Event
    /// </summary>
    public abstract class GameEvent
    {
    }

    /// <summary>
    /// Store a delegate reference along with it's type for lookup when removing listeners
    /// </summary>
    internal struct DelegateAndType
    {
        public Delegate Delegate;
        public Type Type;
    }

    /// <summary>
    /// Event Processor - Singleton
    /// </summary>
    public class Events
    {
        /// <summary>
        /// Event callback function type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        public delegate void EventDelegate<T>(T e) where T : GameEvent;

        /// <summary>
        /// Event callback function type
        /// </summary>
        private delegate void EventDelegate(GameEvent e);

        /// <summary>
        /// List of subscriptions by event type.
        /// Used for looking up subscriptions to notify when an event is raised
        /// </summary>
        private static Dictionary<Type, EventDelegate> delegatesByType = new Dictionary<Type, EventDelegate>();

        /// <summary>
        /// Complete list of active subscriptions
        /// </summary>
        private static Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

        /// <summary>
        /// List of all subscriptions by subcribing target.
        /// Used for removing all subscriptions for a given target object.
        /// </summary>
        private static Dictionary<object, List<DelegateAndType>> delegatesByTarget = new Dictionary<object, List<DelegateAndType>>();

        /// <summary>
        /// Listen for a type of event
        /// </summary>
        /// <typeparam name="T">Type of event</typeparam>
        /// <param name="del">What to do when this event fires</param>
        public static EventDelegate<T> AddListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            // Early-out if we've already registered this delegate
            if (delegateLookup.ContainsKey(del))
            {
                return null;
            }

            Type eventType = typeof(T);

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            EventDelegate internalDelegate = (e) =>
            {
                if (del.Target != null)
                {
                    //var mb = del.Target as MonoBehaviour;
                    //if (mb == null || mb.enabled) {
                    del((T)e);
                    //}
                }
            };
            delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (delegatesByType.TryGetValue(eventType, out tempDel))
            {
                delegatesByType[eventType] = tempDel += internalDelegate;
            }
            else
            {
                delegatesByType[eventType] = internalDelegate;
            }

            if (delegatesByTarget.ContainsKey(del.Target))
            {
                delegatesByTarget[del.Target].Add(
                                        new DelegateAndType
                                        {
                                            Delegate = del,
                                            Type = eventType
                                        }
                );
            }
            else
            {
                delegatesByTarget.Add(del.Target, new List<DelegateAndType> {
                    new DelegateAndType {
                        Delegate = del,
                        Type = eventType
                    }
                });
            }

            return del;
        }

        /// <summary>
        /// Remove all listeners on a given target object
        /// </summary>
        /// <param name="target">Object that event subscriptions were added to</param>
        public static void RemoveListeners(object target)
        {
            if (delegatesByTarget.ContainsKey(target))
            {
                foreach (var delAndType in delegatesByTarget[target])
                {
                    RemoveListenerInternal(delAndType.Type, delAndType.Delegate);
                }
            }
        }

        /// <summary>
        /// Remove a single event listener
        /// </summary>
        /// <param name="eventType">Event Type</param>
        /// <param name="del">Delegate</param>
        private static void RemoveListenerInternal(Type eventType, Delegate del)
        {
            if (del == null)
            {
                return;
            }

            EventDelegate internalDelegate;
            if (delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if (delegatesByType.TryGetValue(eventType, out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        delegatesByType.Remove(eventType);
                    }
                    else
                    {
                        delegatesByType[eventType] = tempDel;
                    }
                }

                delegateLookup.Remove(del);
            }
        }

        /// <summary>
        /// Remove this listener from the event
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="del">Callback to detach</param>
        public static void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            RemoveListenerInternal(typeof(T), del);
        }

        /// <summary>
        /// Broadcast an event
        /// </summary>
        /// <param name="e">Event to broadcast</param>
        public static void Raise(GameEvent e)
        {
            EventDelegate del;
            if (delegatesByType.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }
    }
}
