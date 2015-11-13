using System.Linq;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.Entities;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Jinx.Skills
{
    class JinxE
    {
        public static void HandleELogic()
        {
            var ESpell = Variables.spells[SpellSlot.E];
            if (ESpell.IsEnabledAndReady())
            {
                var meleeEnemiesOnMe =
                    GameObjects.EnemyHeroes.Where(
                        enemy =>
                            enemy.IsMelee &&
                            enemy.Distance(ObjectManager.Player.ServerPosition) < enemy.AttackRange + 65f).ToList();
                var lowHealth = ObjectManager.Player.HealthPercent < 15;

                if (meleeEnemiesOnMe.Any(m => !m.IsRunningAway()) && lowHealth)
                {
                    //There are enemies on me, I am low(ish) health, I cast E on myself to peel them.
                    ESpell.Cast(ObjectManager.Player.ServerPosition);
                }

                var selectedTarget = TargetSelector.GetTarget(Variables.spells[SpellSlot.E].Range * 0.75f, TargetSelector.DamageType.Physical);

                if (selectedTarget.IsValidTarget())
                {
                    //The selected target is valid.
                    if (selectedTarget.HasBuffOfType(BuffType.Slow) && selectedTarget.Path.Count() > 1)
                    {
                        //Target is slowed.
                        var slowEndTime = JinxUtility.GetSlowEndTime(selectedTarget);
                        if (slowEndTime >= ESpell.Delay + 0.5f + Game.Ping / 2f)
                        {
                            ESpell.CastIfHitchanceEquals(selectedTarget, HitChance.VeryHigh);
                        }
                    } 
                    else if (JinxUtility.IsHeavilyImpaired(selectedTarget))
                    {
                        //The target is actually impaired heavily. Let's cast E on them.
                        var immobileEndTime = JinxUtility.GetImpairedEndTime(selectedTarget);
                        if (immobileEndTime >= ESpell.Delay + 0.5f + Game.Ping / 2f)
                        {
                            ESpell.CastIfHitchanceEquals(selectedTarget, HitChance.VeryHigh);
                        }
                    } else if (selectedTarget.GetEnemiesInRange(350f).Count() >= 2 
                        && ESpell.GetPrediction(selectedTarget).Hitchance >= HitChance.High)
                    {
                        //We can almost certainly hit our targets and also at least 2 other targets.
                        var enemiesInRange = selectedTarget.GetEnemiesInRange(350f);
                        if (enemiesInRange.Count(enemy => ESpell.GetPrediction(enemy).Hitchance >= HitChance.High) >= 2)
                        {
                            //Cast E.
                            ESpell.Cast(ESpell.GetPrediction(selectedTarget).CastPosition);
                        }
                    }
                }
            }
        }
    }
}
