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
                    moduleMenu.AddBool("dz191.dza.ward.track", "Track wards").SetTooltip("Tracks Wards, Pinks, Shrooms etc.");
                    moduleMenu.AddKeybind("dz191.dza.ward.extrainfo", "Show Extra informations", new Tuple<uint, KeyBindType>('Z', KeyBindType.Press)).SetTooltip("Click the button and hover a ward polygon for more info.");
                    moduleMenu.AddSlider("dz191.dza.ward.sides", "Sides of Polygon (Higher = Laggier)", new Tuple<int, int, int>(4, 3, 12)).SetTooltip("The sides of the polygon the wards have drawn around.");
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
            GameObject.OnDelete += WardDetector.OnDelete;
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
