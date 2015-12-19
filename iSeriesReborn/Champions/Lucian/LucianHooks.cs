using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Champions.Lucian.Skills;
using iSeriesReborn.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Lucian
{
    class LucianHooks
    {
        public static bool HasPassive = false;

        internal static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            switch (Variables.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo.ExecuteComboLogic(args);
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:

                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:

                    break;
            }
        }

        internal static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            switch (args.Slot)
            {
                case SpellSlot.E:
                case SpellSlot.Q:
                case SpellSlot.W:
                    HasPassive = true;
                    break;
            }
        }

        internal static void OnAfterAA(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            HasPassive = false;
        }
    }
}
