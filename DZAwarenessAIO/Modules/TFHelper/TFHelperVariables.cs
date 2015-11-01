using System;
using System.Collections.Generic;
using System.Linq;
using DZAwarenessAIO.Utility.MenuUtility;
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
                            m.Distance(ObjectManager.Player, true) <= Math.Pow(TFRange, 2) && m.IsValidTarget(TFRange, false) &&
                            m.CountEnemiesInRange(m.IsMelee() ? m.AttackRange * 1.5f : m.AttackRange + 20 * 1.5f) > 0);
            }
        }

        /// <summary>
        /// The teamfight calculation range
        /// </summary>
        public static int TFRange => MenuExtensions.GetItemValue<Slider>("dz191.dza.tf.range").Value;

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
                        m => m.Distance(ObjectManager.Player, true) <= Math.Pow(TFRange, 2) && m.IsValidTarget(TFRange, false));
            }
        }

        /// <summary>
        /// Gets or sets the ally bar sprite.
        /// </summary>
        /// <value>
        /// The ally bar sprite.
        /// </value>
        public static Render.Sprite AllyBarSprite { get; set; }

        /// <summary>
        /// Gets or sets the enemy bar sprite.
        /// </summary>
        /// <value>
        /// The enemy bar sprite.
        /// </value>
        public static Render.Sprite EnemyBarSprite { get; set; }

        /// <summary>
        /// Gets or sets the ally strength % text.
        /// </summary>
        /// <value>
        /// The ally strength % text.
        /// </value>
        public static Render.Text AllyStrengthText { get; set; }

        /// <summary>
        /// Gets or sets the enemy strength % text.
        /// </summary>
        /// <value>
        /// The enemy strength % text.
        /// </value>
        public static Render.Text EnemyStrengthText { get; set; }

        /// <summary>
        /// Gets or sets the teams info text.
        /// </summary>
        /// <value>
        /// The teams info text.
        /// </value>
        public static Render.Text TeamsVSText { get; set; }
    }
}
