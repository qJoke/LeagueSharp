using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Tristana.Skills
{
    class TristanaR
    {
        internal static void HandleLogic()
        {
            //
            if (Variables.spells[SpellSlot.R].IsEnabledAndReady())
            {
                var selectedTarget = TargetSelector.GetTarget(
                    TristanaUtility.GetERRange(), TargetSelector.DamageType.Physical);
                if (selectedTarget.IsValidTarget())
                {
                    var selectedTargetHealth = HealthPrediction.GetHealthPrediction(
                        selectedTarget,
                        (int)
                            (250 + Game.Ping / 2f + ObjectManager.Player.Distance(selectedTarget.ServerPosition) / 2000f));
                    if (selectedTargetHealth > 0 && selectedTargetHealth < TristanaUtility.GetRDamage(selectedTarget))
                    {
                        Variables.spells[SpellSlot.R].Cast(selectedTarget);
                    }

                    var enemiesClose =
                        ObjectManager.Player.GetEnemiesInRange(250f).OrderBy(m => m.Distance(ObjectManager.Player));
                    if (selectedTargetHealth > ObjectManager.Player.Health * 1.5f)
                    {
                        
                    }
                }
            }
        }
    }
}
