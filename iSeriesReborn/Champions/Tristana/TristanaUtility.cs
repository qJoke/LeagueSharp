using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;

namespace iSeriesReborn.Champions.Tristana
{
    class TristanaUtility
    {
        public static float GetERRange()
        {
            return 550 + (7 * (ObjectManager.Player.Level - 1));
        }

        public static BuffInstance GetEBuff(Obj_AI_Base target)
        {
            return target.Buffs.FirstOrDefault(x => x.DisplayName == "TristanaECharge" || x.Name == "TristanaECharge");
        }

        public static bool HasE(Obj_AI_Base target)
        {
            return GetEBuff(target) != null;
        }

        public static bool CanKillWithER(Obj_AI_Base target)
        {
            //TODO Execution calculations
            return false;
        }
    }
}
