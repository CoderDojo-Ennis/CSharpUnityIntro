using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace GeekyMonkey
{
    /// <summary>
    /// Network client for the Geeky Monkey Leaderboards Service
    /// </summary>
    public static class GmGamerTagClient
    {
        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="gamerTagOrEmail">GamerTag or Email Address</param>
        /// <returns>True if account found and code sent</returns>
        public static async UniTask<bool> ForgotPassword(string gamerTagOrEmail)
        {
            GmGameServicesClient.CheckApiKey();

            gamerTagOrEmail = gamerTagOrEmail.Trim();

            string url = $"{GmGameServicesClient.BaseUrl}/Gamer/forgotpassword/{GmGameServicesClient.GameId}";

            WWWForm form = new WWWForm();
            form.AddField("gamerTagOrEmail", gamerTagOrEmail);

            UnityWebRequest uwr = UnityWebRequest.Post(url, form );
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);

            await uwr.SendWebRequest().AsObservable();

            if (uwr.isNetworkError)
            {
                Events.Raise(new GmForgotPasswordEvent
                {
                    GamerTagOrEmail = gamerTagOrEmail,
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1
                });
                return false;
            }

            string responseText = uwr.downloadHandler.text;
            var responseObject = JsonConvert.DeserializeObject<BaseResponse>(responseText);
            if (responseObject == null)
            {
                Events.Raise(new GmForgotPasswordEvent
                {
                    GamerTagOrEmail = gamerTagOrEmail,
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 2
                });

                return false;
            }

            var evnt = new GmForgotPasswordEvent
            {
                GamerTagOrEmail = gamerTagOrEmail,
                Success = responseObject.Success
            };
            Events.Raise(evnt);

            return responseObject.Success;
        }

        /// <summary>
        /// Forgot Password step 2 - reset the password
        /// </summary>
        /// <param name="gamerTagOrEmail">Gamertag or email</param>
        /// <param name="code">Emailverification code</param>
        /// <param name="password">New password</param>
        /// <returns>True if code was verified and password changed</returns>
        public static async Task<bool> ResetPassword(string gamerTagOrEmail, string code, string password)
        {
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/Gamer/passwordreset/{GmGameServicesClient.GameId}";

            WWWForm form = new WWWForm();
            form.AddField("gamerTagOrEmail", gamerTagOrEmail);
            form.AddField("code", code);
            form.AddField("password", password);

            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);

            await uwr.SendWebRequest().AsObservable();

            if (uwr.isNetworkError)
            {
                Events.Raise(new GmResetPasswordEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1
                });
                return false;
            }

            string responseText = uwr.downloadHandler.text;
            var responseObject = JsonConvert.DeserializeObject<BaseResponse>(responseText);
            if (responseObject == null)
            {
                Events.Raise(new GmResetPasswordEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 2
                });

                return false;
            }

            var evnt = new GmResetPasswordEvent
            {
                Success = responseObject.Success
            };
            Events.Raise(evnt);

            return responseObject.Success;
        }

        /// <summary>
        /// Register phase 1
        /// </summary>
        /// <param name="gamerTag">GamerTag</param>
        /// <param name="email">Email Address</param>
        /// <param name="realName">Real Name (optional)</param>
        /// <param name="password">Password</param>
        /// <returns>Temporary gamerId, or null</returns>
        public static async Task<GmGamerTagModel> Register(string gamerTag, string email, string realName, string password)
        {
            GmGameServicesClient.CheckApiKey();

            gamerTag = gamerTag.Trim();

            string url = $"{GmGameServicesClient.BaseUrl}/gamer/register/{GmGameServicesClient.GameId}";

            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("gamerTag", gamerTag);
            form.AddField("password", password);
            form.AddField("realName", realName);

            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);

            await uwr.SendWebRequest().AsObservable();

            if (uwr.isNetworkError)
            {
                Events.Raise(new GmRegisterEvent
                {
                    GamerId = null,
                    Email = null,
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1
                });
                return null;
            }

            string responseText = uwr.downloadHandler.text;
            var responseObject = JsonConvert.DeserializeObject<GmGamerResponse>(responseText);
            if (responseObject == null)
            {
                Events.Raise(new GmRegisterEvent
                {
                    GamerId = null,
                    Email = null,
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = responseObject.ErrorCode
                });

                return null;
            }

            var evnt = new GmRegisterEvent
            {
                ErrorCode = responseObject.ErrorCode,
                ErrorMessage = responseObject.Error,
                GamerId = (responseObject.Gamer != null) ? responseObject.Gamer.GamerId : null,
                Email = (responseObject.Gamer != null) ? responseObject.Gamer.Email : null,
                Success = responseObject.Success
            };
            Events.Raise(evnt);

            return responseObject.Gamer;
        }

        /// <summary>
        /// Verify email account as the 2nd part of registration
        /// </summary>
        /// <param name="gamerId">Gamer ID</param>
        /// <param name="code">Email code to check</param>
        /// <returns></returns>
        public static async Task<GmGamerTagModel> RegisterVerify(string gamerId, string code)
        {
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/gamer/verifyemail/{GmGameServicesClient.GameId}";

            WWWForm form = new WWWForm();
            form.AddField("gamerId", gamerId);
            form.AddField("emailVerificationCode", code);

            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);

            await uwr.SendWebRequest().AsObservable();

            if (uwr.isNetworkError)
            {
                Events.Raise(new GmRegisterVerifyEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    Gamer = null
                });
                return null;
            }

            string responseText = uwr.downloadHandler.text;
            var responseObject = JsonConvert.DeserializeObject<GmGamerResponse>(responseText);
            if (responseObject == null)
            {
                Events.Raise(new GmRegisterVerifyEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 2,
                    Gamer = null
                });

                return null;
            }

            var evnt = new GmRegisterVerifyEvent
            {
                Success = responseObject.Success,
                Gamer = responseObject.Gamer
            };
            Events.Raise(evnt);

            return responseObject.Gamer;
        }

        /// <summary>
        /// Attempt login
        /// </summary>
        /// <param name="GamerTag">GamerTag or Email Address</param>
        /// <param name="password">password</param>
        /// <returns>Gamer details</returns>
        public static async UniTask<GmGamerTagModel> Login(string GamerTag, string password)
        {
            GmGameServicesClient.CheckApiKey();

            GamerTag = GamerTag.Trim();

            string url = $"{GmGameServicesClient.BaseUrl}/Gamer/login/{GmGameServicesClient.GameId}";

            WWWForm form = new WWWForm();
            form.AddField("GamerTag", GamerTag);
            form.AddField("password", password);

            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);

            await uwr.SendWebRequest().AsObservable();

            if (uwr.isNetworkError)
            {
                Events.Raise(new GmLoginEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    Gamer = null
                });
                return null;
            }

            string responseText = uwr.downloadHandler.text;
            var responseObject = JsonConvert.DeserializeObject<GmGamerResponse>(responseText);
            if (responseObject == null || responseObject.Gamer == null)
            {
                Events.Raise(new GmLoginEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 2,
                    Gamer = null
                });

                return null;
            }

            var evnt = new GmLoginEvent
            {
                Success = responseObject.Success,
                Gamer = responseObject.Gamer
            };
            Events.Raise(evnt);

            return responseObject.Gamer;
        }

    }
}
