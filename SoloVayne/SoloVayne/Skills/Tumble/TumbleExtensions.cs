using System.Linq;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility.Entities;

namespace SoloVayne.Skills.Tumble
{
    static class TumbleExtensions
    {
        public static bool IsSafe(this Vector3 position)
        {
            return position.IsSafeEx() && position.IsNotIntoEnemies();
        }

        public static bool IsSafeEx(this Vector3 Position)
        {
            if (Position.UnderTurret(true) && !ObjectManager.Player.UnderTurret())
            {
                return false;
            }

            var lowHealthAllies =
                HeroManager.Allies.Where(a => a.IsValidTarget(1500, false) && a.HealthPercent < 10);
            var lowHealthEnemies =
                HeroManager.Allies.Where(a => a.IsValidTarget(1500) && a.HealthPercent < 10);
            var enemies = ObjectManager.Player.CountAlliesInRange(1500);
            var allies = ObjectManager.Player.CountAlliesInRange(1500);
            var enemyTurrets = GameObjects.EnemyTurrets.Where(m => m.IsValidTarget(975f));
            var allyTurrets = GameObjects.AllyTurrets.Where(m => m.IsValidTarget(975f, false));

            return (allies - 1 - lowHealthAllies.Count() + allyTurrets.Count() * 2 >
                    enemies - lowHealthEnemies.Count() + enemyTurrets.Count() * 2);
        }

        public static bool IsNotIntoEnemies(this Vector3 position)
        {
            if (!MenuExtensions.GetItemValue<bool>("solo.vayne.misc.tumble.smartQ") &&
                !MenuExtensions.GetItemValue<bool>("solo.vayne.misc.tumble.noqintoenemies"))
            {
                return true;
            }

            var enemyPoints = TumbleHelper.GetEnemyPoints();
            if (enemyPoints.ToList().Contains(position.To2D()))
            {
                return false;
            }

            var closeEnemies =
                HeroManager.Enemies.FindAll(
                    en =>
                        en.IsValidTarget(1500f) &&
                        !(en.Distance(ObjectManager.Player.ServerPosition) < en.AttackRange + 65f));
            if (!closeEnemies.All(enemy => position.CountEnemiesInRange(enemy.AttackRange) <= 1))
            {
                return false;
            }

            return true;
        }
    }
}
