using SharpDX;

namespace DZAwarenessAIO.Utility.HudUtility.HudElements
{
    /// <summary>
    /// The Parent Hud Panel class
    /// </summary>
    abstract class HudPanel : HudElement
    {
        /// <summary>
        /// Gets or sets the position of the Panel.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="HudPanel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected HudPanel(string name, int x, int y, int width, int height)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.Position = new Vector2(HudVariables.CurrentPosition.X + x, HudVariables.CurrentPosition.Y + HudVariables.CroppedHeight + y);
            HudVariables.HudElements.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HudPanel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected HudPanel(string name, Vector2 Position, int width, int height)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.Position = new Vector2(HudVariables.CurrentPosition.X + Position.X, HudVariables.CurrentPosition.Y + HudVariables.CroppedHeight + Position.Y);
            HudVariables.HudElements.Add(this);
        }

        /// <summary>
        /// Initializes the drawings.
        /// </summary>
        public override void InitDrawings()
        {
            
        }
    }
}
