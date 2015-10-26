using System;
using System.Linq;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.HudUtility;
using LeagueSharp;
using SharpDX;


namespace DZAwarenessAIO
{
    /// <summary>
    /// The DZAwareness Class
    /// </summary>
    internal class DZAwareness
    {
        /// <summary>
        /// Called when the assembly gets loaded.
        /// </summary>
        public static void OnLoad()
        {
            Game.OnUpdate += OnUpdate;
        }

        /// <summary>
        /// Raises the <see cref="Game.OnUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnUpdate(EventArgs args)
        {
            foreach (var module in Variables.Modules.Where(mod => mod.ShouldRun() && mod.GetModuleType() == ModuleTypes.OnUpdate))
            {
                module.OnTick();
            }
            var k = 0;
            foreach (var sprite in ImageLoader.AddedHeroes)
            {
                var s = sprite.Value.HeroSprite;
                s.Position = new Vector2(
                        HudDisplay.CurrentPosition.X + 15 + (s.Scale.X * (29 + (s.Width))) * k +
                        (s.Width * s.Scale.X) * (k) - (s.Width * s.Scale.X) / 2f,
                        HudDisplay.CurrentPosition.Y + HudDisplay.CroppedHeight - s.Height - 6);
                k++;
            }
        }
    }
}
