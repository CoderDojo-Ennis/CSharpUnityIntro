using System;
using System.Runtime.CompilerServices;

namespace GeekyMonkey
{
    /// <summary>
    /// Enables Awaiting of GmObjectEventPromise
    /// </summary>
    public static class GmAsyncObjectEventSchedule
    {
        public static GmObjectEventPromiseAwaiter GetAwaiter(this GmObjectEventPromise promise)
        {
            return new GmObjectEventPromiseAwaiter(promise);
        }
    }

    /// <summary>
    /// Enables Awaiting of GmObjectEventPromise
    /// </summary>
    public struct GmObjectEventPromiseAwaiter : INotifyCompletion
    {
        private readonly GmObjectEventPromise gmPromise;

        public GmObjectEventPromiseAwaiter(GmObjectEventPromise promise)
        {
            gmPromise = promise;
        }

        public bool IsCompleted
        {
            get { return gmPromise.IsDone; }
        }

        public void OnCompleted(Action continuation)
        {
            gmPromise.Then(continuation);
        }

        public void GetResult() { }
    }
}
