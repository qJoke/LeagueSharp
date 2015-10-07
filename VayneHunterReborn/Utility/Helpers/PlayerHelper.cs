using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace VayneHunter_Reborn.Utility.Helpers
{
    class PlayerHelper
    {
        private static float LastMoveC;

        public static float GetRealAutoAttackRange(Obj_AI_Hero attacker, AttackableUnit target)
        {
            var result = attacker.AttackRange + attacker.BoundingRadius;
            if (target.IsValidTarget())
            {
                return result + target.BoundingRadius;
            }
            return result;
        }

        public static void MoveToLimited(Vector3 where)
        {
            if (Environment.TickCount - LastMoveC < 80)
            {
                return;
            }
            LastMoveC = Environment.TickCount;
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, where);
        }
    }
}
