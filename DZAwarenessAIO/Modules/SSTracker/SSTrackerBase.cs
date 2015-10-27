using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.HudUtility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.SSTracker
{
    class SSTrackerBase : ModuleBase
    {
        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("SS Tracker", "dz191.dza.sstracker");
                {
                    moduleMenu.AddBool("dz191.dza.sstracker.hud", "Track in Hud", true);
                    //moduleMenu.AddBool("dz191.dza.sstracker.minimap", "Track in minimap", true);
                    moduleMenu.AddSlider("dz191.dza.sstracker.mintime", "Minimum SS time", new Tuple<int, int, int>(5,1,10));
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("SSTracker_Base", e));
            }

        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            SSTrackerModule.OnLoad();
        }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <returns></returns>
        public override ModuleTypes GetModuleType()
        {
            return ModuleTypes.General;
        }

        /// <summary>
        /// Determines whether or not the module should run.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRun()
        {
            return HudVariables.ShouldBeVisible;
        }

        /// <summary>
        /// Called On Update
        /// </summary>
        public override void OnTick(){ }
    }
}
