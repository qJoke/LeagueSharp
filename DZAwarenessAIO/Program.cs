using System;
using LeagueSharp.Common;

namespace DZAwarenessAIO
{
    class Program
    {
        /// <summary>
        /// The entry point of the solution.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        /// <summary>
        /// Called when the game is loaded.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void Game_OnGameLoad(EventArgs args)
        {
            DZAwarenessBoostrap.OnLoad();
        }
    }
}
