using System;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.TFHelper
{
    /// <summary>
    /// The Team fight helper base class
    /// </summary>
    class TFHelperBase : ModuleBase
    {
        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("TF Helper", "dz191.dza.tf");
                {
                    moduleMenu.AddSlider("dz191.dza.tf.range", "TF Range", 1200, 500, 1800);
                    RootMenu.AddSubMenu(moduleMenu);
                }

            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("TFHelper_Base", e));
            }
        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            TFHelperDrawings.OnLoad();
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
        /// Determines whether the module should run.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRun()
        {
            return false;
        }

        /// <summary>
        /// Called On Update
        /// </summary>
        public override void OnTick(){ }
    }
}
