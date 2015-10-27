using System;
using System.Drawing;
using DZAwarenessAIO.Properties;
using DZAwarenessAIO.Utility.Extensions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAwarenessAIO.Utility.HudUtility.HudElements
{
    /// <summary>
    /// The Hud Button class
    /// </summary>
    abstract class HudButton : HudElement
    {
        /// <summary>
        /// The position of the button
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Gets or sets the parent Hud Panel.
        /// </summary>
        /// <value>
        /// The parent Hud Panel.
        /// </value>
        public HudPanel Parent { get; set; }

        /// <summary>
        /// The OnButtonClick delegate
        /// </summary>
        public delegate void OnButtonClickDelegate();

        /// <summary>
        /// Gets or sets the delegate on button click.
        /// </summary>
        /// <value>
        /// The delegate on button click.
        /// </value>
        public OnButtonClickDelegate OnButtonClick { get; set; }

        /// <summary>
        /// Gets or sets the button sprite.
        /// </summary>
        /// <value>
        /// The button sprite.
        /// </value>
        public Render.Sprite ButtonSprite { get; set; }

        /// <summary>
        /// Gets or sets the button bitmap.
        /// </summary>
        /// <value>
        /// The button bitmap.
        /// </value>
        public Bitmap ButtonBitmap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HudButton"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="Parent">The parent.</param>
        /// <param name="bitmap">The Bitmap for the button</param>
        /// <param name="disanchor">if set to <c>true</c> it will use absolute positioning.</param>
        protected HudButton(int x, int y, HudPanel Parent, Bitmap bitmap = null, bool disanchor = false)
        {
            this.Parent = Parent;
            this.ButtonBitmap = bitmap;
            Position = disanchor ? 
                new Vector2(HudVariables.CurrentPosition.X + x, HudVariables.CurrentPosition.Y + y) : 
                new Vector2(Parent.Position.X + x, Parent.Position.Y + y);
            HudVariables.HudElements.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HudButton"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="Parent">The parent.</param>
        /// <param name="bitmap">The button bitmap</param>
        /// <param name="disanchor"></param>
        protected HudButton(Vector2 position, HudPanel Parent, Bitmap bitmap = null, bool disanchor = false)
        {
            this.Parent = Parent;
            this.ButtonBitmap = bitmap;
            Position = disanchor ? 
                new Vector2(HudVariables.CurrentPosition.X + position.X, HudVariables.CurrentPosition.Y + position.Y) : 
                new Vector2(Parent.Position.X + position.X, Parent.Position.Y + position.Y);
            HudVariables.HudElements.Add(this);
        }

        /// <summary>
        /// Called when the instance is loaded.
        /// </summary>
        public override void OnLoad()
        {
            Game.OnWndProc += OnWndProc;
        }

        /// <summary>
        /// Gets the parent panel.
        /// </summary>
        /// <returns></returns>
        public HudPanel GetParent()
        {
            return Parent;
        }

        /// <summary>
        /// Initializes the drawings.
        /// </summary>
        public override void InitDrawings()
        {

        }

        /// <summary>
        /// Raises the events.
        /// </summary>
        public override void RaiseEvents()
        {
            if (OnButtonClick != null)
            {
                OnButtonClick();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:WndProc" /> event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
        private void OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint) WindowsMessages.WM_LBUTTONUP)
            {
                if (Helper.IsInside(Utils.GetCursorPos(), (int) this.Position.X, (int) this.Position.Y, 80, 20))
                {
                    RaiseEvents();
                }
            }
        }
    }
}
