using System.Collections.Generic;
using System.Drawing;
using DZAwarenessAIO.Properties;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;

namespace DZAwarenessAIO.Modules.Tracker
{
    class TrackerVariables
    {
        /// <summary>
        /// Gets a value indicating whether to track allies.
        /// </summary>
        /// <value>
        ///   <c>true</c> if track allies menu item is enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool TrackAllies
        {
            get { return MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.allies"); }
        }

        /// <summary>
        /// Gets a value indicating whether to track enemies.
        /// </summary>
        /// <value>
        ///   <c>true</c> if track enemies menu item is enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool TrackEnemies
        {
            get { return MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.enemies"); }
        }


        /// <summary>
        /// The tracker_ vanilla hud
        /// </summary>
        public static Bitmap Tracker_VanillaHud = Resources.hud_vanilla;

        /// <summary>
        /// The tracker_ exp hud
        /// </summary>
        public static Bitmap Tracker_ExpHud = Resources.hud_exp;

        /// <summary>
        /// The summoner spells resources Dictionary
        /// </summary>
        public static Dictionary<string, Bitmap> summonerSpells = new Dictionary<string, Bitmap>()
        {
            { "itemsmiteaoe", Resources.itemsmiteaoe },
            { "s5_summonersmiteduel", Resources.s5_summonersmiteduel },
            { "s5_summonersmiteplayerganker", Resources.s5_summonersmiteplayerganker },
            { "s5_summonersmitequick", Resources.s5_summonersmitequick },
            { "snowballfollowupcast", Resources.snowballfollowupcast },
            { "summonerbarrier", Resources.summonerbarrier },
            { "summonerboost", Resources.summonerboost },
            { "summonerclairvoyance", Resources.summonerclairvoyance },
            { "summonerdot", Resources.summonerdot },
            { "summonerexhaust", Resources.summonerexhaust },
            { "summonerflash", Resources.summonerflash },
            { "summonerhaste", Resources.summonerhaste },
            { "summonerheal", Resources.summonerheal },
            { "summonermana", Resources.summonermana },
            { "summonerodingarrison", Resources.summonerodingarrison },
            { "summonerpororecall", Resources.summonerpororecall },
            { "summonerporothrow", Resources.summonerporothrow },
            { "summonerrevive", Resources.summonerrevive },
            { "summonersmite", Resources.summonersmite },
            { "summonersnowball", Resources.summonersnowball },
            { "summonerteleport", Resources.summonerteleport },
        } ;
 
        /// <summary>
        /// The spell slots
        /// </summary>
        public static readonly SpellSlot[] SpellSlots = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };

        /// <summary>
        /// The summoners slots
        /// </summary>
        public static readonly SpellSlot[] Summoners = { SpellSlot.Summoner1, SpellSlot.Summoner2 };

        /// <summary>
        /// Gets the tracker hud.
        /// </summary>
        /// <value>
        /// The tracker hud.
        /// </value>
        public static Bitmap TrackerHud
        {
            get { return Tracker_VanillaHud; }
        }
    }
}
