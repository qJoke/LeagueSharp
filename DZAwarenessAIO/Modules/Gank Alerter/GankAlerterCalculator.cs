using System;
using System.Linq;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.Gank_Alerter
{
    /// <summary>
    /// Does the calculations which determine whether we are being ganked or not.
    /// </summary>
    class GankAlerterCalculator
    {
        /// <summary>
        /// Called when the Gank Alerter calculator is loaded.
        /// </summary>
        public static void OnLoad()
        {
            GankAlerterVariables.GankAlertText.Centered = true;
            GankAlerterVariables.GankAlertText.VisibleCondition = delegate
            { return MenuExtensions.GetItemValue<bool>("dz191.dza.gank") && GetGankingHero() != null; };
            GankAlerterVariables.GankAlertText.X = (int)(Drawing.Direct3DDevice.Viewport.Width / 2f);
            GankAlerterVariables.GankAlertText.Y = (int)(9);
            GankAlerterVariables.GankAlertText.TextUpdate = () => GetGankingHero() != null
                ? string.Format(GetGankingHero().ChampionName + " is ganking!")
                : string.Empty;
            GankAlerterVariables.GankAlertText.Add(0);
        }

        /// <summary>
        /// Gets the ganking hero.
        /// </summary>
        /// <returns>An istance of the enemy who is ganking me</returns>
        private static Obj_AI_Hero GetGankingHero()
        {
            foreach (
                var hero in
                    HeroManager.Enemies.Where(
                        h => !MenuExtensions.GetItemValue<bool>($"dz191.dza.gank.ignore.{h.ChampionName.ToLower()}") && h.IsValidTarget()))
            {
                var heroDistance = hero.ServerPosition.Distance(ObjectManager.Player.ServerPosition);
                if (heroDistance >= GankAlerterVariables.MinDist && heroDistance <= GankAlerterVariables.MaxDist)
                {
                    var pathList = hero.Path;
                    if (pathList.Count() > 1)
                    {
                        var secondDistance = pathList[2].Distance(ObjectManager.Player.ServerPosition);
                        if (secondDistance < heroDistance)
                        {
                            return hero;
                        }
                    }
                }
            }

            return null;
        }
    }
}
