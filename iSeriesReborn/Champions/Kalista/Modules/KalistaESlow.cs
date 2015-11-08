using System;
using System.Linq;
using DZLib.Logging;
using iSeriesReborn.Champions.Kalista.Skills;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.Entities;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Kalista.Modules
{
    class KalistaESlow : IModule
    {
        private float LastCastTime = 0f;

        public string GetName()
        {
            return "Kalista_ESlow";
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldRun()
        {
            return Variables.spells[SpellSlot.E].IsReady() &&
                   MenuExtensions.GetItemValue<bool>("iseriesr.kalista.combo.useeslow");
        }

        public void Run()
        {
            var minions =
                GameObjects.EnemyMinions.Where(
                    minion =>
                        minion.IsValidTarget(Variables.spells[SpellSlot.E].Range) && KalistaE.CanBeRendKilled(minion));

            var heroWithRendStack =
                        HeroManager.Enemies.Where(
                            target =>
                                target.IsValidTarget(Variables.CurrentChampion.GetSpells()[SpellSlot.E].Range) &&
                                target.HasRend()).OrderByDescending(KalistaE.GetRendDamage).FirstOrDefault();

            if (heroWithRendStack != null 
                && minions.Any() 
                &&  heroWithRendStack.Distance(ObjectManager.Player) < Orbwalking.GetRealAutoAttackRange(null) * 1.4f 
                && (Environment.TickCount - LastCastTime > 250))
            {
                Variables.spells[SpellSlot.E].Cast();
                LastCastTime = Environment.TickCount;
            }
        }
    }
}
