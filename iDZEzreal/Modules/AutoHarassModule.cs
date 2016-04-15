
using System;
using DZLib.Modules;
using iDZEzreal.MenuHelper;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace iDZEzreal.Modules
{
    class AutoHarassModule : IModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Auto Harass Module Loaded.");
        }

        public bool ShouldGetExecuted()
        {
            return Variables.Menu.Item("ezreal.modules." + GetName().ToLowerInvariant()).GetValue<bool>();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            if (Variables.Spells[SpellSlot.Q].IsReady() && Variables.Menu.Item("ezreal.mixed.q").GetValue<bool>())
            {
                var qTarget = TargetSelector.GetTargetNoCollision(Variables.Spells[SpellSlot.Q]);
                if (qTarget.IsValidTarget() && Variables.Spells[SpellSlot.Q].GetSPrediction(qTarget).HitChance >= MenuGenerator.GetHitchance())
                {
                    Variables.Spells[SpellSlot.Q].Cast(qTarget);
                }
            }

            if (Variables.Spells[SpellSlot.W].IsReady() && Variables.Menu.Item("ezreal.mixed.w").GetValue<bool>() && ObjectManager.Player.ManaPercent > 35)
            {
                var qTarget = TargetSelector.GetTargetNoCollision(Variables.Spells[SpellSlot.W]);
                if (qTarget.IsValidTarget() && Variables.Spells[SpellSlot.W].GetSPrediction(qTarget).HitChance >= MenuGenerator.GetHitchance())
                {
                    Variables.Spells[SpellSlot.W].Cast(qTarget);
                }
            }
        }

        public string GetName()
        {
            return "AutoHarass";
        }
    }
}
