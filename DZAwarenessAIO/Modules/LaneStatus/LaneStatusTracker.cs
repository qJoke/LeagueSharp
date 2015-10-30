using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAwarenessAIO.Utility;
using LeagueSharp;
using SharpDX;
using Color = System.Drawing.Color;

namespace DZAwarenessAIO.Modules.LaneStatus
{
    class LaneStatusTracker
    {
        public Geometry.Polygon BottomLanePolyA = new Geometry.Polygon();
        public Geometry.Polygon BottomLanePolyB = new Geometry.Polygon();
        public Geometry.Polygon MiddleLanePolyA = new Geometry.Polygon();
        public Geometry.Polygon MiddleLanePolyB = new Geometry.Polygon();
        public Geometry.Polygon TopLanePolyA = new Geometry.Polygon();
        public Geometry.Polygon TopLanePolyB = new Geometry.Polygon();

        public LaneStatusTracker()
        {

            Drawing.OnDraw += Drawing_OnDraw;
        }

        void Drawing_OnDraw(EventArgs args)
        {
           // BottomLanePolyA.Draw(Color.AliceBlue);
           // BottomLanePolyB.Draw(Color.Aqua);

           // MiddleLanePolyA.Draw(Color.Brown);
            //MiddleLanePolyB.Draw(Color.Chartreuse);

            //TopLanePolyA.Draw(Color.BlueViolet);
            //TopLanePolyA.Draw(Color.White);

        }
    }
}
