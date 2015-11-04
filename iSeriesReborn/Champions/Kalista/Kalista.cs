using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

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

        protected override void LoadMenu()
        {
            var defaultMenu = Variables.Menu;

            var comboMenu = defaultMenu.AddModeMenu(Orbwalking.OrbwalkingMode.Combo);
            {
                comboMenu.AddSkill(SpellSlot.Q, Orbwalking.OrbwalkingMode.Combo, true, 15);
                comboMenu.AddSkill(SpellSlot.E, Orbwalking.OrbwalkingMode.Combo, true, 10);
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

            Console.WriteLine("Kalista Loaded!");
        }

        protected override void OnTick()
        {
        }

        protected override void OnCombo()
        {
            if (spells[SpellSlot.Q].IsEnabledAndReady())
            {
                
            }
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

        protected override Dictionary<SpellSlot, Spell> GetSpells()
        {
            return spells;
        }
    }
}
