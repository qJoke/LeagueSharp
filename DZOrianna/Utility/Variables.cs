using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;

namespace DZOrianna.Utility
{
    class Variables
    {
        /// <summary>
        /// The assembly menu
        /// </summary>
        public static Menu AssemblyMenu = new Menu("Orianna", "dz191.orianna", true);

        public static Orbwalking.Orbwalker Orbwalker { get; set; }


    }
}
