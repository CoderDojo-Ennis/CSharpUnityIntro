namespace GeekyMonkey
{
    /// <summary>
    /// Event fired when a user is logged in, or the login fails
    /// </summary>
    public class GmLoginEvent : GmGameServicesEvent
    {
        /// <summary>
        /// Gamer details if the login was successful
        /// </summary>
        public GmGamerTagModel Gamer { get; set; }
    }
}
