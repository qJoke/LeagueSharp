using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility;
using SoloVayne.Utility.Entities;

namespace SoloVayne.Skills.Tumble
{
    class TumbleLogicProvider
    {
        public Vector3 GetSOLOVayneQPosition()
        {
            var positions = TumbleHelper.GetRotatedQPositions();
            var enemyPositions = TumbleHelper.GetEnemyPoints();
            var safePositions = positions.Where(pos => !enemyPositions.Contains(pos.To2D())).ToList();
            var BestPosition = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f);
            var AverageDistanceWeight = .60f;
            var ClosestDistanceWeight = .40f;

            var bestWeightedAvg = 0f;

            if (ObjectManager.Player.CountEnemiesInRange(1500f) <= 1)
            {
                //Logic for 1 enemy near
                var position = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f);
                return position.IsSafeEx() ? position : Vector3.Zero;
            }

            var targettingEnemies =
                    HeroManager.Enemies.Where(m => !m.IsMelee && m.IsValidTarget(1300f) && m.HealthPercent > 7).ToList();
            
            if (HeroManager.Allies.Count(ally => !ally.IsMe && ally.IsValidTarget(1500f, false)) < 1 &&
                targettingEnemies.Count() <= 2)
            {
                //Logic for 2 enemies Near and alone
                if (
                    targettingEnemies.Any(
                        t =>
                            t.Health + 15 <
                            ObjectManager.Player.GetAutoAttackDamage(t) + Variables.spells[SpellSlot.Q].GetDamage(t) 
                            && t.Distance(ObjectManager.Player) < Orbwalking.GetRealAutoAttackRange(t) + 80f))
                {
                    var QPosition =
                        ObjectManager.Player.ServerPosition.Extend(
                            targettingEnemies.OrderBy(t => t.Health).First().ServerPosition, 300f);

                    if (!QPosition.UnderTurret(true))
                    {
                        return QPosition;
                    }
                }

                var backwardsPosition = (ObjectManager.Player.ServerPosition.To2D() + 300f * ObjectManager.Player.Direction.To2D()).To3D();

                if (!backwardsPosition.UnderTurret(true))
                {
                    return backwardsPosition;
                }
            }

            var closeEnemy = TumbleHelper.GetClosestEnemy(ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f));

            if (ObjectManager.Player.Distance(closeEnemy) <= closeEnemy.AttackRange - 85 && !closeEnemy.IsMelee)
            {
                return ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f).IsSafeEx()
                    ? ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f)
                    : Vector3.Zero;
            }

            foreach (var position in safePositions)
            {
                var enemy = TumbleHelper.GetClosestEnemy(position);
                if (!enemy.IsValidTarget())
                {
                    continue;
                }

                if (ObjectManager.Player.Distance(enemy) <= enemy.AttackRange - 85 && !enemy.IsMelee)
                {
                    return ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f).IsSafeEx()
                        ? ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f)
                        : Vector3.Zero;
                }


                var avgDist = TumbleHelper.GetAvgDistance(position);

                if (avgDist > -1)
                {
                    var closestDist = ObjectManager.Player.ServerPosition.Distance(enemy.ServerPosition);
                    var weightedAvg = closestDist * ClosestDistanceWeight + avgDist * AverageDistanceWeight;
                    if (weightedAvg > bestWeightedAvg && position.IsSafeEx())
                    {
                        bestWeightedAvg = weightedAvg;
                        BestPosition = position;
                    }
                }
            }
            
            
            return (BestPosition.IsSafe()) ? BestPosition : Vector3.Zero;
        }
    }
}
