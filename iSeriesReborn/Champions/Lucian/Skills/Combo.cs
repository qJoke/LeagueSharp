using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Lucian.Skills
{
    class Combo
    {
        internal static void ExecuteComboLogic(LeagueSharp.GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Target is Obj_AI_Hero)
            {
                var qReady = Variables.spells[SpellSlot.Q].IsEnabledAndReady();
                var wReady = Variables.spells[SpellSlot.W].IsEnabledAndReady();
                var eReady = Variables.spells[SpellSlot.E].IsEnabledAndReady();
                var target = args.Target as Obj_AI_Hero;

                if (qReady && Variables.spells[SpellSlot.Q].IsInRange(target) && !LucianHooks.HasPassive)
                {
                    Variables.spells[SpellSlot.Q].CastOnUnit(target);
                }

                if (wReady 
                    && !qReady 
                    && !eReady
                    && target.IsValidTarget(Variables.spells[SpellSlot.W].Range) 
                    && !LucianHooks.HasPassive)
                {
                    Variables.spells[SpellSlot.W].CastOnUnit(target);
                }
            }
        }

    }
}
