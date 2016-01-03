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
                comboMenu.AddBool("dz191.orianna.combo.q", "Use Q", true);
                comboMenu.AddBool("dz191.orianna.combo.w", "Use W", true);
                comboMenu.AddBool("dz191.orianna.combo.e", "Use E", true);
                comboMenu.AddBool("dz191.orianna.combo.r", "Use R", true);
                comboMenu.AddSlider("dz191.orianna.combo.minw", "W Minimum Enemies", 2, 1, 5);
                comboMenu.AddSlider("dz191.orianna.combo.minr", "R Minimum Enemies", 2, 1, 5);
                rootMenu.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("Hybrid", "dz191.orianna.mixed");
            {
                harassMenu.AddBool("dz191.orianna.mixed.q", "Use Q", true);
                harassMenu.AddBool("dz191.orianna.mixed.w", "Use W", true);
                harassMenu.AddBool("dz191.orianna.mixed.e", "Use E", true);
                comboMenu.AddSlider("dz191.orianna.mixed.minw", "W Minimum Enemies", 2, 1, 5);
                rootMenu.AddSubMenu(harassMenu);
            }

            var miscMenu = new Menu("Miscellaneous", "dz191.orianna.misc");
            {
                var miscEMenu = new Menu("E - Command: Protect", "dz191.orianna.misc.e");
                {
                    var shieldMenu = new Menu("E - Shield", "dz191.orianna.misc.e.shield");
                    {
                        foreach (var ally in HeroManager.Allies)
                        {
                            var menuItem = new MenuItem($"dz191.orianna.misc.e.shield.{ally.ChampionName}", ally.ChampionName).SetValue(true);
                            shieldMenu.AddItem(menuItem);
                        }
                        miscEMenu.AddSubMenu(shieldMenu);
                    }
                    miscEMenu.AddBool("dz191.orianna.misc.e.shield", "Use E for Shield", true);
                    miscEMenu.AddSlider("dz191.orianna.misc.e.percent", "E Health %", 15, 1, 100);
                    miscEMenu.AddBool("dz191.orianna.misc.e.damage", "Use E for Damage", true);
                    miscEMenu.AddBool("dz191.orianna.misc.e.initiators", "Use E on initiators", true);

                    miscMenu.AddSubMenu(miscEMenu);
                }

                var miscRMenu = new Menu("R - Command: Shockwave", "dz191.orianna.misc.r");
                {
                    miscRMenu.AddBool("dz191.orianna.misc.r.autor", "Auto R");
                    miscRMenu.AddSlider("dz191.orianna.misc.r.autor.enemies", "Auto R Enemies", 3, 1, 5);
                    miscRMenu.AddBool("dz191.orianna.misc.r.interrupt", "R interrupt", true);

                    miscMenu.AddSubMenu(miscRMenu);
                }

                rootMenu.AddSubMenu(miscMenu);
            }


        }
    }
}
