using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GeekyMonkey
{
    /// <summary>
    /// Extenstions for async/await
    /// </summary>
    public static class GmAsyncExtensions
    {
        /// <summary>
        /// Awaiter for TimeSpan
        /// ex:   await TimeSpan.FromSeconds(2.5)
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan).GetAwaiter();
        }
    }
}
