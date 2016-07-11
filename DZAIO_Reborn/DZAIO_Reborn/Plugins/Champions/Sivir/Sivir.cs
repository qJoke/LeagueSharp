using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Helpers;
using DZAIO_Reborn.Helpers.Entity;
using DZAIO_Reborn.Helpers.Modules;
using DZAIO_Reborn.Plugins.Champions.Veigar.Modules;
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Core;
using DZLib.Menu;
using DZLib.MenuExtensions;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace DZAIO_Reborn.Plugins.Champions.Veigar
{
    class Sivir : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.sivir.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.R }, new[] { true, true, true });
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.sivir.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W}, new[] { true, true });
                mixedMenu.AddSlider("dzaio.champion.sivir.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.sivir.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });
                farmMenu.AddSlider("dzaio.champion.sivir.farm.w.min", "Min Minions for Q", 3, 1, 6);
                farmMenu.AddSlider("dzaio.champion.sivir.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.sivir.extra");
            {
                extraMenu.AddBool("dzaio.champion.sivir.extra.autoE", "E Shield", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 65f, 1900f, false, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.W].SetSkillshot(1.25f, 190f, 0, false, SkillshotType.SkillshotCircle);
            Variables.Spells[SpellSlot.E].SetSkillshot(0.5f, 335f, 0, false, SkillshotType.SkillshotCircle);
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
                                    { SpellSlot.Q, new Spell(SpellSlot.Q, 1250f) },
                                    { SpellSlot.W, new Spell(SpellSlot.W) },
                                    { SpellSlot.E, new Spell(SpellSlot.E) },
                                    { SpellSlot.R, new Spell(SpellSlot.R) }
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
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.sivir.mixed.mana").Value)
            {
                return;
            }

            
        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.sivir.farm.mana").Value)
            {
                return;
            }

        }
    }
}
