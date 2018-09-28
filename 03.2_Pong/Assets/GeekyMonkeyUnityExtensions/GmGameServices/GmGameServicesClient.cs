using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeekyMonkey
{
    /// <summary>
    /// Network client for the Geeky Monkey Service
    /// </summary>
    public class GmGameServicesClient
    {
        /// <summary>
        /// Game specific Game Name used to secure all storage requests
        /// </summary>
        /// <remarks>
        /// Must be set in yor game before making any other requests.
        /// Contact quinn@geekymonkey.com to request an API key and Game ID
        /// This is case sensitive and must be entered exactly as given
        /// </remarks>
        public static string GameId = null;

        /// <summary>
        /// Game specific API key used to secure all storage requests
        /// </summary>
        /// <remarks>
        /// Must be set in yor game before making any other requests.
        /// Contact quinn@geekymonkey.com to request an API key and Game ID
        /// </remarks>
        public static string GameApiKey = null;

        /// <summary>
        /// Domain for all requests
        /// </summary>
        public static readonly string BaseUrl = "https://gameservices.geekymonkey.com";
        //public static readonly string BaseUrl = "http://localhost:50663";

        /// <summary>
        /// Check that the API key has been set before making a request
        /// </summary>
        internal static void CheckApiKey()
        {
            if (string.IsNullOrEmpty(GameId) || string.IsNullOrEmpty(GameApiKey))
            {
                throw new Exception("GmGameServicesClient requires GameId and GameApiKey be initialized before making any network requests.");
            }
        }
    }
}
