using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DZAwarenessAIO.Modules;
using DZAwarenessAIO.Modules.Gank_Alerter;
using DZAwarenessAIO.Modules.LaneStatus;
using DZAwarenessAIO.Modules.Ping;
using DZAwarenessAIO.Modules.Ranges;
using DZAwarenessAIO.Modules.SSTracker;
using DZAwarenessAIO.Modules.TFHelper;
using DZAwarenessAIO.Modules.Tracker;
using DZAwarenessAIO.Modules.WardTracker;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Utility
{
    class Variables
    {
        /// <summary>
        /// The modules list
        /// </summary>
        public static List<ModuleBase> Modules = new List<ModuleBase>()
        {
            new TrackerBase(),
            new RangesBase(),
            new WardTrackerBase(),
            new TFHelperBase(),
            new SSTrackerBase(),
            new PingTrackerBase(),
            //new LaneStatusBase(),
            // new GankAlerterBase(),
        };

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public static Menu Menu { get; set; }

        /// <summary>
        /// Gets the leaguesharp application data folder.
        /// </summary>
        /// <value>
        /// The leaguesharp application data folder.
        /// </value>
        public static string LeagueSharpAppData
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "LS" + Environment.UserName.GetHashCode().ToString("X"));
            }
        }

        /// <summary>
        /// Gets the working dir.
        /// </summary>
        /// <value>
        /// The working dir.
        /// </value>
        public static String WorkingDir
        {
            get { return Path.Combine(LeagueSharpAppData, "DZAwareness"); }
        }

        /// <summary>
        /// Gets the enemy team.
        /// </summary>
        /// <value>
        /// The enemy team.
        /// </value>
        public static string[] EnemyTeam
        {
            get { return HeroManager.Enemies.Select(en => en.ChampionName).ToArray(); }
        }

        /// <summary>
        /// Gets the own team.
        /// </summary>
        /// <value>
        /// The own team.
        /// </value>
        public static string[] OwnTeam
        {
            get { return HeroManager.Allies.Select(en => en.ChampionName).ToArray(); }
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>
        /// The assembly version.
        /// </value>
        public static string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        /// <summary>
        /// Gets the game version.
        /// </summary>
        /// <value>
        /// The game version.
        /// </value>
        public static string GameVersion
        {
            get { return Game.Version; }
        }

        /// <summary>
        /// Gets the game region.
        /// </summary>
        /// <value>
        /// The game region.
        /// </value>
        public static string GameRegion
        {
            get { return Game.Region; }
        }

        /// <summary>
        /// Gets the player instance.
        /// </summary>
        /// <value>
        /// The player instance.
        /// </value>
        public static Obj_AI_Hero Player => ObjectManager.Player;

        /// <summary>
        /// Gets the enemies close.
        /// </summary>
        /// <value>
        /// The enemies close.
        /// </value>
        public static IEnumerable<Obj_AI_Hero> EnemiesClose
        {
            get
            {
                return
                    HeroManager.Enemies.Where(
                        m =>
                            m.Distance(ObjectManager.Player, true) <= Math.Pow(1000, 2) && m.IsValidTarget(1500, false) &&
                            m.CountEnemiesInRange(m.IsMelee() ? m.AttackRange * 1.5f : m.AttackRange + 20 * 1.5f) > 0);
            }
        }

        /// <summary>
        /// Gets the allies close.
        /// </summary>
        /// <value>
        /// The allies close.
        /// </value>
        public static IEnumerable<Obj_AI_Hero> AlliesClose
        {
            get
            {
                return
                    HeroManager.Allies.Where(
                        m => !m.IsMe &&
                            m.Distance(ObjectManager.Player, true) <= Math.Pow(1000, 2) && m.IsValidTarget(1000, false));
            }
        }

        /// <summary>
        /// The minimum ss time
        /// </summary>
        public static int MinSSTime => MenuExtensions.GetItemValue<Slider>("dz191.dza.sstracker.mintime").Value;
    }
}
