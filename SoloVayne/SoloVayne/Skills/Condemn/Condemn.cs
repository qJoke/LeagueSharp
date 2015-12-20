using System;
using System.Linq;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Skills.Condemn;
using SoloVayne.Utility;
using SoloVayne.Utility.Entities;
using SoloVayne.Utility.Enums;
using SOLOVayne.Utility.General;

namespace SoloVayne.Skills.Tumble
{
    class Condemn : Skill
    {
        public CondemnLogicProvider Provider = new CondemnLogicProvider();

        public Condemn()
        {
            Variables.spells[SpellSlot.E].SetTargetted(0.25f, 1250f);
        }

        public SkillMode GetSkillMode()
        {
            return SkillMode.OnUpdate;
        }

        public void Execute(Obj_AI_Base target)
        {
            try
            {
                if (Variables.spells[SpellSlot.E].IsEnabledAndReady())
                {
                    var CondemnTarget = Provider.GetTarget();
                    if (target != null)
                    {
                        Variables.spells[SpellSlot.E].Cast(CondemnTarget);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Condemn", e, LogSeverity.Error));
            }
        }

        public void ExecuteFarm(Obj_AI_Base target)
        {
            if (target is Obj_AI_Minion 
                && ObjectManager.Player.ManaPercent > 40 
                && ObjectManager.Player.CountEnemiesInRange(2000f) == 0)
            {
                if (GameObjects.JungleLarge.Contains(target) && Provider.IsCondemnable(target, ObjectManager.Player.ServerPosition))
                {
                    Variables.spells[SpellSlot.E].Cast(target);
                    return;
                }
            }
        }
    }
}
