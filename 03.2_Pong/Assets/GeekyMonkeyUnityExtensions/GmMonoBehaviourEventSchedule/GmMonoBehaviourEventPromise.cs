using System;
using UnityEngine;

namespace GeekyMonkey
{
    public class GmMonoBehaviourEventPromise
    {
        internal Coroutine coroutine;
        internal MonoBehaviour monobehaviour;
        private Action then;
        internal bool isDone;

        // Protected constructor
        internal GmMonoBehaviourEventPromise()
        {
        }

        /// <summary>
        /// Abort this scheduled action
        /// </summary>
        public void Abort()
        {
            then = null;
            if (coroutine != null && monobehaviour != null)
            {
                monobehaviour.StopCoroutine(coroutine);
            }
        }

        public void Then(Action thenCallback)
        {
            if (isDone)
            {
                thenCallback();
            }
            else
            {
                then += thenCallback;
            }
        }

        /// <summary>
        /// Internally called when done. Calls the Then function
        /// </summary>
        internal void Done()
        {
            isDone = true;
            if (then != null)
            {
                then();
            }
        }
    }
}
