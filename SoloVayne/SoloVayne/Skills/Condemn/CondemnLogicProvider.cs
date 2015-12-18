﻿using System.Collections.Generic;
using System.Linq;
using DZLib.Logging;
using iSeriesReborn.Utility.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility;
using SoloVayne.Utility.Entities;
using SoloVayne.Utility.General;
using Collision = LeagueSharp.Common.Collision;

namespace SoloVayne.Skills.Condemn
{
    class CondemnLogicProvider
    {
        internal Obj_AI_Hero GetTarget(Vector3 position = default(Vector3))
        {
            var HeroList = HeroManager.Enemies.Where(
                                    h =>
                                        h.IsValidTarget(Variables.spells[SpellSlot.E].Range) &&
                                        !h.HasBuffOfType(BuffType.SpellShield) &&
                                        !h.HasBuffOfType(BuffType.SpellImmunity));

            var Accuracy = 38;
            var PushDistance = 425;

            if (ObjectManager.Player.ServerPosition.UnderTurret(true))
            {
                return null;
            }

            var currentTarget = Variables.Orbwalker.GetTarget();

            if (HeroManager.Allies.Count(ally => !ally.IsMe && ally.IsValidTarget(1500f, false)) == 0
                && ObjectManager.Player.CountEnemiesInRange(1500f) == 1)
            {
                //It's a 1v1 situation. We push condemn to the limit and lower the accuracy by 5%.
                Accuracy = 33;
                PushDistance = 460;
            }

            var startPosition = position != default(Vector3) ? position : ObjectManager.Player.ServerPosition;

            foreach (var Hero in HeroList)
            {
                if (MenuExtensions.GetItemValue<bool>("solo.vayne.misc.condemn.current") && !(MenuExtensions.GetItemValue<bool>("solo.vayne.misc.condemn.autoe")))
                {
                    if (Hero.NetworkId != currentTarget.NetworkId)
                    {
                        continue;
                    }
                }

                if (Hero.Health + 10 <= ObjectManager.Player.GetAutoAttackDamage(Hero) * 2)
                {
                    continue;
                }
                var prediction = Variables.spells[SpellSlot.E].GetPrediction(Hero);
                var targetPosition = prediction.UnitPosition;
                var finalPosition = targetPosition.Extend(startPosition, -PushDistance);
                var finalPosition_ex = Hero.ServerPosition.Extend(startPosition, -PushDistance);
                
                //Yasuo Wall Logic
                if (YasuoWall.CollidesWithWall(startPosition, Hero.ServerPosition.Extend(startPosition, -450f)))
                {
                    continue;
                }

                //Condemn to turret logic
                if (GameObjects.AllyTurrets.Any(m => m.IsValidTarget(float.MaxValue, false) && m.Distance(finalPosition) <= 450f))
                {
                    var turret =
                        GameObjects.AllyTurrets.FirstOrDefault(
                            m => m.IsValidTarget(float.MaxValue, false) && m.Distance(finalPosition) <= 450f);
                    if (turret != null)
                    {
                        var enemies = GameObjects.Enemy.Where(m => m.Distance(turret) < 775f && m.IsValidTarget());

                        if (!enemies.Any())
                        {
                            return Hero;
                        }
                    }
                }

                //Condemn To Wall Logic
                var condemnRectangle = new SOLOPolygon(SOLOPolygon.Rectangle(targetPosition.To2D(), finalPosition.To2D(), Hero.BoundingRadius));
                var condemnRectangle_ex = new SOLOPolygon(SOLOPolygon.Rectangle(Hero.ServerPosition.To2D(), finalPosition_ex.To2D(), Hero.BoundingRadius));
                
                if (IsBothNearWall(Hero))
                {
                    return null;
                }

                if (condemnRectangle.Points.Count(point => NavMesh.GetCollisionFlags(point.X, point.Y).HasFlag(CollisionFlags.Wall)) >= condemnRectangle.Points.Count() * (Accuracy / 100f)
                    && condemnRectangle_ex.Points.Count(point => NavMesh.GetCollisionFlags(point.X, point.Y).HasFlag(CollisionFlags.Wall)) >= condemnRectangle_ex.Points.Count() * (Accuracy / 100f))
                {
                    return Hero;
                }
            }
            return null;
        }

        private static bool IsBothNearWall(Obj_AI_Base target)
        {
            var positions = GetWallQPositions(target, 110).ToList().OrderBy(pos => pos.Distance(target.ServerPosition, true));
            var positions_ex = GetWallQPositions(ObjectManager.Player, 110).ToList().OrderBy(pos => pos.Distance(ObjectManager.Player.ServerPosition, true));

            if (positions.Any(p => p.IsWall()) && positions_ex.Any(p => p.IsWall()))
            {
                return true;
            }

            return false;
        }

        private static Vector3[] GetWallQPositions(Obj_AI_Base player, float Range)
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