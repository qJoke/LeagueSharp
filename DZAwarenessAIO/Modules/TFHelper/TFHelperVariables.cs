using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.TFHelper
{
    class TFHelperVariables
    {
        /// <summary>
        /// Gets the enemies close.
        /// </summary>
        /// <value>
        /// The enemies close.
        /// </value>
        public static IEnumerable<Obj_AI_Hero> EnemiesClose
        {
            get
            {
                return
                    HeroManager.Enemies.Where(
                        m =>
                            m.Distance(ObjectManager.Player, true) <= Math.Pow(1000, 2) && m.IsValidTarget(1500, false) &&
                            m.CountEnemiesInRange(m.IsMelee() ? m.AttackRange * 1.5f : m.AttackRange + 20 * 1.5f) > 0);
            }
        }

        /// <summary>
        /// Gets the allies close.
        /// </summary>
        /// <value>
        /// The allies close.
        /// </value>
        public static IEnumerable<Obj_AI_Hero> AlliesClose
        {
            get
            {
                return
                    HeroManager.Allies.Where(
                        m => !m.IsMe &&
                            m.Distance(ObjectManager.Player, true) <= Math.Pow(1000, 2) && m.IsValidTarget(1000, false));
            }
        }
    }
}
