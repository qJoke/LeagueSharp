using DZLib.Logging;
using iSeriesReborn.Champions.Vayne.Skills;
using iSeriesReborn.Champions.Vayne.Utility;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.Geometry;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace iSeriesReborn.Champions.Vayne.Modules
{
    class VayneAutoE : IModule
    {
        public string GetName()
        {
            return "Vayne_AutoE";
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<bool>("iseriesr.vayne.misc.condemn.autoe");
        }

        public void Run()
        {
            VayneE.ECheck();
        }
    }
}
