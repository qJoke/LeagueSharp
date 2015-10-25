using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.TFHelper
{
    class TFHelperBase : ModuleBase
    {
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("TF Helper", "dz191.dza.tf");
                {
                    
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("TFHelper_Base", e));
            }
        }

        public override void InitEvents()
        {
            //TFHelperDrawings.OnLoad();
        }

        public override ModuleTypes GetModuleType()
        {
            return ModuleTypes.General;
        }

        public override bool ShouldRun()
        {
            return false;
        }

        public override void OnTick(){ }
    }
}
