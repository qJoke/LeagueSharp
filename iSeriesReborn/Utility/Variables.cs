using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Champions;
using iSeriesReborn.Champions.Kalista;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Utility
{
    class Variables
    {
        /// <summary>
        /// Gets or sets the assembly menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public static Menu Menu { get; set; } = new Menu($"iSeries: Reborn - {ObjectManager.Player.ChampionName}", "iseriesr", true);

        /// <summary>
        /// Gets or sets the orbwalker.
        /// </summary>
        /// <value>
        /// The orbwalker.
        /// </value>
        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        public static readonly Dictionary<string, Action> ChampList = new Dictionary<string, Action>()
        {
            { "Kalista", () => { CurrentChampion = new Kalista(); CurrentChampion.OnLoad(); } }
        };

        public static ChampionBase CurrentChampion { get; set; }

        public static bool IsLoaded => CurrentChampion != null;
    }
}
