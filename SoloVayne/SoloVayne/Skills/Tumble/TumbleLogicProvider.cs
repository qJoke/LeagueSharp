using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

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
