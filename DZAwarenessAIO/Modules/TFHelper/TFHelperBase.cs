using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    moduleMenu.AddBool("dz191.dza.tf", "Show HUD").SetTooltip("Shows the TF Helper hud");
                    moduleMenu.AddSlider("dz191.dza.tf.hud.x", "HUD X", new Tuple<int, int, int>(200, 0, Drawing.Direct3DDevice.Viewport.Width)).SetTooltip("Hud X Position (You can drag it)");
                    moduleMenu.AddSlider("dz191.dza.tf.hud.y", "HUD Y", new Tuple<int, int, int>(200, 0, Drawing.Direct3DDevice.Viewport.Height)).SetTooltip("Hud Y Position (You can drag it)");
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
            TFHelperDrawings.OnLoad();
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
