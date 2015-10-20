using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.WardTracker
{
    /// <summary>
    /// The Ward Tracker class
    /// </summary>
    class WardTrackerBase : ModuleBase
    {
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("Wards Tracker", "dz191.dza.ward");
                {

                    moduleMenu.AddBool("dz191.dza.ward.track", "Track wards");
                    RootMenu.AddSubMenu(RootMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("WardTracker_Base", e));
            }
        }

        public override void InitEvents()
        {
            throw new NotImplementedException();
        }

        public override ModuleTypes GetModuleType()
        {
            throw new NotImplementedException();
        }

        public override bool ShouldRun()
        {
            throw new NotImplementedException();
        }

        public override void OnTick()
        {
            throw new NotImplementedException();
        }
    }
}
