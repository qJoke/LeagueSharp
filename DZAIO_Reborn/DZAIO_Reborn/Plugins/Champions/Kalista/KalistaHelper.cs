using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Plugins.Champions.Kalista
{
    static class KalistaHelper
    {
        public static bool HasRend(this Obj_AI_Base target)
        {
            return target.GetRendBuff() != null;
        }

        public static BuffInstance GetRendBuff(this Obj_AI_Base target)
        {
            return target.Buffs.FirstOrDefault(b => b.Caster.IsMe && b.IsValidBuff() && b.DisplayName == "KalistaExpungeMarker");
        }

        public static bool IsRendAboutToExpire(this Obj_AI_Base target)
        {
            return target.GetRendBuff().EndTime - Game.Time < 0.35f;
        }
    }
}
