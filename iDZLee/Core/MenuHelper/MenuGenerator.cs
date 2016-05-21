using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using LeagueSharp.Common;

namespace iDZLee.Core.MenuHelper
{
    class MenuGenerator
    {
        internal static void Generate()
        {
            var rootMenu = Variables.Menu;

            try
            {
                Variables.Orbwalker = new Orbwalking.Orbwalker(rootMenu);
                TargetSelector.AddToMenu(rootMenu);

                //TODO Rest of the Menu
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("MenuGenerator", "Error Generating Menu"));
            }
        }
    }
}
