using System;

namespace GeekyMonkey
{
    /// <summary>
    /// File delete response
    /// </summary>
    [Serializable]
    public class GmGamerContentFileDeleteResponse : BaseResponse
    {
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gamer ID
        /// </summary>
        public string GamerId { get; set; }
    }
}
