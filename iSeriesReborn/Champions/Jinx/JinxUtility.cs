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
        /// Gets a list of Movement Impairing buffs.
        /// </summary>
        private static List<BuffType> ImpairedBuffTypes
        {
            get
            {
                return new List<BuffType>
                {
                    BuffType.Stun,
                    BuffType.Snare,
                    BuffType.Charm,
                    BuffType.Fear,
                    BuffType.Taunt,
                    BuffType.Slow
                };
            }
        }


        /// <summary>
        /// Determines whether or not FishBone is active.
        /// </summary>
        /// <returns>Range > 565</returns>
        internal static bool IsFishBone()
        {
            //return ObjectManager.Player.AttackRange > 565f;
            return ObjectManager.Player.HasBuff("JinxQ"); // Jinx's AA Range is 525.
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

        /// <summary>
        /// Get the Slow end time
        /// </summary>
        /// <param name="target">The enemy</param>
        /// <returns>Buff end time</returns>
        public static float GetSlowEndTime(Obj_AI_Base target)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Type.Equals(BuffType.Slow))
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Determines if the target is heavily impaired (stunned/rooted)
        /// </summary>
        /// <param name="enemy">The target</param>
        /// <returns>Whether the target is heavily impaired</returns>
        public static bool IsHeavilyImpaired(Obj_AI_Hero enemy)
        {
            return enemy.HasBuffOfType(BuffType.Stun) || enemy.HasBuffOfType(BuffType.Snare) || enemy.HasBuffOfType(BuffType.Charm) || enemy.HasBuffOfType(BuffType.Fear) || enemy.HasBuffOfType(BuffType.Taunt);
        }

        /// <summary>
        /// Determines if the target is lightly impaired (slowed)
        /// </summary>
        /// <param name="enemy">The target</param>
        /// <returns>Whether the target is lightly impaired</returns>
        public static bool IsLightlyImpaired(Obj_AI_Hero enemy)
        {
            return enemy.HasBuffOfType(BuffType.Slow);
        }

        /// <summary>
        /// Gets the Root/Stun/Immobile buff end time
        /// </summary>
        /// <param name="target">The enemy</param>
        /// <returns>Buff end time</returns>
        public static float GetImpairedEndTime(Obj_AI_Base target)
        {
            return target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => ImpairedBuffTypes.Contains(buff.Type))
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault();
        }
    }
}
