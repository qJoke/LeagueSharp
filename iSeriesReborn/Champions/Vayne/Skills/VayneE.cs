using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using DZLib.Logging;
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
                    ECheck();
            }
        }

        public static void ECheck()
        {
            if (!Variables.spells[SpellSlot.E].IsReady())
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(h => !h.IsInvulnerable && !TargetSelector.IsInvulnerable(h, TargetSelector.DamageType.Physical, false) && h.IsValidTarget()))
            {
                var pushDistance = MenuExtensions.GetItemValue<Slider>("iseriesr.vayne.misc.condemn.pushdist").Value;
                var accuracy = MenuExtensions.GetItemValue<Slider>("iseriesr.vayne.misc.condemn.acc").Value / 100f;
                var targetPosition = Variables.spells[SpellSlot.E].GetPrediction(target).UnitPosition;

                var finalPosition = targetPosition.Extend(ObjectManager.Player.ServerPosition, -pushDistance);
                var finalPosition_ex = target.ServerPosition.Extend(ObjectManager.Player.ServerPosition, -pushDistance);

                var condemnRectangle = new iSRPolygon(iSRPolygon.Rectangle(targetPosition.To2D(), finalPosition.To2D(), target.BoundingRadius));
                var condemnRectangle_ex = new iSRPolygon(iSRPolygon.Rectangle(target.ServerPosition.To2D(), finalPosition_ex.To2D(), target.BoundingRadius));

                if (IsBothNearWall(target))
                {
                    return;
                }

                if (condemnRectangle.Points.Count(point => NavMesh.GetCollisionFlags(point.X, point.Y).HasFlag(CollisionFlags.Wall)) >= condemnRectangle.Points.Count() * accuracy
                    && condemnRectangle_ex.Points.Count(point => NavMesh.GetCollisionFlags(point.X, point.Y).HasFlag(CollisionFlags.Wall)) >= condemnRectangle_ex.Points.Count() * accuracy)
                {
                    Variables.spells[SpellSlot.E].Cast(target);
                    return;
                }
            }
        }

        private static bool IsBothNearWall(Obj_AI_Base target)
        {
            var positions =
                GetWallQPositions(target, 110).ToList().OrderBy(pos => pos.Distance(target.ServerPosition, true));
            var positions_ex =
            GetWallQPositions(ObjectManager.Player, 70).ToList().OrderBy(pos => pos.Distance(ObjectManager.Player.ServerPosition, true));

            if (positions.Any(p => p.IsWall()))
            {
                return true;
            }
            return false;
        }

        private static Vector3[] GetWallQPositions(Obj_AI_Base player,float Range)
        {
            Vector3[] vList =
            {
                (player.ServerPosition.To2D() + Range * player.Direction.To2D()).To3D(),
                (player.ServerPosition.To2D() - Range * player.Direction.To2D()).To3D()

            };

            return vList;
        }
    }
}
