using System;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.HudUtility;
using DZAwarenessAIO.Utility.Logs;
using LeagueSharp;
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
            HudDisplay.OnLoad();

            foreach (var enemy in HeroManager.Allies)
            {
                ImageLoader.AddedHeroes.Add(enemy.ChampionName, new HeroHudImage(enemy.ChampionName));
            }
          
            foreach (var module in Variables.Modules)
            {
                module.OnLoad();
            }

            Variables.Menu.AddToMainMenu();

            DZAwareness.OnLoad();
        }
    }
}
