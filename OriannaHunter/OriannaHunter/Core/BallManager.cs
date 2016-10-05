using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace OriannaHunter.Core
{
    class BallManager
    {
        //The current ball position
        public static Vector3 BallPosition { get; set; }

        //The last time the loop was executed
        private static float LastTick = 0f;

        //Called on Load
        public static void OnLoad()
        {
            //The ball is on the player at the start
            BallPosition = ObjectManager.Player.ServerPosition;

            Obj_AI_Base.OnDoCast += OnDoCast;
        }

        //Used so we can build a list of commands in order and they will be executed in that specific order
        public static void ExecuteCommandList(List<BallCommand> CommandList)
        {
            foreach (var command in CommandList)
            {
                command.Execute();
            }
        } 

        //Called when a hero finishes casting a spell
        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.Slot != SpellSlot.Unknown)
            {
                switch (args.Slot)
                {
                    case SpellSlot.Q:
                        //We delay the setting of the position to prevent inaccuracies
                        var endPos = args.End;
                        Utility.DelayAction.Add((int)(BallPosition.Distance(endPos) / 1.20f - Game.Ping / 2f), () =>
                        {
                            BallPosition = args.End;
                        });
                        //In the meantime we set the ball position to nothing to prevent stuff like random Rs happening
                        BallPosition = Vector3.Zero;
                        break;
                }

                //If the ball is called back to the player (E) then we set the ball position to Zero since the other for loop will take care
                //of handling that already
                if (args.SData.Name == "OrianaRedactCommand")
                {
                    //If the ball position is not Zero already we set it so
                    if (!BallPosition.IsZero)
                    {
                        BallPosition = Vector3.Zero;
                    }
                }
            }
        }

        internal static void OnUpdate()
        {
            //If it was executed in the last 100 ticks let's not do anything
            if (Environment.TickCount - LastTick < 100f)
            {
                return;
            }
            LastTick = Environment.TickCount;

            //If the player or an ally has the Ball buff on him it means the ball is on him

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.IsValidTarget(float.MaxValue, false) && ally.HasBuff("OrianaGhostSelf"))
                {
                    BallPosition = ally.ServerPosition;
                }
            }
        }

    }

    //Enum for the various ball command types
    enum BallCommands
    {
        Q, W, E, R
    }

    class BallCommand
    {
        //To specify the command type
        public BallCommands CommandType { get; set; }

        //For stuff such as Q Command
        public Vector3 BallCommandPosition { get; set; }

        //For stuff such as E Command
        public Obj_AI_Base BallCommandTarget { get; set; }

        public void Execute()
        {
            //Deciding based on command type
            switch (CommandType)
            {
                //We send the Ball to Q position after checking distance
                case BallCommands.Q:
                    if (BallCommandPosition != Vector3.Zero 
                        && Variables.spells[SpellSlot.Q].IsReady() 
                        && BallCommandPosition.Distance(BallManager.BallPosition) < Variables.spells[SpellSlot.Q].Range)
                    {
                        Variables.spells[SpellSlot.Q].Cast(BallCommandPosition);
                    }
                    break;
                //We check if there are any enemies in W range
                case BallCommands.W:
                    if (Variables.spells[SpellSlot.W].IsReady() &&
                        BallManager.BallPosition.CountEnemiesInRange(Variables.spells[SpellSlot.W].Range) > 0)
                    {
                        Variables.spells[SpellSlot.W].Cast();
                    }
                    break;
                //If the command is E we check if the target is valid and the target is an ally
                case BallCommands.E:
                    if (Variables.spells[SpellSlot.E].IsReady() 
                        && BallCommandTarget.IsValidTarget(Variables.spells[SpellSlot.E].Range, false)
                        && BallCommandTarget.IsAlly)
                    {
                        Variables.spells[SpellSlot.E].Cast(BallCommandTarget);
                    }
                    break;
                //We check if there are enemies in range and R is ready
                case BallCommands.R:
                    if (Variables.spells[SpellSlot.R].IsReady() &&
                        BallManager.BallPosition.CountEnemiesInRange(Variables.spells[SpellSlot.R].Range) > 0)
                    {
                        Variables.spells[SpellSlot.R].Cast();
                    }
                    break;
            }
        }
    }
}
