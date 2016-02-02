using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZOrianna.Utility;
using DZOrianna.Utility.Ball;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZOrianna
{
    class Orianna
    {
        internal static void OnLoad()
        {
            CommandQueue.InitEvents();
            Variables.spells[SpellSlot.Q].SetSkillshot(0f, 110f, 1425f, false, SkillshotType.SkillshotLine);

            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            switch (Variables.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    OnCombo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    OnMixed();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:

                    break;
            }
        }

        private static void OnCombo()
        {
            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.q").GetValue<bool>() && Variables.spells[SpellSlot.Q].IsReady())
            {
                var qTarget = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range / 1.5f, TargetSelector.DamageType.Magical);

                if (qTarget.IsValidTarget())
                {
                    Variables.BallManager.ProcessCommand(new Command()
                    {
                        SpellCommand = Commands.Q,
                        Unit = qTarget
                    });
                }
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.w").GetValue<bool>() && Variables.spells[SpellSlot.W].IsReady())
            {
                var ballPosition = Variables.BallManager.BallPosition;
                var minWEnemies = Variables.AssemblyMenu.Item("dz191.orianna.combo.minw").GetValue<Slider>().Value;

                if (ballPosition.CountEnemiesInRange(Variables.spells[SpellSlot.W].Range) >= minWEnemies)
                {
                    Variables.BallManager.ProcessCommand(new Command()
                    {
                        SpellCommand = Commands.W,
                    });
                }
            }
        }

        private static void OnMixed()
        {
            
        }

    }
}
