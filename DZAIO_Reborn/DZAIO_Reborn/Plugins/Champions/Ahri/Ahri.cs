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
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace DZAIO_Reborn.Plugins.Champions.Ahri
{
    class Ahri : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.ahri.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }, new[] { true, true, true, true });
                //comboMenu.AddNoUltiMenu(false);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.ahri.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E }, new[] { true, true, true });
                mixedMenu.AddSlider("dzaio.champion.ahri.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.ahri.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });

                farmMenu.AddSlider("dzaio.champion.ahri.farm.w.min", "Min Minions for W", 2, 1, 6);
                farmMenu.AddSlider("dzaio.champion.ahri.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.ahri.extra");
            {
                extraMenu.AddBool("dzaio.champion.ahri.extra.interrupter", "Interrupter (E)", true);
                extraMenu.AddBool("dzaio.champion.ahri.extra.antigapcloser", "Antigapcloser (E)", true);
                extraMenu.AddBool("dzaio.champion.ahri.extra.farmQ", "Auto Q Farm", true);
                extraMenu.AddBool("dzaio.champion.ahri.extra.autoQ", "Auto Q Stunned / Rooted", true);
                extraMenu.AddBool("dzaio.champion.ahri.extra.autoQKS", "Auto Q KS", true);
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
            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.ahri.extra.antigapcloser")
                && ObjectManager.Player.ManaPercent > 20
                && gapcloser.End.Distance(ObjectManager.Player.ServerPosition) < 400
                && gapcloser.Sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                && Variables.Spells[SpellSlot.E].IsReady())
            {
                Variables.Spells[SpellSlot.E].SPredictionCastRing(gapcloser.Sender, 80f, HitChance.High);
            }
        }

        private void OnInterrupter(Obj_AI_Hero sender, DZInterrupter.InterruptableTargetEventArgs args)
        {
            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.ahri.extra.interrupter")
                && ObjectManager.Player.ManaPercent > 20
                && args.DangerLevel > DZInterrupter.DangerLevel.Medium
                && sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                && Variables.Spells[SpellSlot.E].IsReady())
            {
                Variables.Spells[SpellSlot.E].SPredictionCastRing(sender, 80f, HitChance.Medium);
            }
        }
        public Dictionary<SpellSlot, Spell> GetSpells()
        {
            return new Dictionary<SpellSlot, Spell>
                      {
                            { SpellSlot.Q, new Spell(SpellSlot.Q, 925f) },
                            { SpellSlot.W, new Spell(SpellSlot.W, 700f) },
                            { SpellSlot.E, new Spell(SpellSlot.E, 875f) },
                            { SpellSlot.R, new Spell(SpellSlot.R, 400f) }
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
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.ahri.mixed.mana").Value)
            {
                return;
            }
        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.ahri.farm.mana").Value)
            {
                return;
            }

            
        }
    }
}
