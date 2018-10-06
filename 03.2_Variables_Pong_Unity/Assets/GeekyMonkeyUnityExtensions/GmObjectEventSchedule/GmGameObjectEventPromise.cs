using System;

namespace GeekyMonkey
{
    public class GmObjectEventPromise
    {
        internal bool IsAborted = false;
        internal bool IsDone = false;
        private Action then;
        public GmObjectEventPromise ChildPromise;

        /// <summary>
        /// Stop execution. "Then" will not be called
        /// </summary>
        public void Abort()
        {
            IsAborted = true;
            if (ChildPromise != null)
            {
                ChildPromise.Abort();
            }
        }

        /// <summary>
        /// Execute an action when this promise is done
        /// </summary>
        /// <param name="thenCallback">The callback action</param>
        public GmObjectEventPromise Then(Action thenCallback)
        {
            if (IsDone)
            {
                thenCallback();
            }
            else
            {
                then += thenCallback;
            }

            return this;
        }

        /// <summary>
        /// Mark this promise as done. "Then" actions will be called.
        /// </summary>
        internal void Done()
        {
            // Can only be done once
            if (!IsDone)
            {
                IsDone = true;
                if (then != null)
                {
                    then();
                }
                if (ChildPromise != null)
                {
                    ChildPromise.Done();
                }
            }
        }
    }
}
