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

namespace DZAIO_Reborn.Plugins.Champions.Vladimir
{
    class Vladimir : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.vladimir.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.R }, new[] { true, true, true });
                comboMenu.AddSlider("dzaio.champion.vladimir.combo.r.min", "Min Enemies for R", 2, 1, 5);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.vladimir.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });
                mixedMenu.AddSlider("dzaio.champion.vladimir.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.vladimir.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q }, new[] { true });
                farmMenu.AddSlider("dzaio.champion.vladimir.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.vladimir.extra");
            {
                extraMenu.AddBool("dzaio.champion.vladimir.extra.antigapcloser", "W Antigapcloser (Important Gapclosers)", true);
                extraMenu.AddSlider("dzaio.champion.vladimir.extra.w.antigpdelay", "W Antigapcloser Delay", 120, 0, 350);
                extraMenu.AddBool("dzaio.champion.vladimir.extra.autoQKS", "Q KS", true);
            }

            Variables.Spells[SpellSlot.Q].SetTargetted(0.25f, 2000f);
            Variables.Spells[SpellSlot.R].SetSkillshot(0.25f, 175, 700, false, SkillshotType.SkillshotCircle);

        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
            Orbwalking.AfterAttack += AfterAttack;
        }


        private void AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            
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
                                    { SpellSlot.Q, new Spell(SpellSlot.Q, 600f) },
                                    { SpellSlot.W, new Spell(SpellSlot.W) },
                                    { SpellSlot.E, new Spell(SpellSlot.E, 600f) },
                                    { SpellSlot.R, new Spell(SpellSlot.R, 680f) }
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
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.vladimir.mixed.mana").Value)
            {
                return;
            }

            
        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.vladimir.farm.mana").Value)
            {
                return;
            }

        }

    }
}
