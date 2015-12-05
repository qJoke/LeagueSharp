using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace TestOrbwalker
{
    class Program
    {
        public static Menu Menu;

        public static Orbwalking.Orbwalker OW;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            Menu = new Menu("Test","testOrbwalker", true);
            var owMenu = new Menu("OW","orbwalker");
            {
                OW = new Orbwalking.Orbwalker(owMenu);
                Menu.AddSubMenu(owMenu);
            }
            Menu.AddToMainMenu();
        }
    }
}
