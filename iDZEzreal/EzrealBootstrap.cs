using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using Menu = LeagueSharp.Common.Menu;

namespace iDZEzreal
{
    class EzrealBootstrap
    {

        internal static void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Ezreal")
            {
                return;
            }

            Variables.Menu = new LeagueSharp.Common.Menu("iDZEzreal 3.0", "ezreal", true);
        }
    }
}
