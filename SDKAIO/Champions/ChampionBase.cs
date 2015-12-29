// <copyright file="ChampionBase.cs" company="LeagueSharp">
//    Copyright (c) 2015 LeagueSharp.
// 
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see http://www.gnu.org/licenses/
// </copyright>

namespace SDKAIO.Champions
{
    using System;
    using System.Collections.Generic;

    using global::SDKAIO.Utility;

    using LeagueSharp;
    using LeagueSharp.SDK.Core;
    using LeagueSharp.SDK.Core.Enumerations;
    using LeagueSharp.SDK.Core.Utils;
    using LeagueSharp.SDK.Core.Wrappers;

    /// <summary>
    /// The Base class for Champions in the AIO
    /// </summary>
    abstract class ChampionBase
    {
        /// <summary>
        /// The OrbwalkerDelegate method.
        /// </summary>
        private delegate void OrbwalkerDelegate();

        /// <summary>
        /// The Orbwalker callbacks dictionary, associating the OrbwwalkingMode to a Callback delegate.
        /// </summary>
        private Dictionary<OrbwalkingMode, OrbwalkerDelegate> OrbwalkerCallbacks;

        /// <summary>
        /// Called when the Champion Module gets loaded.
        /// </summary>
        public void OnLoad()
        {
            try
            {


                this.OrbwalkerCallbacks = new Dictionary<OrbwalkingMode, OrbwalkerDelegate>()
                                              {
                                                  { OrbwalkingMode.Combo, this.OnCombo },
                                                  { OrbwalkingMode.Hybrid, this.OnHybrid },
                                                  { OrbwalkingMode.LastHit, this.OnLastHit },
                                                  { OrbwalkingMode.LaneClear, this.OnLaneClear },
                                                  { OrbwalkingMode.None, () => { } }
                                              };

                this.OnChampLoad();
                var menuGenerator = this.GetMenuGenerator();

                if (menuGenerator == null)
                {
                    Logging.Write()(LogLevel.Error, $"[SDK AIO] MenuGenerator instance of ${ObjectManager.Player.ChampionName} is null!.");
                    return;
                }
                
                menuGenerator.LoadToMenu(AIOVariables.AssemblyMenu);
                Game.OnUpdate += this.OnUpdate;
                Obj_AI_Base.OnDoCast += this.AfterAttack;
            }
            catch
            {
                Logging.Write()(LogLevel.Error, $"[SDK AIO] Failed to initialize ${ObjectManager.Player.ChampionName}.");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
            {
                return;
            }

            if (this.OrbwalkerCallbacks.ContainsKey(Variables.Orbwalker.GetActiveMode()))
            {
                this.OrbwalkerCallbacks[Variables.Orbwalker.GetActiveMode()]();
            }

            this.OnTick();
        }


        /// <summary>
        /// Delegate to the OnDoCast event
        /// </summary>
        /// <param name="sender">The sender unit.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private void AfterAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && AutoAttack.IsAutoAttack(args.SData.Name))
            {
                this.OnAfterAttack(sender, args);
            }
        }

        /// <summary>
        /// Called when the Champion Module is loaded.
        /// </summary>
        protected abstract void OnChampLoad();

        /// <summary>
        /// Called every tick of the Game.
        /// </summary>
        protected abstract void OnTick();

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.Combo.
        /// </summary>
        protected abstract void OnCombo();

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.Hybrid.
        /// </summary>
        protected abstract void OnHybrid();

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.LastHit.
        /// </summary>
        protected abstract void OnLastHit();

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.LaneClear.
        /// </summary>
        protected abstract void OnLaneClear();

        /// <summary>
        /// Called when the unit finishes the windup time for the AutoAttack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        protected abstract void OnAfterAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args);

        /// <summary>
        /// Gets the spells dictionary for the champion.
        /// </summary>
        /// <returns>The spell dictionary</returns>
        public abstract Dictionary<SpellSlot, Spell> GetSpells();

        /// <summary>
        /// Gets the MenuGenerator instance for the current champion.
        /// </summary>
        /// <returns>The instance</returns>
        protected abstract IMenuGenerator GetMenuGenerator();

    }
}
