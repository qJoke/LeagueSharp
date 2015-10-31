using System;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Geometry = DZAwarenessAIO.Utility.Geometry;
using Color = System.Drawing.Color;

namespace DZAwarenessAIO.Modules.Gank_Alerter
{
    /// <summary>
    /// The Gank Alerter base Class
    /// </summary>
    class GankAlerterBase : ModuleBase
    {
        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("Gank Alerter", "dz191.dza.gank");
                {
                    moduleMenu.AddBool("dz191.dza.gank", "Gank Alerter", true);
                    moduleMenu.AddSlider("dz191.dza.gank.textsize", "Text Size", 26, 17, 52).SetTooltip("The text size of the Gank alert text (Requires F5 after change)");
                    moduleMenu.AddSlider("dz191.dza.gank.mindist", "Min. Distance", 1100, 300, 1800).SetTooltip("The Minimum detection distance");
                    moduleMenu.AddSlider("dz191.dza.gank.maxdist", "Max. Distance", 3500, 500, 6000).SetTooltip("The Maximum detection distance");

                    var ignoreMenu = new Menu("Ignore Champions", "dz191.dza.gank.ignore");
                    {
                        foreach (var hero in HeroManager.Enemies)
                        {
                            ignoreMenu.AddItem(
                                new MenuItem(
                                    $"dz191.dza.gank.ignore.{hero.ChampionName.ToLower()}",
                                    hero.ChampionName)).SetValue(false);
                        }
                        moduleMenu.AddSubMenu(ignoreMenu);
                    }
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("GankAlerter_Base", e));
            }

        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            try
            {
                GankAlerterVariables.GankAlertText = new Render.Text("", 0, 0, GankAlerterVariables.TextSize, SharpDX.Color.Chartreuse)
                {
                    Visible = false
                };
                GankAlerterCalculator.OnLoad();
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("GankAlerter_Base", e));
            }
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
        /// Shoulds the module run.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<bool>("dz191.dza.gank");
        }

        /// <summary>
        /// Called On Update
        /// </summary>
        public override void OnTick()
        {
            GankAlerterCalculator.GetGankingHero();
        }
    }
}
