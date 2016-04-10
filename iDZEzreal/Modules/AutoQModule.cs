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

        public string GetName()
        {
            return "Auto Q Module";
        }

        public bool ShouldGetExecuted()
        {
<<<<<<< HEAD
            return true;
            //return Variables.Menu.Item("ezreal.modules.autoq").GetValue<bool>() && Variables.Spells[SpellSlot.Q].IsReady();
=======
            return Variables.Menu.Item("ezreal.modules.autoq").GetValue<bool>() 
                && Variables.Spells[SpellSlot.Q].IsReady() 
                && ObjectManager.Player.ManaPercent > 30;
>>>>>>> 613cf4c995a8d42ca560c66f07972d3a4c3cf284
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range * 0.85f, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
            {
                return;
            }

            var prediction = Variables.Spells[SpellSlot.Q].GetSPrediction(target);
            
            if (prediction.HitChance >= MenuGenerator.GetHitchance())
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