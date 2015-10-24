using System;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
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
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("WardTracker_Base", e));
            }
        }

        public override void InitEvents()
        {
            Obj_AI_Base.OnProcessSpellCast += WardDetector.OnProcessSpellCast;
            GameObject.OnCreate += WardDetector.OnCreate;
        }

        public override ModuleTypes GetModuleType()
        {
            return ModuleTypes.OnUpdate;
        }

        public override bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<bool>("dz191.dza.ward.track");
        }

        public override void OnTick()
        {
            WardDetector.OnTick();
        }
    }
}
