using System;
using System.Collections.Generic;

namespace GeekyMonkey
{
    /// <summary>
    /// One entry in the leaderboard
    /// </summary>
    public class GmLeaderboardRecord
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public GmLeaderboardRecord()
        {
        }

        /// <summary>
        /// Crate a record from an api response string
        /// </summary>
        /// <remarks>
        /// Example: 11111|123|Boop|0
        /// </remarks>
        /// <param name="apiResponse"></param>
        public GmLeaderboardRecord(string apiResponse)
        {
            // Blank is OK for a new user
            if (!string.IsNullOrEmpty(apiResponse))
            {
                try
                {
                    string[] chunks = apiResponse.Split(columnDelimeters);
                    GamerId = chunks[0];
                    Score = int.Parse(chunks[1]);
                    GamerName = chunks[2];
                    Index = int.Parse(chunks[3]);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error parsing leaderboard record: '{apiResponse}'", ex);
                }
            }
        }

        /// <summary>
        /// Delimiter used for parsing
        /// </summary>
        static char[] columnDelimeters = new char[] { '|' };

        /// <summary>
        /// Unique gamer identifier.
        /// This could be a Geeky Monkey GamerTag ID or a device ID
        /// </summary>
        public string GamerId { get; set; }

        /// <summary>
        /// Name to display in the leaderboard
        /// </summary>
        public string GamerName { get; set; }

        /// <summary>
        /// The player's best score for the current game mode
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// This user's index in the leaderboard
        /// </summary>
        public int Index { get; set; }
    }

    /// <summary>
    /// A complete leaderboard
    /// </summary>
    public class GmLeaderboardModel
    {
        /// <summary>
        /// Construct the model empty
        /// </summary>
        public GmLeaderboardModel()
        {
            LeaderbaordRecords = new List<GmLeaderboardRecord>();
        }

        /// <summary>
        /// Construct the model from an api response
        /// </summary>
        public GmLeaderboardModel(string gameMode, string apiResponse)
        {
            GameMode = gameMode;
            LeaderbaordRecords = new List<GmLeaderboardRecord>();
            string[] rows = apiResponse.Split(rowDelimeters, StringSplitOptions.None);
            foreach(string row in rows)
            {
                if (!string.IsNullOrEmpty(row.Trim()))
                {
                    LeaderbaordRecords.Add(new GmLeaderboardRecord(row));
                }
            }
        }

        /// <summary>
        /// Delimiter used for parsing
        /// </summary>
        static string[] rowDelimeters = new string[] { "\r\n", "\r", "\n" };

        /// <summary>
        /// If your game supports seprate leaderboards for different game modes,
        /// this is where you specify the mode you're interrested in
        /// </summary>
        public string GameMode { get; set; }

        /// <summary>
        /// All records in this leaderboard
        /// </summary>
        public List<GmLeaderboardRecord> LeaderbaordRecords { get; set; }
    }
}
