using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using LeagueSharp.Common;

namespace DZAwarenessAIO
{
    /// <summary>
    /// The Bootstrap for the assembly.
    /// </summary>
    internal class DZAwarenessBoostrap
    {
        /// <summary>
        /// Called when the assembly is loaded.
        /// </summary>
        public static void OnLoad()
        {
            Variables.Menu = new Menu("DZAwareness", "dz191.dza", true);

            LogHelper.OnLoad();

            foreach (var module in Variables.Modules)
            {
                module.OnLoad();
            }

            Variables.Menu.AddToMainMenu();
        }
    }
}
