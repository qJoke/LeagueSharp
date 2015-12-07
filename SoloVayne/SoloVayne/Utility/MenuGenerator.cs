using DZLib.Logging;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace SoloVayne.Utility
{
    class MenuGenerator
    {
        private Menu MainMenu;

        public MenuGenerator()
        {
            if (Variables.Menu == null)
            {
                Variables.Menu = new Menu("[SOLO] Vayne", "solo.vayne", true);
            }

            MainMenu = Variables.Menu;
        }

        public void GenerateMenu()
        {
            var owMenu = new Menu("[SOLO] Orbwalker", "solo.vayne.orbwalker");
            {
                Variables.Orbwalker = new Orbwalking.Orbwalker(owMenu);
                MainMenu.AddSubMenu(owMenu);
            }

            var comboMenu = MainMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Combo);
            {
                comboMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Combo, true, 10);
                comboMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Combo, true, 5);
                comboMenu.AddSkill(SpellSlot.R, Orbwalking.OrbwalkingMode.Combo, true, 5);
            }

            var harassMenu = MainMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Mixed);
            {
                harassMenu.AddStringList("solo.vayne.mixed.mode", "Harass Mode", new[] { "Passive", "Agressive" }, 1);
                harassMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Combo, true, 10);
                harassMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Combo, true, 5);
            }

            var miscMenu = MainMenu.AddSubMenu(new Menu("[SOLO] Miscellaneous", "solo.vayne.misc"));
            {
                var QMenu = miscMenu.AddSubMenu(new Menu("[SOLO] Tumble", "solo.vayne.misc.tumble"));
                {
                    QMenu.AddBool("solo.vayne.misc.tumble.noqintoenemies", "Don't Q into enemies", true);
                    QMenu.AddBool("solo.vayne.misc.tumble.smartQ", "Use SOLO Vayne Q Logic", true);
                }

                var EMenu = miscMenu.AddSubMenu(new Menu("[SOLO] Condemn", "solo.vayne.misc.condemn"));
                {
                    EMenu.AddBool("solo.vayne.misc.condemn.autoe", "Auto E");
                    EMenu.AddBool("solo.vayne.misc.condemn.current", "Only E Current Target", true);
                }
            }

            MainMenu.AddToMainMenu();
        }
    }
}
