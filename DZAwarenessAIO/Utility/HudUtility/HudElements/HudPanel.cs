using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace DZAwarenessAIO.Utility.HudUtility.HudElements
{
    /// <summary>
    /// The Parent Hud Panel class
    /// </summary>
    class HudPanel : HudElement
    {
        /// <summary>
        /// Gets or sets the position of the Panel.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position => new Vector2(HudVariables.CurrentPosition.X + this.X, HudVariables.CurrentPosition.Y + HudVariables.CroppedHeight + this.Y);

        public int X;

        public int Y;

        /// <summary>
        /// Gets or sets the name of the panel.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        public Rectangle_Ex Rectangle { get; set; }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="HudPanel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public HudPanel(string name, int x, int y, int width, int height)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.X = x;
            this.Y = y;
            HudVariables.HudElements.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HudPanel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public HudPanel(string name, Vector2 Position, int width, int height)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.X = (int) Position.X;
            this.Y = (int) Position.Y;
            HudVariables.HudElements.Add(this);
        }

        public override void OnLoad()
        {
            Game.OnUpdate += OnUpdate;
        }

        private void OnUpdate(System.EventArgs args)
        {
            if (this.Rectangle != null)
            {
                this.Rectangle.X = (int) this.Position.X;
                this.Rectangle.Y = (int) this.Position.Y;
            }
        }

        public override void RaiseEvents()
        {
            //
        }

        /// <summary>
        /// Initializes the drawings.
        /// </summary>
        public override void InitDrawings()
        {
            Rectangle = new Rectangle_Ex((int)this.Position.X, (int)this.Position.Y, (int)this.Width, (int)this.Height, Color.Black)
            {
                VisibleCondition = delegate
                { return HudVariables.ShouldBeVisible && HudVariables.CurrentStatus == SpriteStatus.Expanded; }
            };
            Rectangle.Add(2);

        }

        public override void RemoveDrawings()
        {
            //
        }
    }

    public class Rectangle_Ex : Render.RenderObject
        {
            public delegate Vector2 PositionDelegate();
            public delegate bool VisibleDelegate();

            private readonly SharpDX.Direct3D9.Line _line;
            public ColorBGRA Color;
            public float border;

            public Rectangle_Ex(int x, int y, int width, int height, ColorBGRA color, float border = 2)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = color;
                this.border = border;
                _line = new SharpDX.Direct3D9.Line(Drawing.Direct3DDevice) { Width = border};
                Game.OnUpdate += Game_OnUpdate;
            }

            private void Game_OnUpdate(EventArgs args)
            {
                if (PositionUpdate != null)
                {
                    Vector2 pos = PositionUpdate();
                    X = (int) pos.X;
                    Y = (int) pos.Y;
                }
            }

            public int X { get; set; }

            public int Y { get; set; }

            public int Width { get; set; }
            public int Height { get; set; }
            public PositionDelegate PositionUpdate { get; set; }

            public override void OnEndScene()
            {
                try
                {
                    if (_line.IsDisposed)
                    {
                        return;
                    }

                    _line.Begin();
                    _line.Draw(new[] { new Vector2(X, Y), new Vector2(X + Width + border, Y) }, Color);
                    _line.Draw(new[] { new Vector2(X, Y + Height / 2), new Vector2(X + Width - 1, Y + Height / 2) }, Color);
                    _line.Draw(new[] { new Vector2(X, Y), new Vector2(X, Y + Height / 2) }, Color);
                    _line.Draw(new[] { new Vector2(X + Width, Y), new Vector2(X + Width, Y + Height / 2) }, Color);
                    _line.End();
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Rectangle.OnEndScene: " + e);
                }
            }

            public override void OnPreReset()
            {
                _line.OnLostDevice();
            }

            public override void OnPostReset()
            {
                _line.OnResetDevice();
            }

            public override void Dispose()
            {
                if (!_line.IsDisposed)
                {
                    _line.Dispose();
                }
                Game.OnUpdate -= Game_OnUpdate;
            }
        }
}
