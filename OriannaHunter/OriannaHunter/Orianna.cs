using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using OriannaHunter.Core;

namespace OriannaHunter
{
    class Orianna
    {
        internal static void OnLoad()
        {
            //Perfectioning the skills delays, widths and stuff
            Variables.spells[SpellSlot.Q].SetSkillshot(0.75f, 110f, 1425f, false, SkillshotType.SkillshotLine);
            Variables.spells[SpellSlot.E].SetSkillshot(0.25f, 80f, 1700f, true, SkillshotType.SkillshotLine);

            Game.OnUpdate += OnTick;
        }

        //Called every game tick
        private static void OnTick(EventArgs args)
        {
            BallManager.OnUpdate();

            switch (Variables.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    OnCombo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    OnHarass();
                    break;
                case Orbwalking.OrbwalkingMode.LastHit:
                case Orbwalking.OrbwalkingMode.LaneClear:
                    OnFarm();
                    break;
            }
        }

        //Called when pressing the combo key
        private static void OnCombo()
        {
            //Find a target for Q
            var qTarget = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range,
                TargetSelector.DamageType.Magical);

            //If the Q Target found is a valid one
            if (qTarget.IsValidTarget())
            {
                //Check if Q is ready and enabled in the menu
                if (Variables.AssemblyMenu.Item("dz191.orianna.combo.q").IsActive() &&
                    Variables.spells[SpellSlot.Q].IsReady())
                {
                    var qPrediction = Variables.spells[SpellSlot.Q].GetPrediction(qTarget);

                    //TODO AOE Q->R Setup

                    //Check if the qPrediction Cast Position is in range of Q from the player
                    if (qPrediction.CastPosition.Distance(ObjectManager.Player.ServerPosition) <
                        Variables.spells[SpellSlot.Q].Range)
                    {
                        //Create a new command instance
                        var ballCommand = new BallCommand()
                        {
                            BallCommandPosition = qPrediction.CastPosition,
                            CommandType = BallCommands.Q
                        };

                        //Execute the command
                        ballCommand.Execute();
                    }
                }
            }

            //Get another target for W based on the Ball position this time
            var wTarget =
                HeroManager.Enemies.FirstOrDefault(
                    m => m.IsValidTarget(Variables.spells[SpellSlot.W].Range, true, BallManager.BallPosition));

            if (Variables.spells[SpellSlot.W].IsKillable(wTarget))
            {
                //Create a new command instance
                var ballCommand = new BallCommand()
                {
                    CommandType = BallCommands.W
                };

                //Execute the command
                ballCommand.Execute();
            }

        }

        //Called when pressing the Harass key
        private static void OnHarass()
        {
            throw new NotImplementedException();
        }


        //Called when pressing the Farm key
        private static void OnFarm()
        {
            throw new NotImplementedException();
        }
    }
}
