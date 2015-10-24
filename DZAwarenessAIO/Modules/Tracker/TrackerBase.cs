using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.Tracker
{
    class TrackerBase : ModuleBase
    {
        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            var RootMenu = Variables.Menu;

            var TrackerMenu = new Menu("Tracker", "dz191.dza.tracker");
            {
                TrackerMenu.AddBool("dz191.dza.tracker.track.allies", "Track Allies", true).SetTooltip("Track allies Cooldowns");
                TrackerMenu.AddBool("dz191.dza.tracker.track.enemies", "Track Enemies", true).SetTooltip("Track enemies Cooldowns");
                TrackerMenu.AddBool("dz191.dza.tracker.track.cd", "Show Cooldowns", true).SetTooltip("Track enemies Cooldowns");
                //TrackerMenu.AddBool("dz191.dza.tracker.track.exp", "Exp Tracker").SetTooltip("Track exp");
                RootMenu.AddSubMenu(TrackerMenu);
            }
        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            TrackerDrawings.OnLoad();
        }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <returns>The type of the module</returns>
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
            return false;
        }

        /// <summary>
        /// Called On update.
        /// </summary>
        public override void OnTick(){ }
    }
}
