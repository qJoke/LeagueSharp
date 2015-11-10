using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSeriesReborn.Utility;
using LeagueSharp;

namespace iSeriesReborn.Champions.Jinx
{
    class JinxUtility
    {
        /// <summary>
        /// Determines whether or not FishBone is active.
        /// </summary>
        /// <returns>Range > 565</returns>
        internal static bool IsFishBone()
        {
            //return ObjectManager.Player.AttackRange > 565f;
            return ObjectManager.Player.AttackRange > 525f; // Jinx's AA Range is 525.
        }

        /// <summary>
        /// Determines the minigun range
        /// </summary>
        /// <param name="target">Current target</param>
        /// <returns>The minigun range</returns>
        internal static float GetMinigunRange(GameObject target)
        {
            return 525f + ObjectManager.Player.BoundingRadius + (target?.BoundingRadius ?? 0f);
        }

        /// <summary>
        /// Determines the extra range.
        /// </summary>
        /// <returns>Extra fishbone range</returns>
        internal static float GetFishboneRange()
        {
            //return 50f + 25f * Spells[SpellSlot.Q].Level;
            return 75f + 25f * Variables.spells[SpellSlot.Q].Level; //it starts from +75.
        }
    }
}
