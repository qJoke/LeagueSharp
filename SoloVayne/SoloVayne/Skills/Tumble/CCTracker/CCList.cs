using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Utility;

namespace SoloVayne.Skills.Tumble.CCTracker
{
    class CCList
    {
        private Dictionary<string, CC> CCDictionary;
 
        public void Init()
        {
            //TODO For Annie. Pyro stacks > 3
            CCDictionary = new Dictionary<string, CC>()
            {
                {"Annie", new CC(SpellSlot.Q, 1400, CCRange.Ranged, CCType.Targetted, () => Variables.tracker.GetModuleByName("Annie").GetChampion() != null
                    ? Variables.tracker.GetModuleByName("Annie").GetChampion().HasBuff("pyromania_particle")
                    : false)},
                {"Annie", new CC(SpellSlot.W, 650f, CCRange.Ranged, CCType.AOEFromChamp, () => Variables.tracker.GetModuleByName("Annie").GetChampion() != null
                    ? Variables.tracker.GetModuleByName("Annie").GetChampion().HasBuff("pyromania_particle")
                    : false)},
                {"Annie", new CC(SpellSlot.R, 1400f, CCRange.Ranged, CCType.AOE, () => Variables.tracker.GetModuleByName("Annie").GetChampion() != null
                    ? Variables.tracker.GetModuleByName("Annie").GetChampion().HasBuff("pyromania_particle")
                    : false)},

            };
        }

        public Obj_AI_Hero GetChampionByName(string name)
        {
            return HeroManager.Enemies.FirstOrDefault(m => m.ChampionName.ToLower() == name.ToLower());
        }
    }
}
