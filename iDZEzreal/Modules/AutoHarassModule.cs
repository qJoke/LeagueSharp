using System;
using DZLib.Modules;
using iDZEzreal.MenuHelper;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
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
                var target = TargetSelector.GetTargetNoCollision(Variables.Spells[SpellSlot.Q]);
                if (target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range) &&
                    ObjectManager.Player.Distance(target.ServerPosition) <= Variables.Spells[SpellSlot.Q].Range)
                {
                    var prediction = Variables.Spells[SpellSlot.Q].GetSPrediction(target);
                    var castPosition = prediction.CastPosition.Extend((Vector2)ObjectManager.Player.Position, -140);
                    if (prediction.HitChance >= MenuGenerator.GetHitchance())
                    {
                        Variables.Spells[SpellSlot.Q].Cast(castPosition);
                    }
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
