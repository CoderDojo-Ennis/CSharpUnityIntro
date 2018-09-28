using System;
using System.Collections.Generic;

namespace GeekyMonkey
{
    /// <summary>
    /// File info returned from api
    /// </summary>
    [Serializable]
    public class GmGamerContentFileInfoResponse : BaseResponse
    {
        /// <summary>
        /// File information
        /// </summary>
        public GmGamerContentFileInfo FileInfo { get; set; }
    }
}
