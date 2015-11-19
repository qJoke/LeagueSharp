using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using iSeriesReborn.Utility.Positioning;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Tristana.Skills
{
    class TristanaE
    {
        internal static void HandleLogic()
        {
            //TODO More Logics in here
            if (Variables.spells[SpellSlot.E].IsEnabledAndReady())
            {
                var eTarget = TargetSelector.GetTarget(TristanaUtility.GetERRange(), TargetSelector.DamageType.Physical);
                if ((eTarget.IsValidTarget() && MenuExtensions.GetItemValue<bool>($"iseriesr.tristana.combo.eon.{eTarget.ChampionName.ToLower()}")) 
                    || PositioningVariables.EnemiesClose.Count() == 1)
                {
                    Variables.spells[SpellSlot.E].Cast(eTarget);
                }
            }
        }
    }
}
