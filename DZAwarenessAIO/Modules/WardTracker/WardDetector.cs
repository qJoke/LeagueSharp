using System;
using System.Linq;
using ClipperLib;
using DZAwarenessAIO.Utility.Extensions;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace DZAwarenessAIO.Modules.WardTracker
{
    class WardDetector
    {
        public static float lastTick;

        public static void OnTick()
        {
            if (Environment.TickCount - lastTick < 30)
            {
                return;
            }
            lastTick = Environment.TickCount;

            if (MenuExtensions.GetItemValue<KeyBind>("dz191.dza.ward.test").Active)
            {
                WardTrackerVariables.detectedWards.Add(new Ward()
                {
                    Position = Game.CursorPos,
                    startTick = Environment.TickCount,
                    WardTypeW = WardTrackerVariables.wrapperTypes.First()
                });
            }

            foreach (var s in WardTrackerVariables.detectedWards)
            {
                if (Environment.TickCount > s.startTick + s.WardTypeW.WardDuration)
                {
                    s.RemoveRenderObjects();   
                }
            }

            WardTrackerVariables.detectedWards.RemoveAll(s => Environment.TickCount > s.startTick + s.WardTypeW.WardDuration);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
           //TODO See if I will use this eventually
        }

        public static void OnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_AI_Base)
            {
                var sender_ex = sender as Obj_AI_Base;
                var ward = WardTrackerVariables.wrapperTypes.FirstOrDefault(
                    w => w.ObjectName.ToLower().Equals(sender_ex.CharData.BaseSkinName.ToLower()));
                if (ward != null)
                {
                    var StartTick = Environment.TickCount - (int)((sender_ex.MaxMana - sender_ex.Mana) * 1000);
                    
                    var AlreadyDetected =
                        WardTrackerVariables.detectedWards.FirstOrDefault(
                            w =>
                                w.Position.Distance(sender_ex.ServerPosition) < 125 &&
                                (Math.Abs(w.startTick - StartTick) < 800 || w.WardTypeW.WardType != WardType.Green ||
                                 w.WardTypeW.WardType != WardType.Trinket));
                    if (AlreadyDetected != null)
                    {
                        AlreadyDetected.RemoveRenderObjects();
                        WardTrackerVariables.detectedWards.RemoveAll(
                            w =>
                                w.Position.Distance(sender_ex.ServerPosition) < 125 &&
                                (Math.Abs(w.startTick - StartTick) < 800 || w.WardTypeW.WardType != WardType.Green ||
                                 w.WardTypeW.WardType != WardType.Trinket));
                    }
                    
                    WardTrackerVariables.detectedWards.Add(new Ward()
                    {
                        Position = sender_ex.ServerPosition,
                        startTick = StartTick,
                        WardTypeW = ward
                    });
                }
            }
        }

        internal static void OnDraw(EventArgs args)
        {
            foreach (var ward in WardTrackerVariables.detectedWards)
            {
                var position = ward.Position;
                var shape = Helper.GetPolygonVertices(new Vector2(position.X, position.Y + 15.5f).To3D(), 4, 65f, 0);
                var list = shape.Select(v2 => new IntPoint(v2.X, v2.Y)).ToList();
                var currentPoly = list.ToPolygon();
                var colour = Color.Chartreuse;
                switch (ward.WardTypeW.WardType)
                {
                    case WardType.Green:
                        colour = Color.Chartreuse;
                        break;
                    case WardType.Pink:
                        colour= Color.DarkMagenta;
                        break;
                    case WardType.Trinket:
                    case WardType.TrinketUpgrade:
                        colour = Color.Yellow;
                        break;
                }

                currentPoly.Draw(colour);
            }
        }
    }
}
