using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Geometry = DZAwarenessAIO.Utility.Geometry;
using Color = System.Drawing.Color;

namespace DZAwarenessAIO.Modules.LaneStatus
{
    /// <summary>
    /// The Ranges Tracking Class
    /// </summary>
    class LaneStatusBase : ModuleBase
    {
        private LaneStatusTracker Tracker = new LaneStatusTracker();

        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("Lane Status", "dz191.dza.lane");
                {
                    moduleMenu.AddBool("dz191.dza.lane.show", "Track lane status", true);
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Lane_Base", e));
            }

        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            try
            {

            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Lane_Base", e));
            }
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
        /// Shoulds the module run.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<bool>("dz191.dza.ping.show");
        }

        /// <summary>
        /// Called On Update
        /// </summary>
        public override void OnTick() { }
    }
}
