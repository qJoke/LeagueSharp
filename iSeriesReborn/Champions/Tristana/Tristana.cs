using System;
using System.Collections.Generic;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Tristana
{
    class Tristana : ChampionBase
    {
        private readonly Dictionary<SpellSlot, Spell> spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q) },
            { SpellSlot.W, new Spell(SpellSlot.W) },
            { SpellSlot.E, new Spell(SpellSlot.E) },
            { SpellSlot.R, new Spell(SpellSlot.R) }
        };

        protected override void OnChampLoad()
        {
            throw new NotImplementedException();
        }

        protected override void LoadMenu()
        {
            var defaultMenu = Variables.Menu;
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
