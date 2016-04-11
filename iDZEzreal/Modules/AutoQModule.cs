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
    public class AutoQModule : IModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Auto Q Module Loaded");
        }

        public bool ShouldGetExecuted()
        {
            return Variables.Spells[SpellSlot.Q].IsReady() &&
                   Variables.Menu.Item("ezreal.modules." + GetName().ToLowerInvariant()).GetValue<bool>()
                   && ObjectManager.Player.ManaPercent > 30;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range*0.85f,
                TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
            {
                return;
            }

            var prediction = Variables.Spells[SpellSlot.Q].GetSPrediction(target);

            if (prediction.HitChance >= HitChance.Medium)
            {
                Variables.Spells[SpellSlot.Q].Cast(prediction.CastPosition);
            }
        }

        public string GetName()
        {
            return "Auto Q";
        }
    }
}