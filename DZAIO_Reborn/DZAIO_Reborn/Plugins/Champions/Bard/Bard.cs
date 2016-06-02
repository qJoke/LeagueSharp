using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Helpers;
using DZAIO_Reborn.Helpers.Entity;
using DZAIO_Reborn.Helpers.Modules;
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Core;
using DZLib.Menu;
using DZLib.MenuExtensions;
using DZLib.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace DZAIO_Reborn.Plugins.Champions.Bard
{
    class Bard : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.bard.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.R }, new[] { true, true });
                comboMenu.AddSlider("dzaio.champion.bard.combo.r.min", "Min Enemies for R", 2, 1, 5);

                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.bard.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q}, new[] { true });
                mixedMenu.AddSlider("dzaio.champion.bard.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.bard.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q }, new[] { true });

                farmMenu.AddSlider("dzaio.champion.bard.farm.w.min", "Min Minions for Q", 2, 1, 6);
                farmMenu.AddSlider("dzaio.champion.bard.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.bard.extra");
            {
                extraMenu.AddBool("dzaio.champion.bard.extra.interrupter", "Interrupter (Q)", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.antigapcloser", "Antigapcloser (Q)", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.autoQ", "Auto Q Stunned / Rooted", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.autoQKS", "Auto Q KS", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 100, 1600, false, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.E].SetSkillshot(0.25f, 60, 1200, true, SkillshotType.SkillshotLine);
        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
        }

        private void OnGapcloser(DZLib.Core.ActiveGapcloser gapcloser)
        {
            
        }

        private void OnInterrupter(Obj_AI_Hero sender, DZInterrupter.InterruptableTargetEventArgs args)
        {
            
        }
        public Dictionary<SpellSlot, Spell> GetSpells()
        {
            return new Dictionary<SpellSlot, Spell>
                      {
                            {SpellSlot.Q, new Spell(SpellSlot.Q, 950f)},
                            {SpellSlot.W, new Spell(SpellSlot.W, 945f)},
                            {SpellSlot.E, new Spell(SpellSlot.E, float.MaxValue)}
                      };
        }

        public List<IModule> GetModules()
        {
            return new List<IModule>()
            {

            };
        }

        public void OnTick()
        {

        }

        public void OnCombo()
        {
        }


        public void OnMixed()
        {

        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {

        }
    }
}
