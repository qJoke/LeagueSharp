using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Properties;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAwarenessAIO.Modules.TFHelper
{
    class TFHelperDrawings
    {
        private static Render.Sprite TFHelperBGSprite { get; set; }

        private static Vector2 CurrentPosition
        {
            get
            {
                return new Vector2(
                    MenuExtensions.GetItemValue<Slider>("dz191.dza.tf.hud.x").Value,
                    MenuExtensions.GetItemValue<Slider>("dz191.dza.tf.hud.y").Value
                    );
            }
        }

        private static readonly float SpriteWidth = Resources.TFHelperBG.Width;

        private static readonly float SpriteHeight = Resources.TFHelperBG.Height;

        private static bool IsDragging = false;

        private static Vector2 InitialDragPoint = new Vector2();

        private static float XDistanceFromEdge = 0;

        private static float YDistanceFromEdge = 0;

        private static bool ShouldBeVisible
        {
            get { return MenuExtensions.GetItemValue<bool>("dz191.dza.tf"); }
        }

        public static void OnLoad()
        {
            InitSprites();
            Game.OnWndProc += OnWndProc;
        }

        private static void OnWndProc(WndEventArgs args)
        {
            if (TFHelperBGSprite == null || !ShouldBeVisible)
            {
                return;
            }

            if (IsDragging)
            {
                Variables.Menu.Item("dz191.dza.tf.hud.x")
                   .SetValue(new Slider((int)(Utils.GetCursorPos().X - XDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Width));
                Variables.Menu.Item("dz191.dza.tf.hud.y")
                    .SetValue(new Slider((int)(Utils.GetCursorPos().Y - YDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Height));
            }

            if (IsInside(Utils.GetCursorPos()) && args.Msg == (uint)WindowsMessages.WM_LBUTTONDOWN)
            {
                if (!IsDragging)
                {
                    if (InitialDragPoint == new Vector2())
                    {
                        InitialDragPoint = CurrentPosition;
                        XDistanceFromEdge = Math.Abs(InitialDragPoint.X - Utils.GetCursorPos().X);
                        YDistanceFromEdge = Math.Abs(InitialDragPoint.Y - Utils.GetCursorPos().Y);
                    }

                    IsDragging = true;
                }
            }else if (IsDragging && args.Msg == (uint) WindowsMessages.WM_LBUTTONUP)
            {
                TFHelperBGSprite.PositionUpdate = () => CurrentPosition;

                Variables.Menu.Item("dz191.dza.tf.hud.x")
                    .SetValue(new Slider((int)(Utils.GetCursorPos().X - XDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Width));
                Variables.Menu.Item("dz191.dza.tf.hud.y")
                    .SetValue(new Slider((int)(Utils.GetCursorPos().Y - YDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Height));

                InitialDragPoint = new Vector2();
                XDistanceFromEdge = 0;
                YDistanceFromEdge = 0;

                IsDragging = false;
            }

        }

        public static void InitSprites()
        {
            try
            {
                TFHelperBGSprite = new Render.Sprite(Resources.TFHelperBG, CurrentPosition)
                {
                    PositionUpdate = () => CurrentPosition,
                    VisibleCondition = delegate { return ShouldBeVisible; },
                };
                TFHelperBGSprite.Add(0);
            }
            catch (Exception ex)
            {
                LogHelper.AddToLog(new LogItem("TFHelper_Drawings", ex, LogSeverity.Error));
            }
        }

        private static bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(position, CurrentPosition.X, CurrentPosition.Y, SpriteWidth, SpriteHeight);
        }
    }
}
