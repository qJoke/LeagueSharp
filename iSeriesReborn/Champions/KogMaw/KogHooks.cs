using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Champions.KogMaw.Skills;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.KogMaw
{
    class KogHooks
    {
        internal static void BeforeAttack(LeagueSharp.Common.Orbwalking.BeforeAttackEventArgs args)
        {
            KogW.OnBeforeAttack(args);
        }

        internal static void OnBuffAdd(LeagueSharp.Obj_AI_Base sender, LeagueSharp.Obj_AI_BaseBuffAddEventArgs args)
        {
            
        }

        internal static void OnBuffRemove(LeagueSharp.Obj_AI_Base sender, LeagueSharp.Obj_AI_BaseBuffRemoveEventArgs args)
        {
            
        }
    }
}
