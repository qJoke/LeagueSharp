using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Extensions;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using Geometry = LeagueSharp.Common.Geometry;

namespace DZAwarenessAIO.Modules.WardTracker
{
    class WardDetector
    {
        /// <summary>
        /// The last tick the OnTick cycle performed
        /// </summary>
        public static float lastTick;

        /// <summary>
        /// Called when the assembly updates.
        /// </summary>
        public static void OnTick()
        {
            if (Environment.TickCount - lastTick < 30)
            {
                return;
            }
            lastTick = Environment.TickCount;

            foreach (var s in WardTrackerVariables.detectedWards)
            {
                if (Environment.TickCount > s.startTick + s.WardTypeW.WardDuration)
                {
                    s.RemoveRenderObjects();   
                }
            }

            WardTrackerVariables.detectedWards.RemoveAll(s => Environment.TickCount > s.startTick + s.WardTypeW.WardDuration);

        }

        /// <summary>
        /// Called when an spell is processed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
           //TODO See if I will use this eventually
        }

        /// <summary>
        /// Called when an object is created.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void OnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_AI_Base && !sender.IsAlly)
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

                    WardTrackerVariables.detectedWards.Add(new Ward(ward)
                    {
                        Position = sender_ex.ServerPosition,
                        startTick = StartTick,
                    });
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Draw" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void OnDraw(EventArgs args)
        {
            var closeWard =
                    WardTrackerVariables.detectedWards.FirstOrDefault(
                        w => w.Position.Distance(Game.CursorPos, true) < 80 * 80);

                if (closeWard != null)
                {
                    if (MenuExtensions.GetItemValue<KeyBind>("dz191.dza.ward.extrainfo").Active)
                    {
                        var polyPoints = GetWardPolygon(closeWard);
                        var currentPath = polyPoints.Select(v2 => new IntPoint(v2.X, v2.Y)).ToList();
                        var currentPoly = Geometry.ToPolygon(currentPath);
                        currentPoly.Draw(GetWardColor(closeWard.WardTypeW));
                    }
                }
            foreach (var ward in WardTrackerVariables.detectedWards)
            {
                if (closeWard != null && ward.Position.Distance(closeWard.Position) < float.Epsilon && MenuExtensions.GetItemValue<KeyBind>("dz191.dza.ward.extrainfo").Active)
                {
                    continue;
                }

                var position = ward.Position;
                var shape = Helper.GetPolygonVertices(
                    new Vector2(position.X, position.Y + 15.5f).To3D(),
                    MenuExtensions.GetItemValue<Slider>("dz191.dza.ward.sides").Value, 65f, 0);
                var list = shape.Select(v2 => new IntPoint(v2.X, v2.Y)).ToList();
                var currentPoly = Geometry.ToPolygon(list);
                var colour = GetWardColor(ward.WardTypeW);
                currentPoly.Draw(colour);
            }
        }

        private static List<Vector2> GetWardPolygon(Ward w)
        {
            var position = w.Position;
            var closeWards = GetWardsCloseTo(w);
            var polygonsList = closeWards.Select(enemy => new Utility.Geometry.Circle(position.To2D(), w.WardTypeW.WardVisionRange).ToPolygon()).ToList();
            var pathList = Utility.Geometry.ClipPolygons(polygonsList);
            var pointList = pathList.SelectMany(path => path, (path, point) => new Vector2(point.X, point.Y)).Where(currentPoint => !currentPoint.IsWall()).ToList();
            return pointList;
        }

        private static List<Ward> GetWardsCloseTo(Ward w)
        {
            return WardTrackerVariables.detectedWards.Where(m => m.Position.Distance(w.Position, true) <= Math.Pow(w.WardTypeW.WardVisionRange, 2)).ToList();
        }

        private static Color GetWardColor(WardTypeWrapper w)
        {
            var colour = Color.Chartreuse;
            switch (w.WardType)
            {
                case WardType.Green:
                    colour = Color.Chartreuse;
                    break;
                case WardType.Pink:
                    colour = Color.DarkMagenta;
                    break;
                case WardType.Trinket:
                case WardType.TrinketUpgrade:
                    colour = Color.Yellow;
                    break;
                case WardType.TeemoShroom:
                case WardType.ShacoBox:
                    colour = Color.DarkRed;
                    break;
            }
            return colour;
        }

        public static void OnDelete(GameObject sender, EventArgs args)
        {
            if (sender is Obj_AI_Base && !sender.IsAlly)
            {
                var sender_ex = sender as Obj_AI_Base;

                WardTrackerVariables.detectedWards.RemoveAll(s => s.Position.Distance(sender_ex.ServerPosition, true) < 10 * 10);
            }
        }
    }
}
