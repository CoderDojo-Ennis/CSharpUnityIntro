namespace GeekyMonkey
{
    /// <summary>
    /// Event fired when a forgot password attempt passes or fails
    /// </summary>
    public class GmForgotPasswordEvent : GmGameServicesEvent
    {
        /// <summary>
        /// GamerTag or Email of the account being reset
        /// </summary>
        public string GamerTagOrEmail { get; set; }
    }
}
