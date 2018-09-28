using System;
using System.Runtime.CompilerServices;

namespace GeekyMonkey
{
    /// <summary>
    /// Enables Awaiting of GmObjectEventPromise
    /// </summary>
    public static class GmAsyncMonoBehaviourEventSchedule
    {
        public static GmMonoBehaviourEventPromiseAwaiter GetAwaiter(this GmMonoBehaviourEventPromise promise)
        {
            return new GmMonoBehaviourEventPromiseAwaiter(promise);
        }
    }

    /// <summary>
    /// Enables Awaiting of GmObjectEventPromise
    /// </summary>
    public struct GmMonoBehaviourEventPromiseAwaiter : INotifyCompletion
    {
        private readonly GmMonoBehaviourEventPromise gmPromise;

        public GmMonoBehaviourEventPromiseAwaiter(GmMonoBehaviourEventPromise promise)
        {
            gmPromise = promise;
        }

        public bool IsCompleted
        {
            get { return gmPromise.isDone; }
        }

        public void OnCompleted(Action continuation)
        {
            gmPromise.Then(continuation);
        }

        public void GetResult() { }
    }
}
