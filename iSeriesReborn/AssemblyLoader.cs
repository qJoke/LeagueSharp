using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Utility;
using LeagueSharp;

namespace iSeriesReborn
{
    class AssemblyLoader
    {
        public static void OnLoad()
        {
            BuildDefaultMenu();
            LoadChampion();
        }

        private static void BuildDefaultMenu()
        {
            var defaultMenu = Variables.Menu;


            defaultMenu.AddToMainMenu();
        }

        private static void LoadChampion()
        {
            var ChampionToLoad = ObjectManager.Player.ChampionName;

        }
    }
}
