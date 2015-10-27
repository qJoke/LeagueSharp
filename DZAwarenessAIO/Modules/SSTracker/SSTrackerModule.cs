using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Utility.Logs;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.SSTracker
{
    class SSTrackerModule
    {
        public static Dictionary<string, HeroTracker> Trackers = new Dictionary<string, HeroTracker>(); 

        public static void OnLoad()
        {
            try
            {
                foreach (var enemy in HeroManager.Enemies)
                {
                    Trackers.Add(enemy.ChampionName, new HeroTracker() { Hero = enemy, LastSeen = -1 });
                }

                AttackableUnit.OnEnterLocalVisiblityClient += OnGainVision;
                Game.OnUpdate += Game_OnUpdate;
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("SSTracker", e, LogSeverity.Severe));
            }
            
        }

        static void Game_OnUpdate(EventArgs args)
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

        private static void OnGainVision(AttackableUnit sender, EventArgs args)
        {
            try
            {
                return;
                if (sender is Obj_AI_Hero && sender.IsEnemy)
                {
                    var hero = Trackers.Values.FirstOrDefault(h => h.Hero.NetworkId.Equals(sender.NetworkId));

                    if (hero != null)
                    {
                        Console.WriteLine(@"Gained vision of {0}", (hero.Hero).ChampionName);

                        hero.LastSeen = -1;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("SSTracker", e, LogSeverity.Error));
            }
            
        }

        private static void OnLoseVision(AttackableUnit sender, EventArgs args)
        {
            try
            {
                if (sender is Obj_AI_Hero && sender.IsEnemy)
                {
                    var hero = Trackers.Values.FirstOrDefault(h => h.Hero.NetworkId.Equals(sender.NetworkId));

                    if (hero != null)
                    {
                        Console.WriteLine(@"Lost vision of {0}", (hero.Hero).ChampionName);

                        hero.LastSeen = Environment.TickCount;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("SSTracker", e, LogSeverity.Error));
            }

        }
    }

    public class HeroTracker
    {
        public Obj_AI_Hero Hero { get; set; }

        public float LastSeen { get; set; }

        public float SSTimeFloat
        {
            get { return LastSeen > -1 ? (float) Math.Ceiling((Environment.TickCount - LastSeen) / 1000 + 0.5) : 0; }
        }

        public string SSTime
        {
            get { return LastSeen > -1 ? Math.Ceiling((Environment.TickCount - LastSeen)/1000 + 0.5).ToString() : ""; }
        }
    }
}
