using System;
using DZLib.Logging;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using SoloVayne.Skills.Condemn;
using SoloVayne.Utility;
using SoloVayne.Utility.Enums;

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
            //
        }
    }
}
