using System;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.WardTracker
{
    /// <summary>
    /// The Ward Tracker base class
    /// </summary>
    class WardTrackerBase : ModuleBase
    {
        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("Wards Tracker", "dz191.dza.ward");
                {
                    moduleMenu.AddBool("dz191.dza.ward.track", "Track wards");
                    moduleMenu.AddSlider("dz191.dza.ward.sides", "Sides of Polygon (Higher = Laggier)", new Tuple<int, int, int>(4, 3, 12));
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("WardTracker_Base", e));
            }
        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            Obj_AI_Base.OnProcessSpellCast += WardDetector.OnProcessSpellCast;
            GameObject.OnCreate += WardDetector.OnCreate;
            Drawing.OnDraw += WardDetector.OnDraw;
        }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <returns></returns>
        public override ModuleTypes GetModuleType()
        {
            return ModuleTypes.OnUpdate;
        }

        /// <summary>
        /// Determines whether or not the module should run.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<bool>("dz191.dza.ward.track");
        }

        /// <summary>
        /// Called OnUpdate
        /// </summary>
        public override void OnTick()
        {
            WardDetector.OnTick();
        }
    }
}
