namespace GeekyMonkey
{
    /// <summary>
    /// Json response base
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// Succsss or fail
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public int ErrorCode { get; set; }
    }
}
