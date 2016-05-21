using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDZLee.Core;
using LeagueSharp.Common;

namespace iDZLee.Modes
{
    class Combo : IMode
    {
        public bool GetRunCondition()
        {
            return Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo;
        }

        public string GetModeName()
        {
            return "Combo";
        }

        public void Run()
        {
            
        }
    }
}
