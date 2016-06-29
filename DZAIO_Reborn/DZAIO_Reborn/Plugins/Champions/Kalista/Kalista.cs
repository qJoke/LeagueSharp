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

namespace DZAIO_Reborn.Plugins.Champions.Kalista
{
    class Kalista : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.kalista.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.E, SpellSlot.R }, new[] { true, true, true });
                //comboMenu.AddNoUltiMenu(false);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.kalista.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.E }, new[] { true, true });
                mixedMenu.AddSlider("dzaio.champion.kalista.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.kalista.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.E }, new[] { true, true });

                farmMenu.AddSlider("dzaio.champion.kalista.farm.e.min", "Min Minions for E", 3, 1, 6);
                farmMenu.AddSlider("dzaio.champion.kalista.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.kalista.extra");
            {
                extraMenu.AddBool("dzaio.champion.kalista.extra.interrupter", "Interrupter (Q)", true);
                extraMenu.AddBool("dzaio.champion.kalista.extra.antigapcloser", "Antigapcloser (Q)", true);
                extraMenu.AddBool("dzaio.champion.kalista.extra.autoQ", "Auto Q Stunned / Rooted", true);
                extraMenu.AddBool("dzaio.champion.kalista.kalista.autoEKS", "Auto E KS", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 40f, 1200f, true, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.R].SetSkillshot(0.50f, 1500, float.MaxValue, false, SkillshotType.SkillshotCircle);

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
                            { SpellSlot.Q, new Spell(SpellSlot.Q, 1150) }, 
                            { SpellSlot.W, new Spell(SpellSlot.W, 5200) }, 
                            { SpellSlot.E, new Spell(SpellSlot.E, 950) }, 
                            { SpellSlot.R, new Spell(SpellSlot.R, 1200) }
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
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.kalista.farm.mana").Value)
            {
                return;
            }

        }
    }
}
