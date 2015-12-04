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

        public void Execute()
        {
            if (Variables.spells[SpellSlot.E].IsEnabledAndReady())
            {
                var target = Provider.GetTarget();
                if (target != null)
                {
                    Variables.spells[SpellSlot.E].Cast(target);
                }
            }
        }
    }
}
