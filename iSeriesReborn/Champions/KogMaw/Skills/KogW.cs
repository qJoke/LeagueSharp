﻿using DZLib.Logging;
using iSeriesReborn.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.KogMaw.Skills
{
    class KogW
    {
        internal static void ExecuteLogic()
        {
            
        }

        internal static void OnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (Variables.spells[SpellSlot.W].IsReady())
            {
                if (MenuExtensions.GetItemValue<bool>("iseriesr.kogmaw.misc.w.on.nexus") && args.Target is Obj_HQ)
                {
                    Variables.spells[SpellSlot.W].Cast();
                }

                if (MenuExtensions.GetItemValue<bool>("iseriesr.kogmaw.misc.w.on.inhib") && args.Target is Obj_BarracksDampener)
                {
                    Variables.spells[SpellSlot.W].Cast();
                }

                if (MenuExtensions.GetItemValue<bool>("iseriesr.kogmaw.misc.w.on.nexus") && args.Target is Obj_HQ)
                {
                    Variables.spells[SpellSlot.W].Cast();
                }
            }
            
        }
    }
}
