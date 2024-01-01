using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardProtocol
{
    /// <summary>
    /// Handles events related to player kills and leaderboard initialization.
    /// </summary>
    class LeaderboardKills : PlayerEvents
    {
        /// <summary>
        /// Event triggered upon player death.
        /// </summary>
        /// <param name="destroyable">The destroyed entity.</param>
        /// <param name="killer">The player who caused the death.</param>
        /// <returns>Returns true if the event was handled successfully.</returns>
        [Execution(ExecutionMode.Event)]
        public override bool Death(ShDestroyable destroyable, ShPlayer killer)
        {
            if (destroyable is ShPlayer killed && killed.isHuman)
            {
                PlayerScore.AddDeath(killed.username);
                PlayerScore.AddKill(killer.username);
            }
            return true;
        }

        /// <summary>
        /// Event triggered upon player initialization.
        /// </summary>
        /// <param name="entity">The initialized entity.</param>
        /// <returns>Returns true if the event was handled successfully.</returns>
        [Execution(ExecutionMode.Event)]
        public override bool Initialize(ShEntity entity)
        {
            if (entity is ShPlayer player && !Core.Configuration.PlayerScores.Any(x => x.Username == player.username) && player.isHuman)
            {
                Core.Configuration.PlayerScores.Add(new PlayerScore { Username = player.username, Deaths = 0, Kills = 0 });
            }
            return true;
        }
    }
}
