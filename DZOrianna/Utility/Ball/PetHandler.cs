using System;
using System.Collections.Generic;
using DZOrianna.Utility.Ball;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZOrianna.Utility
{
    class PetHandler
    {
        public Vector3 BallPosition { get; private set; }

        private float LastExecuteTick = 0f;

        public void OnLoad()
        {
            BallPosition = ObjectManager.Player.ServerPosition;

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSCast;
        }

        public void ProcessCommandList(List<Command> commands)
        {
            foreach (var command in commands)
            {
                ProcessCommand(command);
            }
        }

        public void ProcessCommand(Command command)
        {
            switch (command.SpellCommand)
            {
                case Commands.Q:
                    if (command.Where != default(Vector3))
                    {
                        Variables.spells[SpellSlot.Q].Cast(command.Where);
                    }

                    if (command.Unit != null)
                    {
                        Variables.spells[SpellSlot.Q].Cast(command.Unit.ServerPosition);
                    }
                    break;
                case Commands.W:
                    Variables.spells[SpellSlot.W].Cast();
                    break;
                case Commands.E:
                    if (command.Unit.IsValidTarget(float.MaxValue, false))
                    {
                        Variables.spells[SpellSlot.E].Cast(command.Unit);
                    }
                    break;
                case Commands.R:
                    if (BallPosition.CountEnemiesInRange(300f) > 0)
                    {
                        Variables.spells[SpellSlot.R].Cast();
                    }
                    break;
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Environment.TickCount - LastExecuteTick < 80)
            {
                return;
            }
            LastExecuteTick = Environment.TickCount;

            if (ObjectManager.Player.HasBuff(BallStringList.OriannaSelfShield))
            {
                BallPosition = ObjectManager.Player.ServerPosition;
                return;
            }

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.HasBuff(BallStringList.OriannaShieldAlly))
                {
                    BallPosition = ally.ServerPosition;
                    return;
                }
            }
        }

        private void OnProcessSCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.SData.Name)
                {
                    case BallStringList.IzunaCommand:
                            LeagueSharp.Common.Utility.DelayAction.Add((int)(BallPosition.Distance(args.End) / 1.25f - 60 - Game.Ping), () => BallPosition = args.End);
                            BallPosition = Vector3.Zero;
                        break;
                    case BallStringList.RedactCommand:
                        BallPosition = Vector3.Zero;
                        break;
                }
            }
        }
    }
    public enum Commands
    {
        Q, W, E, R
    }

    public class Command
    {
        public Commands SpellCommand { get; set; }

        public Vector3 Where { get; set; }

        public Obj_AI_Base Unit { get; set; }
    }
}
