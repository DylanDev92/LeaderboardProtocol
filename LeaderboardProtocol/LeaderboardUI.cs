using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LeaderboardProtocol
{
    /// <summary>
    /// Handles events related to the leaderboard user interface.
    /// </summary>
    class LeaderboardUI : ManagerEvents
    {
        /// <summary>
        /// Event triggered to save the current state.
        /// </summary>
        /// <returns>Returns true if the event was handled successfully.</returns>
        [Execution(ExecutionMode.Event)]
        public override bool Save()
        {
            Core.Save();
            return true;
        }

        /// <summary>
        /// Event triggered when the manager starts.
        /// </summary>
        /// <returns>Returns true if the event was handled successfully.</returns>
        [Execution(ExecutionMode.Event)]
        public override bool Start()
        {
            SvManager.Instance.StartCoroutine(ShowAndUpdateUI());
            return true;
        }

        /// <summary>
        /// Coroutine to continuously update and display the leaderboard UI.
        /// </summary>
        /// <returns>Returns an enumerator for the coroutine.</returns>
        private IEnumerator ShowAndUpdateUI()
        {
            while (true)
            {
                string panelText = ParseScoresToText();

                foreach (ShPlayer player in EntityCollections.Humans)
                {
                    player.svPlayer.SendTextPanel(panelText);
                }

                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// Parses player scores into a formatted text for display.
        /// </summary>
        /// <returns>Returns the formatted text containing player scores.</returns>
        private string ParseScoresToText()
        {
            return $"{Core.Configuration.Title}\n" + string.Join("\n", PlayerScore.GetScores()
                .Select((score, index) => $"{string.Format(Core.Configuration.KillsText, index + 1, score.Username, score.Kills, score.Deaths)}"));
        }
    }
}
