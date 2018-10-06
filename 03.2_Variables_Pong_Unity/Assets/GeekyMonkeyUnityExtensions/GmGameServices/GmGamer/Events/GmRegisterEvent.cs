namespace GeekyMonkey
{
    /// <summary>
    /// Event fired when the first registration step completes or fails
    /// </summary>
    public class GmRegisterEvent : GmGameServicesEvent
    {
        public string GamerId { get; set; }
        public string Email { get; set; }
    }
}
