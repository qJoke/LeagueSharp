using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Ezreal.Skills
{
    class EzrealQ
    {
        internal static void ExecuteLogic()
        {
            if (Variables.spells[SpellSlot.Q].IsEnabledAndReady())
            {
                var target = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range * 0.75f, TargetSelector.DamageType.Physical);

                if (target.IsValidTarget(Variables.spells[SpellSlot.Q].Range))
                {
                    Variables.spells[SpellSlot.Q].CastIfHitchanceEquals(
                      target, target.IsMoving ? HitChance.VeryHigh : HitChance.High);
                }
            }
        }
    }
}
