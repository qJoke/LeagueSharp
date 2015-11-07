using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iSeriesReborn.Champions.Kalista.Modules;
using iSeriesReborn.Champions.Kalista.Skills;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace iSeriesReborn.Champions.Kalista
{
    class Kalista : ChampionBase
    {
        private Dictionary<SpellSlot, Spell> spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q, 1150) }, 
            { SpellSlot.W, new Spell(SpellSlot.W, 5200) }, 
            { SpellSlot.E, new Spell(SpellSlot.E, 950) }, 
            { SpellSlot.R, new Spell(SpellSlot.R, 1200) }
        };

        protected override void OnChampLoad()
        {
            spells[SpellSlot.Q].SetSkillshot(0.25f, 40f, 1200f, true, SkillshotType.SkillshotLine);
            spells[SpellSlot.R].SetSkillshot(0.50f, 1500, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Obj_AI_Base.OnIssueOrder += KalistaHooks.OnIssueOrder;
            Obj_AI_Base.OnProcessSpellCast += KalistaHooks.OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser += KalistaAGP.OnGapclose;
        }

        protected override void LoadMenu()
        {
            
            var defaultMenu = Variables.Menu;

            var comboMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Combo);
            {
                comboMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Combo, true, 15);
                comboMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Combo, true, 10);
                comboMenu.AddSlider("iseriesr.kalista.e.minstacks", "Min Stacks for E (Leave/Expire)", 9, 1, 15).SetTooltip("The min number of stacks to use E when target is about to leave the range or the rend buff is about to expire.");
            }

            var mixedMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Mixed);
            {
                mixedMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Mixed, true, 15);
                mixedMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Mixed, true, 10);
            }

            var laneclearMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.LaneClear);
            {
                laneclearMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.LaneClear, true, 50);
                laneclearMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.LaneClear, true, 50);
            }

            var miscMenu = defaultMenu.AddSubMenu(new Menu("[iSR] Misc", "iseriesr.kalista.misc"));
            {
                miscMenu.AddBool("iseriesr.kalista.misc.steale", "Steal Drake / Baron with E");
                miscMenu.AddBool("iseriesr.kalista.misc.kse", "KS With E");
            }

            Console.WriteLine("Kalista Loaded!");
        }

        protected override void OnTick()
        {
        }

        protected override void OnCombo()
        {
            KalistaQ.ExecuteComboLogic();
            KalistaE.ExecuteComboLogic();
        }

        protected override void OnMixed()
        {

        }

        protected override void OnLastHit()
        {

        }
        
        protected override void OnLaneClear()
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
                new KalistaMobStealer(),
                new KalistaEKs(),
            };
        }
    }
}
