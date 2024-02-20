using PAG.Managers; // Import RankManager class from NET.Managers namespace
using System;

namespace PAG.Events
{
    /// <summary>
    /// Static class that provides methods for invoking different game events.
    /// </summary>
    public static class EventRunner
    {
        /// <summary>
        /// Method to start the game.
        /// </summary>
        public static void StartGame()
        {
            // Invoke the LevelStart event through the event manager
            MainManager.Instance.EventManager.InvokeEvent(EventTypes.LevelStart);
        }

        /// <summary>
        /// Method to indicate that a player has completed the race.
        /// </summary>
        /// <param name="args">The arguments to pass along with the event.</param>
        public static void PlayerCompletedRace(EventArgs args)
        {
            // Invoke the PlayerReachedEnd event through the event manager
            MainManager.Instance.EventManager.InvokeEvent(EventTypes.PlayerReachedEnd, args);
        }

        /// <summary>
        /// Method to finish the game.
        /// </summary>
        public static void FinishGame()
        {
            // Invoke the LevelFinish event through the event manager
            MainManager.Instance.EventManager.InvokeEvent(EventTypes.LevelFinish);
        }
    }
}