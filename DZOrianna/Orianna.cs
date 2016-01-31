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
            
        }

        private static void OnMixed()
        {
            
        }

    }
}
