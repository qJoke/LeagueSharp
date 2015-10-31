using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Properties;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.HudUtility;
using DZAwarenessAIO.Utility.HudUtility.HudElements;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace DZAwarenessAIO.Modules.TFHelper
{
    class TFHelperDrawings
    {
        public static void OnLoad()
        {
                var hudPanel = new HudPanel("Teamfight Helper", 10, 10, 250, 200);
                {
                    TFHelperVariables.AllyBarSprite = new Render.Sprite(Resources.AllyTeamStrength, new Vector2(hudPanel.Position.X + 3, hudPanel.Position.Y + 15))
                    {
                        PositionUpdate = () => new Vector2(hudPanel.Position.X + 5, hudPanel.Position.Y + 18),
                        Visible = true,
                        Scale = new Vector2(0.95f, 0.95f),
                        VisibleCondition = delegate
                        { return HudVariables.ShouldBeVisible && HudVariables.CurrentStatus == SpriteStatus.Expanded; }
                    };

                    TFHelperVariables.EnemyBarSprite = new Render.Sprite(Resources.EnemyTeamStength, new Vector2(hudPanel.Position.X + 3, hudPanel.Position.Y + 36))
                    {
                        PositionUpdate = () => new Vector2(hudPanel.Position.X + 5, hudPanel.Position.Y + 48),
                        Visible = true,
                        Scale = new Vector2(0.95f, 0.95f),
                        VisibleCondition = delegate
                        { return HudVariables.ShouldBeVisible && HudVariables.CurrentStatus == SpriteStatus.Expanded; }
                    };

                    TFHelperVariables.AllyStrengthText = new Render.Text(
                        "",
                        new Vector2(0, 0), 19, Color.White)
                    {
                        Centered = true,
                        PositionUpdate =
                            () =>
                                new Vector2(
                                    TFHelperVariables.AllyBarSprite.Position.X + TFHelperVariables.AllyBarSprite.Width / 2f,
                                     TFHelperVariables.AllyBarSprite.Position.Y + TFHelperVariables.AllyBarSprite.Height / 2f),
                        TextUpdate = () => $"{TFHelperCalculator.GetAllyStrength() * 100} %",
                        VisibleCondition = delegate
                        { return HudVariables.ShouldBeVisible && HudVariables.CurrentStatus == SpriteStatus.Expanded; }
                    };

                    TFHelperVariables.EnemyStrengthText = new Render.Text(
                        "",
                        new Vector2(0,0) , 19, Color.White)
                    {
                        Centered = true,
                        PositionUpdate =
                            () =>
                                new Vector2(
                                    TFHelperVariables.EnemyBarSprite.Position.X + TFHelperVariables.EnemyBarSprite.Width / 2f,
                                     TFHelperVariables.EnemyBarSprite.Position.Y + TFHelperVariables.EnemyBarSprite.Height / 2f),
                        TextUpdate = () => $"{TFHelperCalculator.GetEnemyStrength() * 100} %",
                        VisibleCondition = delegate
                        { return HudVariables.ShouldBeVisible && HudVariables.CurrentStatus == SpriteStatus.Expanded; }
                    };

                    TFHelperVariables.TeamsVSText = new Render.Text("", new Vector2(0, 0), 19, Color.White)
                    {
                        Centered = true,
                        TextUpdate = () => TFHelperCalculator.GetText(),
                        PositionUpdate =
                            () =>
                                new Vector2(hudPanel.Position.X + hudPanel.Width / 2f, hudPanel.Position.Y + hudPanel.Height / 2f - 12),
                        VisibleCondition = delegate
                            { return HudVariables.ShouldBeVisible && HudVariables.CurrentStatus == SpriteStatus.Expanded; }
                    };

                    TFHelperVariables.AllyBarSprite.Add(3);
                    TFHelperVariables.EnemyBarSprite.Add(3);
                    TFHelperVariables.AllyStrengthText.Add(4);
                    TFHelperVariables.EnemyStrengthText.Add(4);
                    TFHelperVariables.TeamsVSText.Add(4);

                    Game.OnUpdate += OnUpdate;
                }

        }

        private static void OnUpdate(EventArgs args)
        {
            var allyStrength = TFHelperCalculator.GetAllyStrength();
            var enemyStrength = TFHelperCalculator.GetEnemyStrength();

            TFHelperVariables.AllyBarSprite.Crop(
                0, 0, (int) (TFHelperVariables.AllyBarSprite.Width * allyStrength), (int) TFHelperVariables.AllyBarSprite.Height);

            TFHelperVariables.EnemyBarSprite.Crop(
                0, 0, (int) (TFHelperVariables.EnemyBarSprite.Width * enemyStrength),
                (int) TFHelperVariables.EnemyBarSprite.Height);

        }

        private static float Round(float number)
        {
            return (float) Math.Ceiling(number + 0.5);
        }
    }
}
