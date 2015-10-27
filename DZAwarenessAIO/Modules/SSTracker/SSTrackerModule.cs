using System;
using System.Collections.Generic;
using System.Linq;
using DZAwarenessAIO.Utility.Logs;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.SSTracker
{

    /// <summary>
    /// The missing in action time tracker.
    /// </summary>
    class SSTrackerModule
    {
        /// <summary>
        /// The various Hero position trackers.
        /// </summary>
        public static Dictionary<string, HeroTracker> Trackers = new Dictionary<string, HeroTracker>();

        /// <summary>
        /// Init point of the class.
        /// </summary>
        public static void OnLoad()
        {
            try
            {
                foreach (var enemy in HeroManager.Enemies)
                {
                    Trackers.Add(enemy.ChampionName, new HeroTracker() { Hero = enemy, LastSeen = -1 });
                }

                Game.OnUpdate += OnUpdate;
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("SSTracker", e, LogSeverity.Severe));
            }
            
        }

        /// <summary>
        /// Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnUpdate(EventArgs args)
        {
            foreach (var h in HeroManager.Enemies)
            {
                var hero = Trackers.Values.FirstOrDefault(h2 => h2.Hero.NetworkId.Equals(h.NetworkId));
                if (hero != null)
                {
                    if (hero.LastSeen < 0 && !h.IsVisible)
                    {
                        hero.LastSeen = Environment.TickCount;
                    }
                    if (hero.SSTimeFloat > 1 && (h.IsVisible))
                    {
                        hero.LastSeen =  -1;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Tracks the enemy missing time.
    /// </summary>
    public class HeroTracker
    {
        /// <summary>
        /// Gets or sets the hero instance.
        /// </summary>
        /// <value>
        /// The hero instance.
        /// </value>
        public Obj_AI_Hero Hero { get; set; }

        /// <summary>
        /// Gets or sets the last seen tick.
        /// </summary>
        /// <value>
        /// The last seen tick.
        /// </value>
        public float LastSeen { get; set; }

        /// <summary>
        /// Gets the MIA time (float).
        /// </summary>
        /// <value>
        /// The MIA time (float).
        /// </value>
        public float SSTimeFloat
        {
            get { return LastSeen > -1 ? (float) Math.Ceiling((Environment.TickCount - LastSeen) / 1000 + 0.5) : 0; }
        }

        /// <summary>
        /// Gets the MIA time.
        /// </summary>
        /// <value>
        /// The MIA time.
        /// </value>
        public string SSTime
        {
            get { return LastSeen > -1 ? Math.Ceiling((Environment.TickCount - LastSeen)/1000 + 0.5).ToString() : ""; }
        }
    }
}
