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

namespace DZAIO_Reborn.Plugins.Champions.Diana
{
    class Diana : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.diana.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }, new[] { true, true, true, true });
                comboMenu.AddBool("dzaio.champion.diana.combo.waitforE", "Wait for charm", true);
                comboMenu.AddBool("dzaio.champion.diana.combo.onlyInitR", "Only use First R (Only to initiate)", true);

                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.diana.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });
                mixedMenu.AddSlider("dzaio.champion.diana.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.diana.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });

                farmMenu.AddSlider("dzaio.champion.diana.farm.w.min", "Min Minions for W", 2, 1, 6);
                farmMenu.AddSlider("dzaio.champion.diana.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.diana.extra");
            {
                extraMenu.AddBool("dzaio.champion.diana.extra.interrupter", "Interrupter (E)", true);
                extraMenu.AddBool("dzaio.champion.diana.extra.antigapcloser", "Antigapcloser (E)", true);
                extraMenu.AddBool("dzaio.champion.diana.extra.autoQ", "Auto Q Stunned / Rooted", true);
                extraMenu.AddBool("dzaio.champion.diana.extra.autoQKS", "Auto Q KS", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 185f, 1620f, false, SkillshotType.SkillshotCircle);
        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
        }

        private void OnGapcloser(DZLib.Core.ActiveGapcloser gapcloser)
        {
            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.diana.extra.antigapcloser")
                && ObjectManager.Player.ManaPercent > 20
                && gapcloser.End.Distance(ObjectManager.Player.ServerPosition) < 400
                && gapcloser.Sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                && Variables.Spells[SpellSlot.E].IsReady())
            {
                Variables.Spells[SpellSlot.E].CastIfHitchanceEquals(gapcloser.Sender, HitChance.Medium);
            }
        }

        private void OnInterrupter(Obj_AI_Hero sender, DZInterrupter.InterruptableTargetEventArgs args)
        {
            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.diana.extra.interrupter")
                && ObjectManager.Player.ManaPercent > 20
                && args.DangerLevel > DZInterrupter.DangerLevel.Medium
                && sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                && Variables.Spells[SpellSlot.E].IsReady())
            {
                Variables.Spells[SpellSlot.E].CastIfHitchanceEquals(sender, HitChance.High);
            }
        }
        public Dictionary<SpellSlot, Spell> GetSpells()
        {
            return new Dictionary<SpellSlot, Spell>
                      {
                            { SpellSlot.Q, new Spell(SpellSlot.Q, 830f) },
                            { SpellSlot.W, new Spell(SpellSlot.W, 250f) },
                            { SpellSlot.E, new Spell(SpellSlot.E, 450f) },
                            { SpellSlot.R, new Spell(SpellSlot.R, 800f) }
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
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.diana.mixed.mana").Value)
            {
                return;
            }
           
        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.diana.farm.mana").Value)
            {
                return;
            }

        }
    }
}
