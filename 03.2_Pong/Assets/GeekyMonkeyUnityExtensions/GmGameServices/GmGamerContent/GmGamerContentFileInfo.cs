using System;
using System.Collections.Generic;

namespace GeekyMonkey
{
    /// <summary>
    /// File info returned from api
    /// </summary>
    [Serializable]
    public class GmGamerContentFileInfo
    {
        /// <summary>
        /// File name with extension
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Mime Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Optional additional data to store with the file
        /// </summary>
        public Dictionary<string, string> MetaData { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Last file modifiecation date/time
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// E-Tag
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// Content MD5 hash
        /// </summary>
        public string ContentMD5 { get; set; }
    }
}
