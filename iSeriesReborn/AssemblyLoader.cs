using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn
{
    class AssemblyLoader
    {
        public static void OnLoad()
        {
            BuildDefaultMenu();
            LoadChampion();

            Variables.Menu.AddToMainMenu();
        }

        private static void BuildDefaultMenu()
        {
            var defaultMenu = Variables.Menu;

            var OWMenu = new Menu("[iSR] Orbwalker", "iseriesr.orbwalker");
            {
                Variables.Orbwalker = new Orbwalking.Orbwalker(OWMenu);
                defaultMenu.AddSubMenu(OWMenu);
            }

            var TSMenu = new Menu("[iSR] TS", "iseriesr.ts");
            {
                TargetSelector.AddToMenu(TSMenu);
                defaultMenu.AddSubMenu(TSMenu);
            }
        }

        private static void LoadChampion()
        {
            var ChampionToLoad = ObjectManager.Player.ChampionName;

            if (Variables.ChampList.ContainsKey(ChampionToLoad))
            {
                Variables.ChampList[ChampionToLoad]();
            }
        }
    }
}
