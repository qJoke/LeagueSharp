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
            BottomLanePolyA.Add(new Vector2(12058, 2633));
            BottomLanePolyA.Add(new Vector2(14617, 547));
            BottomLanePolyA.Add(new Vector2(1981, 774));
            BottomLanePolyA.Add(new Vector2(2012, 1635));

            BottomLanePolyB.Add(new Vector2(12058, 2633));
            BottomLanePolyB.Add(new Vector2(14617, 547));
            BottomLanePolyB.Add(new Vector2(13843, 12905));
            BottomLanePolyB.Add(new Vector2(13166, 12802));

            MiddleLanePolyA.Add(new Vector2(2384, 1626));
            MiddleLanePolyA.Add(new Vector2(1578, 2371));
            MiddleLanePolyA.Add(new Vector2(6860, 7900));
            MiddleLanePolyA.Add(new Vector2(7850, 6870));

            MiddleLanePolyB.Add(new Vector2(12422, 13252));
            MiddleLanePolyB.Add(new Vector2(13291, 12332));
            MiddleLanePolyB.Add(new Vector2(7850, 6870));
            MiddleLanePolyB.Add(new Vector2(6800, 7900));

            TopLanePolyA.Add(new Vector2(3021, 12006));
            TopLanePolyA.Add(new Vector2(550, 14358));
            TopLanePolyA.Add(new Vector2(800, 2000));
            TopLanePolyA.Add(new Vector2(1732, 1933));

            TopLanePolyB.Add(new Vector2(3021, 12006));
            TopLanePolyB.Add(new Vector2(550, 14358));
            TopLanePolyB.Add(new Vector2(12531, 14101));
            TopLanePolyB.Add(new Vector2(12651, 13203));

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
