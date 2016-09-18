using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OriannaHunter.Core;
using OriannaHunter.Menu;

namespace OriannaHunter
{
    class Bootstrap
    {
        internal static void Init()
        {
            Variables.AssemblyMenu = new LeagueSharp.Common.Menu("Orianna", "dz191.oriannahunter", true);
            MenuGenerator.GenerateMenu(Variables.AssemblyMenu);
            Variables.AssemblyMenu.AddToMainMenu();
            SpellQueue.OnLoad();
            BallManager.OnLoad();

            Orianna.OnLoad();
        }
    }
}
