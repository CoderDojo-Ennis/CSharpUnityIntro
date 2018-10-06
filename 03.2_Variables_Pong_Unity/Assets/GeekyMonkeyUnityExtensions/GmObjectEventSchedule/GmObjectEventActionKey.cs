using System;

namespace GeekyMonkey
{
    /// <summary>
    /// The key for a scheduled action
    /// </summary>
    public class GmObjectEventActionKey : IComparable
    {
        /// <summary>
        /// Each action gets a unique key - used as a tiebreaker when sorting
        /// </summary>
        public static int NextId;

        /// <summary>
        /// The scheduled time - primary sort key
        /// </summary>
        public float ScheduledTime;

        /// <summary>
        /// A unique identifier used as a secondary sort key
        /// </summary>
        public int Id;

        /// <summary>
        /// Construct the key
        /// </summary>
        /// <param name="scheduledTime"></param>
        public GmObjectEventActionKey(float scheduledTime)
        {
            NextId++;
            Id = NextId;
            ScheduledTime = scheduledTime;
        }

        /// <summary>
        /// Compare - used for sorting
        /// </summary>
        /// <param name="obj">Ohter scheduled action key</param>
        /// <returns>-1, 0, or 1</returns>
        public int CompareTo(object obj)
        {
            int compareTime = (ScheduledTime.CompareTo(((GmObjectEventActionKey)obj).ScheduledTime));
            if (compareTime == 0)
            {
                return Id.CompareTo(((GmObjectEventActionKey)obj).Id);
            }
            return compareTime;
        }
    }
}
