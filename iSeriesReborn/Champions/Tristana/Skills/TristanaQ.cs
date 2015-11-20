using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Tristana.Skills
{
    class TristanaQ
    {
        internal static void HandleLogic()
        {
            if (Variables.spells[SpellSlot.Q].IsEnabledAndReady())
            {
                var target = Variables.Orbwalker.GetTarget();
                if (target is Obj_AI_Hero)
                {
                    var targetHero = target as Obj_AI_Hero;

                    if (targetHero.IsValidTarget())
                    {
                        //Determine whether we should use Q or not.
                        var enemiesInRange =
                            ObjectManager.Player.GetEnemiesInRange(Orbwalking.GetRealAutoAttackRange(null));
                        if (enemiesInRange.Any())
                        {
                            var maxPriorityHero =
                                enemiesInRange.OrderByDescending(TargetSelector.GetPriority).FirstOrDefault();
                            if (maxPriorityHero != null 
                                && maxPriorityHero.IsValidTarget() 
                                && target.NetworkId == maxPriorityHero.NetworkId)
                            {
                                Variables.spells[SpellSlot.Q].Cast();
                                return;
                            }

                            if (targetHero.Health + 5 < ObjectManager.Player.GetAutoAttackDamage(targetHero, true) * 4)
                            {
                                Variables.spells[SpellSlot.Q].Cast();
                            }
                        }
                       
                    }
                }
            }
        }
    }
}
