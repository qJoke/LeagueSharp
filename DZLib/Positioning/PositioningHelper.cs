using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZLib.Positioning
{
    class PositioningHelper
    {
        public static float GetAvgDistanceFromEnemyTeam(Vector3 from, float MaxRange = 1200f)
        {
            var numberOfEnemies = from.CountEnemiesInRange(MaxRange);
            if (numberOfEnemies != 0)
            {
                var enemies = HeroManager.Enemies.Where(en => en.IsValidTarget(MaxRange, true, from)).ToList();
                var totalDistance = enemies.Sum(en => en.Distance(ObjectManager.Player.ServerPosition));

                return totalDistance / numberOfEnemies;
            }
            return -1;
        }

        public static float GetAvgDistanceFromAllyTeam(Vector3 from, float MaxRange = 1200f)
        {
            var numberOfAllies = from.CountAlliesInRange(MaxRange);
            if (numberOfAllies != 0)
            {
                var allies = HeroManager.Allies.Where(ally => ally.IsValidTarget(MaxRange, false, from) && !ally.IsMe).ToList();
                var totalDistance = allies.Sum(ally => ally.Distance(ObjectManager.Player.ServerPosition));

                return totalDistance / numberOfAllies;
            }
            return -1;
        }

        public static Obj_AI_Hero GetClosestEnemy(float MaxRange, Vector3 from = default(Vector3))
        {
            return
                HeroManager.Enemies
                .FirstOrDefault(en => en.IsValidTarget(MaxRange, true, from == default(Vector3) ? ObjectManager.Player.ServerPosition : from));
        }


        public static Obj_AI_Hero GetClosestAlly(float MaxRange, Vector3 from = default(Vector3))
        {
            return
               HeroManager.Allies
               .FirstOrDefault(ally => !ally.IsMe && ally.IsValidTarget(MaxRange, false, from == default(Vector3) ? ObjectManager.Player.ServerPosition : from));
        }

    }
}
