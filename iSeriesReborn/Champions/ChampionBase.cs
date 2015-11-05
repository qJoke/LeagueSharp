using System;
using System.Collections.Generic;
using iSeriesReborn.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions
{
    abstract class ChampionBase
    {
        private delegate void OrbwalkerDelegate();

        private Dictionary<Orbwalking.OrbwalkingMode, OrbwalkerDelegate> OrbwalkerCallbacks;

        public void OnLoad()
        {
            OrbwalkerCallbacks = new Dictionary<Orbwalking.OrbwalkingMode, OrbwalkerDelegate>()
            {
                { Orbwalking.OrbwalkingMode.Combo, OnCombo },
                { Orbwalking.OrbwalkingMode.Mixed, OnMixed },
                { Orbwalking.OrbwalkingMode.LastHit, OnLastHit },
                { Orbwalking.OrbwalkingMode.LaneClear, OnLaneClear },
                { Orbwalking.OrbwalkingMode.None, () => { } }
            };

            OnChampLoad();
            LoadMenu();
            Game.OnUpdate += OnUpdate;
        }

        private void OnUpdate(EventArgs args)
        {
            if (OrbwalkerCallbacks.ContainsKey(Variables.Orbwalker.ActiveMode))
            {
                OrbwalkerCallbacks[Variables.Orbwalker.ActiveMode]();
            }

            OnTick();
        }

        protected abstract void OnChampLoad();

        protected abstract void LoadMenu();

        protected abstract void OnTick();

        protected abstract void OnCombo();

        protected abstract void OnMixed();

        protected abstract void OnLastHit();

        protected abstract void OnLaneClear();

        public abstract Dictionary<SpellSlot, Spell> GetSpells();
    }
}
