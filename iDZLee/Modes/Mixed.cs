using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDZLee.Core;
using LeagueSharp.Common;

namespace iDZLee.Modes
{
    internal class Mixed : IMode
    {
        public bool GetRunCondition()
        {
            return Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed;
        }

        public string GetModeName()
        {
            return "Mixed";
        }

        public void Run()
        {
            
        }
    }
}
