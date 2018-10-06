using System.Collections.Generic;

namespace GeekyMonkey
{
    public class GmGamerContentDownloadEvent : GmGameServicesEvent
    {
        public byte[] FileBytes { get; set; }

        public string FileName { get; set; }

        public string GamerId { get; set; }
    }
}
