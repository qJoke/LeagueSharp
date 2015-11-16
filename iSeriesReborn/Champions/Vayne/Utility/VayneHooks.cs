using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.Positioning;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Vayne.Utility
{
    class VayneHooks
    {
        internal static void OnGapCloser(LeagueSharp.Common.ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsValidTarget() &&
                MenuExtensions.GetItemValue<bool>("iseriesr.vayne.misc.general.antigp") &&
                Variables.spells[SpellSlot.E].IsReady())
            {
                Variables.spells[SpellSlot.E].Cast(gapcloser.Sender);
            }
        }

        internal static void OnInterrupt(LeagueSharp.Obj_AI_Hero sender, LeagueSharp.Common.Interrupter2.InterruptableTargetEventArgs args)
        {
            if (args.DangerLevel >= Interrupter2.DangerLevel.Medium 
                && MenuExtensions.GetItemValue<bool>("iseriesr.vayne.misc.general.antigp") 
                && Variables.spells[SpellSlot.E].IsReady() 
                && sender.IsValidTarget(Variables.spells[SpellSlot.E].Range))
            {
                Variables.spells[SpellSlot.E].Cast(sender);
            }
        }

        internal static void BeforeAttack(LeagueSharp.Common.Orbwalking.BeforeAttackEventArgs args)
        {
            if (MenuExtensions.GetItemValue<KeyBind>("iseriesr.vayne.misc.noaastealthex").Active && VayneUtility.IsStealthed())
            {
                var owTarget = Variables.Orbwalker.GetTarget();
                if (owTarget is Obj_AI_Hero)
                {
                    var owHero = owTarget as Obj_AI_Hero;
                    if (owHero.Health < ObjectManager.Player.GetAutoAttackDamage(owHero) * 2)
                    {
                        return;
                    }

                    if (PositioningVariables.EnemiesClose.Count() == 1)
                    {
                        return;
                    }
                    args.Process = false;
                }
            }
        }
    }
}
