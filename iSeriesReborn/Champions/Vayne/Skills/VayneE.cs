using System;
using System.Linq;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.Entities;
using iSeriesReborn.Utility.MenuUtility;
using iSeriesReborn.Utility.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace iSeriesReborn.Champions.Vayne.Skills
{
    class VayneE
    {
        //This retard Dev still can't fix Condemn??????? EleGiggle
        public static void HandleELogic()
        {
            if (Variables.spells[SpellSlot.E].IsEnabledAndReady())
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(h => !h.IsInvulnerable && !TargetSelector.IsInvulnerable(h, TargetSelector.DamageType.Physical, false) && h.IsValidTarget()))
                {
                    var pushDistance = 420;
                    var accuracy = 0.35f;
                    var targetPosition = Variables.spells[SpellSlot.E].GetPrediction(target).UnitPosition;

                    var finalPosition = targetPosition.Extend(ObjectManager.Player.ServerPosition, -pushDistance);
                    var condemnRectangle = new iSRPolygon(iSRPolygon.Rectangle(targetPosition.To2D(), finalPosition.To2D(), target.BoundingRadius));
                    var condemnRectangle_ex = new iSRPolygon(iSRPolygon.Rectangle(target.ServerPosition.To2D(), finalPosition.To2D(), target.BoundingRadius));

                    if (condemnRectangle.Points.Count(point => point.IsWall()) >= condemnRectangle.Points.Count() * accuracy
                        && condemnRectangle_ex.Points.Count(point => point.IsWall()) >= condemnRectangle_ex.Points.Count() * accuracy)
                    {
                        Variables.spells[SpellSlot.E].Cast(target);
                        return;
                    }
                }
            }
        }
    }
}
