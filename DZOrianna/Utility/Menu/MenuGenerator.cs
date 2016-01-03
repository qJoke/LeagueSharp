using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using LeagueSharp.Common;

namespace DZOrianna.Utility
{
    class MenuGenerator
    {
        internal static void OnLoad()
        {
            var rootMenu = Variables.AssemblyMenu;

            var owMenu = new Menu("Orbwalker", "dz191.orianna.orbwalker");
            {
                Variables.Orbwalker = new Orbwalking.Orbwalker(owMenu);
                rootMenu.AddSubMenu(owMenu);
            }

            var comboMenu = new Menu("Combo", "dz191.orianna.combo");
            {
                comboMenu.AddBool("dz191.orianna.combo.q", "Use Q");
                comboMenu.AddBool("dz191.orianna.combo.w", "Use W");
                comboMenu.AddBool("dz191.orianna.combo.e", "Use E");
                comboMenu.AddBool("dz191.orianna.combo.r", "Use R");
                comboMenu.AddSlider("dz191.orianna.combo.minr", "R Minimum Enemies", 2, 1, 5);
            }


        }
    }
}
