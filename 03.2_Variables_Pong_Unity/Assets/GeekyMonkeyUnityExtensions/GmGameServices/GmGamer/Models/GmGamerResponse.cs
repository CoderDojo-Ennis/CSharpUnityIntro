using System;
using System.Collections.Generic;

namespace GeekyMonkey
{
    /// <summary>
    /// Gamer info returned from api
    /// </summary>
    [Serializable]
    public class GmGamerResponse : BaseResponse
    {
        /// <summary>
        /// Gamer information
        /// </summary>
        public GmGamerTagModel Gamer { get; set; }
    }
}
