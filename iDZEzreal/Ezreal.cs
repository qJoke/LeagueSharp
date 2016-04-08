using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace iDZEzreal
{
    class Ezreal
    {
        public static void OnLoad()
        {
            LoadSpells();
            LoadEvents();  
        }

        private static void LoadSpells()
        {
            //TODO
        }

        private static void LoadEvents()
        {
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
