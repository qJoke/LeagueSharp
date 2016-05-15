using System;
using DZAIO_Reborn.Core;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn
{
    class DZAIO
    {
        public static void Init()
        {
            Game.OnUpdate += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (Variables.CurrentChampion != null && Variables.Orbwalker != null)
            {
                Variables.CurrentChampion.OnTick();

                switch (Variables.Orbwalker.ActiveMode)
                {
                    case Orbwalking.OrbwalkingMode.Combo:
                        Variables.CurrentChampion.OnCombo();
                        break;
                    case Orbwalking.OrbwalkingMode.Mixed:
                        Variables.CurrentChampion.OnMixed();
                        break;
                    case Orbwalking.OrbwalkingMode.LastHit:
                        Variables.CurrentChampion.OnLastHit();
                        break;
                    case Orbwalking.OrbwalkingMode.LaneClear:
                        Variables.CurrentChampion.OnLaneclear();
                        break;
                }
            }
            
        }
    }
}
