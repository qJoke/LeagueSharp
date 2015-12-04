using System.Linq;
using DZLib.Logging;
using iSeriesReborn.Utility.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility;

namespace SoloVayne.Skills.Condemn
{
    class CondemnLogicProvider
    {
        internal Obj_AI_Hero GetTarget()
        {
            var HeroList = HeroManager.Enemies.Where(
                                    h =>
                                        h.IsValidTarget(Variables.spells[SpellSlot.E].Range) &&
                                        !h.HasBuffOfType(BuffType.SpellShield) &&
                                        !h.HasBuffOfType(BuffType.SpellImmunity));

            var Accuracy = 45;
            var PushDistance = 420;

            if (ObjectManager.Player.ServerPosition.UnderTurret(true))
            {
                return null;
            }

            var currentTarget = Variables.Orbwalker.GetTarget();

            foreach (var Hero in HeroList)
            {
                if (MenuExtensions.GetItemValue<bool>("solo.vayne.misc.condemn.current"))
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

                var targetPosition = Variables.spells[SpellSlot.E].GetPrediction(Hero).UnitPosition;
                var finalPosition = targetPosition.Extend(ObjectManager.Player.ServerPosition, -PushDistance);
                var finalPosition_ex = Hero.ServerPosition.Extend(ObjectManager.Player.ServerPosition, -PushDistance);

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
