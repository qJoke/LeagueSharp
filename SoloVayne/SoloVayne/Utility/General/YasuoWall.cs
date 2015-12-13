using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace SoloVayne.Utility.General
{
    class YasuoWall
    {
        private static int _wallCastT;

        /// <summary>
        /// The yasuo wind wall casted position.
        /// </summary>
        private static Vector2 _yasuoWallCastedPos;


        internal static void OnProcessSpellCast(LeagueSharp.Obj_AI_Base sender, LeagueSharp.GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsValid && sender.Team != ObjectManager.Player.Team && args.SData.Name == "YasuoWMovingWall")
            {
                _wallCastT = Utils.TickCount;
                _yasuoWallCastedPos = sender.ServerPosition.To2D();
            }
        }

        internal static bool CollidesWithWall(Vector3 start, Vector3 end)
        {
            if (Utils.TickCount - _wallCastT > 4000)
            {
                return false;
            }

            GameObject wall = null;
            foreach (var gameObject in
                ObjectManager.Get<GameObject>()
                    .Where(
                        gameObject =>
                            gameObject.IsValid &&
                            Regex.IsMatch(
                                gameObject.Name, "_w_windwall_enemy_0.\\.troy", RegexOptions.IgnoreCase))
                )
            {
                wall = gameObject;
            }
            if (wall == null)
            {
                return false;
            }
            var level = wall.Name.Substring(wall.Name.Length - 6, 1);
            var wallWidth = (300 + 50 * Convert.ToInt32(level));

            var wallDirection =
                (wall.Position.To2D() - _yasuoWallCastedPos).Normalized().Perpendicular();
            var wallStart = wall.Position.To2D() + wallWidth / 2f * wallDirection;
            var wallEnd = wallStart - wallWidth * wallDirection;

            for (int i = 0; i < start.Distance(end); i += 30)
            {
                var currentPosition = start.Extend(end, i);
                if (wallStart.Intersection(wallEnd, currentPosition.To2D(), start.To2D()).Intersects)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
