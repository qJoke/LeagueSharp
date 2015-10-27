using System;
using System.Drawing;
using DZAwarenessAIO.Properties;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAwarenessAIO.Utility.HudUtility
{
    class HudDisplay
    {

        /// <summary>
        /// Gets or sets the hud sprite.
        /// </summary>
        /// <value>
        /// The hud sprite.
        /// </value>
        public static Render.Sprite HudSprite { get; set; }

        /// <summary>
        /// Gets or sets the expand button sprite.
        /// </summary>
        /// <value>
        /// The expand button.
        /// </value>
        public static Render.Sprite ExpandShrinkButton { get; set; }

        /// <summary>
        /// Gets the current position of the HUD.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public static Vector2 CurrentPosition
        {
            get
            {
                return IsDragging ? DraggingPosition : new Vector2(
                    MenuExtensions.GetItemValue<Slider>("dz191.dza.hud.x").Value,
                    MenuExtensions.GetItemValue<Slider>("dz191.dza.hud.y").Value
                    );
            }
        }

        /// <summary>
        /// The dragging position of the hud
        /// </summary>
        public static Vector2 DraggingPosition = new Vector2();

        /// <summary>
        /// The HUD sprite width
        /// </summary>
        public static readonly float SpriteWidth = Resources.TFHelperBG.Width;

        /// <summary>
        /// The HUD sprite height
        /// </summary>
        public static readonly float SpriteHeight = Resources.TFHelperBG.Height;

        /// <summary>
        /// Indicates whether or not the hud is being dragged
        /// </summary>
        public static bool IsDragging = false;

        /// <summary>
        /// The cropped height of the sprite
        /// </summary>
        public const int CroppedHeight = 80;

        /// <summary>
        /// The initial drag point
        /// </summary>
        private static Vector2 InitialDragPoint = new Vector2();

        /// <summary>
        /// The x distance from the top left edge
        /// </summary>
        private static float XDistanceFromEdge = 0;

        /// <summary>
        /// The y distance from the top left edge
        /// </summary>
        private static float YDistanceFromEdge = 0;


        /// <summary>
        /// Gets a value indicating whether the hud should be visible
        /// </summary>
        /// <value>
        ///   <c>true</c> if the hud should be visible; otherwise, <c>false</c>.
        /// </value>
        public static bool ShouldBeVisible
        {
            get { return MenuExtensions.GetItemValue<bool>("dz191.dza.hud.show"); }
        }

        /// <summary>
        /// The current status of the hud
        /// </summary>
        public static SpriteStatus CurrentStatus = SpriteStatus.Shrinked;

        /// <summary>
        /// The init point of the class
        /// </summary>
        public static void OnLoad()
        {
            var RootMenu = Variables.Menu;
            var moduleMenu = new Menu("HUD!", "dz191.dza.hud").SetFontStyle(FontStyle.Bold, SharpDX.Color.Red);
            {
                moduleMenu.AddBool("dz191.dza.hud.show", "Show HUD").SetTooltip("Shows the DZAwareness Helper hud").ValueChanged += (sender, args) =>
                {
                    /**Dispose of the sprites*/ 
                    if (args.GetNewValue<bool>())
                    {
                        InitSprites(true);
                    }
                    else
                    {
                        HudSprite.Remove();
                        ExpandShrinkButton.Remove();
                    }
                };
                moduleMenu.AddSlider("dz191.dza.hud.x", "HUD X", new Tuple<int, int, int>(200, 0, Drawing.Direct3DDevice.Viewport.Width)).SetTooltip("Hud X Position (You can drag it)");
                moduleMenu.AddSlider("dz191.dza.hud.y", "HUD Y", new Tuple<int, int, int>(200, 0, Drawing.Direct3DDevice.Viewport.Height)).SetTooltip("Hud Y Position (You can drag it)");
                RootMenu.AddSubMenu(moduleMenu);
            }

            InitSprites();

            Game.OnWndProc += OnWndProc;
        }

        /// <summary>
        /// Raises the <see cref="E:WndProc" /> event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
        private static void OnWndProc(WndEventArgs args)
        {
            if (HudSprite == null || !ShouldBeVisible)
            {
                return;
            }

            if (IsDragging)
            {
                DraggingPosition.X = (int) (Utils.GetCursorPos().X - XDistanceFromEdge);
                DraggingPosition.Y = (int)(Utils.GetCursorPos().Y - YDistanceFromEdge);

               // Variables.Menu.Item("dz191.dza.hud.x")
                //   .SetValue(new Slider((int)(Utils.GetCursorPos().X - XDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Width));
               // Variables.Menu.Item("dz191.dza.hud.y")
                 //   .SetValue(new Slider((int)(Utils.GetCursorPos().Y - YDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Height));
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

                        DraggingPosition.X = (int)(Utils.GetCursorPos().X - XDistanceFromEdge);
                        DraggingPosition.Y = (int)(Utils.GetCursorPos().Y - YDistanceFromEdge);

                    }

                    IsDragging = true;
                }
            }
            else if (IsDragging && args.Msg == (uint)WindowsMessages.WM_LBUTTONUP)
            {
                HudSprite.PositionUpdate = () => CurrentPosition;

                Variables.Menu.Item("dz191.dza.hud.x")
                    .SetValue(new Slider((int)(Utils.GetCursorPos().X - XDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Width));
                Variables.Menu.Item("dz191.dza.hud.y")
                    .SetValue(new Slider((int)(Utils.GetCursorPos().Y - YDistanceFromEdge), 0, Drawing.Direct3DDevice.Viewport.Height));

                InitialDragPoint = new Vector2();
                XDistanceFromEdge = 0;
                YDistanceFromEdge = 0;
                DraggingPosition = new Vector2();

                IsDragging = false;
            }

            if (args.Msg == (uint) WindowsMessages.WM_LBUTTONUP)
            {
                if (
                    Utils.GetCursorPos()
                        .Distance(new Vector2(CurrentPosition.X + SpriteWidth - 15,
                            CurrentPosition.Y + CroppedHeight - 15)) < 7)
                {
                    if (CurrentStatus == SpriteStatus.Shrinked)
                    {
                        CurrentStatus = SpriteStatus.Expanded;
                        HudSprite.Crop(0, 0, (int) SpriteWidth, (int)SpriteHeight);

                        ExpandShrinkButton.Remove();
                        ExpandShrinkButton = new Render.Sprite(Resources.Shrink, CurrentPosition)
                        {
                            PositionUpdate = () => new Vector2(CurrentPosition.X + SpriteWidth - 20, CurrentPosition.Y + CroppedHeight - 20),
                            Scale = new Vector2(0.7f, 0.7f),
                            VisibleCondition = delegate { return ShouldBeVisible; }
                        };

                        ExpandShrinkButton.Add(1);
                    }
                    else
                    {
                        CurrentStatus = SpriteStatus.Shrinked;
                        HudSprite.Crop(0, 0, (int)SpriteWidth, CroppedHeight);

                        ExpandShrinkButton.Remove();
                        ExpandShrinkButton = new Render.Sprite(Resources.Expand, CurrentPosition)
                        {
                            PositionUpdate = () => new Vector2(CurrentPosition.X + SpriteWidth - 20, CurrentPosition.Y + CroppedHeight - 20),
                            Scale = new Vector2(0.7f, 0.7f),
                            VisibleCondition = delegate { return ShouldBeVisible; }
                        };

                        ExpandShrinkButton.Add(1);
                    }

                }
            }
        }

        /// <summary>
        /// Initializes the sprites.
        /// </summary>
        /// <param name="bypass">if set to <c>true</c> it will bypass the Menu enabled check.</param>
        public static void InitSprites(bool bypass = false)
        {
            try
            {
                if (!MenuExtensions.GetItemValue<bool>("dz191.dza.hud.show") && !bypass)
                {
                    return;
                }

                HudSprite = new Render.Sprite(Resources.TFHelperBG, CurrentPosition)
                {
                    PositionUpdate = () => CurrentPosition,
                    VisibleCondition = delegate { return ShouldBeVisible; },
                };
                HudSprite.Crop(0, 0, (int) SpriteWidth, CroppedHeight);

                ExpandShrinkButton = new Render.Sprite(Resources.Expand, CurrentPosition)
                {
                    PositionUpdate = () => new Vector2(CurrentPosition.X + SpriteWidth - 20, CurrentPosition.Y + CroppedHeight - 20),
                    Scale = new Vector2(0.7f, 0.7f),
                    VisibleCondition = delegate { return ShouldBeVisible; }
                };

                ExpandShrinkButton.Add(1);
                HudSprite.Add(0);
            }
            catch (Exception ex)
            {
                LogHelper.AddToLog(new LogItem("Hud_Init", ex, LogSeverity.Error));
            }
        }

        /// <summary>
        /// Determines whether the specified position is inside the rectangle.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private static bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(position, CurrentPosition.X, CurrentPosition.Y, SpriteWidth, (CurrentStatus == SpriteStatus.Shrinked ? CroppedHeight : SpriteHeight));
        }
    }

    /// <summary>
    /// The Hud status enum
    /// </summary>
    enum SpriteStatus
    {
        Expanded, Shrinked
    }
}
