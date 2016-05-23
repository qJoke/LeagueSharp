using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Helpers.Entity
{
    class DZTargetHelper
    {
        public static Obj_AI_Hero GetNearlyKillableTarget(Spell Spell, TargetSelector.DamageType DamageType)
        {
            var targetSelectorTarget = TargetSelector.GetTarget(Spell.Range, TargetSelector.DamageType.Magical);
            var targetSelectorTargetIsKillable = Spell.GetDamage(targetSelectorTarget) > targetSelectorTarget.Health + 5;
            
            foreach (var target in HeroManager.Enemies.Where(n => n.IsValidTarget(Spell.Range)))
            {
                var SpellDamage = Spell.GetDamage(target);
                if (target.Health + 5 > SpellDamage 
                    && target.Health + 5 < SpellDamage 
                    + ObjectManager.Player.GetAutoAttackDamage(target) 
                    + ObjectManager.Player.GetComboDamage(target, new [] {SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R}
                    .Except(new List<SpellSlot>() { Spell.Slot }).ToList()))
                {
                    return target;
                }
            }

            return targetSelectorTarget;
        }
    }
}
