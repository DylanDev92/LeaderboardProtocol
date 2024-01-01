using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BrokeProtocol.API;
using Newtonsoft.Json;

namespace LeaderboardProtocol
{
    /// <summary>
    /// Represents the core functionality of the LeaderboardProtocol plugin.
    /// </summary>
    public class Core : Plugin
    {
        /// <summary>
        /// Gets or sets the configuration settings for the LeaderboardProtocol.
        /// </summary>
        public static Configuration Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the Core class.
        /// </summary>
        public Core()
        {
            Info = new PluginInfo("LeaderboardProtocol", "lbp", "Show off your kills!");

            Configuration = LoadConfig();
        }

        private static readonly string PATH = Path.Combine("Plugins", "LeaderboardProtocol.json");

        /// <summary>
        /// Loads the configuration from the specified file path or creates a default configuration if the file doesn't exist.
        /// </summary>
        /// <returns>The loaded or default configuration.</returns>
        private Configuration LoadConfig()
        {
            if (File.Exists(PATH))
            {
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(PATH));
            }
            else
            {
                Configuration defaultConfig = new Configuration
                {
                    Title = "<color=#FFAF33>Kills Leaderboard</color>",
                    KillsText = " <color=#55FF33>{0} - {1} ({2} Kills) ({3} Deaths)</color>",
                    PlayersShowInTop = 5,
                    PlayerScores = new List<PlayerScore>()
                };
                Save();
                return defaultConfig;
            }
        }

        /// <summary>
        /// Saves the current configuration to the file.
        /// </summary>
        public static void Save()
        {
            File.WriteAllText(PATH, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
        }
    }

    /// <summary>
    /// Represents the configuration settings for the LeaderboardProtocol.
    /// </summary>
    public class Configuration
    {
        public string Title { get; set; }
        public string KillsText { get; set; }
        public int PlayersShowInTop { get; set; }
        public List<PlayerScore> PlayerScores { get; set; }
    }

    /// <summary>
    /// Represents a player's scores in the LeaderboardProtocol.
    /// </summary>
    public class PlayerScore
    {
        public string Username { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }

        /// <summary>
        /// Gets the top player scores based on kills.
        /// </summary>
        /// <returns>The top player scores.</returns>
        public static List<PlayerScore> GetScores()
        {
            return Core.Configuration.PlayerScores.OrderByDescending(score => score.Kills).Take(Core.Configuration.PlayersShowInTop).ToList();
        }

        /// <summary>
        /// Increments the death count for the specified player.
        /// </summary>
        /// <param name="username">The username of the player.</param>
        public static void AddDeath(string username)
        {
            Core.Configuration.PlayerScores.Find(x => x.Username == username).Deaths++;
        }

        /// <summary>
        /// Increments the kill count for the specified player.
        /// </summary>
        /// <param name="username">The username of the player.</param>
        public static void AddKill(string username)
        {
            Core.Configuration.PlayerScores.Find(x => x.Username == username).Kills++;
        }
    }
}
