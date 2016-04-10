using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Modules;
using iDZEzreal.MenuHelper;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace iDZEzreal.Modules
{
    public class QKSModule : IModule
    {
        public void OnLoad()
        {
            Console.WriteLine("QKS Module Loaded");
        }

        public bool ShouldGetExecuted()
        {
            return Variables.Spells[SpellSlot.Q].IsReady();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            foreach (var hero in
                HeroManager.Enemies.Where(
                    x =>
                        x.IsValidTarget(Variables.Spells[SpellSlot.Q].Range) &&
                        Variables.Spells[SpellSlot.Q].GetDamage(x) >= x.Health)
                    .Where(hero => Variables.Spells[SpellSlot.Q].IsReady()))
            {
                var SPrediction = Variables.Spells[SpellSlot.Q].GetSPrediction(hero);
                if (SPrediction.HitChance >= HitChance.Medium)
                {
                    Variables.Spells[SpellSlot.Q].Cast(SPrediction.CastPosition);
                }
            }
        }

        public string GetName()
        {
            return "Q KS";
        }
    }
}