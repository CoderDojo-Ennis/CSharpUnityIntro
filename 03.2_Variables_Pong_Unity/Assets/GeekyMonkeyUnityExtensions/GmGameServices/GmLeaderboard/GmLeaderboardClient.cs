using System.Threading.Tasks;
using UniRx;
using UniRx.Async;
using UnityEngine.Networking;

namespace GeekyMonkey
{
    /// <summary>
    /// Network client for the Geeky Monkey Leaderboards Service
    /// </summary>
    public static class GmLeaderboardClient
    {
        /// <summary>
        /// Change the gamer's name in all existing leaderboard records for this game (all modes)
        /// </summary>
        /// <remarks>
        /// Raises GmLeaderboardNameChangeEvent on success or failure
        /// </remarks>
        /// <param name="gamerId">Gamer ID</param>
        /// <param name="gamerName">Gamer Name</param>
        public static async UniTask SetGamerNameAsync(string gamerId, string gamerName)
        {
            gamerName = CleanGamerName(gamerName);
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/leaderboards/setname/{GmGameServicesClient.GameId}/{gamerId}/{gamerName}";

            UnityWebRequest uwr = UnityWebRequest.Post(url, "");
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);
            await uwr.SendWebRequest().AsObservable();
            if (uwr.isNetworkError)
            {
                Events.Raise(new GmLeaderboardNameChangeEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    GamerId = gamerId,
                    GamerName = gamerName
                });
            }
            else
            {
                Events.Raise(new GmLeaderboardNameChangeEvent
                {
                    Success = true,
                    GamerId = gamerId,
                    GamerName = gamerName
                });
            }
        }

        /// <summary>
        /// Clean the gamer name
        /// </summary>
        /// <param name="gamerName"></param>
        /// <returns></returns>
        private static string CleanGamerName(string gamerName)
        {
            string cleanName = (gamerName ?? "").Trim();
            // todo - other invalid characters removed
            cleanName = cleanName.Replace("/", "");
            return cleanName;
        }

        /// <summary>
        /// Save the player's name and score to the leaderboard
        /// </summary>
        /// <param name="gameMode">Game Mode (leaderboard name)</param>
        /// <param name="gamerId">Gamer ID</param>
        /// <param name="gamerName">Gamer Name</param>
        /// <param name="score">Score</param>
        /// <returns>Gamer's leaderbaord record</returns>
        public static async UniTask<GmLeaderboardRecord> SetScoreAsync(string gameMode, string gamerId, string gamerName, int score)
        {
            gamerName = CleanGamerName(gamerName);
            if (gamerName == "")
            {
                return null;
            }

            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/leaderboards/set/{GmGameServicesClient.GameId}/{gameMode}/{gamerId}/{gamerName}/{score}";

            UnityWebRequest uwr = UnityWebRequest.Post(url, "");
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);
            await uwr.SendWebRequest().AsObservable();
            if (uwr.isNetworkError)
            {
                Events.Raise(new GmLeaderboardSetScoreEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    LeaderboardRecord = null
                });
                return null;
            }

            string response = uwr.downloadHandler.text;
            var leaderboardRecord = new GmLeaderboardRecord(response);

            Events.Raise(new GmLeaderboardSetScoreEvent
            {
                Success = true,
                LeaderboardRecord = leaderboardRecord
            });

            return leaderboardRecord;
        }

        /// <summary>
        /// Get one player's name and score from the leaderboard
        /// </summary>
        /// <param name="gameMode">Game Mode (leaderboard name)</param>
        /// <param name="gamerId">Gamer ID</param>
        /// <returns>Gamer's leaderbaord record</returns>
        public static async UniTask<GmLeaderboardRecord> GetGamerScore(string gameMode, string gamerId)
        {
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/leaderboards/get/{GmGameServicesClient.GameId}/{gameMode}/{gamerId}";

            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);
            await uwr.SendWebRequest().AsObservable();
            if (uwr.isNetworkError)
            {
                Events.Raise(new GmLeaderboardGetScoreEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    LeaderboardRecord = null
                });
                return null;
            }

            string response = uwr.downloadHandler.text;
            var leaderboardRecord = new GmLeaderboardRecord(response);

            Events.Raise(new GmLeaderboardGetScoreEvent
            {
                Success = true,
                LeaderboardRecord = leaderboardRecord
            });

            return leaderboardRecord;
        }

        /// <summary>
        /// Get all player's names and scores from the leaderboard
        /// </summary>
        /// <param name="gameMode">Game Mode (leaderboard name)</param>
        /// <returns>All leaderbaord records for this mode</returns>
        public static async UniTask<GmLeaderboardModel> GetAllScores(string gameMode)
        {
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/leaderboards/get/{GmGameServicesClient.GameId}/{gameMode}";

            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);
            await uwr.SendWebRequest().AsObservable();
            if (uwr.isNetworkError)
            {
                Events.Raise(new GmLeaderboardGetAllScoresEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    Leaderboard = null
                });
                return null;
            }

            string response = uwr.downloadHandler.text;
            var leaderboard = new GmLeaderboardModel(gameMode, response);

            Events.Raise(new GmLeaderboardGetAllScoresEvent
            {
                Success = true,
                Leaderboard = leaderboard
            });

            return leaderboard;
        }
    }
}
