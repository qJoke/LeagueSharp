// <copyright file="Janna.cs" company="LeagueSharp">
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

namespace SDKAIO.Champions.Janna
{
    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.SDK.Core.Wrappers;

    /// <summary>
    /// This class handles the Janna champion.
    /// </summary>
    class Janna : ChampionBase
    {
        private IMenuGenerator JannaMenuGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Janna"/> class.
        /// </summary>
        public Janna()
        {
            this.JannaMenuGenerator = new JannaMenuGenerator();
        }

        /// <summary>
        /// Called when the Champion Module is loaded.
        /// </summary>
        protected override void OnChampLoad()
        {
            
        }

        /// <summary>
        /// Called every tick of the Game.
        /// </summary>
        protected override void OnTick()
        {
            
        }

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.Combo.
        /// </summary>
        protected override void OnCombo()
        {
           
        }

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.Hybrid.
        /// </summary>
        protected override void OnHybrid()
        {
            
        }

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.LastHit.
        /// </summary>
        protected override void OnLastHit()
        {
            
        }

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.LaneClear.
        /// </summary>
        protected override void OnLaneClear()
        {
            
        }

        /// <summary>
        /// Called when the unit finishes the windup time for the AutoAttack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        protected override void OnAfterAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            
        }

        /// <summary>
        /// Gets the spells dictionary for the champion.
        /// </summary>
        /// <returns>The spell dictionary</returns>
        public override Dictionary<SpellSlot, Spell> GetSpells()
        {
            return null;
        }

        /// <summary>
        /// Gets the MenuGenerator instance for the current champion.
        /// </summary>
        /// <returns>The instance</returns>
        protected override IMenuGenerator GetMenuGenerator()
        {
            return this.JannaMenuGenerator;
        }
    }
}
