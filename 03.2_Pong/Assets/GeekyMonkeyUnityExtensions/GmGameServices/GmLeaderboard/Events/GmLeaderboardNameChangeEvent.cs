namespace GeekyMonkey
{
    class GmLeaderboardNameChangeEvent : GmGameServicesEvent
    {
        public string GamerId { get; set; }

        public string GamerName { get; set; }
    }
}
