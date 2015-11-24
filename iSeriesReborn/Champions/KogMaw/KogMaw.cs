﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Configuration;
using DZLib.Logging;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.KogMaw
{
    class KogMaw : ChampionBase
    {
        private readonly Dictionary<SpellSlot, Spell> spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q, 1175f) },
            { SpellSlot.W, new Spell(SpellSlot.W) },
            { SpellSlot.E, new Spell(SpellSlot.E, 1280f) },
            { SpellSlot.R, new Spell(SpellSlot.R) }
        };

        protected override void OnChampLoad()
        {
            spells[SpellSlot.Q].SetSkillshot(0.25f, 70f, 1650f, true, SkillshotType.SkillshotLine);
            spells[SpellSlot.E].SetSkillshot(0.50f, 120f, 1350, false, SkillshotType.SkillshotLine);
            spells[SpellSlot.R].SetSkillshot(1.2f, 120f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Orbwalking.BeforeAttack += KogHooks.BeforeAttack;
            Obj_AI_Base.OnBuffAdd += KogHooks.OnBuffAdd;
            Obj_AI_Base.OnBuffRemove += KogHooks.OnBuffRemove;
        }

        protected override void LoadMenu()
        {
            var defaultMenu = Variables.Menu;

            var comboMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Combo);
            {
                comboMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Combo, true, 15);
                comboMenu.AddSkill(SpellSlot.W, Orbwalking.OrbwalkingMode.Combo, true, 5);
                comboMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Combo, true, 30);
                comboMenu.AddSkill(SpellSlot.R, Orbwalking.OrbwalkingMode.Combo, false, 10);
                comboMenu.AddSlider("iseriesr.kogmaw.combo.r.limit", "R Limiter", 3, 2, 7);
            }

            var mixedMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Mixed);
            {
                mixedMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Mixed, true, 15);
                mixedMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Mixed, true, 5);
                mixedMenu.AddSkill(SpellSlot.R, Orbwalking.OrbwalkingMode.Mixed, true, 5);
                mixedMenu.AddSlider("iseriesr.kogmaw.mixed.r.limit", "R Limiter", 3, 2, 7);
            }

            var laneclearMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.LaneClear);
            {
                laneclearMenu.AddSkill(SpellSlot.W, Orbwalking.OrbwalkingMode.LaneClear, true, 50);
            }

            var miscMenu = defaultMenu.AddSubMenu(new Menu("[iSR] Misc", "iseriesr.kogmaw.misc"));
            {
                var sMenu = new Menu("Use W On", "iseriesr.kogmaw.misc.w.on");
                {
                    sMenu.AddBool("iseriesr.kogmaw.misc.w.on.tower", "Tower", true);
                    sMenu.AddBool("iseriesr.kogmaw.misc.w.on.inhib", "Inhib", true);
                    sMenu.AddBool("iseriesr.kogmaw.misc.w.on.nexus", "Nexus", true);
                }

                miscMenu.AddBool("iseriesr.kogmaw.misc.q.ks", "Use Q KS");
                miscMenu.AddBool("iseriesr.kogmaw.misc.r.ks", "Use R KS");
                miscMenu.AddBool("iseriesr.kogmaw.misc.r.slow", "Auto R Slow/Impaired");
                miscMenu.AddSubMenu(sMenu);
            }
        }

        protected override void OnTick()
        {
        }

        protected override void OnCombo()
        {

        }

        protected override void OnMixed()
        {

        }

        protected override void OnLastHit() { }

        protected override void OnLaneClear()
        {

        }

        protected override void OnAfterAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            
        }

        public override Dictionary<SpellSlot, Spell> GetSpells()
        {
            return spells;
        }

        public override List<IModule> GetModules()
        {
            return new List<IModule>()
            {
                
            };
        }
    }
}
