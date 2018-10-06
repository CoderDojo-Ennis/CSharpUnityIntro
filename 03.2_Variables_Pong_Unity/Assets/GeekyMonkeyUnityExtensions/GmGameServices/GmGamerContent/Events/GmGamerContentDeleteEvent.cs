namespace GeekyMonkey
{
    /// <summary>
    /// Event fires at the end of a gamer content file delete
    /// </summary>
    public class GmGamerContentDeleteEvent : GmGameServicesEvent
    {
        /// <summary>
        /// Gamer owning the file that was deleted
        /// </summary>
        public string GamerId { get; set; }

        /// <summary>
        /// Deleted file name
        /// </summary>
        public string FileName { get; set; }
    }
}
