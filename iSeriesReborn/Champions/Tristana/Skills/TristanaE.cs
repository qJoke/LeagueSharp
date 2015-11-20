using System.Linq;
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
