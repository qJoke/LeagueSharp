using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.SDK;

namespace DZAIO_Reborn.Helpers.Entity
{
    static class EntityHelper
    {
        internal static bool IsJungleMob(this AttackableUnit Target)
        {
            return Target is Obj_AI_Minion && GameObjects.Jungle.Select(m => m.CharData.BaseSkinName)
                .Contains((Target as Obj_AI_Minion).CharData.BaseSkinName);
        }

        internal static bool PlayerIsClearingJungle()
        {
            //Assume we are clearing jungle based on the last target
            return !PlayerMonitor.GetLastTarget().IsDead && PlayerMonitor.GetLastTarget().IsJungleMob();
        }

        internal static Obj_AI_Hero GetStunnedTarget(float range)
        {
            return HeroManager.Enemies.Where(en => LeagueSharp.Common.Utility.IsValidTarget(en, range) && en.HasBuffOfType(BuffType.Stun))
                .OrderBy(LeagueSharp.Common.TargetSelector.GetPriority).FirstOrDefault();

        }
    }
}
