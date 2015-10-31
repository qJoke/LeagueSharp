using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.Gank_Alerter
{
    /// <summary>
    /// The Class containing the Gank Alerter variables
    /// </summary>
    class GankAlerterVariables
    {
        /// <summary>
        /// Gets or sets the gank alert text.
        /// </summary>
        /// <value>
        /// The gank alert text.
        /// </value>
        public static Render.Text GankAlertText { get; set; }

        /// <summary>
        /// The text size
        /// </summary>
        public static int TextSize => MenuExtensions.GetItemValue<Slider>("dz191.dza.gank.textsize").Value;

        /// <summary>
        /// The minimum distance for the enemy to be in.
        /// </summary>
        public static int MinDist => MenuExtensions.GetItemValue<Slider>("dz191.dza.gank.mindist").Value;

        /// <summary>
        /// The maximum distance for the gank alerter enemy.
        /// </summary>
        public static int MaxDist => MenuExtensions.GetItemValue<Slider>("dz191.dza.gank.maxdist").Value;

    }
}
