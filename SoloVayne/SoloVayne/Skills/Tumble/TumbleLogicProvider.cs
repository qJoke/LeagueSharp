using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
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
            var AverageDistanceWeight = .70f;
            var ClosestDistanceWeight = .30f;

            var bestWeightedAvg = 0f;

            if (ObjectManager.Player.CountEnemiesInRange(1500f) <= 1)
            {
                var position = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f);
                return position.IsSafe() ? position : Vector3.Zero;
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
                    return ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f).IsSafe()
                        ? ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f)
                        : Vector3.Zero;
                }

                if (HeroManager.Allies.Count(ally => !ally.IsMe && ally.IsValidTarget(1500f, false)) <= 1)
                {
                    var targettingEnemies =
                        HeroManager.Enemies.Where(m => !m.IsMelee && m.IsValidTarget(1500f) && m.HealthPercent < 7).ToList();
                    if (targettingEnemies.Any())
                    {
                        var total =
                            targettingEnemies.Sum(v => AttackTracker.ActiveAttacks.Count(m => m.Key == v.NetworkId));

                        if (total >= 2)
                        {
                            var backwardsPosition = ObjectManager.Player.ServerPosition.Extend(targettingEnemies.First().ServerPosition, -300f);

                            if (backwardsPosition.IsSafe())
                            {
                                return backwardsPosition;
                            }
                        }
                    }
                }

                var avgDist = TumbleHelper.GetAvgDistance(position);

                if (avgDist > -1)
                {
                    var closestDist = ObjectManager.Player.ServerPosition.Distance(enemy.ServerPosition);
                    var weightedAvg = closestDist * ClosestDistanceWeight + avgDist * AverageDistanceWeight;
                    if (weightedAvg > bestWeightedAvg && position.IsSafe())
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
