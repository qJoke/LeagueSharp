using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;

namespace iSeriesReborn.Champions.Tristana
{
    class TristanaUtility
    {
        public static float GetERRange()
        {
            return 550 + (7 * (ObjectManager.Player.Level - 1));
        }
    }
}
