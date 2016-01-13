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
    using System.Linq;

    using global::SDKAIO.Utility;

    using LeagueSharp;
    using LeagueSharp.SDK;
    using LeagueSharp.SDK.Core;
    using LeagueSharp.SDK.Core.UI.IMenu.Values;

    /// <summary>
    /// This class handles the Janna champion.
    /// </summary>
    class Janna : ChampionBase
    {
        /// <summary>
        /// The Janna MenuGenerator Instance.
        /// </summary>
        private IMenuGenerator JannaMenuGenerator;

        /// <summary>
        /// True if the ulti channel is in progress, false otherwise.
        /// </summary>
        private bool UltiChannelInProgress;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="Janna"/> class.
        /// </summary>
        public Janna()
        {
            this.JannaMenuGenerator = new JannaMenuGenerator();
            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
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
           if (Events.IsCastingInterruptableSpell(GameObjects.Player, true) && !(ObjectManager.Player.CountEnemyHeroesInRange(450f) >= 1))
           {
              return; 
           }

            if (this.UltiChannelInProgress)
            {
                 Variables.Orbwalker.SetAttackState(true);
                 Variables.Orbwalker.SetMovementState(true);
                 this.UltiChannelInProgress = false;
            }
        }

        /// <summary>
        /// Called when the Active mode of the Orbwalker is OrbwalkingMode.Combo.
        /// </summary>
        protected override void OnCombo()
        {
            var qEnabled = AIOVariables.AssemblyMenu["sdkaio.janna.combo"]["UseQ"].GetValue<MenuBool>().Value;
            var wEnabled = AIOVariables.AssemblyMenu["sdkaio.janna.combo"]["UseW"].GetValue<MenuBool>().Value;
            var eEnabled = AIOVariables.AssemblyMenu["sdkaio.janna.combo"]["UseE"].GetValue<MenuBool>().Value;
            var rMenu = AIOVariables.AssemblyMenu["sdkaio.janna.combo"]["RMinAlliesSB"].GetValue<MenuSliderButton>();
            var target = Variables.TargetSelector.GetTarget(this.GetSpells()[SpellSlot.Q]);

            if (qEnabled && this.GetSpells()[SpellSlot.Q].IsReady())
            {
                var prediction = this.GetSpells()[SpellSlot.Q].GetPrediction(target);
                if (prediction.Hitchance >= HitChance.High)
                {
                    this.GetSpells()[SpellSlot.Q].Cast(prediction.CastPosition);
                    this.GetSpells()[SpellSlot.Q].Cast();
                }
            }

            if (wEnabled && this.GetSpells()[SpellSlot.W].IsReady() && target.IsValidTarget(this.GetSpells()[SpellSlot.W].Range))
            {
                this.GetSpells()[SpellSlot.W].Cast(target);
            }

            if (rMenu.BValue 
                && this.GetSpells()[SpellSlot.R].IsReady() 
                && GameObjects.Player.CountEnemyHeroesInRange(950f) > 0 
                && GameObjects.AllyHeroes.Count(h => h.IsValidTarget(950f, false) && h.HealthPercent < 10) > 0)
            {
                this.GetSpells()[SpellSlot.R].Cast();
            }
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
        /// Called when a spell is processed by the game after its casting (After a spell)
        /// </summary>
        /// <param name="sender">The unit who casted the spell.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name.Equals("ReapTheWhirlwind"))
            {
                Variables.Orbwalker.SetAttackState(false);
                Variables.Orbwalker.SetAttackState(false);
                this.UltiChannelInProgress = true;
            }
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
