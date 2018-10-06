using System.Collections.Generic;

namespace GeekyMonkey
{
    public class GmGamerContentListResponse : BaseResponse
    {
        public List<GmGamerContentFileInfo> Files { get; set; }
    }
}
