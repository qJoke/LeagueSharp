using System;
using System.Collections.Generic;
using DZAIO_Reborn.Plugins.Champions.Trundle;
using DZAIO_Reborn.Plugins.Interface;
using LeagueSharp.Common;
using Menu = LeagueSharp.Common.Menu;

namespace DZAIO_Reborn.Core
{
    class Variables
    {
        public static Bootstrap BootstrapInstance;

        public static Menu AssemblyMenu = new Menu("DZAio: Reborn","dzaio", true);

        public static Orbwalking.Orbwalker Orbwalker = new Orbwalking.Orbwalker(AssemblyMenu);

        public static Dictionary<String, Func<IChampion>> ChampList = new Dictionary<string, Func<IChampion>>
        {
            //{"Jinx",() => new Jinx()},
            {"Trundle", () => new  Trundle()}
        };

        public static IChampion CurrentChampion { get; set; }
    }
}
