using System.Collections.Generic;

namespace GeekyMonkey
{
    public class GmGamerContentListEvent : GmGameServicesEvent
    {
        public List<GmGamerContentFileInfo> FileInfos { get; set; }
    }
}
