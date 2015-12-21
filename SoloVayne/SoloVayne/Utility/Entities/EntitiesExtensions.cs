using System.Linq;
using LeagueSharp;

namespace SoloVayne.Utility.Entities
{
    static class EntitiesExtensions
    {
        /// <summary>
        /// Does the target have 2W stacks.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static bool Has2WStacks(this Obj_AI_Hero target)
        {
            return target.Buffs.Any(bu => bu.Name == "vaynesilvereddebuff" && bu.Count == 2);
        }

        /// <summary>
        /// Gets the W buff.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static BuffInstance GetWBuff(this Obj_AI_Hero target)
        {
            return target.Buffs.FirstOrDefault(bu => bu.Name == "vaynesilvereddebuff");
        }
    }
}
