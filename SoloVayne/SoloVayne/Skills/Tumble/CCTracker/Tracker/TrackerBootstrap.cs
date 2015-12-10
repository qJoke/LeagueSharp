using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace SoloVayne.Skills.Tumble.CCTracker.Tracker
{
    class TrackerBootstrap
    {
        public Dictionary<Obj_AI_Hero, TrackerModule> modules = new Dictionary<Obj_AI_Hero, TrackerModule>();

        public TrackerBootstrap()
        {
            Init();
        }

        private void Init()
        {
            foreach (var champion in HeroManager.Enemies)
            {
                var module = new TrackerModule(champion);

                modules.Add(champion, module);
            }
        }

        public TrackerModule GetModuleByName(string name)
        {
            foreach (var kvp in modules)
            {
                if (kvp.Key.ChampionName.ToLower() == name.ToLower())
                {
                    return kvp.Value;
                }
            }

            return new TrackerModule();
        }
    }
}
