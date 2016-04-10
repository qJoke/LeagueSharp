using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Modules;
using iDZEzreal.MenuHelper;
using LeagueSharp;
using LeagueSharp.Common;

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
            return Variables.Menu.Item("ezreal.modules.autoq").GetValue<bool>() && Variables.Spells[SpellSlot.Q].IsReady();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range * 0.80f, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range)) return;
            var prediction = Variables.Spells[SpellSlot.Q].GetPrediction(target);
            if (prediction.Hitchance >= MenuGenerator.GetHitchance())
            {
                Variables.Spells[SpellSlot.Q].Cast(target);
            }
        }
    }
}