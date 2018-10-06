namespace GeekyMonkey
{
    /// <summary>
    /// Event fired when the registration verification succeeds or fails
    /// </summary>
    public class GmRegisterVerifyEvent : GmGameServicesEvent
    {
        /// <summary>
        /// Gamer details if the login was successful
        /// </summary>
        public GmGamerTagModel Gamer { get; set; }
    }
}
